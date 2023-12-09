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
    List<ChampClass> champList = new();
     List<ItemClass> chestList = new();  


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

        foreach (var item in debugDataChampList)
        {
            debugChampList.Add(new ChampClass(item));
        }

        StartNewRaid(debugStageData, debugChampList);

    }

    

    public void StartNewRaid(RaidStageData stageData, List<ChampClass> champList)
    {
        //load the

        if(PlayerHandler.instance != null)
        {
            Debug.Log("found something here");

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



    public void EndRaid(List<ChampClass> champList, List<ItemClass> newItens, int newGoldValue)
    {
        //this must be given to the playerhabndler only once its turned on.
        //the inventory will be completely updated. as thats not a problem
        //the champ is a bhit more complex. i will get the used champ and exchange the especifc parts based in teh champdata




        GameHandler.instance.loader.LoadScene(0);

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