using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuyData : ScriptableObject
{
    //this represents anything that can be built.

    public int price;


    public abstract bool Act();

}
