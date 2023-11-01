using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorLeave : Sequence2
{
    //in this behavior the char will try to leave for the already set way out.

    NPCBase npc;
    bool hasStarted;

    public BehaviorLeave(NPCBase npc)
    {
        this.npc = npc;

    }

    public override NodeState Evaluate()
    {
        if(npc.wayOutTarget == null)
        {
            hasStarted = false;
            return NodeState.Success;
        }


        if (!hasStarted)
        {
            //start courotin towars th thing.
            npc.MoveTo(npc.wayOutTarget.position);
            hasStarted = true;
        }


        float distance = Vector3.Distance(npc.transform.position, npc.wayOutTarget.transform.position);

        if(distance <= 0.05f)
        {
            hasStarted = false;
        }



        return NodeState.Running;
    }

}
