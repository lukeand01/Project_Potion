using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class NPCBase : Tree, IInteractable, IInventory
{
    //i can allocate the different parts of it to different scripts 

    //movement
    //graphic
    //controller
    //


    //what npcs can be different
    //npcs have differnt levels of wealth, which is just a modifier and a graphic
    //npcs also have differeent tastes. combat, drinking, medicine, 
    //each potion is one of the three. the quality of the potion defines how many points and mony are gaine by each.
    //these npcs will only buy what interests them. otherwise they will no come in to your storee or they will leave.
    //the selection is random but for the type.

    //starts walking in the streets.


    [HideInInspector]public string id;

    [HideInInspector] public ItemHolder itemHolderTarget;
    [HideInInspector] public SellingObject sellingObjectTarget;
    [HideInInspector] public Transform wayOutTarget;

    bool canBeTalked;
    float currentMovePacience;
    bool isWalking;

    [Separator("CONTAINER")]
    [SerializeField] Transform handContainer;

    [Separator("VARIABLES")]
    [SerializeField]float totalMovePacience;
    [SerializeField] float rateMovePacience;
    [SerializeField] float moveSpeed;

    float timeModifier = 0.01f; 

    Vector2 currentDir;
    [HideInInspector] public Vector2 targetPos;

    PotionType potionType;

    float priceModifier = 1;
    
    #region SETTERS
    public void SetUp(PotionType potionType)
    {
        this.potionType = potionType;
    }
    public void SetWayOut(Transform wayOut, bool canBeTalked = false)
    {

        itemHolderTarget = null;
        sellingObjectTarget = null;

        wayOutTarget = wayOut;
        this.canBeTalked = canBeTalked;
    }
    public void SetItemHolderTarget(ItemHolder newItemHolder)
    {
        wayOutTarget = null;
        sellingObjectTarget = null;
        itemHolderTarget = newItemHolder;
    }

    void SetSellingObject(SellingObject sellingObject)
    {
        wayOutTarget = null;
        itemHolderTarget = null;
        sellingObjectTarget = sellingObject;

        if (sellingObjectTarget == null)
        {
            Debug.Log("somthing wrong wiht sellingobject");
        }
    }
    #endregion

    private void Start()
    {
        id = Guid.NewGuid().ToString();

        UpdateTree(GetBehavior());
    }
    Sequence2 GetBehavior()
    {
        return new Sequence2(new List<Node>
        {
            new BehaviorLeave(this),
            new BehaviorPickPotion(this),
            new BehaviorTakeItemToBuy(this)
        });
    }

    #region MOVING

    Coroutine moveCoroutine;
    public void MoveTo(Vector3 pos)
    {
        targetPos = pos;
        StopMoving();
        moveCoroutine = StartCoroutine(MoveProcess(pos));
    }
    public void StopMoving()
    {
        if(moveCoroutine != null) StopCoroutine(moveCoroutine);
        isWalking = false;
    }
    
    IEnumerator MoveProcess(Vector3 pos)
    {     
        List<MyNode> pathList = MyPathfind.instance.GetPathThroughVector(transform.position, pos);
        isWalking = true;
        float step = moveSpeed * timeModifier;

        for (int i = 0; i < pathList.Count; i++)
        {

            while(transform.position != pathList[i].transform.position)
            {
                currentDir = GetDir(pathList[i].transform.position);


                if (IsPlayerAhead() && canBeTalked)
                {
                    currentMovePacience += timeModifier * rateMovePacience;
                    transform.position = transform.position;

                    //if its time enough then we set the 


                    if(currentMovePacience > totalMovePacience)
                    {
                        canBeTalked = false;
                    }
                }
                else
                {
                    if (canBeTalked) currentMovePacience = 0;
                    transform.position = Vector3.MoveTowards(transform.position, pathList[i].transform.position, step);
                }

                
                yield return new WaitForSeconds(timeModifier);
            }


            
        }

        isWalking = false;
    }

    public bool HasArrivedToTarget(Transform target)
    {
        float distance = Vector3.Distance(target.position, targetPos);

        return distance <= 0.1f && isWalking;
    }

    #endregion

    #region ORDERS

    public bool OrderHasItemToChoose()
    {
        //check every itemholder to see something he likes.
        ItemHolder selectedItemHolder = GameHandler.instance.store.GetItemHolder(potionType);

        itemHolderTarget = selectedItemHolder;
      
        if(itemHolderTarget != null)
        {
            SetItemHolderTarget(itemHolderTarget);
        }


        return itemHolderTarget != null;
    }

    public void OrderGetItem()
    {
        //we turn to the thing.
        //we remove one itme from the itemholder.
        //set course for selling machine.

        StartCoroutine(GetItemProcess());
    }

    IEnumerator GetItemProcess()
    {
        yield return new WaitForSeconds(0.5f);

        if(itemHolderTarget == null)
        {
            Debug.Log("this was wrong?");
            yield break;
        }


        if (itemHolderTarget.HasItem())
        {
            itemHolderTarget.GiveItemNPC(gameObject);
        }


    }


    public void OrderLeave()
    {
        Transform wayOut = GameHandler.instance.npc.GetRandomWayOut();
        SetWayOut(wayOut);
    }

    public void OrderGoBuyCurrentItem()
    {
        SetSellingObject(GameHandler.instance.store.GetSellingObject());
     
    }

    #endregion

    #region EMOTES

    #endregion

    #region GRAPHIC

    void Rotate()
    {
        //rotate always based in current dir

    }

    #endregion

    #region INTERACT
    public string GetInteractName(PlayerInventory inventory, bool isSecond = false)
    {
        return "Talk";
    }

    public void Interact(PlayerInventory inventory)
    {
        //if can talk then that means it tries to neegotatie.
        //and it works very easily. just click talk and the player stays still for a second.
        //it calculates if it worked, then if it has something to buy then it makes the decision.
        if (!CanPersuade()) return;
        StartPersuasion();

    }

    public bool IsInteractable(PlayerInventory inventory)
    {
        
        return wayOutTarget != null && canBeTalked;
    }

    public bool IsSecondInteractable(PlayerInventory inventory)
    {
        return false;
    }

    public void SecondInteract(PlayerInventory inventory)
    {
        
    }

    public void UIInteract(PlayerInventory inventory, bool isClose = false)
    {
        
    }

    #endregion

    #region PERSUASION
    //
    [Separator("PERSUADE STUFF")]
    [SerializeField] Image persuadeImage;
    Coroutine persuadeCoroutine;

    bool CanPersuade()
    {
        //need to increase level.

        return true;
    }

    bool HasPersuasionSucceded()
    {

        return true;
    }

    [ContextMenu("DEBUG START PERSUASION")]
    void StartPersuasion()
    {
        //the character stays still.
        //
        StopAllCoroutines();
        persuadeCoroutine = StartCoroutine(PersuasionProcess());
    }

    IEnumerator PersuasionProcess()
    {
        canBeTalked = false;

        bool success = HasPersuasionSucceded();

        int rolls = UnityEngine.Random.Range(10, 14);

        persuadeImage.transform.localScale = Vector3.zero;

        persuadeImage.gameObject.SetActive(true);

        for (int i = 0; i < rolls; i++)
        {
            if(i % 2 == 0)
            {
                //even
                persuadeImage.color = Color.red;
            }
            else
            {
                persuadeImage.color = Color.green;
            }


            while (persuadeImage.transform.localScale.y < 0.4f)
            {

                persuadeImage.transform.localScale += new Vector3(0.05f, 0.05f, 0);
                yield return new WaitForSeconds(0.008f);
            }


            if (i >= rolls - 2)
            {
                
                if (i % 2 == 0 && !success)
                {
                    //even
                    persuadeImage.color = Color.red;
                    StartCoroutine(PersuasionEndProcess(success));
                    yield break;
                }

                if(i % 2 == 1 && success)
                {
                    persuadeImage.color = Color.green;
                    StartCoroutine(PersuasionEndProcess(success));
                    yield break;
                }
                Debug.Log("got here but found nothing");


            }

            yield return new WaitForSeconds(0.05f);

            while (persuadeImage.transform.localScale.y > 0)
            {

                persuadeImage.transform.localScale -= new Vector3(0.05f, 0.05f, 0);
                yield return new WaitForSeconds(0.008f);
            }



           
        }

        
    }


    IEnumerator PersuasionEndProcess(bool sucess)
    {
        //first we enlarge the thing.

        while (persuadeImage.transform.localScale.y < 0.6f)
        {
            persuadeImage.transform.localScale += new Vector3(0.05f, 0.05f, 0);
            yield return new WaitForSeconds(0.008f);
        }


        yield return new WaitForSeconds(0.4f);
        while (persuadeImage.transform.localScale.y > 0)
        {
            persuadeImage.transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        HandlePersuasion(sucess);
        persuadeImage.gameObject.SetActive(false);
    }


    void HandlePersuasion(bool sucess)
    {

        ItemHolder itemHolder = GameHandler.instance.store.GetItemHolder(potionType);



        
        if (sucess && itemHolder != null)
        {
            
            SetItemHolderTarget(itemHolder);

        }
        else
        {
            MoveTo(wayOutTarget.position);
        }
    }

    #endregion


    #region UTILS
    public bool IsPlayerAhead()
    {
        //check if the player is ahead based on current dir.
        bool isAhead = Physics2D.Raycast(transform.position, currentDir, 0.5f, LayerMask.GetMask("Player"));


        return isAhead;
    }
    Vector2 GetDir(Vector3 targetTile)
    {
        Vector3 dir = (targetTile - transform.position).normalized;
        return dir;
    }

    public float GetTotalItemCost()
    {
        float baseCost = 0;

        foreach (var item in handItemList)
        {
            ItemDataPotion potion = item.data.GetPotion();

            if(potion == null)
            {
                Debug.LogError("SOMETHING WRONG. NPC GOT A NONPOTION " + item.data.itemName);
                break;
            }

            baseCost += item.quantity * potion.basePrice;
        }
        
        

        return baseCost * priceModifier;
    }


    #endregion

    #region HAND
    //char can have many differnt item later on the game.
    //

    List<ItemClass> handItemList = new();
    void AddItemToHand(ItemClass item)
    {
        //from where i get the template?

        for (int i = 0; i < item.quantity; i++)
        {
            ItemHandUnit newObject = GameHandler.instance.CreateItemHandUnit();          
            newObject.SetItem(item);
            newObject.transform.parent = handContainer.transform;
            newObject.SetSortingOrder(25 - handItemList.Count);
            newObject.transform.localPosition = GetPos(handItemList.Count);
            handItemList.Add(new ItemClass(item.data, 1));
        }
         
        
    }
    Vector2 GetPos(int index)
    {
        float x = index % 2 == 0 ? 0 : -0.1f;
        float y = (index * 0.15f) + 0.15f;

        Debug.Log("valus " + x + " " + y);
        return new Vector2(x, y);
    }
    #endregion


    #region IINVENTORY
    public void IReceiveItem(ItemClass item)
    {
        //when this receivees item it ddcides to go into selling machine.
        AddItemToHand(item);
        OrderGoBuyCurrentItem();
    }

    public void IReceiveItemList()
    {
        throw new System.NotImplementedException();
    }

    

    #endregion


}

public enum ClientWealthType
{
    Miserable,
    Poor,
    Struggling,
    Stable,
    Well_Off,
    Rich,
    Mega_Rich,
    Godly
}




//types of wealth: miserable, poor, struggling, stable, well off, rich, mega rich, godly. 