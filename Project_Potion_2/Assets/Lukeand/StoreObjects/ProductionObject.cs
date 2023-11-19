using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProductionObject : StoreObject, IInteractable, IInventory
{

    //always at most three itens.

    [SerializeField] int slotsAmount = 3;

    bool isDone;
    CraftData currentCraft;
    List<ItemClass> slotList = new();
    List<CraftData> currentPossibleCraftList = new();
    float totalDiff;
    float currentDiff;

    DateTime timeWhenComplete;

    private void Start()
    {
        slotList.Clear();
        for (int i = 0; i < slotsAmount; i++)
        {
            ItemClass item = new ItemClass(null, 0);
            item.UpdateIndex(i);
            slotList.Add(item);
        }

        //i need to get a ref to all possible fellas.



    }


    #region INTERACT
    public string GetInteractName(PlayerInventory inventory, bool isSecond = false)
    {
        if (currentCraft != null && isDone) return "Get Item";
        return "Open Production";

    }

    public void Interact(PlayerInventory inventory)
    {
        //when interact it checks some things.
        //first if we have a ready craft.

        if(currentCraft != null )
        {
            Debug.Log("this stuff");

            if (isDone)
            {

            }
            else
            {

            }

        }
        else
        {
            UIHolder.instance.production.StartUI(this);
        }    

    }

    public bool IsInteractable(PlayerInventory inventory)
    {
        if (currentCraft != null && !isDone) return false;

        return true;
    }

    public bool IsSecondInteractable(PlayerInventory inventory)
    {
        return false;
    }

    public void SecondInteract(PlayerInventory inventory)
    {
        //in second interact you choose 
    }

    public void UIInteract(PlayerInventory inventory, bool isClose = false)
    {
       
    }
    #endregion

    //it always open the thing for choosing.
    public List<ItemClass> GetSlotList() => slotList;

    public void RemoveIngredient(int index)
    {
        slotList[index].EmptyData();
    }
    public void AddIngredient(ItemClass item, int index)
    {
        slotList[index].ReceiveNewData(item.data);
    }

    public bool CanAddIngredient(ItemDataIngredient data)
    {
        if (currentPossibleCraftList.Count <= 0) return true;

        foreach (var item in currentPossibleCraftList)
        {
            if (item.HasIngredient(data)) return true;
        }

        return false;
    }

    #region START AND END
    void StartTimer()
    {
        timeWhenComplete = DateTime.UtcNow.AddSeconds(currentCraft.timeToComplete.GetTotal());
        totalDiff = (timeWhenComplete - DateTime.UtcNow).Seconds;
    }
    void StartCrafting()
    {
        isDone = false;
        StartTimer();
    }
    void DoneCrafting()
    {
        isDone = true;

        slotList.Clear();
        currentPossibleCraftList.Clear();
    }


    #endregion


    #region IINVENTORY

    public bool ICanReceive(ItemClass item)
    {
        return true;
    }

    public void IReceiveItem(ItemClass item)
    {
        //then it receivees through here.
        //now we updated the list.
        ItemDataIngredient ingredientData = item.data.GetIngredient();

        if(ingredientData == null)
        {
            Debug.Log("no ingredient here for some reason");
            return;
        }

        if(currentPossibleCraftList.Count > 0)
        {
            GameHandler.instance.craft.UpdateListFromIngredient(ingredientData, currentPossibleCraftList);
        }
        else
        {
            currentPossibleCraftList = GameHandler.instance.craft.GetListFromIngredient(item.data.GetIngredient());
        }

        

    }

    public void IReceiveItemList()
    {
        
    }

    #endregion

    public int GetFreeSpace()
    {
        for (int i = 0; i < slotList.Count; i++)
        {
            if (slotList[i].data == null) return i;
        }
        return -1;
    }
}

//
