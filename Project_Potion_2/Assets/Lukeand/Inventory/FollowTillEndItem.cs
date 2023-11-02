using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTillEndItem : FollowTillEnd
{
    //this 

    ItemClass item;
    IInventory inventoryTarget;
    

    public void SetUp(ItemClass item, GameObject target, float speed)
    {
        SetUpBase(target.transform, speed);


        inventoryTarget = target.GetComponent<IInventory>();
        this.item = new ItemClass(item.data, item.quantity,0);

        rend.sprite = item.data.itemSprite;

        
    }

    protected override void Act()
    {
        //we also give the information to the fella.

       if(inventoryTarget != null) inventoryTarget.IReceiveItem(item);
        base.Act();
    }

}
