using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemData : ScriptableObject
{
    public string itemName;
    [TextArea]public string itemDescription;
    public Sprite itemSprite;
    [HideInInspector]public int stackLimit; //you can infinetely stack in the chest now
    public ItemType itemType;

    private void Awake()
    {
        stackLimit = 99999;
    }

    public virtual ItemDataIngredient GetIngredient() => null;

    public virtual ItemDataPotion GetPotion() => null;

    public virtual ItemDataEquip GetEquip() => null;
}

public enum ItemType
{
    Plant,
    Potion,
    Ingredient,
    Mineral,
    Equip
}

public enum PotionType
{
    Combat,
    Drink,
    Medicine
}