using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class WinPopup : BaseBox
{
    private static WinPopup instance;

    public static WinPopup Setup(bool isSaveBox = false, Action actionOpenBoxSave = null)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<WinPopup>(PathPrefabs.WIN_BOX));
            instance.Init();
        }
        instance.OnShow();
        return instance;
    }

    [SerializeField] private Button nextLevelBtn;
    [SerializeField] private Text levelText;

    private void Init()
    {
        nextLevelBtn.onClick.AddListener(OnClickNextLevelButton);
    }

    private void OnShow()
    {
        GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.Win);
        levelText.text = UseProfile.CurrentLevel > Context.MAX_LEVEL ? Context.MAX_LEVEL.ToString() : (UseProfile.CurrentLevel - 1).ToString();
    }

    private void OnClickNextLevelButton()
    {
        if (UseProfile.IsCheatAd)
        {
            GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);
            Close();
            GameController.Instance.LoadScene("_gameplay");
            return;
        }
        GameController.Instance.admobAds.ShowInterstitial(actionIniterClose: () =>
        {
            GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);
            Close();
            GameController.Instance.LoadScene("_gameplay");
        });
    }
}
