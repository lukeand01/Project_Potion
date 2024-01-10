using DG.Tweening;
using MyBox.EditorTools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RaidChampEndUnit : MonoBehaviour
{

    //this thing here will show the 
    ChampClass champ;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Transform starContainer;
    [SerializeField] Image starTemplate;

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

        SpawnStars(champ.champStar);
        float currentExperience = champ.champCurrentExperience;
        float totalRequiredExperience = Utils.GetRequiredExperience(champ.champLevel);

        experienceBar.fillAmount = currentExperience / totalExperienceGained;


        this.totalExperienceGained = totalExperienceGained;
        experienceGainedText.text = totalExperienceGained.ToString();

        this.isMain = isMain;   
        this.scoreModifier = scoreModifier;
        //StartCoroutine(ShowExperienceProcess());

    }

    void SpawnStars(int quantity)
    {
        for (int i = 0; i < starContainer.childCount; i++)
        {
            Destroy(starContainer.GetChild(i).gameObject);
        }

        for (int i = 0; i < quantity; i++)
        {
            Image newObject = Instantiate(starTemplate, Vector3.zero, Quaternion.identity);
            newObject.transform.parent = starContainer;
            newObject.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        gameObject.SetActive(false);
    }
    public void Show()
    {
        //show just means it increases in size.       
        transform.DOScale(1.8f, 0.25f);

    }

    public void UpdatGainedExperienceText(float value)
    {
        experienceGainedText.text = value.ToString();
    }

    //

    public void ShowExperience()
    {
        StartCoroutine(ShowExperienceProcess());
    }

    //

    IEnumerator ShowExperienceProcess()
    {
        //teh experience was already stored.
        //give the score.
        //this champ is not real so i can do whatever i want with it.
        //it shows the experience gained and slowly raise the experience

        //float expForNextLevel = champ.exper


       
        float remainingExperience = totalExperienceGained;

        float timeModifier = 0;


        float currentExp = champ.champCurrentExperience;
        float requiredExp = champ.champRequiredExperience;
        int level = champ.champLevel;

        
        while (remainingExperience > 0 && !champ.IsMaxLevel())
        {
            //we keep on giving.
            //till we either done or out ofresources.


            currentExp += 1;
            remainingExperience -= 1;

            experienceBar.fillAmount = currentExp / requiredExp;


            if(currentExp >= requiredExp)
            {            
                currentExp = 0;
                level += 1;
                yield return StartCoroutine(IncreaseLevelProcess(level));
                totalExperienceGained = Utils.GetRequiredExperience(level);
                experienceBar.fillAmount = currentExp / requiredExp;
            }
            


            yield return new WaitForSeconds(0.008f);
        }



    }

    IEnumerator IncreaseLevelProcess(int newLevel)
    {

        //increase 
        if (champ.IsMaxLevel())
        {
            levelText.text = "LVL MAX";
        }
        else
        {
            levelText.text = "LVL" + newLevel.ToString();
        }
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

    }


}
