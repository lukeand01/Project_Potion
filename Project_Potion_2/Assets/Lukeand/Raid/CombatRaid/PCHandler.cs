using MyBox;
using PlayFab.ProfilesModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PCHandler : MonoBehaviour
{
    ChampClass champ;

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
    public FloatingJoystick joystick;
    public InputButton interactButton;
    public InputButton interactSecondButton;
    [SerializeField] AbilityButton skill1Button;
    [SerializeField] AbilityButton skill2Button;

    [Separator("ALLY")]
    public AllyCombatHandler allyTemplate;
    List<AllyCombatHandler> allyChampList = new();

    [Separator("DEBUG")]
    [SerializeField] ChampData DEBUGstartChampData;

    [Separator("TARGETTING")]
    [SerializeField] GameObject targettingAim;


    [Separator("DEBUG")]
    [SerializeField] bool DEBUGcannotAutoAttack;

    private void Start()
    {

        controller = GetComponent<PlayerController>();
        entityHandler = GetComponent<EntityHandler>();
        
        
        if(DEBUGstartChampData != null)
        {
            SetUp(new ChampClass(DEBUGstartChampData), new List<ChampClass>());
        }    
    }




    private void FixedUpdate()
    {
        HandleTargetting();
        AutoAttacking();
        HandleCooldown();
    }


    #region SETTERS
    public void SetUp(ChampClass mainChamp, List<ChampClass> allyChampList)
    {
        champ = mainChamp;

        foreach (var item in allyChampList)
        {
            CreateAlly(item);
        }
        
        SetUpAbilities();
    }

    void SetUpAbilities()
    {
        champ.autoAttack.SetUp(entityHandler);

        champ.skill1.SetUp(entityHandler);
        champ.skill1.SetUpUnit(skill1Button);
        //champ.skill2.SetUp(entityHandler);
        //champ.skill2.SetUpUnit(skill2Button);

        champ.passiveMain.SetUp(entityHandler);
        champ.passiveSupport.SetUp(entityHandler);

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


    #region ATTACKING
    //if you are not moving you are attacking the currentarget.
    //



    void AutoAttacking()
    {

        if (DEBUGcannotAutoAttack) return;

        champ.autoAttack.HandleCooldown();

        if (currentTarget == null) return;
        if (controller.IsMoving()) return;

        champ.autoAttack.TryToCall();



       
        

    }

    void HandleCooldown()
    {
        champ.skill1.HandleCooldown();
        champ.skill2.HandleCooldown();


    }

    #endregion
}

