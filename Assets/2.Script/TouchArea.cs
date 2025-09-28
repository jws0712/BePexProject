//System
using System.Collections;
using System.Collections.Generic;

//UnityEngine
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class TouchArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private LineType lineType;
    [SerializeField] private GameObject areaEffect;

    public void OnPointerDown(PointerEventData eventData)
    {
        areaEffect.SetActive(true);
        JudgeManager.Instance.JudgeNote((int)lineType);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        areaEffect.SetActive(false);
    }
}
