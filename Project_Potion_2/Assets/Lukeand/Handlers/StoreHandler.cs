using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class StoreHandler : MonoBehaviour
{
    //how to place the items?

    //each place has a a money cost modifier and maybe some other cost.

    [SerializeField] List<ObjectBuildClass> itemHolderList;
    Dictionary<PotionType, List<ItemHolder>> dictionaryForItemHolderTypes = new();

    
    [SerializeField] List<ObjectBuildClass> sellingObjectList;

    private void Awake()
    {
        dictionaryForItemHolderTypes.Add(PotionType.Combat, new List<ItemHolder>());
        dictionaryForItemHolderTypes.Add(PotionType.Drink, new List<ItemHolder>());
        dictionaryForItemHolderTypes.Add(PotionType.Medicine, new List<ItemHolder>());
    }

    #region PLACING OBJECTS

    public ObjectBuildClass GetItemHolderClass()
    {
        foreach (var item in itemHolderList)
        {
            if (!item.HasObject()) return item;
        }
        return null;
    }

    public ObjectBuildClass GetSellingObjectClass()
    {
        foreach (var item in sellingObjectList)
        {
            if (!item.HasObject()) return item;
        }

        return null;
    }

    #endregion

    #region GETTING

    public ItemHolder GetItemHolder(PotionType potionType)
    {
        if (!dictionaryForItemHolderTypes.ContainsKey(potionType)) return null;
        List<ItemHolder> itemHolderList = dictionaryForItemHolderTypes[potionType];
        if (itemHolderList.Count == 0)
        {
            Debug.Log("has found no list of item for this fella.");
            return null;
        }

        //otherwisee we will get all the options and randomly select one.

        return itemHolderList[Random.Range(0, itemHolderList.Count)];

    }

    public SellingObject GetSellingObject()
    {
        //get the selling object with the least things.

        SellingObject currentSellingObject = null;

        for (int i = 0; i < sellingObjectList.Count; i++)
        {
            if (sellingObjectList[i].HasObject())
            {
                //then we check the

                SellingObject sellingObject = (SellingObject)sellingObjectList[i].storeObject;


                if (currentSellingObject == null)
                {
                    currentSellingObject = sellingObject;
                }
                else
                {
                    if(sellingObject.GetNpcTotalCount() < currentSellingObject.GetNpcTotalCount())
                    {
                        currentSellingObject = sellingObject;
                    }
                }

                if (currentSellingObject.GetNpcTotalCount() == 1) return currentSellingObject; 

            }


        }

        return currentSellingObject;
    }


    #endregion

    #region UPDATELISTS
    public void AddItemHolder(ItemHolder itemHolder, ItemDataPotion data)
    {
        if (dictionaryForItemHolderTypes.ContainsKey(data.potionType))
        {
            dictionaryForItemHolderTypes[data.potionType].Add(itemHolder);         
        }
        else
        {
            List<ItemHolder> newList = new()
            {
                itemHolder
            };
            dictionaryForItemHolderTypes.Add(data.potionType, newList);
        }
    }
    public void RemoveItemHolder(ItemHolder itemHolder, ItemDataPotion data)
    {
        if (dictionaryForItemHolderTypes.ContainsKey(data.potionType))
        {
            List<ItemHolder> holderList = dictionaryForItemHolderTypes[data.potionType];

            for (int i = 0; i < holderList.Count; i++)
            {
                if (holderList[i].GUID == itemHolder.GUID)
                {
                    Debug.Log("found the one to remove");
                    holderList.RemoveAt(i);
                    break;
                }
            }

        }
        
    }

    #endregion

    

}

[System.Serializable]
public class ObjectBuildClass
{
    public float costModifier = 1;
    public Transform buildHolder; //use this as transform and to check if its built already.
    [SerializeField] public StoreObject storeObject;

    public void SetObject(StoreObject storeObject)
    {
        this.storeObject = storeObject;
    }

    public bool HasObject() => storeObject != null;
}