//System
using System.Collections;
using System.Collections.Generic;

//UnityEngine
using UnityEngine;

//TMP
using TMPro;

public class JudgeEffect : Effect
{
    [SerializeField] private TextMeshProUGUI judge;
    [SerializeField] private TextMeshProUGUI combo;


    protected override void OnEnable()
    {
        base.OnEnable();

        switch (JudgeManager.Instance.JudgeResult)
        {
            case JudgeType.Perfect:
                {
                    SetEffect(true, Color.green, "PERFECT");
                    break;
                }

            case JudgeType.Greate:
                {
                    SetEffect(true, Color.blue, "GREATE");
                    break;
                }

            case JudgeType.Good:
                {
                    SetEffect(true, Color.yellow, "GOOD");
                    break;
                }

            case JudgeType.Bad:
                {
                    SetEffect(false, Color.red, "BAD");
                    break;
                }
        }

    }

    private void SetEffect(bool isEnableComboText, Color textColor, string judgeText)
    {
        combo.enabled = isEnableComboText;
        combo.text = JudgeManager.Instance.ComboCount.ToString("D2");

        judge.color = textColor;
        judge.text = judgeText;
    }
}
