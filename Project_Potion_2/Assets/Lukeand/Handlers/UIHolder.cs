using MyBox;
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

    [Separator("INSIDE RAID")]
    public RaidInventoryUI raidInventory;
    public GameObject raidUtilityButtonsHolder;
    public RaidEndUI raidEnd;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void OpenRaidMenu()
    {
        //this has a bunch of different uis 
        raidInventory.Open();
        raidUtilityButtonsHolder.SetActive(true);
    }
    public void CloseRaidMenu()
    {
        raidInventory.Close();
        raidUtilityButtonsHolder.SetActive(false);
    }

    public void OnMove()
    {
        if (chest != null) chest.CloseUI();
    }

}
