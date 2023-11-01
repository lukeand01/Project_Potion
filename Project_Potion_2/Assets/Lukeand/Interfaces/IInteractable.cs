using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    public bool IsInteractable(PlayerInventory inventory);

    public bool IsSecondInteractable(PlayerInventory inventory);

    public void SecondInteract(PlayerInventory inventory);

    public string GetInteractName(PlayerInventory inventory, bool isSecond = false);

    public void Interact(PlayerInventory inventory);

    public void UIInteract(PlayerInventory inventory, bool isClose = false);
    

}
