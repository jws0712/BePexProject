//System
using System.Collections;
using System.Collections.Generic;

//UnityEngine
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//TMP
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject pausePanel;

    [Header("Button")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button replayButton;
    [SerializeField] private Button continueButton;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI timeText;

    private void Start()
    {
        pauseButton.onClick.AddListener(Pause);
        replayButton.onClick.AddListener(Replay);
        continueButton.onClick.AddListener(UnPause);
    }

    private void Update()
    {
        timeText.text = GameManager.Instance.SongPosition.ToString();
    }

    //일시정지 버튼 이벤트
    private void Pause()
    {
        pausePanel.SetActive(true);
        GameManager.Instance.Pause();
    }

    //계속하기 버튼 이벤트
    private void UnPause()
    {
        pausePanel.SetActive(false);
        GameManager.Instance.UnPause();
    }

    //다시하기 버튼 이벤트
    private void Replay()
    {
        pausePanel.SetActive(false);
        GameManager.Instance.ReStart();
    }

}
