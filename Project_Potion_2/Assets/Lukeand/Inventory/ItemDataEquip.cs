using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item / Equip")]
public class ItemDataEquip : ItemData
{
    private void Awake()
    {
        itemType = ItemType.Equip;
    }




    public override ItemDataEquip GetEquip() => this; 
}
