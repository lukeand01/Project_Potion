using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreObject : MonoBehaviour
{
    //this exists to organize all store objects: like itemholdeers or planters.
    //and we can recivee information 

    public void SetUp()
    {

    }

    public virtual ItemHolder GetItemHolder() => null;

    public virtual SellingObject GetSellingObject() => null;
}
