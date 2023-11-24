using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityActiveData : AbilityData
{
    //we will create each ability. fuck it. as long as it works and its not stupidly hard to deal with.
    //projectil will be one typ.
    //we might want to know if we can act.

    [Separator("VALUES")]
    public float baseDamage;
    public List<DamageScaleClass> damageScaleClasses = new List<DamageScaleClass>();

    public float GetTotalScaleValue(EntityStat stat)
    {
        if(stat == null)
        {
            Debug.Log("there was no entity stat here");
            return baseDamage;
        }
        float newValue = baseDamage;

        foreach (var item in damageScaleClasses)
        {
            newValue += stat.GetStatValue(item.stat) * item.scaleValue;
        }

        return newValue;
    }


    public virtual void Act(AbilityClass ability)
    {

    }

    public virtual bool CanAct(AbilityClass ability)
    {
        return true;
    }

}
[System.Serializable]
public class DamageScaleClass
{
    public StatType stat;
    public float scaleValue;
}