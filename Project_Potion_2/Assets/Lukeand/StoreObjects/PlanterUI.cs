using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlanterUI : MonoBehaviour
{

    GameObject holder;
    [SerializeField]GameObject completionHolder;
    [SerializeField] Image progressBar;
    [SerializeField] TextMeshProUGUI progressText;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI timeLeftText;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    public void ControlHolder(bool choice) => holder.SetActive(choice);


    public void UpdateUI(ItemDataIngredient data)
    {
        portrait.sprite = data.itemSprite;
        nameText.text = data.itemName;
    }

    public void UpdateProgress(float total, float current, bool isComplete)
    {
        completionHolder.SetActive(isComplete);
        progressBar.fillAmount = 1 -(current / total) ;
        progressText.text = ((1 - (current / total)) * 100).ToString("F0") + "%";
    }

    public void UpdateTimeLeft(TimeSpan newTime)
    {
        if(newTime.Days > 0)
        {
            Debug.Log("day");
            timeLeftText.text = "Days Left " + newTime.Days;
        }
        if(newTime.Hours > 0)
        {
            Debug.Log("hour");
            timeLeftText.text = "Hours Left: " + newTime.Hours;
            return;
        }
        if(newTime.Minutes > 0)
        {
            Debug.Log("minute");
            timeLeftText.text = "Minutes Left: " + newTime.Hours;
            return;
        }

        timeLeftText.text = "Seconds Left: " + newTime.Seconds;
    }

}
