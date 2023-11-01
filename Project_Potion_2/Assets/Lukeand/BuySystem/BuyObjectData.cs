using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BuyData / Object")]
public class BuyObjectData : BuyData
{
    public StoreObject storeObjectTemplate;
    


    public override bool Act()
    {
        if (storeObjectTemplate.GetItemHolder() != null) return HandleItemHolder();

        if (storeObjectTemplate.GetSellingObject() != null) return GetSellingObject();

        Debug.Log("found nothing");
        return false;
    }


    bool HandleItemHolder()
    {
        //first check if we can add the item. if we cannot there was a problem wtih the buy spot.
        //then we send the model to the gamehandler.
        ObjectBuildClass objectClass = GameHandler.instance.store.GetItemHolderClass();


        if(objectClass == null)
        {
            Debug.Log("found nothing :(");
            return false;
        }

        StoreObject newObject = Instantiate(storeObjectTemplate, objectClass.buildHolder.transform.position, Quaternion.identity);
        objectClass.SetObject(newObject);

        //then we check fi there is space now.
        ObjectBuildClass nextClass = GameHandler.instance.store.GetItemHolderClass();

        Debug.Log("got here");
        return nextClass != null;
    }
    
    bool GetSellingObject()
    {
        ObjectBuildClass objectClass = GameHandler.instance.store.GetSellingObjectClass();


        if (objectClass == null)
        {
            Debug.Log("found nothing :(");
            return false;
        }


        StoreObject newObject = Instantiate(storeObjectTemplate, objectClass.buildHolder.transform.position, Quaternion.identity);
        objectClass.SetObject(newObject);

        //then we check fi there is space now.
        ObjectBuildClass nextClass = GameHandler.instance.store.GetSellingObjectClass();


        return nextClass != null;



    }
}
