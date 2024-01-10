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


    //these abilities have nothing on them.
    //

    public int champLevel { get; private set; }//the current level
    public float champCurrentExperience { get; private set; } //the progress to the next level.
    public float champRequiredExperience { get; private set; } //the experience necessary for the current level
    public int champStar { get; private set; } //the cap of the max level
    public int champCopies { get; private set; }

    public List<StatClass> statList = new List<StatClass>();
    Dictionary<StatType, StatClass> statDictionary = new Dictionary<StatType, StatClass>();


    public ChampClass(ChampData data)
    {
        this.data = data;
        champLevel = 1;
        champCopies = 1;
        champStar = 1;
        champRequiredExperience = Utils.GetRequiredExperience(champLevel);
        GenerateAbilities();
        CreateStatList();
    }
    public ChampClass(ChampData data, int champLevel, int champCopies, int champStar, float champCurrentExperience)
    {
        this.data = data;
        this.champLevel = champLevel;
        this.champCopies = champCopies;
        this.champStar = champStar;

        this.champCurrentExperience = champCurrentExperience;
        champRequiredExperience = Utils.GetRequiredExperience(champLevel);

        GenerateAbilities();
        CreateStatList();
    }
  
    public ChampClass(ChampClass refClass)
    {
        data = refClass.data;
        champLevel = refClass.champLevel;
        champCopies = refClass.champCopies;
        champStar = refClass.champStar;
        champCurrentExperience = refClass.champCurrentExperience;
        champRequiredExperience = Utils.GetRequiredExperience(champLevel);
    }


    //how to define how many copies are required?
    //based on ccurrent level and rarity.
    //

    public bool HasEnoughCopies()
    {
        return champCopies >= champStar * 10;
    }

    public int GetCurrentRequiredCopies()
    {
        return 0;
    }

    #region ABILITIES

    Dictionary<AbilityType, AbilityClass> abilityDictionary = new();
    public AbilityActiveClass autoAttack;
    public AbilityActiveClass skill1;
    public AbilityActiveClass skill2;
    public AbilityPassiveClass passiveMain;
    public AbilityPassiveClass passiveSupport;


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

    #region STATS

    void CreateStatList()
    {
        //1 - in here we use the ref list to create a list
        //2 - then we copy that list to the dictionary.
        //3 - we use the dictionary to add everything from the baselist.
        //4 - then we do it for the scaling.

        foreach (var item in data.refList)
        {
            statList.Add(new StatClass(item, 0));
        }

        foreach (var item in statList)
        {
            statDictionary.Add(item.stat, item);
        }

        foreach (var item in data.baseStatLst)
        {
            if (statDictionary.ContainsKey(item.stat))
            {
                statDictionary[item.stat].value += item.value;
            }
            else
            {
                Debug.Log("something wrong here");
            }
        }

        foreach (var item in data.scalingStatList)
        {
            if (statDictionary.ContainsKey(item.stat))
            {
                float scaledValue = item.value * champLevel;
                statDictionary[item.stat].value += scaledValue;
            }
            else
            {
                Debug.Log("something wrong here");
            }


        }

    }

    //this is just for increasing it once.
    void IncreaseStatByOneLevel()
    {
        foreach (var item in data.scalingStatList)
        {
            if (statDictionary.ContainsKey(item.stat))
            {
                statDictionary[item.stat].value += item.value;
            }
            else
            {
                Debug.Log("something wrong here");
            }


        }
    }



    public List<string> GetStatStringList()
    {
        List<string> newList = new();

        foreach (var item in data.refList)
        {
            newList.Add(item.ToString());
        }


        return newList;
    }
    #endregion

    #region UI UNIT
    public void SetUIUnit(ChampUIUnit champUnit)
    {
        this.champUnit = champUnit;
    }
    ChampUIUnit champUnit;
    #endregion

    #region LEVEL


    public void GainExperience(float value)
    {

        float currentValue = value;
        int brake = 0;

        while(currentValue > 0 && !IsMaxLevel())
        {
            brake++;

            if(brake > 1000)
            {
                Debug.Log("had to brakelevel in uppping char " + data.champName);
                break;
            }

            float experienceForNextLevel = champRequiredExperience - champCurrentExperience;
            experienceForNextLevel = Mathf.Clamp(experienceForNextLevel, 0, value);
            currentValue -= experienceForNextLevel;
            champCurrentExperience += experienceForNextLevel;

            if (champCurrentExperience >= champRequiredExperience)
            {
                champCurrentExperience = 0;
                champLevel += 1;
                champRequiredExperience = Utils.GetRequiredExperience(champLevel);
                
            }
        }



    }

    public void GainExperienceForJustUI(float value)
    {
        champCurrentExperience += value;
    }

    public bool IsMaxLevel()
    {
        return champLevel >= champStar * 10;
    }




    public void GainLevel()
    {
        //we increase the level and change the stats.
        champLevel += 1;
        IncreaseStatByOneLevel();
    }

    #endregion

    
}

