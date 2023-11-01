using MyBox;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerGUI : MonoBehaviour
{
    //it does a little animation.
    //the number counts up and 

    private void Awake()
    {
        moneyTextOriginalScale = moneyHolder.transform.localScale;
    }

    [Separator("MONEY")]
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] GameObject moneyHolder;
    Coroutine moneyProcess;
    
    float currentMoneyText;
    Vector2 moneyTextOriginalScale;
    public void UpdateMoney(int current, int change)
    {
        if(moneyProcess != null) StopCoroutine(moneyProcess);

        StopCoroutine(ShowMoneyProcess());
        StopCoroutine(CloseMoneyProcess());

        if(change != 0)
        {
            moneyProcess = StartCoroutine(UpdateMoneyProcess(current, change));
        }
        else
        {
            currentMoneyText = current;
            moneyText.text = currentMoneyText.ToString();
        }

        
    }

    IEnumerator UpdateMoneyProcess(int current, int change)
    {
        //increase based in the differnce.
        float diff = Mathf.Abs(current - change);
        diff = Mathf.Clamp(diff, 2, 10);
        float speed = 0.01f / diff;
        int actualChange = change > 0 ? 1 : -1;
        StartCoroutine(ShowMoneyProcess());

        while(currentMoneyText != current)
        {

            currentMoneyText += actualChange;
            moneyText.text = currentMoneyText.ToString();
            yield return new WaitForSeconds(speed);
        }

        StartCoroutine(CloseMoneyProcess());
        
    }

    IEnumerator ShowMoneyProcess()
    {

        while (moneyHolder.transform.localScale.x < 1.5)
        {
            moneyHolder.transform.localScale += new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.01f);
        }



    }
    IEnumerator CloseMoneyProcess()
    {
        while (moneyHolder.transform.localScale.x > 1.2)
        {
            moneyHolder.transform.localScale -= new Vector3(0.1f, 0.1f, 0);
            yield return new WaitForSeconds(0.01f);
        }
    }



}
