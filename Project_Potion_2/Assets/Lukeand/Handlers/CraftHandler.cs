using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftHandler : MonoBehaviour
{
    //here we will deal with logic.

    [SerializeField]List<ItemDataIngredient> allIngredientList = new();
    [SerializeField]List<CraftData> allCraftList = new();
    Dictionary<ItemDataIngredient, List<CraftData>> dictionaryCraftDividedByIngredient;

    private void Start()
    {
        return;
        foreach (var item in allIngredientList)
        {
            dictionaryCraftDividedByIngredient.Add(item, AllCraftPossibleWithIngredient(item));
        }

        Debug.Log(dictionaryCraftDividedByIngredient.Count);
    }

    List<CraftData> AllCraftPossibleWithIngredient(ItemDataIngredient data)
    {
        List<CraftData> newList = new();
        foreach (var item in allCraftList)
        {
            if (item.HasIngredient(data)) newList.Add(item);
        }
        Debug.Log("created a list " + newList.Count);
        return newList;
    }

    public List<CraftData> GetListFromIngredient(ItemDataIngredient data)
    {
        if (!dictionaryCraftDividedByIngredient.ContainsKey(data)) return null;
        return dictionaryCraftDividedByIngredient[data];
    }
    public List<CraftData> UpdateListFromIngredient(ItemDataIngredient data, List<CraftData> oldList)
    {
        List<CraftData> newList = new();

        foreach (var item in oldList)
        {
            if (item.HasIngredient(data))
            {
                newList.Add(item);
            }


        }
      
        return newList;
    }
}
