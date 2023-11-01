using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    //this should just relate the things from the server.

    //this will actually just receive the info from the player.


    #region INTERACT
    public string GetInteractName(PlayerInventory inventory, bool isSecond = false)
    {
        return "Open Chest";
    }

    public void Interact(PlayerInventory inventory)
    {
        //how to synch with the servere.
        inventory.OpenChest();
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
        
    }

    public void UIInteract(PlayerInventory inventory, bool isClose = false)
    {
        
    }

    #endregion
}
