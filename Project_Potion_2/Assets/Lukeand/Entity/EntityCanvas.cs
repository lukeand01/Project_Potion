using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityCanvas : MonoBehaviour
{
    [Separator("Damage Pop Up")]
    [SerializeField] FadeUI fadeDamageTemplate;
    [SerializeField] Transform originalPosDamage;
    public void CreateDamageFadeUI(string text, Color color)
    {
        float x = Random.Range(-0.25f, 0.50f);
        float y = Random.Range(-0.25f, 0.5f);

        FadeUI newObject = Instantiate(fadeDamageTemplate, Vector3.zero, Quaternion.identity);
        newObject.transform.parent = transform;
        newObject.transform.position = originalPosDamage.position + new Vector3(x, y, 0);

        newObject.SetUp(text, color);
    }




}
