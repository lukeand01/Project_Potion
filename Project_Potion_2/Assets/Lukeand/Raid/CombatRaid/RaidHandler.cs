using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Aseprite;
using UnityEngine;

public class RaidHandler : MonoBehaviour
{
    //raid is meant to be fast.
    //you can change speed from 1x to 3x
    //maybe the player should be able to control the party.
    //

    GameHandler handler;

    public PCHandler mainChampTemplate; //this has the pchandler
    public AllyCombatHandler allyChampTemplate; //this is for the allies.
    

    //we give the information of waht has been chosen
    public RaidStageData currentStageData { get; private set; }
    public List<ChampClass> champList = new();
    public List<ItemClass> chestList = new();  


    [Separator("DEBUGGING STUFF")]
    [SerializeField] RaidStageData debugStageData;
    [SerializeField] List<ChampData> debugDataChampList = new();

    private void Awake()
    {
        handler = GetComponent<GameHandler>();  
    }

    public void StartDebugRaid()
    {
        List<ChampClass> debugChampList = new();

        foreach (var champ in debugDataChampList)
        {
            debugChampList.Add(new ChampClass(champ));
        }

        StartNewRaid(debugStageData, debugChampList);

    }

    

    public void StartNewRaid(RaidStageData stageData, List<ChampClass> champList)
    {
        //load the

        if(PlayerHandler.instance != null)
        {


            PlayerHandler.instance.gameObject.SetActive(false);
        }



        currentStageData = stageData;
        this.champList = champList; 
        this.chestList = handler.playerHandler.inventory.chestList; 

        

        GameHandler.instance.loader.LoadScene(stageData.stageID);
        
    }

    public void CallLocalHandler()
    {
        RaidLocalHandler localHandler = RaidLocalHandler.instance;

        if (localHandler == null)
        {
            Debug.Log("there was a problem with it");
            return;
        }

        localHandler.GenerateRaid(champList[0], champList, currentStageData);

    }


    public void WonRaid()
    {
        //1 - send the information that will simulating it being delivery.
        //2 - then we do the actual process here
        //3 - then we send the information to the player

        //xp is based in stage level and perfomance.

        


    }
    public void LostRaid()
    {
        //the same thing as victory but there will be less.
    }



    public void EndRaid()
    {
        //this is callled.
        //bnow we need to tell if we won or not.

    }




    //you get a score for your pérformancec in teh raid
    //S, A, B, C, D
    //its based n how many enemies are killed, if all chests are

    public RaidScoreType GetRaidScore()
    {
        return RaidScoreType.S;
    }

}

public enum RaidScoreType
{
    S = 200,
    A = 150,
    B = 120,
    C = 100,
    D = 80
}