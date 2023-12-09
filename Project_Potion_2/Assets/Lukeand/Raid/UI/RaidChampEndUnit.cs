using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaidChampEndUnit : MonoBehaviour
{

    //this thing here will show the 

    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI starText;
    [SerializeField] Image experienceBar;
    [SerializeField] TextMeshProUGUI experienceGainedText;
    [SerializeField] TextMeshProUGUI modifierText;
    float totalExperienceGained;
    float scoreModifier;

    bool isMain;
    public void SetUp(ChampClass champ, float totalExperienceGained, float scoreModifier, bool isMain)
    {
        portrait.sprite = champ.data.champSprite;
        nameText.text = champ.data.champName;

        if (champ.IsMaxLevel())
        {
            levelText.text = "LVL MAX";
        }
        else
        {
            levelText.text = "LVL" + champ.champLevel.ToString();
        }
        
        starText.text = champ.champStar.ToString();
        float currentExperience = champ.champCurrentExperience;
        float totalRequiredExperience = Utils.GetRequiredExperience(champ.champLevel);

        experienceBar.fillAmount = currentExperience / totalExperienceGained;


        this.totalExperienceGained = totalExperienceGained;
        experienceGainedText.text = totalExperienceGained.ToString();

        this.isMain = isMain;   
        this.scoreModifier = scoreModifier;
        StartCoroutine(ShowExperienceProcess());

    }

    public void Hide()
    {
        transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        gameObject.SetActive(false);
    }
    public void Show()
    {
        //show just means it increases in size.
        transform.DOScale(1, 0.25f);
    }

    //

    IEnumerator ShowExperienceProcess()
    {
        yield return null;
    }

}
