using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DraggableSelectChampHandler : MonoBehaviour
{

    //i will create something especifc for it.
    [SerializeField] Image draggingImage; //its just an image because all the data is in the handler.

    RaidUI uiHandler;

    RaidSelectChampUnit currentHoverUnit;
    RaidSelectChampUnit draggingUnit;
    ChampClass currentChamp;


    private void Awake()
    {
        uiHandler = GetComponent<RaidUI>();
    }

    private void FixedUpdate()
    {
        

        if(Input.touchCount <= 0 && draggingUnit != null)
        {
            //if there are no touchs then we dod this.
            //but only do this if hovering nothing.
            

            if(currentHoverUnit != null)
            {
                //give the information.

                uiHandler.ResetUnits(currentChamp);
                currentHoverUnit.UpdateChamp(currentChamp);
            }

            draggingUnit.DeselectThisChamp();
            currentChamp = null;
            currentHoverUnit = null;
            draggingUnit = null;
            

        }

        

        ControlImageVisibility();
        MoveImage();

    }

    void ControlImageVisibility()
    {
        draggingImage.gameObject.SetActive(draggingUnit != null);
    }
    void MoveImage()
    {
        if (Input.touchCount <= 0) return;
        Vector2 touchPos = Input.touches[0].position;
        draggingImage.transform.position = touchPos + new Vector2(15, 15);
    }

    public void StartDragging(RaidSelectChampUnit draggingUnit)
    {
        if(draggingUnit == null)
        {
            Debug.Log("no dragging unit");

        }
        currentChamp = draggingUnit.GetChamp();

        if(currentChamp == null)
        {
            Debug.Log("no champ");
            return;
        }

        this.draggingUnit = draggingUnit;
        
        draggingImage.sprite = currentChamp.data.champSprite;
    }

    public void SetCurrentHover(RaidSelectChampUnit hover)
    {
        currentHoverUnit = hover;
    }

    #region GETTERS AND BOOLEANS
    public ChampClass GetDraggingChamp()
    {
        ChampClass champ = currentChamp;
        currentChamp = null;
        return champ;
    }

    public bool IsDragging()
    {
        return draggingImage.gameObject.activeInHierarchy;
    }
    #endregion
}
