using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
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
        return item.quantity + iinventoryComingList.Count < limitStorage;
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
        if (ICanReceive(item))
        {
            inventory.SendHandToTarget(transform, itemIndex);
        }
        else
        {
            Debug.Log("couldnt send it");
        }
        
    }

    
    public void GiveItemNPC(GameObject npc)
    {
        IInventory inventory = npc.GetComponent<IInventory>();

        if(inventory == null)
        {
            Debug.Log("null");
            return;
        }
        GameHandler.instance.CreateFTEItem(new ItemClass(item.data, 1), transform, npc.transform, 1f);
        DestroyUnit();
    }
    void GiveItemToInventory(PlayerInventory inventory)
    {
        if(inventory.ICanReceive(item))
        {
            GameHandler.instance.CreateFTEItem(new ItemClass(item.data, 1), transform, inventory.transform, 5);
            DestroyUnit();
        }
        else
        {
        }
        
    }
    #region INTERACT
    public string GetInteractName(PlayerInventory inventory, bool isSecond = false)
    {
        bool give = PlayerCanGive(inventory);
        bool take = PlayerCanTake(inventory);
        bool both = give && take;

        if (isSecond)
        {
            if (both) return "Grab Potion";
        }
        else
        {
            if (give)
            {
                return "Place Potion";
            }

            if (take)
            {
                return "Take Potion";
            }
        }

        if (both)
        {
            return "Grab Potion";
        }

        

        return "";

         
        

        
    }

    public void Interact(PlayerInventory inventory)
    {
        if(inventory == null)
        {
            //this means its an npc.

        }
        else
        {
            //if its not an npc that we do thing differntly.
            bool give = PlayerCanGive(inventory);
            bool take = PlayerCanTake(inventory);


            if (give)
            {
                //give always first.
                //
                int index = 0;
                if(item.data == null)
                {
                    index = inventory.GetNextEspecificItem(ItemType.Potion);
                }
                else
                {
                    index = inventory.GetSameItemInHand(item.data);
                }
                
                if(index == -1)
                {
                    Debug.Log("something wrong");
                }

                OrderInventoryToSendItem(inventory, index);
                return;
            }

            if (take)
            {
                //then take.
                GiveItemToInventory(inventory);
                return;
            }
      
        }

    }

    public bool IsInteractable(PlayerInventory inventory)
    {
        bool give = PlayerCanGive(inventory);
        bool take = PlayerCanTake(inventory);
        bool both = give && take;

        //

        if (give)
        {
            return true;
        }

        if (take)
        {
            return true;
        }

        return false;

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

        bool both = PlayerCanGive(inventory) && PlayerCanTake(inventory);

        return both;

        
    }

    public void SecondInteract(PlayerInventory inventory)
    {
        //grab it.
        GiveItemToInventory(inventory);
    }
    public void UIInteract(PlayerInventory inventory, bool isClose = false)
    {
        
    }

    #endregion

    bool PlayerCanTake(PlayerInventory inventory)
    {
        //player can only take if there is something to take.
        if (!HasItem()) return false;
        if (!inventory.HasSpaceInHand()) return false;


        return true;
    }
    bool PlayerCanGive(PlayerInventory inventory)
    {
        //to give the player must:
        //have an item
        //have the right item

        //you can only place it if you have potion.
        if (inventory.GetNextEspecificItem(ItemType.Potion) == -1) return false;


        if (!HasItem())
        {
            return true;
        }

        //but if it has iteme thee player needs to have the same itme.
        if(HasItem() && HasSpace() && inventory.GetSameItemInHand(item.data) != -1)
        {
            return true;
        }


        return false;
    }





    #region IINVENTORY  

    List<ItemClass> iinventoryComingList = new();
    public bool ICanReceive(ItemClass item)
    {

        //in here we will make a list of things coming.
        if (HasSpace())
        {
            iinventoryComingList.Add(item);
            return true;
        }
        return false;
    }
    public void IReceiveItem(ItemClass item)
    {
        iinventoryComingList.RemoveAt(0);
        ReceiveItem(item);
    }

    public void IReceiveItemList()
    {
        
    }

    #endregion

    public override ItemHolder GetItemHolder() => this;

    
}
