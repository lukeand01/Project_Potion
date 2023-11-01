using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTakeItemToBuy : Node
{
    NPCBase npc;
    bool hasStarted;

    public BehaviorTakeItemToBuy(NPCBase npc)
    {
        this.npc = npc;

    }


    //we need to go to the list.

    public override NodeState Evaluate()
    {
        if(npc.sellingObjectTarget == null)
        {
            return NodeState.Success;
        }


        if (!hasStarted)
        {
            npc.sellingObjectTarget.AddNpcToMovingList(npc);
            npc.MoveTo(npc.sellingObjectTarget.GetCurrentFreePos());
            hasStarted = true;
        }

        
        if (npc.HasArrivedToTarget(npc.transform))
        {
            hasStarted = false;
            npc.sellingObjectTarget.AddNpcToArrivedList(npc);
            npc.sellingObjectTarget = null;
        }


        return NodeState.Running;
    }


}
