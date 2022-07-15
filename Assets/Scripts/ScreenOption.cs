using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenOption : MonoBehaviour
{
    FullScreenMode mFullScreenMode;
    public TMP_Dropdown Dropdown;
    public Toggle fullScreenClick;
    [SerializeField] int resolutionNum;
    List<Resolution> resolutions = new List<Resolution> ();

    private void Start()
    {
        InitUI();
    }

    void InitUI()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRate == 60)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }

        Dropdown.options.Clear();

        int optionNum = 0;

        foreach (Resolution item in resolutions)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " " + item.refreshRate + "hz";
            Dropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
                Dropdown.value = optionNum;
            optionNum++;
        }
        Dropdown.RefreshShownValue();

        fullScreenClick.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow) ? true : false;
    }

    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        mFullScreenMode = isFull ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
    }

    public void SetClick()
    {
        Screen.SetResolution(resolutions[resolutionNum].width,
            resolutions[resolutionNum].height,
            mFullScreenMode);
    }
}
