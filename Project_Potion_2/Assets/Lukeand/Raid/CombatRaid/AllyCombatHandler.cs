using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyCombatHandler : MonoBehaviour
{
    //

    public ChampClass champ {  get; private set; }
    public void SetUp(ChampClass champ)
    {
        this.champ = champ;
    }



}
