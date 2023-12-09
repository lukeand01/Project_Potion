using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    //this describes one enemy.
    //all stat values are influenced by a stage modifier

    public string enemyName;

    public float experienceBase;

    public GameObject enemyModel; //we get the prefab with the script and its behavior.

}
