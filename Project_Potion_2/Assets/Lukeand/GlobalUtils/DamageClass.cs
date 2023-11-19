using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageClass 
{

    float baseDamage;
    float critChance;
    float critDamage;
    float damageBasedInHealth;

    //we get the health scaling.



    public bool cannotFinishEntity { get; private set;}

    List<BDClass> bdList = new();

    public DamageClass(float baseDamage)
    {
        this.baseDamage = baseDamage;
        
    }

    #region MAKE

    public void MakeBlockFromFinishingEntity()
    {
        cannotFinishEntity = true;
    }

    public void MakeCritChance(float critChance)
    {
        this.critChance = critChance;
    }

    public void MakeBDList(List<BDClass> bdList)
    {
        foreach (var item in bdList)
        {
            this.bdList.Add(item);
        }
    }

    #endregion


    public void ApplyBDToStat(EntityStat stat)
    {
        if (bdList.Count <= 0) return;

        foreach (var item in bdList)
        {
            stat.AddBD(item);
        }

    }

    public float GetDamage()
    {
        return baseDamage;
    }
}
