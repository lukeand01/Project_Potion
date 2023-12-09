using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class RaidInventoryUnit : ButtonBase
{
    ItemClass item;
    RaidInventoryUI handler;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI quantityText;



    public void SetUp(ItemClass item, RaidInventoryUI handler)
    {
        this.item = item;
        this.handler = handler;
    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        handler.SelectItemForDescription(item);

    }
}
