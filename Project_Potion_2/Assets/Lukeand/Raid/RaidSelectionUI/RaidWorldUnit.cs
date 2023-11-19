using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaidWorldUnit : ButtonBase
{
    RaidWorldData data;
    RaidUI uiHandler;
    public void SetUp(RaidUI uiHandler, RaidWorldData data)
    {
        this.data = data;
        this.uiHandler = uiHandler;

    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        uiHandler.SelectWorld(data);
    }
}

//important things i need in raid 
//there are the world.
//every world has stages.
//when a stage is selected it goes to the champ selection.


//what is the difference betweeen each stages.
//there are few stages. each stage is a map.
//there should from 3 to 5 stages per world.
//the stages are bigger.
//the difference is the challenge.
//how to make it easier? 
//so basically each world is one or two maps.
//each stage is a mission.
//i need the stages always to be a straightforward process. just walking forward.
//
//we reource spawn places and enemy spawn places.
//