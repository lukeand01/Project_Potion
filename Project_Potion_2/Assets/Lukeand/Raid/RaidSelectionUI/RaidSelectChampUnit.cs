using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RaidSelectChampUnit : ButtonBase, IDraggable,IDragHandler,  IBeginDragHandler
{
    //this is the champ choosen

    [SerializeField]ChampClass champ;
    RaidUI uiHandler;

    [SerializeField] GameObject empty;
    [SerializeField] GameObject selected;
    [SerializeField] Image portrait;

    bool isSlot; //the slot is the champ that will be spawned.

    public void SetUp(RaidUI uiHandler, ChampClass champ, bool isSlot)
    {
        UpdateChamp(champ);
        this.isSlot = isSlot;
        this.uiHandler = uiHandler;
    }


    #region UPDATE
    
    void UpdateUI()
    {

        empty.SetActive(!HasChamp());

        if (!HasChamp())
        {
            return;
        }

        portrait.sprite = champ.data.champSprite;

    }

    public void UpdateChamp(ChampClass champ)
    {       
        this.champ = champ;
        UpdateUI();
    }
    #endregion

    #region CLICK FUNCTION

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        //if this happens then we start dragging.
        uiHandler.draggableHandler.SetCurrentHover(this);

    }  
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        uiHandler.draggableHandler.SetCurrentHover(null);
    }

    #endregion

    #region SELECTING AND DESELECTING
    public void Selected(bool choice)
    {
        selected.SetActive(choice);
    }

    public void TryRemoveChampHere(ChampClass champ)
    {
        if (champ.data == null)
        {
            Debug.Log("it receiveed nothing here");
            return;
        }

        if (!HasChamp()) return;
        if (this.champ.data != champ.data) return;

        DeselectThisChamp();
    }

    public void DeselectThisChamp()
    {
        if (!isSlot) return;
        if (!HasChamp()) return;

        champ = null;
        UpdateUI();
    }
    #endregion

    #region UTILS
    public bool HasChamp()
    {
        if (champ == null) return false;
        if (champ.data == null) return false;

        return true;
    }

    
    public ChampClass GetChamp() => champ;



    #endregion

    #region DRAGGING

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        uiHandler.draggableHandler.StartDragging(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    #endregion
}

//


