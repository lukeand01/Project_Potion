using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuySpot : MonoBehaviour
{

    //this is the bar progress. 
    float current;
    float total;
    float speedModifier;


    GameObject canvasHolder;
    [SerializeField] Image mark;
    [SerializeField] Image bar;
    [SerializeField] GameObject hoverHolder;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI titleText;
    Vector3 originalScale;

    [Separator("BUY DATA")]
    [SerializeField] BuyData data;


    [SerializeField] public bool cannotBeUsed { get; private set; }
    bool cannotBePressed;
    bool isReducing;

    private void Awake()
    {
        canvasHolder = transform.GetChild(0).gameObject;
        originalScale = mark.transform.localScale;
        total = 2.5f;
        speedModifier = 0.08f;
    }

    public void OpenHover()
    {
        hoverHolder.SetActive(true);
        priceText.text = "3";
        titleText.text = "just for test";

        current = 0;
        bar.fillAmount = 0;
    }

    public void ReleaseButton()
    {
        cannotBePressed = false;
    }

    void StartSpot()
    {
        cannotBePressed = false;
        isReducing = false;
        PlayerHandler.instance.StartBuySpot(this);
        OpenHover();
        StopAllCoroutines();
        StartCoroutine(OpenProcess());
    }
    public void Cancel()
    {
        StopAllCoroutines();
        StartCoroutine(CloseProcess());
        hoverHolder.SetActive(false);       
    }
    public void Progress()
    {
        if (cannotBePressed) return;
        if (isReducing) return;

        if (current > total)
        {
            Act();
            StartCoroutine(ReduceProcess());
        }
        else
        {
            current += speedModifier;
        }

        bar.fillAmount = current / total;
    }

    void Act()
    {
        cannotBePressed = true;
        cannotBeUsed = !data.Act();
        canvasHolder.SetActive(!cannotBeUsed);
        
    }

    public bool CanBeUsed() => cannotBeUsed;


    public void Regress()
    {
        if (cannotBePressed) return;
        if (isReducing) return;

        if(current > 0)
        {
            current -= speedModifier;
        }

        bar.fillAmount = current / total;
    }

    IEnumerator ReduceProcess()
    {
        isReducing = true;
        while(current > 0)
        {
            current -= speedModifier;
            bar.fillAmount = current / total;
            yield return new WaitForSeconds(0.01f);
        }
        isReducing = false;
    }


    public string GetInteractName()
    {
        return "Teste buy stuff";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;
        StartSpot();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") return;
        PlayerHandler.instance.StopBuySpot();
    }


    IEnumerator OpenProcess()
    {

        while(originalScale.x + 0.5f > mark.transform.localScale.x)
        {
            mark.transform.localScale += new Vector3(0.05f, 0.05f, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }


    }

    IEnumerator CloseProcess()
    {
        while (originalScale.x  < mark.transform.localScale.x)
        {
            mark.transform.localScale -= new Vector3(0.05f, 0.05f, 0);
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

}
