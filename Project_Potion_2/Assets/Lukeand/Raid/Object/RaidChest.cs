using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidChest : MonoBehaviour, IRaidInteractable
{
    //

    List<ItemClass> itemList = new();

    public void Interact(PCHandler handler)
    {
        //shoot the itens up and give to the player

        //we throw everyone in the list and update the inventory.

        foreach (var item in itemList)
        {
            PCHandler.instance.inventory.AddRaidItem(item);
        }

        



        gameObject.layer = (int)LayerMaskEnum.Default;
        Destroy(this); //destroy the chest script so it cannot be interacted anymore.

    }
}
