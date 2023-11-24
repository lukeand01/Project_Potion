using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorPickPotion : Node
{

    NPCBase npc;
    bool hasStarted;
    bool cantAct;
    Vector3 pos;
    public BehaviorPickPotion(NPCBase npc)
    {
        this.npc = npc;

    }

    public override NodeState Evaluate()
    {
        //i need to be constantly checking if the target 

        if(npc.itemHolderTarget == null)
        {
            //we first try to look for another place. if we cant then it leaves.
            hasStarted = false;
            cantAct = false;
            return NodeState.Success;
        }

        if (cantAct) return NodeState.Running;

        if (!hasStarted)
        {
            //start it.
            pos = npc.itemHolderTarget.GetUsePos(npc.transform);
            npc.MoveTo(pos);
            hasStarted = true;
        }

        //if at any time

        if (!npc.itemHolderTarget.HasItem())
        {
            //then we try to find another place.
            if (!npc.OrderHasItemToChoose())
            {
                Debug.Log("no longer any choice");
                npc.OrderLeave();
                return NodeState.Failure;
            }
            else
            {
                Debug.Log("npc has another place to go.");
            }

        }
        else
        {
            Debug.Log("itemholder has item");
        }


        float distance = Vector3.Distance(npc.transform.position, pos);

        if (distance <= 0.1f)
        {

            npc.OrderGetItem();
            cantAct = true;
            hasStarted = false;
        }

        //if it arrives by the itemholder. then it grabs.
        return NodeState.Running;
    }



}
