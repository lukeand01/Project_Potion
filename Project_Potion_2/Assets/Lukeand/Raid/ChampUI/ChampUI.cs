using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChampUI : MonoBehaviour
{

    #region UI UNITS
    [Separator("UNITS")]
    [SerializeField] Transform champUnitContainer;
    [SerializeField] ChampUIUnit champUnitTemplate;

    public void CreateUnitForChampClass(ChampClass champ)
    {
        ChampUIUnit newObject = Instantiate(champUnitTemplate, transform.position, Quaternion.identity);
        newObject.transform.parent = champUnitContainer;
        champ.SetUIUnit(newObject);
        newObject.SetUp(champ);
    }
    #endregion

    #region SELECT
    [Separator("SELECT")]
    [SerializeField] GameObject selectHolder;
    [SerializeField] TextMeshProUGUI selectNameText;
    [SerializeField] Image selectPortrait;
    [SerializeField] ChampSkillUnit[] selectSkills;
    ChampClass currentChamp;
    public void SelectChamp(ChampClass newChamp)
    {
        currentChamp = newChamp;

    }

    public void StopSelect()
    {
        currentChamp = null;

    }
    #endregion

    #region DESCRIPTION
    public void StartDescription()
    {

    }

    public void StopDescription()
    {

    }

    #endregion

}

//the ui needs to be simple. nothing complicated.
//all the hero appear in the inventory but only the ones found are visible.
//they come with stats, copies value.
//cannot change weapon. they each have stats and maybe a passive.
