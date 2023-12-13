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

    [Separator("RAID BUTTON")]
    public AbilityButton skill1Button;
    public AbilityButton skill2Button;
    public FloatingJoystick joystick;


    private void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Debug.Log("called this");

        }
    }

    private void ReDoInstanceCheck()
    {
        if (instance == null) instance = this;
        else
        {
            Debug.Log("called this");
            Destroy(gameObject);
        }
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
