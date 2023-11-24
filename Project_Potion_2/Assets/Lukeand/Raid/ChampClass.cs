using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChampClass 
{
    public ChampData data;

    //create a copy of the classes.
    //we gonna pass all the refs here so we can use without affect the data.

    //the difference is that here. 
    Dictionary<AbilityType, AbilityClass> abilityDictionary = new();
    public AbilityActiveClass autoAttack;
    public AbilityActiveClass skill1;
    public AbilityActiveClass skill2;
    public AbilityPassiveClass passiveMain;
    public AbilityPassiveClass passiveSupport;

    //these abilities have nothing on them.
    //

    public int champLevel;
    public int champCopies;
    public ChampClass(ChampData data)
    {
        this.data = data;
        champLevel = 1;
        champCopies = 1;
        GenerateAbilities();



    }
    public ChampClass(ChampData data, int champLevel, int champCopies)
    {
        this.data = data;
        this.champLevel = champLevel;
        this.champCopies = champCopies;
        GenerateAbilities();
    }


    public List<string> GetStatStringList()
    {
        List<string> newList = new();

        return newList;
    }

    #region ABILITIES
    



    void GenerateAbilities()
    {
        data.GetCopyOfAttackClasses(this);


        abilityDictionary.Add(AbilityType.AutoAttack, autoAttack);
        abilityDictionary.Add(AbilityType.Skill1, skill1);
        abilityDictionary.Add(AbilityType.Skill2, skill2);
        abilityDictionary.Add(AbilityType.PassiveMain, passiveMain);
        abilityDictionary.Add(AbilityType.PassiveSupport, passiveSupport);

        //at only the start of the game do we assign the passives.
        //deciding either to call support or main


    }

    public bool CanCallAbility(AbilityType ability)
    {
        //check for cooldowns or other problems.
        return abilityDictionary[ability].CanCall();
    }
    public void CallAbility(AbilityType ability, bool remove = false)
    {
        Debug.Log("call ability " + ability);
        abilityDictionary[ability].Call(remove);
    }

    #endregion


    #region UI UNIT
    public void SetUIUnit(ChampUIUnit champUnit)
    {
        this.champUnit = champUnit;
    }
    ChampUIUnit champUnit;
    #endregion
}

