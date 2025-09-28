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

    //�Ͻ����� ��ư �̺�Ʈ
    private void Pause()
    {
        pausePanel.SetActive(true);
        GameManager.Instance.Pause();
    }

    //����ϱ� ��ư �̺�Ʈ
    private void UnPause()
    {
        pausePanel.SetActive(false);
        GameManager.Instance.UnPause();
    }

    //�ٽ��ϱ� ��ư �̺�Ʈ
    private void Replay()
    {
        pausePanel.SetActive(false);
        GameManager.Instance.ReStart();
    }

}
