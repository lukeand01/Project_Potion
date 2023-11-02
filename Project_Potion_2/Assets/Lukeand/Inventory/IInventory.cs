using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventory 
{

    public bool ICanReceive(ItemClass item);
    public void IReceiveItem(ItemClass item);

    public void IReceiveItemList();
}
