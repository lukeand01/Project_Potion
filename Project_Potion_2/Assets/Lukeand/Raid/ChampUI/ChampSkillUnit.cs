using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChampSkillUnit : ButtonBase
{
    ChampUI uiHandler;

    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI titleText;


    public void UpdateSkillUnit()
    {

    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        //
    }

}
