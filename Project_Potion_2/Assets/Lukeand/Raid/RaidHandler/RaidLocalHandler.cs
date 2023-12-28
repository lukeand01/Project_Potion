using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaidLocalHandler : MonoBehaviour
{
    //this will be out locally just to hold some variables.

    public static RaidLocalHandler instance;
    RaidStageData currentStageData;

    public bool isReady {  get; private set; }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        //currentStageData = GameHandler.currentStageData;
    }

    //chest spots
    //group spots
    [SerializeField] Transform playerPos;
    [SerializeField] Transform[] chestSpots;
    [SerializeField] Transform[] enemyGroupSpots;
    

    public void GenerateRaid(ChampClass mainChamp, List<ChampClass> allyList, RaidStageData stageData)
    {
        //1 - spawn the player. it needs the template for the info and the info itself.
        //2 - spawn the allies.
        //3 - spawn the enemies.
        //4 - spawn the chest.

        RaidHandler raid = GameHandler.instance.raid;

        if (raid == null)
        {
            Debug.Log("raid null");
            return;
        }


        PCHandler newChamp = Instantiate(raid.mainChampTemplate, playerPos.position, Quaternion.identity);
        newChamp.SetUp(mainChamp);


        for (int i = 1; i < allyList.Count; i++)
        {
            //it starts at 1 because 0 is always the main
        }

        List<EnemyGroupClass> enemyGroupList = stageData.enemyGroupClass;
        for (int i = 0; i < enemyGroupList.Count; i++)
        {
            if(i > enemyGroupSpots.Length )
            {
                Debug.Log("cannot use the enemy group spot");
                return;
            }

            List<EnemyData> enemyList = enemyGroupList[i].enemyList;
            for (int y = 0; y < enemyList.Count; y++)
            {
                //spawn this fella scale it in relation to the stage level.

            }

        }

        foreach (var item in stageData.itemChanceClasses)
        {
            //we check how many chests there should be level.
        }


    }


    //this will be responsivel for grading the score as well.


    public void DebugWin()
    {
        GameHandler.instance.raid.WonRaid();
    }

    public RaidScoreType GetRaidScore()
    {
        return RaidScoreType.S;
    }
}

