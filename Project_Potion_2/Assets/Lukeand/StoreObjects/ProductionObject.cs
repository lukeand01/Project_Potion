using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionObject : StoreObject, IInteractable
{
    //this is more flexible. it takes anything that it is an ingredient.
    //then it needs to check all possible fella to determine who it can produce.

    //the list is taken from where?

    //as itens are added a list is made of all things that can be.

    //it appears an ui by the side of all itens in hand.
    //


    //seecond interact is grabbing whats inside.


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
