using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChampUI : MonoBehaviour
{


    //at the start it spawns all champs.
    //it also spawns all equip
    //whn you click in a champ it selects it.
    //wheen you clikc in an equip it describes it. the second click it equips to the currnt champ selected.
    //when you select a champ it shows stats and how they are changed based in level andd equip.
    //and it also shows the twp abilities.
    

    //first i will do the whole champ part.
    //


    GameObject holder;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }
    private void Start()
    {
        SetUpSelect();
    }

    public void Control()
    {
        holder.SetActive(!holder.activeInHierarchy);      
    }

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

    public void CreateUnitForEquipClass()
    {

    }

    #endregion

    #region SELECT
    [Separator("SELECT")]
    [SerializeField] GameObject selectHolder;
    [SerializeField] TextMeshProUGUI selectNameText;
    [SerializeField] TextMeshProUGUI selectStatText;
    [SerializeField] Image selectPortrait;
    [SerializeField] Transform selectStatContainer;
    [SerializeField] ChampSkillUnit[] selectSkills;
    ChampClass currentChamp;
    List<TextMeshProUGUI> statTextList = new();

    public void SetUpSelect()
    {
        int numberOfStats = 5;

        for (int i = 0; i < numberOfStats; i++)
        {
            TextMeshProUGUI newObject = Instantiate(selectStatText, transform.position, Quaternion.identity);
            newObject.transform.parent = selectStatContainer;
            statTextList.Add(newObject);
        }
    }

    public void SelectChamp(ChampClass newChamp)
    {
        selectHolder.SetActive(newChamp.data != null);
        if (newChamp.data == null)
        {
            Debug.LogError("there was no champ data here");
            return;
        }  

        currentChamp = newChamp;

        selectNameText.text = currentChamp.data.champName;
        selectPortrait.sprite = currentChamp.data.champSprite;

        CreateStatUI();
        CreateAbilityUI();
    }

    void CreateStatUI()
    {
        List<string> statList = currentChamp.GetStatStringList();
       
        for (int i = 0; i < statTextList.Count; i++)
        {
            if (statList.Count > i)
            {
                statTextList[i].text = statList[i];
            }
            
        }
    }

    void CreateAbilityUI()
    {
        //every champ has two abilities, one active passive and one supportive passive.


    }

    public void StopSelect()
    {
        currentChamp = null;
        selectHolder.SetActive(false);
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




