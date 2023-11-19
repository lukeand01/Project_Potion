using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaidStageUnit : ButtonBase
{
    RaidStageData data;
    RaidUI uiHandler;
    [SerializeField] GameObject selected;

    public void SetUp(RaidUI uiHandler, RaidStageData data)
    {
        this.data = data;
        this.uiHandler = uiHandler;
        selected.SetActive(false);
    }

    public void Select(bool choice)
    {
        selected.SetActive(choice);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        uiHandler.SelectStage(data, this);

    }


}
