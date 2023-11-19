using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityPassiveData : AbilityData
{
    public abstract void Add(AbilityClass ability);


    public abstract void Remove(AbilityClass ability);
    
}
