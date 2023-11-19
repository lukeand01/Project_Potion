using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Raid / Stage")]
public class RaidStageData : ScriptableObject
{
    //gain money for first completing the stage.
    //gain diamond if you do it very well.

    public int moneyForCompletion;
    public int diamondForCompletion;

}
