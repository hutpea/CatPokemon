using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidelinePopup : BaseBox
{
    private static GuidelinePopup instance;

    public static GuidelinePopup Setup(string contentText = "", bool isSaveBox = false, Action actionOpenBoxSave = null)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<GuidelinePopup>(PathPrefabs.GUIDELINE_BOX));
            instance.Init();
        }
        //instance.OnShow(contentText);
        instance.InState(contentText);
        return instance;
    }

    [SerializeField] private Button closeBtn;
    [SerializeField] private Text content;

    private void Init()
    {
        closeBtn.onClick.AddListener(() => OnClickCloseButton());
    }
    public void InState(String param)
    {
        OnShow(param);
    }    
    private void OnShow(string contentText = "")
    {
        GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.Win);
        content.text = contentText;
    }

    private void OnClickCloseButton()
    {
        Close();
    }
}
