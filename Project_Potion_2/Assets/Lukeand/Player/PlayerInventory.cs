using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour, IInventory
{

    [Separator("DEBUG")]
    [SerializeField] List<ItemData> initialItemList = new();

    int pickaxeLevel;
   
    List<ItemClass> equipList = new();

    [SerializeField] List<ItemClass> handList = new();
    [SerializeField] List<ItemHandUnit> handUnitList = new();

    [Separator("HAND REF")]
    [SerializeField] GameObject handContainer;    
    [SerializeField] int handLimit;

    [Separator("CHEST")]
    [SerializeField] List<ItemClass> initialChestList = new();
    [SerializeField] int chestLimit;
    [SerializeField]List<ItemClass> chestList = new();

    [Separator("RAID")]
    int raidMoney;
    List<ItemClass> raidList = new();
    

    private void Start()
    {
        //send the item to the player graphic.
        StartChest();

        foreach (var item in initialItemList)
        {
            AddItemToHand(new ItemClass(item, 1));
        }
    }

    #region CHEST - HAND 
    public bool SendItemChestToHand(ItemClass item, Transform parent)
    {
        if (item.data == null) return false;
        if (!HasSpaceInHand()) return false;


        //need to update the hand ui.
        UIHolder.instance.chest.FTEForHand(item);
        AddItemToHand(item);
        item.DecreaseQuantity();
        return true;
    }
    public bool SendItemHandToChest(ItemClass item, Transform parent)
    {
        if (item.data == null) return false;
        int index = CanChestReceiveOne(item.data);
        if (index == -1)  return false;



        //do the effeect first.
        GameHandler.instance.CreateFTEImage(item, item.GetTransfromOfChestUnit(), chestList[index].GetTransfromOfChestUnit(), parent, 100f);

        AddItemToChest(new ItemClass(item.data, 1));
        RemoveHandUnit(item.listIndex);
        item.DecreaseQuantity();
        return true;
        //need to remove it from hand.

    }
    #endregion

    #region CHEST
    public void OpenChest()
    {
        UIHolder.instance.chest.CreateHandUnits(handList);
        UIHolder.instance.chest.OpenUI();
    }

    void StartChest()
    {
        ChestUI chestUI = UIHolder.instance.chest;

        if (chestUI == null) return;

        for (int i = 0; i < chestLimit; i++)
        {
            ItemClass item = new ItemClass(null, 0, i);         
            chestList.Add(item);
        }

        chestUI.SetUpChestUnits(chestList);

        foreach (var item in initialChestList)
        {
            AddItemToChest(item);
        }
    }
    

    public int AddItemToChest(ItemClass item)
    {
        //we only stop checking when there is no more space
        //then we return the amount we couldnt 

        List<ItemClass> stackableList = GetChestStackableList(item.data);

        int brakeFirst = 0;

        while(item.quantity > 0)
        {
            brakeFirst++;
            if(brakeFirst > 1000)
            {
                Debug.Log("break first");
                break;
            }


            if (stackableList.Count > 0)
            {
                StackItem(item, stackableList);
                continue;
            }

            int freeSlot = GetChestNextFreeSlot();

            if(freeSlot != -1)
            {
                CreateItem(freeSlot, item, stackableList);
                continue;
            }

            return item.quantity;
        }


        return 0;
    }


    void StackItem(ItemClass item, List<ItemClass> stackableList)
    {
        int diff = stackableList[0].GetAmountToStack();
        float brake = 0;

        while (item.quantity > 0 && diff > 0)
        {
            brake++;
            if(brake > 1000)
            {
                Debug.Log("broke in stackable");
                break;
            }

            stackableList[0].IncreaseQuantity();
            diff = stackableList[0].GetAmountToStack();
            item.DecreaseQuantity();


            if(diff == 0)
            {
                stackableList.RemoveAt(0);
            }
        }
    }
    void CreateItem(int listIndex, ItemClass item, List<ItemClass> stackableList)
    {
        chestList[listIndex].ReceiveNewData(item.data);
        item.DecreaseQuantity();
        if(item.quantity > 0) stackableList.Add(chestList[listIndex]);
    }

    

    
    //first we check if there is space to stack.
    //then we check the next free slot.

    List<ItemClass> GetChestStackableList(ItemData data)
    {
        List<ItemClass> newList = new();

        for (int i = 0; i < chestList.Count; i++)
        {
            if (chestList[i].data == data && chestList[i].quantity < data.stackLimit) newList.Add(chestList[i]);
        }

        return newList;
    }
    int GetChestNextStackable(ItemData data)
    {
        for (int i = 0; i < chestList.Count; i++)
        {
            if (chestList[i].data == data && chestList[i].quantity < data.stackLimit) return i;          
        }

        return -1;
    }
    int GetChestNextFreeSlot()
    {
        for (int i = 0; i < chestList.Count; i++)
        {
            if (chestList[i].data == null) return i;
        }
        return -1;

    }

    int CanChestReceiveOne(ItemData data)
    {
        int stackIndex = GetChestNextStackable(data);

        if (stackIndex != -1) return stackIndex;

        int freeIndex = GetChestNextFreeSlot();
        if (freeIndex != -1) return freeIndex;

        return -1;
    }


    #endregion

    #region HAND    

    public bool AddItemToHand(ItemClass item)
    {
        //every thing has only 1 of quantity.

        if(!HasSpaceInHand())
        {
            Debug.Log("cannot carry anymore");
            return false;
        }

        ItemClass newItem = new(item.data, 1, handList.Count);

        handList.Add(newItem);
        CreateHandUnit(item);

        UIHolder.instance.chest.CreateHandUnits(handList);
        UIHolder.instance.production.UpdateHandList(handList);

        return true;
    }
    
    void CreateHandUnit(ItemClass item)
    {
        ItemHandUnit newObject = GameHandler.instance.CreateItemHandUnit();                       
        newObject.SetItem(item);
        newObject.transform.parent = handContainer.transform;
        handUnitList.Add(newObject);
        UpdateHand();
    }
    public void RemoveHandUnit(int index)
    {

        handList.RemoveAt(index);
        Destroy(handUnitList[index].gameObject);
        handUnitList.RemoveAt(index);
        
        UpdateHand();
    }

    void UpdateHand()
    {

        for (int i = 0; i < handList.Count; i++)
        {
            handList[i].UpdateIndex(i);
        }

        for (int i = 0; i < handUnitList.Count; i++)
        {
            handUnitList[i].SetSortingOrder(25 - i);           
            handContainer.transform.GetChild(i).transform.localPosition = GetPos(i );
        }
    }

    public bool HasSpaceInHand()
    {
        return handList.Count + iinventoryComingList.Count < handLimit;
    }
   
    Vector2 GetPos(int index)
    {
        float y = (index * 0.15f) + 0.15f;
        float x = index % 2 == 0 ? 0 : -0.1f;

        return new Vector2(x, y);
    }

    public ItemClass GetItemInHand(int index)
    {
        if (handList.Count <= index) return null;       
        return handList[index];
    }

    public void SendHandToTarget(Transform target, int index)
    {        
        GameHandler.instance.CreateFTEItem(handList[index], transform, target, Time.deltaTime * 50);
        RemoveHandUnit(index);
    }

    public int GetNextEspecificItem(ItemType type)
    {
        //start fromt hee top and go down.

        if (handList.Count == 0) return -1;

        for (int i = handList.Count; i > -1; i--)
        {
            if (handList[i - 1].data.itemType == type)
            {
                return i - 1;
            }
        }

        return -1;
    }

    public int GetSameItemInHand(ItemData data)
    {

        if(data == null)
        {
            return -1;
        }

        for (int i = 0; i < handList.Count; i++)
        {

            if (handList[i] == null) Debug.Log("this was null");

            if (handList[i].data.name == data.name)
            {
                return i;
            }
        }

        return -1;
    }

    //i want to get the first item in hand that belong to a craftrecipe.

    #endregion

    #region RAID

    public void AddRaidMoney(int value)
    {
        raidMoney += value;
    }
    public void ReduceRaidMoney(int value)
    {
        raidMoney -= value;
    }

    public void AddRaidItem(ItemClass item)
    {


    }


    #endregion

    //make effect for the item going from end to receiver.
    #region IINVENTORY
    List<ItemClass> iinventoryComingList = new();

    public bool ICanReceive(ItemClass item)
    {
        //if can receive 
        if (HasSpaceInHand())
        {
            iinventoryComingList.Add(item);
            return true;
        }
        else
        {
            return false;
        }


    }
    public IInventory GetIInventory()
    {
        return this;
    }
    public void IReceiveItem(ItemClass item)
    {
        Debug.Log("receive item");
        iinventoryComingList.RemoveAt(0);
        AddItemToHand(item);
    }

    public void IReceiveItemList()
    {
        
    }

    

    #endregion

}

//