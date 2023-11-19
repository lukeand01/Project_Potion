using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityDebugger : MonoBehaviour
{
    EntityHandler handler;

    

    private void Awake()
    {
        handler = GetComponent<EntityHandler>();

        if(handler == null)
        {
            Debug.LogError("found no handler so debugger will destroy itslef " + gameObject.name);
            Destroy(this);
        }
    }

    

}
