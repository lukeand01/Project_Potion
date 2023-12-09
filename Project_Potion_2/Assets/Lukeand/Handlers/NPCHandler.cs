using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHandler : MonoBehaviour
{
    //only two important factors: taste and money.
    //each taste has a level of reputation. which unlocks richers clients.

    //the sprites and the animation need to come from this place.
    //level of wealth: from 1 to 5 as modifier.
    //types of wealth: miserable, poor, struggling, stable, well off, rich, mega rich, godly. 

    [Separator("DEBUG")]
    public bool startWithSpawn;

    [Separator("TEMPLATE")]
    [SerializeField] NPCBase npcTemplate;

    [Separator("TRANSFORMS")]
    [SerializeField] Transform[] wayRight;
    [SerializeField] Transform[] wayLeft;

    private void Start()
    {
       if(startWithSpawn) SpawnNPC();
    }

    [ContextMenu("Spawn NPC")]
    public void SpawnNPC()
    {
        Transform[] WayInAndOut = GetWayInAndOut();
        NPCBase newObject = Instantiate(npcTemplate, WayInAndOut[0].position, Quaternion.identity);
        newObject.SetUp(PotionType.Combat);

        //then we decide if its just walking or if it goes to the store.
        //to go to the store it neeeds to have an item it wants.
        //
       

        
        bool found = newObject.OrderHasItemToChoose();



        Debug.Log("found ? " + found);
        if (!found)
        {
            newObject.SetWayOut(WayInAndOut[1], true);
        }
        


        

    }

    Transform[] GetWayInAndOut()
    {
        int firstRandom = Random.Range(0, 1);
        int secondRandom = Random.Range(0, wayLeft.Length - 1);

        Transform right = wayRight[secondRandom];
        Transform left = wayLeft[secondRandom];

        if (firstRandom == 0)
        {
            Transform[] result = { right, left };
            return result;

        }
        else
        {
            Transform[] result = { left, right};
            return result;
        }

    }

    public Transform GetRandomWayOut()
    {
        Transform[] wayInAndOut = GetWayInAndOut();

        return wayInAndOut[Random.Range(0, 2)];

    }
}

//the logic for the npc.
//we choose a potion type. if there is any avaible potion the potion type will only be one available
//then we choose the wealth modifier. that is based in reputation. the higher the higher the chancec of the wealth 
//the cchoice itself of the potion is random only based in potion type.
//
