using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputButton : ButtonBase
{

    //hold, release and 

    [Separator("COMPONENTS")]
    [SerializeField] GameObject isHoldingImage;
    [SerializeField] TextMeshProUGUI inputText;
    protected GameObject holder;

    public event Action EventPressed;
    public void OnPressed() => EventPressed?.Invoke();

    public event Action EventReleased;
    public void OnReleased() => EventReleased?.Invoke();

    //want to be able to hold.

    public bool isPressing;

    private void Awake()
    {
        holder = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
       if(isHoldingImage != null) isHoldingImage.SetActive(isPressing);
    }
    public void ChangeText(string name)
    {
        inputText.text = name;
    }

    public void Control(bool choice)
    {
        if(holder == null)
        {
            Debug.Log("the holder is null");
            return;
        }

        holder.SetActive(choice);
    }

    //
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        OnPressed();

    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        isPressing = true;
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        isPressing = false;
        OnReleased();
    }
}
