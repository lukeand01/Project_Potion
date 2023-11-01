using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChampUIUnit : ButtonBase
{
    ChampClass champ;
    [SerializeField] Image portrait;
    [SerializeField] GameObject unlockedHolder;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI copiesText;
    [SerializeField] TextMeshProUGUI levelText;

    public void SetUp(ChampClass champ)
    {
        this.champ = champ;
        UpdateUI();
    }

    void UpdateUI()
    {

        if(champ.data == null)
        {
            Debug.Log("something wrong here.");
            return;
        }

        portrait.sprite = champ.data.champSprite;
        unlockedHolder.SetActive(champ.champCopies > 0);

        if(champ.champCopies > 0)
        {
            //we have it.
            portrait.color = Color.white;

            nameText.text = champ.data.champName;
            copiesText.text = champ.champCopies.ToString();
            levelText.text = champ.champLevel.ToString();
        }
        else
        {
            //we dont have it.
            portrait.color = Color.black;
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        //when we click on this we check if we have the stuff.




    }
}
