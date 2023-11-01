using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCEmote : MonoBehaviour
{
    [SerializeField] GameObject bubble;
    [SerializeField] GameObject angryEmote;
    [SerializeField] GameObject ConfusedEmote;
    [SerializeField] GameObject LoveEmote;



}

public enum EmoteType
{
    Confused,
    Angry,
    Love
}