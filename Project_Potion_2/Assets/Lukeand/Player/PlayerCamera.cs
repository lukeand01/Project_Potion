using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    //it decides who it will be following.

    Camera cam;
    

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        
    }

}
