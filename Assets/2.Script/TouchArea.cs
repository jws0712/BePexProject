//System
using System.Collections;
using System.Collections.Generic;

//UnityEngine
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private LineType lineType;

    public void OnPointerDown(PointerEventData eventData)
    {
        JudgeManager.Instance.JudgeNote((int)lineType);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
