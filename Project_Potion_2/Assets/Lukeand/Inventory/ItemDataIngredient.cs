using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item / Ingredient")]
public class ItemDataIngredient : ItemData
{

    [Separator("PLANT")]
    [ConditionalField(nameof(itemType), false, ItemType.Plant)] public TimeClass timeForHarvest;


    public override ItemDataIngredient GetIngredient()
    {
        if(itemType == ItemType.Potion)
        {
            Debug.LogError("something wrong with item " + itemName);
            return null;
        }

        return this;
    }
    
}
[System.Serializable]
public class TimeClass
{
    [SerializeField] float hours;
    [SerializeField] float minutes;
    [SerializeField] float seconds;

    public double GetTotal() //this is the second.
    {
        float minutesInSeconds = minutes * 60;
        float hoursInSeconds = hours * 3600;
        
        return minutesInSeconds + hoursInSeconds + seconds;
    }


}