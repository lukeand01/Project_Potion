using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityActiveData : AbilityData
{
    //we will create each ability. fuck it. as long as it works and its not stupidly hard to deal with.
    //projectil will be one typ.
    //we might want to know if we can act.

    public virtual void Act(AbilityClass ability)
    {

    }

    public virtual bool CanAct(AbilityClass ability)
    {
        return true;
    }

}
