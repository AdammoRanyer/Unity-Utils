using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPressed : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[Header("Button variables")]
	[SerializeField] public bool pressed;

    public void OnPointerDown(PointerEventData pointerEventData){
        pressed = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData){
        pressed = false;
    }
}
