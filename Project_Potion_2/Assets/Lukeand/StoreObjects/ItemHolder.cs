using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Unity.VisualScripting.Member;
using static UnityEngine.UI.CanvasScaler;

public class ItemHolder : StoreObject, IInteractable, IInventory
{
    //
    [SerializeField] ItemClass testItem;
    [SerializeField] ItemHandUnit handUnitTemplate;
    [SerializeField] int limitStorage = 5;
    [SerializeField] Transform initalItemPos;
    [SerializeField] Transform container;


    [SerializeField] ItemClass item;
    List<ItemHandUnit> handUnitList = new();

    [SerializeField] ItemType itemType;

    //any item class has always 1 only in chest it has more.
    //here it can also use quantity. but those items are always single when interacting with player.
    //we will stantiate iteem. and stack them in the table.


    //now its important the updating
    //
    public string GUID { get; private set; }




    private void Awake()
    {
        GUID = Guid.NewGuid().ToString();

        item = new ItemClass(testItem.data ? testItem.data : null, testItem.data ? testItem.quantity : 0);
        itemType = ItemType.Potion;

              
        if (HasItem())
        {
            for (int i = 0; i < item.quantity; i++)
            {
                //so we create the fellas.
                CreateUnit(GetPos(i), GetSortingLayer(i));
            }
        }
     
    }

    private void Start()
    {
        if (HasItem())
        {
            GameHandler.instance.store.AddItemHolder(this, item.data.GetPotion());
        }
    }

    Vector3 GetPos(int index)
    {
        Vector3 offset = Vector2.zero;

        if (index == 1) offset = new Vector2(0.2f, -0.1f);
        if (index == 2) offset = new Vector2(-0.2f, -0.1f);
        if (index == 3) offset = new Vector2(-0.2f, 0.1f);
        if (index == 4) offset = new Vector2(0.2f, 0.1f);
        if (index > 4) Debug.LogError("someething wrong when placing itens");

        return initalItemPos.position + offset;
    }

    int GetSortingLayer(int index)
    {
        if(index > 2)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }


    void DestroyUnit()
    {
        int index = handUnitList.Count - 1;
        Destroy(handUnitList[index].gameObject);
        handUnitList.RemoveAt(index);

        if(handUnitList.Count == 0)
        {
            GameHandler.instance.store.RemoveItemHolder(this, item.data.GetPotion());
            item.EmptyData();
        }

    }
    void CreateUnit(Vector3 pos, int sortingLayer)
    {
        ItemHandUnit newObject = Instantiate(handUnitTemplate, pos, Quaternion.identity);
        newObject.SetUp(item, sortingLayer);
        newObject.transform.parent = container;
        handUnitList.Add(newObject);
    }

   

    protected virtual void HandUnitStackBehavior()
    {

    }

    void ReceiveItem(ItemClass item)
    {
        //then we make the ahnd appea.r

        if(item.data.GetPotion() == null)
        {
            Debug.Log("this is not potion for some reason");
            return;
        }


        if (!HasItem())
        {
            this.item = new ItemClass(item.data, 1);
            GameHandler.instance.store.AddItemHolder(this, item.data.GetPotion());
            //also we inform 
        }
        

        int currentIndex = handUnitList.Count;
        CreateUnit(GetPos(currentIndex), 25 - currentIndex);
    }
    

    #region UTILS
    public bool HasItem()
    {
        if (item == null) return false;
        if (item.data == null) return false;
        if (item.quantity <= 0) return false;
        
        return true;
    }

    bool HasSpace()
    {
        return item.quantity < limitStorage;
    }

    //this is not htat goo but it will work for now.
    public Vector2 GetUsePos(Transform user)
    {
        //
        List<float> distanceList = GetDistanceToAllUsePoints(user);

        int index = 0;
        float current = 0;
        current = distanceList[0];

        for (int i = 0; i < distanceList.Count; i++)
        {
            if (current > distanceList[i])
            {
                current = distanceList[i];
                index = i;
            }
        }

        if (index == 0) return transform.position + Vector3.down;
        if (index == 1) return transform.position + Vector3.up;
        if (index == 2) return transform.position + Vector3.left;
        if (index == 3) return transform.position + Vector3.right;

        Debug.Log("yo");
        return Vector2.zero;
    }

    


    List<float> GetDistanceToAllUsePoints(Transform user)
    {
        List<float> newList = new();

        newList.Add(Vector2.Distance(transform.position + Vector3.down, user.position));
        newList.Add(Vector2.Distance(transform.position + Vector3.up, user.position));
        newList.Add(Vector2.Distance(transform.position + Vector3.left, user.position));
        newList.Add(Vector2.Distance(transform.position + Vector3.right, user.position));

        return newList;
    }


    #endregion
    void OrderInventoryToSendItem(PlayerInventory inventory, int itemIndex)
    {
        //we send from the inventory to this fella.
        inventory.SendHandToTarget(this, transform, itemIndex);
    }

    
    public void GiveItemNPC(GameObject npc)
    {
        IInventory inventory = npc.GetComponent<IInventory>();

        if(inventory == null)
        {
            Debug.Log("null");
            return;
        }
        GameHandler.instance.CreateFTEItem(new ItemClass(item.data, 1), inventory, transform, npc.transform, 1f);
        DestroyUnit();
    }
    void GiveItem(PlayerInventory inventory)
    {
        GameHandler.instance.CreateFTEItem(new ItemClass(item.data, 1), inventory.GetIInventory(), transform, inventory.transform, 5);
        DestroyUnit();
    }
    #region INTERACT
    public string GetInteractName(PlayerInventory inventory, bool isSecond = false)
    {
        if(isSecond && IsSecondInteractable(inventory))
        {
            return "Grab item";
        }


        if (HasItem())
        {
            if (!inventory.HasSpaceInHand()) return "No Space in hand";
            if (inventory.GetSameItemInHand(item.data) > -1 && HasSpace())
            {
                //then we can interact because we can send an additional fella into it.
                return "Place Item";
            }

            return "Grab item";
        }
        else
        {
            return "Place item";

        }     
        

        
    }

    public void Interact(PlayerInventory inventory)
    {
        if(inventory == null)
        {
            //this means its an npc.

        }
        else
        {
            if (HasItem())
            {
                int index = inventory.GetSameItemInHand(item.data);

                
                if(index == -1)
                {
                    //this means that we dont have more of the same itens. so we ejust grab it.
                    GiveItem(inventory);
                }
                else
                {
                    //this means that we DO have more itens and so we should give them.
                    OrderInventoryToSendItem(inventory, index);
                }

            }

            if (!HasItem())
            {
                int itemIndex = inventory.GetNextEspecificItem(itemType);

                if (itemIndex == -1)
                {
                    Debug.LogError("PROBLEM");
                    return;
                }


                OrderInventoryToSendItem(inventory, itemIndex);
            }
        
        }

    }

    public bool IsInteractable(PlayerInventory inventory)
    {
        if (HasItem())
        {
            if(inventory.GetSameItemInHand(item.data) > - 1 && HasSpace())
            {
                //then we can interact because we can send an additional fella into it.
                return true;
            }

            if (inventory.HasSpaceInHand())
            {
                //then this means we have space in hands.
                return true;
            }

        }

        if (!HasItem() && inventory.GetNextEspecificItem(ItemType.Potion) != -1) return true;
        
       return false;
        
    }

    public bool IsSecondInteractable(PlayerInventory inventory)
    {
        
        if (HasItem() && inventory.HasSpaceInHand() && inventory.GetSameItemInHand(item.data) != -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SecondInteract(PlayerInventory inventory)
    {
        //grab it.
        GiveItem(inventory);
    }
    public void UIInteract(PlayerInventory inventory, bool isClose = false)
    {
        throw new NotImplementedException();
    }

    //you can only use the second button 

    //bascially two differnt modes.
    //when holder has item and when it does not have itens.

    //does have items
    //then we first check if we have the same the item in the list
    //and if there is enough space in the holder.
    //else
    //have enough 



    //does NOT have itens
    //you can interact as loong as you have at least one item.


    #endregion

    #region IINVENTORY  

    public void IReceiveItem(ItemClass item)
    {
        ReceiveItem(item);
    }

    public void IReceiveItemList()
    {
        
    }

    #endregion

    public override ItemHolder GetItemHolder() => this;

    
}
