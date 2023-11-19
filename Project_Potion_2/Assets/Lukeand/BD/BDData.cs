using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BDData : ScriptableObject
{
    //this purpouse is to bring descriptions or graphics to conditions.
    //i will keep this in the game handler.

    public string idName;
    [TextArea]public string BDDescription;
    public GameObject graphic; //it can be just a sprite or an animator.

}
