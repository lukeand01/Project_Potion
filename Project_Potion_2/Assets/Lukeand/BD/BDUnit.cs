using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BDUnit : MonoBehaviour
{
    BDClass bd;

    [SerializeField] Image progressBar;
    [SerializeField] Image portrait;
    [SerializeField] TextMeshProUGUI stackText;
    [SerializeField] TextMeshProUGUI strenghtText;
    //we only call the bd data here.

    public void SetUp(BDClass bd)
    {
        this.bd = bd;
    }

    public void UpdateUI()
    {

    }

}
