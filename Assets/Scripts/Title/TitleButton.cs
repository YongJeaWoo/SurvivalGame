using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour
{
    public GameObject title;
    public GameObject fadeOut;
    public GameObject targetCam;
    public GameObject movingCam;

    public GameObject optionPanel;
    public GameObject buttonCollect;

    public void StartButton()
    {
        title.SetActive(false);
        fadeOut.SetActive(true);
        targetCam.SetActive(false);
        movingCam.SetActive(true);
    }

    public void OptionButton()
    {
        Time.timeScale = 0.5f;
        optionPanel.SetActive(true);
        buttonCollect.SetActive(false);
    }

    public void OptionExit()
    {
        Time.timeScale = 1f;
        optionPanel.SetActive(false);
        buttonCollect.SetActive(true);
    }

    public void ExitButton()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
