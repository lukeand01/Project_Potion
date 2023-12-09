using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityEvents : MonoBehaviour
{

    public event Action<float> EventChangedHealth;
    public void OnChangedHealth(float value) => EventChangedHealth?.Invoke(value);


    public event Action<EntityHandler> EventKillEnemy;
    public void OnKillEnemy( EntityHandler attacked)
    {
        Debug.Log("triggered");
        EventKillEnemy?.Invoke(attacked);
    }


}
