using MyBox;
using PlayFab.ProfilesModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PCHandler : MonoBehaviour
{
    public static PCHandler instance;


    public ChampClass champ { get; private set; }

    EntityHandler entityHandler;

    [Separator("SCRIPT")]
    [HideInInspector] public PlayerMove move;
    [HideInInspector] public PlayerInventory inventory;
    [HideInInspector] public PlayerController controller;

    [Separator("COMPONENTS")]
    [HideInInspector] public GameObject body;
    [HideInInspector] public SpriteRenderer rend;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;

    [Separator("INPUT")]
      FloatingJoystick joystick;
    public InputButton interactButton;
    public InputButton interactSecondButton;
    AbilityButton skill1Button;
    AbilityButton skill2Button;

    [Separator("ALLY")]
    public List<AllyCombatHandler> allyChampList = new();

    [Separator("DEBUG")]
    [SerializeField] ChampData DEBUGstartChampData;

    [Separator("TARGETTING")]
    [SerializeField] GameObject targettingAim;


    [Separator("DEBUG")]
    [SerializeField] bool DEBUGcannotAutoAttack;
    [SerializeField] ChampData currentChampData;
    [SerializeField] float currentChampLevel;

    public float raidGainedExperience { get; private set; } //this is given to the char in the end.

    private void Awake()
    {

        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        
    }

    private void Start()
    {
        gameObject.layer = 3;
        controller = GetComponent<PlayerController>();
        entityHandler = GetComponent<EntityHandler>();
        inventory = GetComponent<PlayerInventory>();
        inventory.StartRaidInventory();
        //i need to set up the ability button and joystick.



        if (DEBUGstartChampData != null)
        {
            SetUp(new ChampClass(DEBUGstartChampData));
        }    
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("triggered");
            entityHandler.ttEvents.OnKillEnemy(entityHandler);
        }
    }

    private void FixedUpdate()
    {

        if (Input.GetKeyDown(KeyCode.T))
        {
            EndRaid();
        }

        HandleTargetting();
        AutoAttacking();
        HandleCooldown();
        HandleRaidInteract();
    }

    #region GETTERS
    public List<ChampClass> GetAllies()
    {
        List<ChampClass> newList = new();

        foreach (var item in allyChampList)
        {
            newList.Add(item.champ);
        }

        return newList;
    }

    #endregion

    #region SETTERS
    public void SetUp(ChampClass mainChamp)
    {
        champ = mainChamp;

        if(UIHolder.instance == null) 
        {
            Debug.Log("there was no ui instance");
        }

        skill1Button = UIHolder.instance.skill1Button;
        skill2Button = UIHolder.instance.skill2Button;
        joystick = UIHolder.instance.joystick;

       

        if(entityHandler != null)
        {
            //then we pass this information to the stat.
            if(entityHandler.ttStat != null)
            {
                entityHandler.ttStat.SetUp(champ.statList);
            }
            else
            {
                Debug.Log("there was no entity handler.");
            }
        }

        //we also need to put the entityhandler in the skills.

        SetUpAbilities();


        currentChampData = champ.data;
        currentChampLevel = champ.champLevel;

    }

    public void SetAllies(List<AllyCombatHandler> allyList)
    {
        allyChampList = allyList;
    }


    void SetUpAbilities()
    {

        if(entityHandler == null)
        {
            entityHandler = GetComponent<EntityHandler>();
        }

        champ.autoAttack.SetUp(entityHandler, AbilityType.AutoAttack);
        champ.skill1.SetUp(entityHandler, AbilityType.Skill1);
        champ.skill1.SetUpUnit(skill1Button);
        champ.skill2.SetUp(entityHandler, AbilityType.Skill2);
        champ.skill2.SetUpUnit(skill2Button);

        champ.passiveMain.SetUp(entityHandler, AbilityType.PassiveMain);
        champ.passiveMain.Call();


        

    }

    void CreateAlly(ChampClass allyChamp)
    {

    }
    #endregion

    #region INTERACT SYSTEM
    IInteractable currentInteract;

    
    void HandleInteract()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.8f, move.lastDir, 0.5f, LayerMask.GetMask("Interact"));


        if (hit.collider == null)
        {
            if (currentInteract != null) currentInteract.UIInteract(null, false);
            currentInteract = null;
            return;
        }

        IInteractable interact = hit.collider.GetComponent<IInteractable>();

        if (interact == null)
        {
            if (currentInteract != null) currentInteract.UIInteract(null, false);
            currentInteract = null;
            return;
        }




        currentInteract = interact;

        currentInteract.UIInteract(null, true);
        interactButton.ChangeText(interact.GetInteractName(null));
        interactSecondButton.ChangeText(interact.GetInteractName(null, true));
    }

    public void InputInteract()
    {
        if (currentInteract == null) return;
        if (!currentInteract.IsInteractable(null)) return;
        currentInteract.Interact(null);
    }
    public void InputSecondInteract()
    {
        if (currentInteract == null) return;
        if (!currentInteract.IsSecondInteractable(null)) return;
        currentInteract.SecondInteract(null);
    }

    #endregion

    #region TARGETTING

    EntityDamageable currentTarget;

    void HandleTargetting()
    {
        //attack the closest target.

       

        if(entityHandler == null) return;

        targettingAim.SetActive(currentTarget != null);
       
        if (currentTarget != null)
        {
            //we control thee ui telling it where to go.
            targettingAim.transform.position = currentTarget.transform.position;
        }
        
        float distance = 0;


        if(currentTarget != null)
        {
            float currentTargetDistance = Vector2.Distance(currentTarget.transform.position, transform.position);

            if(currentTargetDistance > 5)
            {
                distance = 5;
            }
            else
            {
                distance = currentTargetDistance;
            }
        }
        else
        {
            distance = 5; //range of autoattack.
        }



       RaycastHit2D cast = Physics2D.CircleCast(transform.position, distance, Vector2.up, 3, LayerMask.GetMask("Enemy"));
        //how to save performance?

        if (cast.collider == null)
        {
            currentTarget = null;
            entityHandler.SetTarget(null);
            return;
        }

        EntityDamageable damageable = cast.collider.GetComponent<EntityDamageable>();

        if (damageable == null)
        {
            Debug.Log("no dadmageable");
            currentTarget = null;
            entityHandler.SetTarget(null);
            return;
        }

        currentTarget = damageable;
        entityHandler.SetTarget(currentTarget);
        
        //and we keep spawning the target in the currenttarget.

    }

    #endregion

    #region INTERACTIONRANGE
    //in the raid you interact with anything you get close.

    void HandleRaidInteract()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, Vector2.one, 50, LayerMask.GetMask(LayerMaskEnum.Interact.ToString()));

        if(hit.collider == null)
        {            
            return;
        }
        Debug.Log("hit something");

        IRaidInteractable raidInteractable = hit.collider.gameObject.GetComponent<IRaidInteractable>();

        if (raidInteractable == null) return;

        raidInteractable.Interact(this);

    }


    #endregion

    #region ATTACKING
    //if you are not moving you are attacking the currentarget.
    //



    void AutoAttacking()
    {

        if (DEBUGcannotAutoAttack) return;

        champ.autoAttack.HandleCooldown();

        if (currentTarget == null) return;
        if (!entityHandler.ttStat.HasBDBoolean(BDBooleanType.ShootAndMove) && controller.IsMoving()) return;

        champ.autoAttack.TryToCall();
        //but here we attribute a different cooldown value.
        //cooldown of abiities work as percentage. 

       
        

    }

    void HandleCooldown()
    {
        champ.skill1.HandleCooldown();
        champ.skill2.HandleCooldown();


    }

    #endregion

    #region EXPERIENCE
    public void GainExperience(float value)
    {
        raidGainedExperience += value;
    }

    public void ReduceExperience(float value)
    {
        raidGainedExperience -= value;

        if(raidGainedExperience < 0)
        {
            Debug.Log("soimething went wrong");
        }
    }

    public float GetExperinece(float min)
    {

        if(raidGainedExperience >= min) 
        {
            return min;
        }
        else
        {
            return raidGainedExperience;
        }


    }

    #endregion


    bool debugCalledEndRaid;

    public void EndRaid()
    {
        //1 - acctually give the score
        //2 - then call the ui

        //RaidScoreType scoreType = RaidHandler.instance.GetRaidScore();

    }
}

//but i want to set it up for enemies as well.
//