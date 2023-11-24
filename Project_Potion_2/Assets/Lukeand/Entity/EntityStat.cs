using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class EntityStat : MonoBehaviour
{
    //this will hold all stat informations
    //also hold its level.
    //buff and debuffs.

    //we hold a bunch of stats that can be altered.

    //it can be altered in a couple of ways: flat, percentage or event (something as explosion when something happens)
    //it can be perma or it can be momentarily, or/and waiting for trigger.
    //it can receive a conditional ability as well.
    public List<BDClass> tempList = new();
    public List<BDClass> permaList = new();
    public List<BDClass> tickList = new();


    //and now we want a list of values 
    List<StatType> refList = new();

    List<StatClass> baseStatList = new();
    Dictionary<StatType, float> currentStatDictionary = new();


    private void Awake()
    {
        CreateRefList();
    }
    private void FixedUpdate()
    {
        ProgressTemp();
        ProgressTick();

        
    }


    void ProgressTemp()
    {
        for (int i = 0; i < tempList.Count; i++)
        {
            tempList[i].ProgressTemp();
            if (!tempList[i].ShouldTempExist())
            {
                //then this means we should delete this.
                RemoveBD(i, tempList);
            }
        }
    }
    void ProgressTick()
    {
        for (int i = 0; i < tickList.Count; i++)
        {

        }
    }


    public void SetUp(List<StatClass> originalStatList)
    {
        foreach (var item in refList)
        {
            currentStatDictionary.Add(item, 0);
        }

        foreach (var item in originalStatList)
        {
            baseStatList.Add(new StatClass(item.stat, item.value));
            currentStatDictionary[item.stat] += item.value;
        }
    }

    //when a stat bd is added or removed we take that value and store.
    //i prefer to not check every turn.
    public void ChangeStat(StatType stat, float value)
    {
        currentStatDictionary[stat] += value;
    }

    void CreateRefList()
    {      
        refList = new()
        {
            StatType.Health,
            StatType.Armor,
            StatType.MoveSpeed,
            StatType.Damage,
            StatType.AutoAttackCooldown,
            StatType.AbilityCooldown,
        };
    }

    public void AddBD(BDClass bd)
    {
        if(bd.bdType == BDType.Stat)
        {
            float currenStat = bd.GetStrenghtForStatChange(currentStatDictionary[bd.statType]);
            ChangeStat(bd.statType, currenStat);
        }



        if (bd.IsTick())
        {
            
        }

        if (bd.IsTemp())
        {

        }

        //then here we do the stuff.
    }

    void RemoveBD(int index, List<BDClass> targetList)
    {
        BDClass bd = targetList[index];

        if (bd.bdType == BDType.Stat)
        {
            ChangeStat(bd.statType,  bd.GetLastValueToRemove());
        }


        targetList.RemoveAt(index);

    }
    public void AddBDWithID(BDClass bd, string id)
    {       
        if (bd.IsTick())
        {
            foreach (var item in tickList)
            {
                if (item.IsSameID(id))
                {
                    //if we find it and it is stat.
                    
                    if (item.bdType == BDType.Stat)
                    {
                        //then we will remove it and add back with the new value.
                        ChangeStat(item.statType, item.GetLastValueToRemove());
                        item.Stack(bd);
                        float currenStat = bd.GetStrenghtForStatChange(currentStatDictionary[bd.statType]);
                        ChangeStat(item.statType, item.GetStrenghtForStatChange(currenStat));
                    }
                    else
                    {
                        item.Stack(bd);
                    }
                    return;
                }
            }


            return;
        }

        if (bd.IsTemp())
        {
            foreach (var item in tempList)
            {
                if (item.IsSameID(id))
                {
                    //if we find it and it is stat.

                    if (item.bdType == BDType.Stat)
                    {
                        //then we will remove it and add back with the new value.
                        ChangeStat(item.statType, item.GetLastValueToRemove());
                        item.Stack(bd);
                        float currenStat = bd.GetStrenghtForStatChange(currentStatDictionary[bd.statType]);
                        ChangeStat(item.statType, item.GetStrenghtForStatChange(currenStat));
                    }
                    else
                    {
                        item.Stack(bd);
                    }
                    return;
                }
            }
            return;
        }

        foreach (var item in permaList)
        {
            if (item.IsSameID(id))
            {
                //if we find it and it is stat.

                if (item.bdType == BDType.Stat)
                {
                    //then we will remove it and add back with the new value.
                    ChangeStat(item.statType, item.GetLastValueToRemove());
                    item.Stack(bd);
                    float currenStat = bd.GetStrenghtForStatChange(currentStatDictionary[bd.statType]);
                    ChangeStat(item.statType, item.GetStrenghtForStatChange(currenStat));
                }
                else
                {
                    item.Stack(bd);
                }
                return;
            }
        }

    }


    public float GetStatValue(StatType statType)
    {
        if (!currentStatDictionary.ContainsKey(statType)) return -1;
        return currentStatDictionary[statType];
    }

}
//thosee are all posibles stats.
//

[System.Serializable]
public class StatClass
{
    public StatType stat;
    public float value;

    public StatClass(StatType stat, float value)
    {
        this.stat = stat;
        this.value = value;
    }

}
public enum StatType
{
    Health = 0,
    Armor = 1,
    MoveSpeed = 2,
    Damage = 3,
    AutoAttackCooldown = 4,
    AbilityCooldown = 5
}