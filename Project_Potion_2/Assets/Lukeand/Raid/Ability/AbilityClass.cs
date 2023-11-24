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
    public AbilityButton uiButton { get; private set; }

    [Separator("GENERAL VARIABLES")]
    public Sprite abilityIcon;
    public string abilityName;
    [TextArea] public string abilityDescription;


    //need to get the current target from here.
    public void SetUp(EntityHandler entityHandler)
    {
        this.entityHandler = entityHandler;
        SetUpCooldown();
    }

    public void SetUpUnit(AbilityButton uiButton)
    {
        this.uiButton = uiButton;
        uiButton.SetUpAbility(this);
    }

    public virtual void Call(bool remove = false)
    {

    }

    public virtual bool CanCall()
    {
        return true;
    }



    public virtual AbilityActiveClass GetActive() => null;
    public virtual AbilityPassiveClass GetPassive() => null;

    



    #region COOLDOWN
    //even passives can have cooldown.
    [Separator("COOLDOWN")]
    public float initialCooldown;
    float currentCooldown;
    float totalCooldown;

    public void SetUpCooldown()
    {
        totalCooldown = initialCooldown;
        currentCooldown = 0;
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

    public void HandleCooldown()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= 0.01f;

        }

        if(uiButton != null) uiButton.ControlCooldown(currentCooldown, totalCooldown);
    }

    public void TryToCall()
    {
        if (currentCooldown > 0) return;
        if (!CanCall()) return;

        Call(); //this is just for abilities so its always false.
        currentCooldown = totalCooldown;
    }


    public bool IsReadyToUse()
    {
        return currentCooldown <= 0;
    }

    #endregion

    public virtual bool HasCompleteData()
    {
        return false;
    }



    #region ATTACKER



    #endregion
}

[System.Serializable]
public class AbilityActiveClass : AbilityClass
{
    public List<AbilityActiveData> activeList = new();

    public override bool HasCompleteData()
    {
        if(activeList.Count <= 0 )
        {
            Debug.Log("it was zero: " + abilityName);
            return false;
        }

        for (int i = 0; i < activeList.Count; i++)
        {
            if (activeList[i] == null)
            {
                Debug.Log("this was null " + abilityName);
                return false;
            }
        }

        return true;
    }


    public AbilityActiveClass(AbilityActiveClass refClass)
    {
        foreach (var item in refClass.activeList)
        {
            activeList.Add(item);
        }
        abilityIcon = refClass.abilityIcon;
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

    public List<float> GetScaledValueFromActiveList()
    {
        //the problem is that this can be more than one.
        List<float> newList = new List<float>();

        //i get the value for eah and put in the list

        foreach (var item in activeList)
        {
            float newValue = 0;
            newValue = item.baseDamage;



        }




        return newList;
    }

    //if one cannot then none can or if only those that cannot.
    //
    public override bool CanCall()
    {
        //for now this only not goes if all are not true.

        if (!IsReadyToUse()) return false; //if its in cooldown

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

    public override bool HasCompleteData()
    {
        if (passiveList.Count <= 0) return false;

        for (int i = 0; i < passiveList.Count; i++)
        {
            if (passiveList[i] == null) return false;
        }

        return true;
    }


    public AbilityPassiveClass(AbilityPassiveClass refClass)
    {
        foreach (var item in refClass.passiveList)
        {
            passiveList.Add(item);
        }
        abilityIcon = refClass.abilityIcon;
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


