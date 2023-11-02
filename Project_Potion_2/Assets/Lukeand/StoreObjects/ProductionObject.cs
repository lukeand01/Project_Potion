using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionObject : StoreObject, IInteractable
{
    

    


    #region INTERACT
    public string GetInteractName(PlayerInventory inventory, bool isSecond = false)
    {
        return "Craft";
    }

    public void Interact(PlayerInventory inventory)
    {
        //get the first item that can be put in crafting.
        //when there is at least one item we form a new list of the on


    }

    public bool IsInteractable(PlayerInventory inventory)
    {
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
}
