using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public static PlayerHandler instance;

    [Separator("SCRIPTS")]
    [HideInInspector] public PlayerController controller;
    [HideInInspector] public PlayerMove move;
    [HideInInspector] public PlayerInventory inventory;
    [HideInInspector] public PlayerResource resource;
    [HideInInspector] public PlayerParty party;

    [Separator("COMPONENTS")]
    [HideInInspector] public GameObject body;
    [HideInInspector] public SpriteRenderer rend;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Rigidbody2D rb;

    [Separator("UI")]
    public UIHolder uiHolder;

    [Separator("INPUT")]
    public FloatingJoystick joystick;
    public InputButton interactButton;
    public InputButton interactSecondButton;

    BlockClass block;


    #region MONEY
    public int currentMoney { get; private set; }

    public bool HasEnoughMoney(int money) => currentMoney >= money;
    public void GainMoney(int money)
    {
        currentMoney += money;
        uiHolder.player.UpdateMoney(currentMoney, money);
    }
    public void LoseMoney(int money)
    {
        currentMoney -= money;
        currentMoney = Mathf.Clamp(currentMoney, 0, int.MaxValue);
        uiHolder.player.UpdateMoney(currentMoney, -money);
    }

    [ContextMenu("DEBUG LOSE MONEY")]
    public void DEBUGLOSEMONEY()
    {
        LoseMoney(Random.Range(20, 50));
    }
    #endregion


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        SetUpComponents();
        SetUpScripts();
    }

    private void Start()
    {
        uiHolder.player.UpdateMoney(currentMoney, 0);
        interactButton.EventPressed += InputInteract;
        interactButton.EventReleased += InputRelease;
        interactSecondButton.EventPressed += InputSecondInteract;

      
    }

    private void OnDestroy()
    {
        interactButton.EventPressed -= InputInteract;
        interactButton.EventReleased -= InputRelease;
        interactSecondButton.EventPressed -= InputSecondInteract;
    }


    private void FixedUpdate()
    {
        CheckInteractButton();
        HandleInteract();
        InteractWithBuySpot();
    }

    #region SET UP
    void SetUpComponents()
    {
        body = transform.GetChild(0).gameObject;
        rend = body.GetComponent<SpriteRenderer>();
        anim = body.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void SetUpScripts()
    {
        controller = GetComponent<PlayerController>();
        move = GetComponent<PlayerMove>();
        inventory = GetComponent<PlayerInventory>();
        resource = GetComponent<PlayerResource>();
        party = GetComponent<PlayerParty>();
    }
    #endregion

    void CheckInteractButton()
    {
        if(currentInteract != null)
        {
            interactButton.Control(currentInteract.IsInteractable(inventory));
        }
        else
        {
            interactButton.Control(currentBuySpot != null);
        }
        

        if(currentInteract != null)
        {
            interactSecondButton.Control(currentInteract.IsSecondInteractable(inventory));
        }
        else
        {
            interactSecondButton.Control(false);
        }

        

    }

    #region INTERACT SYSTEM
    IInteractable currentInteract;


    void HandleInteract()
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.8f, move.lastDir, 0.5f, LayerMask.GetMask("Interact"));

       
        if (hit.collider == null)
        {
            if (currentInteract != null) currentInteract.UIInteract(inventory, false);
            currentInteract = null;
            return;
        }

        IInteractable interact = hit.collider.GetComponent<IInteractable>();

        if (interact == null)
        {
            if(currentInteract != null) currentInteract.UIInteract(inventory, false);
            currentInteract = null;
            return;
        }

   
       
        
        currentInteract = interact;

        currentInteract.UIInteract(inventory, true);
        interactButton.ChangeText(interact.GetInteractName(inventory));
        interactSecondButton.ChangeText(interact.GetInteractName(inventory, true));
    }

    public void InputInteract()
    {
        if (currentInteract == null) return;
        if (!currentInteract.IsInteractable(inventory)) return;
        currentInteract.Interact(inventory);
    }
    public void InputSecondInteract()
    {
        if (currentInteract == null) return;
        if (!currentInteract.IsSecondInteractable(inventory)) return;
        currentInteract.SecondInteract(inventory);
    }

    #endregion

    #region INTERACTION SYSTEM - BUYSPOT
    BuySpot currentBuySpot;

    public void StartBuySpot(BuySpot buySpot)
    {
        if(currentBuySpot != null)
        {
            StopBuySpot();
        }

        currentBuySpot = buySpot;
        interactButton.ChangeText(buySpot.GetInteractName());
    }
    public void StopBuySpot()
    {
        if(currentBuySpot != null)
        {
            currentBuySpot.Cancel();
        }
        currentBuySpot = null;
    }

    public void InteractWithBuySpot()
    {
        //while we hold the button we interact.

        if (currentBuySpot == null) return;

        if (currentBuySpot.cannotBeUsed)
        {
            StopBuySpot();
            return;
        }

        if (!interactButton.isPressing)
        {
            currentBuySpot.Regress();
            return;
        }
        //start charging the effeect up. there is a little bar there.

        currentBuySpot.Progress();
    }

    void InputRelease()
    {
        if (currentBuySpot == null) return;
        currentBuySpot.ReleaseButton();
    }

    #endregion



    #region ONRAID
    //it cannot be controlled or affect when in raid.
    

    #endregion

}

//