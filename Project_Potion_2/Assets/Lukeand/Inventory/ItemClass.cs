using PlayFab.EconomyModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemClass 
{
    public ItemData data;
    public int quantity;
    [SerializeField]public int listIndex;
    [HideInInspector] public RaidInventoryType raidinventoryType {  get; private set; } 

    public void SetRaidInventoryType(RaidInventoryType raidinventoryType)
    {
        this.raidinventoryType = raidinventoryType;
    }


    public ItemClass(ItemData data, int quantity, int listIndex)
    {
        this.data = data;
        this.quantity = quantity;
        this.listIndex = listIndex;
    }
    public ItemClass(ItemData data, int quantity)
    {
        this.data = data;
        this.quantity = quantity;
    }
    
    public void UpdateIndex(int listIndex)
    {
        this.listIndex = listIndex;
    }

    public void ReceiveNewData(ItemData data)
    {
        this.data = data;
        quantity = 1;
        UpdateUI();
    }
    public void IncreaseQuantity()
    {
        quantity += 1;
        UpdateUI();
    }
    public void DecreaseQuantity()
    {
        quantity -= 1;

        if(quantity <= 0)
        {
            EmptyData();
        }
        else
        {
            UpdateUI();
        }
        
    }

    public int GetAmountToStack()
    {
        return data.stackLimit - quantity;
    }

    public void EmptyData()
    {
        data = null;

        UpdateUI();
    }

    public void UseItemAsRef(ItemClass item)
    {
        this.data = item.data;
        this.quantity = item.quantity;
    }

    #region UI UNIT

    void UpdateUI()
    {
        if (chestUnit != null) chestUnit.UpdateUI();
        if (ingredientUnit != null)
        {
            ingredientUnit.UpdateUI();
        }
    }

    ItemHandUnit handUnit;
    public void UpdateHandUnit(ItemHandUnit handUnit)
    {
        this.handUnit = handUnit;
    }

    ChestUIUnit chestUnit;
    public void UpdateChestUnit(ChestUIUnit chestUnit)
    {
        this.chestUnit = chestUnit;
    }
    public Transform GetTransfromOfChestUnit()
    {
        return chestUnit.transform;
    }

    IngredientUnit ingredientUnit;
    public void UpdateIngredientUnit(IngredientUnit ingredientUnit)
    {
        this.ingredientUnit = ingredientUnit;
    }


    #endregion


   


}

