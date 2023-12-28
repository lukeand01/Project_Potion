using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RaidChampEndUnit : MonoBehaviour
{

    //this thing here will show the 
    ChampClass champ;
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
        this.champ = champ;
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
        //StartCoroutine(ShowExperienceProcess());

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

    public void ShowExperience()
    {
        StartCoroutine(ShowExperienceProcess());
    }

    IEnumerator ShowExperienceProcess()
    {
        //teh experience was already stored.
        //give the score.
        //this champ is not real so i can do whatever i want with it.
        //it shows the experience gained and slowly raise the experience

        //float expForNextLevel = champ.exper

        
      
        float remainingExperience = totalExperienceGained;
        float singleLoopRemainingExp = 0;

        float currentExp = champ.champCurrentExperience;
        float totalExp = champ.champTotalExperience;
        int level = champ.champLevel;


        while (remainingExperience > 0 && !champ.IsMaxLevel())
        {
            //we keep on giving.
            //till we either done or out ofresources.



            yield return new WaitForSeconds(0.01f);
        }


        

    }

    IEnumerator IncreaseLevelProcess(int newLevel)
    {
        //increase 
        levelText.text = newLevel.ToString();
        levelText.transform.localScale = Vector3.one;

        while(levelText.transform.localScale.x < 1.5f)
        {
            levelText.transform.localScale += new Vector3(0.01f, 0.01f, 0);
            yield return new WaitForSeconds(0.005f);
        }

        while(levelText.transform.localScale.x > 1)
        {
            levelText.transform.localScale -= new Vector3(0.01f, 0.01f, 0);
            yield return new WaitForSeconds(0.005f);
        }

        yield return null;
    }


}
