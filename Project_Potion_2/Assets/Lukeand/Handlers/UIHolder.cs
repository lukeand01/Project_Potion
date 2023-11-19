using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHolder : MonoBehaviour
{
    public static UIHolder instance;

    public PlayerGUI player;
    public ChampUI champ;
    public ChestUI chest;
    public ProductionUI production;
    public RaidUI raid;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void OnMove()
    {
        if (chest != null) chest.CloseUI();
    }

}
