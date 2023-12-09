using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{
    
    public static float GetRequiredExperience(int level)
    {
        return (level * 0.2f) * (10 * level);
    }


}
