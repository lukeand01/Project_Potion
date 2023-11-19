using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Raid / World")]
public class RaidWorldData : ScriptableObject
{
    //
    public List<RaidStageData> stageList = new();
    public int numberOfFollowers = 2; //this represents only the additional followers.


}
