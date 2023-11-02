using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftHandler : MonoBehaviour
{
    //here we will deal with logic.

    public List<ItemDataIngredient> allIngredientList;
    //we will create a dictonary from this at the start of everygame and leave stored to more easily use.

    public bool CanThisItemBeUsedInRecipe()
    {
        return false;
    }
}
