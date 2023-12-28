using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Raid / Stage")]
public class RaidStageData : ScriptableObject
{
    //gain money for first completing the stage.
    //gain diamond if you do it very well.
    public int stageID;

    public float experienceForCompletion;
    public int moneyForCompletion;
    public int diamondForCompletion;

    public List<ItemChanceClass> itemChanceClasses = new();
    public List<EnemyGroupClass> enemyGroupClass = new();


}
[System.Serializable]
public class EnemyGroupClass
{
    //these are enemies that will be spwaned close to each other in a general area.
    public List<EnemyData> enemyList = new();
}


[System.Serializable]
public class ItemChanceClass
{
    public ItemData data;
    [Range(0,100)]public float chance;
}