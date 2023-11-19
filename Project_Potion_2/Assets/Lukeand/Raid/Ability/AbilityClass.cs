using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class AbilityClass
{
    //
    public EntityHandler entityHandler { get; private set; }
    public void SetUpEntity(EntityHandler entityHandler)
    {
        this.entityHandler = entityHandler;
    }

    public virtual void Call(bool remove = false)
    {
        
    }

    public virtual bool CanCall()
    {
        return false;
    }

    

    public virtual AbilityActiveClass GetActive() => null;
    public virtual AbilityPassiveClass GetPassive() => null;

    

    #region GENERAL VARIABLES
    public string abilityName;
    [TextArea]public string abilityDescription;

    #endregion

    #region COOLDOWN
    //even passives can have cooldown.
    [Separator("COOLDOWN")]
    public float initialCooldown;
    float currentCooldown;
    float totalCooldown;
    
    public void SetUpCooldown(float total, float current)
    {
        currentCooldown = current;
        totalCooldown = total;
    }

    public void ChangeCurrentCooldown(float value)
    {
        currentCooldown += value;
        currentCooldown = Mathf.Clamp(currentCooldown, 0, totalCooldown);
    }

    public void ChangeTotalCooldown(float value)
    {
        totalCooldown += value;
        totalCooldown = Mathf.Clamp(totalCooldown, 0, 999);
    }

    void HandleCooldown()
    {
        if(currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }
    public bool IsReadyToUse()
    {
        return currentCooldown <= 0;
    }

    #endregion
}

[System.Serializable]
public class AbilityActiveClass : AbilityClass
{
    public List<AbilityActiveData> activeList = new();

    public AbilityActiveClass(AbilityActiveClass refClass)
    {
        foreach (var item in refClass.activeList)
        {
            activeList.Add(item);
        }

        initialCooldown = refClass.initialCooldown;
        abilityName = refClass.abilityName;
        abilityDescription = refClass.abilityDescription;

    }


    public override void Call(bool remove = false)
    {
        base.Call();
        foreach (var item in activeList)
        {
            if (!item.CanAct(this)) continue;
            item.Act(this);
        }
        
    }

    

    public override AbilityActiveClass GetActive() => this;


    //if one cannot then none can or if only those that cannot.
    //
    public override bool CanCall()
    {
        //for now this only not goes if all are not true.

        if (!IsReadyToUse()) return false;

        int amountFailed = 0;
       
        foreach (var item in activeList)
        {
            if (!item.CanAct(this)) amountFailed += 1;
        }

        return amountFailed < activeList.Count;
    }
}

[System.Serializable]
public class AbilityPassiveClass : AbilityClass
{
    public List<AbilityPassiveData> passiveList = new(); //the passive list is set up right at the start.

    public AbilityPassiveClass(AbilityPassiveClass refClass)
    {
        foreach (var item in refClass.passiveList)
        {
            passiveList.Add(item);
        }
        initialCooldown = refClass.initialCooldown;
        abilityName = refClass.abilityName;
        abilityDescription = refClass.abilityDescription;
    }

    public override AbilityPassiveClass GetPassive() => this;

    public override void Call(bool remove = false)
    {
        base.Call(remove);

        if (remove)
        {
            foreach (var item in passiveList)
            {
                item.Remove(this);
            }
        }
        else
        {
            foreach (var item in passiveList)
            {
                item.Add(this);
            }
        }

    }

    public override bool CanCall()
    {
        return true;
    }

}




public enum AbilityType
{
    AutoAttack,
    Skill1,
    Skill2,
    PassiveMain,
    PassiveSupport
}





//CONCEPTS
//there are certain behaviors such as shooting a projectil that i dont want to make for every thing thing.
//projectil = entitymover and entityDamageDealer.
//a fireball and iceball.
//both are projectils but have differnt stats.
//both will stantiate someone and try to talk with entitymover and entity damage but they required differnet graphics, ddamage, conditions.
//we have the ability data that dictates 
//so there is one ability that do the follow: throw fireball andd addd a buff to itself
//a projetil requires graphic, direction or target, 
//a buff requires 

//but ability can do more than one thing. for example creating a trigger condition. or a buff.


