using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class BDClass 
{
    //this is a buff or debuff class.
    //but can also be a burn.
    //
    public string id { get; private set; }
    BDData data; //description and graphics data. it gets it once the bd is given.
    [HideInInspector]public BDType bdType;

    AbilityPassiveClass triggerPassive;
    public StatType statType { get; private set; }
    public DamageType damageType { get; private set; }


    float current;
    float total;

    int tickTotal;
    int tickCurrent;
    float tickTimeTotal;
    float tickTimeCurrent;
    
    public float valueFlat { get; private set; } //the value which changes 
    public float ValuePercentBasedOnCurrentValue { get; private set; }

    float lastValue; //this is the value that we will take from. it is based in the last time it was used to influence an entity stat.

    //and we can increase this value.

    List<PercentStatClass> valueBasedInTargetStatList = new();
    List<PercentStatClass> valueBasedInAttackerStatList = new();

    int stackTotal;
    int stackCurrent;
    bool doesStackingRefreshTimer;

    bool isRefreshable;

    EntityHandler attacker;
    EntityHandler attacked;

    public BDBooleanType booleanType { get; private set; }



    public BDClass(string id, StatType stat)
    {
        bdType = BDType.Stat; 
        this.statType = stat;
        this.id = id;

        //data = GameHandler.instance.GetBDRef(stat.ToString());

    }
    public BDClass(string id, AbilityPassiveClass triggerPassive)
    {
        this.triggerPassive = new AbilityPassiveClass(triggerPassive);
        this.id = id;

        //data = GameHandler.instance.GetBDRef(triggerPassive.abilityName);
    }
    public BDClass(string id, DamageType damageType, int tickTotal, float tickTimeTotal)
    {
        this.id = id;
        this.damageType = damageType;
        MakeTick(tickTotal, tickTimeTotal);
    }

    public BDClass(string id, BDBooleanType booleanType)
    {
        this.id = id;
        this.booleanType = booleanType;
        bdType = BDType.Boolean;
    }


    public BDClass(BDClass refClass)
    {
        //copy everything here.
    }



    public void Stack(BDClass bd)
    {
        //get another bd to stack here
        Debug.Log("stack");
        if (doesStackingRefreshTimer)
        {
            current = total;
        }

        if(stackCurrent >= stackTotal)
        {
            Debug.Log("can no longer stack");
            return;
        }

        stackCurrent += 1;
        valueFlat += bd.valueFlat;
        ValuePercentBasedOnCurrentValue += bd.ValuePercentBasedOnCurrentValue;
        
    }

    #region CONTROL BD

    public void MakeFlatValue(float value)
    {
        valueFlat = value;
    }
    public void MakeValuePercent(float value)
    {
        ValuePercentBasedOnCurrentValue = value;
    }
    public void MakeStackable(int stackTotal, bool doesStackingRefreshTimer)
    {
        this.stackTotal = stackTotal;
        this.doesStackingRefreshTimer = doesStackingRefreshTimer;
    }
    public void MakeTemp(float total)
    {
        this.total = total;
        current = total;
    }
    public void MakeTick(int tickTotal, float tickTimeTotal)
    {
        this.tickTotal = tickTotal;
        this.tickTimeTotal = tickTimeTotal;
    }    
    public void MakeAdditionalValueBasedInTargetStat(StatType stat, float percentageValue)
    {
        //frorm 0 to 100.
        valueBasedInTargetStatList.Add(new PercentStatClass(stat, percentageValue));
    }
    public void MakeAdditionalValueBasedInAttackerStat(StatType stat, float percentageValue)
    {
        valueBasedInAttackerStatList.Add(new PercentStatClass(stat, percentageValue));
    }
    public void MakeAttackerAndAttacked(EntityHandler attacker, EntityHandler attacked)
    {
        this.attacker = attacker;
        this.attacked = attacked;
    }



    #endregion


    #region PASSIVE
    void AddPassive()
    {
        triggerPassive.Call();
    }
    void RemovePassive()
    {
        triggerPassive.Call(true);
    }
    #endregion

    #region TEMP
    public void ProgressTemp()
    {
        current -= 0.01f;       
    }

    public bool ShouldTempExist()
    {
        return current > 0;
    }


    #endregion

    #region TICK

    public void ProgressTick()
    {
        tickTimeCurrent += 0.01f;

        if(tickTimeCurrent > tickTimeTotal)
        {
            //we proc the damage here.
            ApplyTick();
            tickCurrent += 1;
            tickTimeCurrent = 0;
        }


    }

    void ApplyTick()
    {
        //we can apply stats or damage.
        if(bdType == BDType.Undecided || bdType == BDType.Passive)
        {
            Debug.Log("something wrong here " + id);
            return;
        }

        if(bdType == BDType.Stat)
        {
            //and we need to group these stats and then show it.
        }

        if(bdType == BDType.Damage)
        {
            if(damageType == DamageType.Normal)
            {
                //just deal a normal admage to the target.
                Debug.Log("dealt bleed damage");
                DamageClass damage = new DamageClass(5);
                damage.MakeBlockFromFinishingEntity();

                attacked.ttDamageable.TakeDamage(attacker, damage);
            }
        }

    }

    public bool ShouldTickExist()
    {
        return tickCurrent < tickTotal;
    }


    #endregion

    public float GetValueBasedInCurrentValue(float currentValue)
    {
        //we change info here based in stats.
        float percentAdd = (currentValue * ValuePercentBasedOnCurrentValue) * 0.01f;

        lastValue = valueFlat + percentAdd;
        return lastValue;
    }
    public float GetLastValueToRemove() => -1 * lastValue;

    public void Remove()
    {
        //remove passive.        

    }


    #region BOOLEAN
    public bool IsTick() => tickTotal > 0;
    public bool IsTemp() => total > 0;
    public bool IsStackable() => stackTotal > 0;

    public bool IsSameID(string id) => id == this.id;

    #endregion

    #region UI
    BDUnit unit;
    public void SetUnit(BDUnit unit)
    {
        this.unit = unit;
    }

    #endregion

}

public enum BDType
{
    Undecided,
    Stat,
    Passive,
    Damage,
    Boolean
}




public class PercentStatClass
{
    public StatType stat;
    public float percentValue;

    public PercentStatClass(StatType stat, float percentValue)
    {
        this.stat = stat;
        this.percentValue = percentValue;
        this.percentValue = Mathf.Clamp(this.percentValue, 0, 100);
    }
}


public enum BDBooleanType
{
    ShootAndMove


}


//also two things
//we can have damage-tick or stattick
//stattick we change a value

//goal now is to create something stackable.

//LIST OF ALL IT NEEDS TO BE CAPABLE
//affect stats.
//assign passives during the active time.
//updating any ui avaialbe about this.
//it needs to be able to apply damage of some sort every tick during a duration and end after its done.
//it needs to be able to change stats per tick for a duration. then decide if it wants to hold in that till being removed or it stops.
//it needs to be stackalbe.
//certains stackable wwill refresh after every new stack.