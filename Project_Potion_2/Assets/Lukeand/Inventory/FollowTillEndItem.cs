using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTillEndItem : FollowTillEnd
{
    //this 

    ItemClass item;
    IInventory inventoryTarget;
    

    public void SetUp(ItemClass item, IInventory inventoryTarget,Transform target, float speed)
    {
        SetUpBase(target, speed);

        this.inventoryTarget = inventoryTarget;
        this.item = new ItemClass(item.data, item.quantity,0);

        rend.sprite = item.data.itemSprite;

        
    }

    protected override void Act()
    {
        //we also give the information to the fella.

        inventoryTarget.IReceiveItem(item);
        base.Act();
    }

}
