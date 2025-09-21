using System.Collections;
using System.Collections.Generic;

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

    private void Start()
    {
        pauseButton.onClick.AddListener(Pause);
        replayButton.onClick.AddListener(Replay);
        continueButton.onClick.AddListener(Continue);
    }


    public void Pause()
    {
        pausePanel.SetActive(true);
    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Continue()
    {
        pausePanel.SetActive(false);
    }
}
