using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CraftData : ScriptableObject
{
    //they are all equal in the sense that they produce a potion.

    [Separator("RESULT")]
    public PotionClass potionResult;


    [Separator("INGREDIENTS")]
    public List<IngredientClass> ingredientList = new();






}


[System.Serializable]
public class IngredientClass
{
    public ItemDataIngredient data;
    public int quantity = 1;
}

[System.Serializable]
public class PotionClass
{
    public ItemDataPotion data;
    public int quantity = 1;
}