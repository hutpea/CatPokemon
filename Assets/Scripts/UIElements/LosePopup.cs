using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LosePopup : BaseBox
{
    private static LosePopup instance;

    public static LosePopup Setup(bool isSaveBox = false, Action actionOpenBoxSave = null)
    {
        if (instance == null)
        {
            instance = Instantiate(Resources.Load<LosePopup>(PathPrefabs.LOSE_BOX));
            instance.Init();
        }
        instance.OnShow();
        return instance;
    }

    [SerializeField] private Button replayBtn;
    [SerializeField] private Button watchAdBtn;
    [SerializeField] private Text levelText;

    private void Init()
    {
        //TODO: DO SOMETHING!
        /*decorItemBtn.onClick.AddListener(HandleClickDecorItem);
        selectRoomBtn.onClick.AddListener(HandleClickSelectRoom);
        decorRoomBtn.onClick.AddListener(HandleClickDecorRoom);*/
        replayBtn.onClick.AddListener(OnClickReplayButton);
        watchAdBtn.onClick.AddListener(OnClickWatchAdButton);
    }

    private void OnShow()
    {
        GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.Lose);
        levelText.text = UseProfile.CurrentLevel > Context.MAX_LEVEL ? Context.MAX_LEVEL.ToString() : (UseProfile.CurrentLevel).ToString();
    }

    private void OnClickReplayButton()
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

    private void OnClickWatchAdButton()
    {
        if (UseProfile.IsCheatAd)
        {
            GameplayController.Instance.level.board.isCompletelyDisableControl = false;
            GameplayController.Instance.level.board.EnableClickControlAllCellsOnBoard();
            Close();
            return;
        }
        GameController.Instance.admobAds.ShowVideoReward(
           actionReward: () =>
           {
               GameplayController.Instance.level.board.isCompletelyDisableControl = false;
               GameplayController.Instance.level.board.EnableClickControlAllCellsOnBoard();
               Close();
           },
           actionNotLoadedVideo: () =>
           {
               GameController.Instance.moneyEffectController.SpawnEffectText_FlyUp
                (
                GameplayController.Instance.gameplayUIController.randomizeButton.transform.position,
                "No video at the moment!",
                Color.white,
                isSpawnItemPlayer: true
                );
           },
           actionClose: null,
           ActionWatchVideo.None,
           GameController.Instance.useProfile.CurrentLevelPlay.ToString()
        );
    }
}
