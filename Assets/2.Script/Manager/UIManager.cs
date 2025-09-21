using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        continueButton.onClick.AddListener(Continue);
    }

    private void Update()
    {
        timeText.text = SoundManager.Instance.SongPosition.ToString();
    }


    public void Pause()
    {
        pausePanel.SetActive(true);
        GameManager.Instance.Pause();
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Continue()
    {
        pausePanel.SetActive(false);
        GameManager.Instance.Continue();
    }
}
