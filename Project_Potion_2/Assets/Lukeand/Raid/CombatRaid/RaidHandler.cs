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

    //REF FROM PLAYER
    public RaidStageData currentStageData { get; private set; }
    public List<ChampClass> champList = new();
    public List<ItemClass> chestList = new();


    //STOREEDE HERE
    public List<ChampClass> changedChampList { get; private set; } = new();
    public List<ItemClass> gainedItemList { get; private set; } = new();


    [Separator("DEBUGGING STUFF")]
    [SerializeField] RaidStageData debugStageData;
    [SerializeField] List<ChampData> debugDataChampList = new();

    private void Awake()
    {
        handler = GetComponent<GameHandler>();  

        if(debugStageData != null)
        {
            currentStageData = debugStageData;
        }

        foreach (var item in debugDataChampList)
        {
            champList.Add(new ChampClass(item));
        }
        
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
        //2 - then we do the ui process here
        //3 - then we send the information to the player

        //xp is based in stage level and perfomance.

        RaidScoreType raidScore = RaidLocalHandler.instance.GetRaidScore();
      
        float totalExperienceGained = (currentStageData.experienceForCompletion * (float)raidScore) / 100;

        List<ChampClass> copyList = new();

        foreach (var item in champList)
        {
            copyList.Add(new ChampClass(item));
        }

        

        UIHolder.instance.raidEnd.StartVictory(copyList, totalExperienceGained, raidScore);

        champList[0].GainExperience(totalExperienceGained);




        for (int i = 1; i < champList.Count; i++)
        {
            champList[i].GainExperience(totalExperienceGained * 0.6f);
        }
        
    }
    public void LostRaid()
    {
        //the same thing as victory but there will be less.

        RaidScoreType raidScore = RaidScoreType.D;
        float totalExperienceGained = (currentStageData.experienceForCompletion * (float)raidScore) / 100;

        List<ChampClass> copyList = new();

        foreach (var item in champList)
        {
            copyList.Add(new ChampClass(item));
        }


        UIHolder.instance.raidEnd.StartDefeat(copyList, totalExperienceGained, raidScore);

        champList[0].GainExperience(totalExperienceGained);
        for (int i = 1; i < champList.Count; i++)
        {
            champList[i].GainExperience(totalExperienceGained * 0.6f);
        }
    }


    public void ReceiveNewItens(List<ItemClass> newItemList)
    {
        //this is chosen when the character clicks the button.
        //it first gives the itens then passes the new itens.

    }

    public void EndRaid()
    {
        //this is callled to load back the other scene
        //
        //bnow we need to tell if we won or not.
        GameHandler.instance.loader.LoadScene(0);
    }
    public void NextRaid()
    {
        GameHandler.instance.loader.LoadScene(currentStageData.stageID + 1);
    }

    public void ReplayRaid()
    {
        GameHandler.instance.loader.LoadScene(currentStageData.stageID);
    }

    public void GiveDataFromRaidToPlayer()
    {
        //wee check what we have stored.
    }

    


    //you get a score for your pérformancec in teh raid
    //S, A, B, C, D
    //its based n how many enemies are killed, if all chests are

    

}

public enum RaidScoreType
{
    S = 200,
    A = 150,
    B = 120,
    C = 80,
    D = 40
}