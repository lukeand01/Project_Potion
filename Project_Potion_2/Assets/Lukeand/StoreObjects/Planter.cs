using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planter : StoreObject, IInteractable
{
    //i think i will use server to check this because i cant really in client side.
    //this does not take anything from player. its always the same plant so it just keeps growing till its ready.
    //

    //progress percetnage.
    //if more than an hour then just the ehour otherwise show

    DateTime timeWhenComplete;
    [SerializeField]ItemDataIngredient data;
    float currentDiff;
    float totalDiff;
    [SerializeField] PlanterUI planterUI;

    bool isReadyForHarvest;


    private void Start()
    {
        SetUpPlanter(data);
        
        //i dont want to keep using the calculation.


    }

    private void Update()
    {
        if (isReadyForHarvest) return;

        TimeSpan time = timeWhenComplete - DateTime.UtcNow;
        currentDiff = time.Seconds;

        if(currentDiff <= 0)
        {
            Debug.Log("this was called");
            isReadyForHarvest = true;
            planterUI.UpdateProgress(1, 1, true);
            return;
        }

        planterUI.UpdateProgress(totalDiff, currentDiff, false);   
        planterUI.UpdateTimeLeft(time);
    }



    public void SetUpPlanter(ItemDataIngredient data)
    {
        //set up the plant.
        this.data = data;
        planterUI.UpdateUI(data);
        timeWhenComplete = DateTime.UtcNow.AddSeconds(data.timeForHarvest.GetTotal());
        totalDiff = (timeWhenComplete - DateTime.UtcNow).Seconds;
    }


    #region INTERACTABLE
    public string GetInteractName(PlayerInventory inventory, bool isSecond = false)
    {
        return "Harvest";
    }

    public void Interact(PlayerInventory inventory)
    {
        
    }

    public bool IsInteractable(PlayerInventory inventory)
    {
        Debug.Log("planter is being callede");
        return isReadyForHarvest;
    }

    public bool IsSecondInteractable(PlayerInventory inventory)
    {
        return false;
    }

    public void SecondInteract(PlayerInventory inventory)
    {
        
    }

    public void UIInteract(PlayerInventory inventory, bool isClose = false)
    {
        //we call the ui heere.
        planterUI.ControlHolder(isClose);

    }
    #endregion


}
