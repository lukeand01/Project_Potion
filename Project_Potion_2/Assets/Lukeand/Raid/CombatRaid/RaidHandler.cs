using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidHandler : MonoBehaviour
{
    //raid is meant to be fast.
    //you can change speed from 1x to 3x
    //maybe the player should be able to control the party.
    //

    public static RaidHandler instance;

    [SerializeField] GameObject mainChamp;
    [SerializeField] GameObject champTemplate;
    [SerializeField] Transform initialSpot;

    private void Awake()
    {
        instance = this;
    }

    public void StartNewRaid(RaidStageData stageData, List<ChampClass> champList)
    {
        //we give this information after loading the scene.
        //the first in the list is the main.


    }



}
