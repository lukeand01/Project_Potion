using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item / Potion")]
public class ItemDataPotion : ItemData
{
    //should i use this?
    //for organiztion.

    [Separator("Potion")]
     public PotionType potionType;
     public float basePrice = 1;


    private void Awake()
    {
        itemType = ItemType.Potion;
    }


    public override ItemDataPotion GetPotion() => this;
    
}
