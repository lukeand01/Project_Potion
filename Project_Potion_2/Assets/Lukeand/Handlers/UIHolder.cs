using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHolder : MonoBehaviour
{
    public static UIHolder instance;

    public PlayerGUI player;
    public ChampUI champ;
    public ChestUI chest;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }
}
