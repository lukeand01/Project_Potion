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
    Dictionary<string, BDClass> dictionaryForStacking = new Dictionary<string, BDClass>();

    //and now we want a list of values 

    List<StatClass> initialStatList = new();
    Dictionary<StatType, float> currentStatDictionary = new();


    
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


    public void SetUp(List<StatClass> currentStatList)
    {
        //we give this from the champclass;

        //1 - we first set this new list as a local list
        //2 - we set up the currenstatlist with these values. the dictionary will be the thing being affected by buffs or debuffs.        

        foreach (var item in currentStatList)
        {
            initialStatList.Add(new StatClass(item.stat, item.value));
        }

        foreach (var item in initialStatList)
        {
            currentStatDictionary.Add(item.stat, item.value);
        }



    }

    //when a stat bd is added or removed we take that value and store.
    //i prefer to not check every turn.
    public void ChangeStat(StatType stat, float value)
    {
        currentStatDictionary[stat] += value;
    }

    

    public void AddBD(BDClass bd)
    {
        if(bd.bdType == BDType.Stat)
        {
            float currenStat = bd.GetValueBasedInCurrentValue(currentStatDictionary[bd.statType]);
            ChangeStat(bd.statType, currenStat);
        }

        if (bd.IsStackable())
        {
            dictionaryForStacking.Add(bd.id, bd);
        }

        if (bd.IsTick())
        {
            tickList.Add(bd);
            return;
        }

        if (bd.IsTemp())
        {
            
            tempList.Add(bd);
            return;
        }

        permaList.Add(bd);

        //then here we do the stuff.
    }

    void RemoveBD(int index, List<BDClass> targetList)
    {
        BDClass bd = targetList[index];

        if (bd.bdType == BDType.Stat)
        {
            ChangeStat(bd.statType,  bd.GetLastValueToRemove());
        }


        if (dictionaryForStacking.ContainsKey(bd.id))
        {
            dictionaryForStacking.Remove(bd.id);
        }

        targetList.RemoveAt(index);

        
    }
    public void AddBDWithID(BDClass bd)
    {

       


        if (dictionaryForStacking.ContainsKey(bd.id))
        {
            BDClass bdToStack = dictionaryForStacking[bd.id];
            ChangeStat(bdToStack.statType, bdToStack.GetLastValueToRemove());
            bdToStack.Stack(bd);
            float currenStat = bd.GetValueBasedInCurrentValue(currentStatDictionary[bd.statType]);
            ChangeStat(bdToStack.statType, bdToStack.GetValueBasedInCurrentValue(currenStat));
        }
        else
        {
            AddBD(bd);
        }

        


    }


    public float GetStatValue(StatType statType)
    {
        if (!currentStatDictionary.ContainsKey(statType)) return -1;
        return currentStatDictionary[statType];
    }

    //i dont want to be checking lists everytime but maybe we can jsut do that for now.
    public bool HasBDBoolean(BDBooleanType boolType)
    {
        foreach (var item in tempList)
        {
            if(item.bdType == BDType.Boolean)
            {
                if (item.booleanType == boolType) return true;
            }
        }

        foreach (var item in permaList)
        {
            if (item.bdType == BDType.Boolean)
            {
                if (item.booleanType == boolType) return true;
            }
        }

        return false;
    }
}

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