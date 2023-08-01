using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using EventDispatcher;
using DG.Tweening;

public class GameplayUIController : MonoBehaviour
{
    [SerializeField] private Text levelText;

    [SerializeField] private Button settingButton;
    [SerializeField] private Transform settingPanel;
    [SerializeField] private Button musicToggleButton;
    [SerializeField] private Button soundToggleButton;
    [SerializeField] private Button vibrationToggleButton;

    [SerializeField] private Button replayButton;

    public Button randomizeButton;
    public Button hintButton;
    [SerializeField] private Text randomizeCountText;
    [SerializeField] private Text hintCountText;
    [SerializeField] private Image randomizeAdIcon;
    [SerializeField] private Image hintAdIcon;
    [SerializeField] private GameObject hintContainer;
    [SerializeField] private Transform hintCircle;
    [SerializeField] private Transform randomizeCircle;

    [SerializeField] private Image powerupOpt1AdIcon;
    [SerializeField] private Image powerupOpt2AdIcon;


    [SerializeField] private Button resetToLv1Button;
    [SerializeField] private Button skipLevelButton;
    [SerializeField] private Button printBoardButton;
    [SerializeField] private Button cheatAdButton;
    [SerializeField] private Text cheatAdText;
    [SerializeField] private Button playLevelButton;
    [SerializeField] private InputField levelInputField;

    [SerializeField] private Button powerup1Button;
    [SerializeField] private Button powerup2Button;
    [SerializeField] private GameObject DISBALE_PANEL;

    public Transform catHome1;
    public Transform catHome2;
    public Transform catHomeHeart;

    public Transform hand;

    private bool isSettingPanelOpening = false;
    private bool existHintOnBoard = false;

    public float timeSincePlayerNotActive = 0f;
    public bool enablePowerUpInteraction;

    public void Initialize()
    {
        this.RegisterListener(EventID.MOVE_COMPLETED, (param) => OnMoveCompleted());

        settingButton.onClick.AddListener(() => OnClickSettingButton());

        musicToggleButton.onClick.AddListener(() => OnClickToggleMusicButton());
        soundToggleButton.onClick.AddListener(() => OnClickToggleSoundButton());
        vibrationToggleButton.onClick.AddListener(() => OnClickToggleVibrationButton());

        replayButton.onClick.AddListener(() => OnClickReplayButton());
        randomizeButton.onClick.AddListener(() => OnRandomizeButtonClicked());
        hintButton.onClick.AddListener(() => OnHintButtonClicked());
        resetToLv1Button.onClick.AddListener(() => OnResetToLv1ButtonClicked());
        skipLevelButton.onClick.AddListener(() => OnSkipLevelButton());
        printBoardButton.onClick.AddListener(() => OnPrintBoardButton());
        cheatAdButton.onClick.AddListener(() => OnClickCheatADButton());
        playLevelButton.onClick.AddListener(() => OnClickPlayLevelButton());

        Debug.Log("--- SOUND PLAYERPREFS ---");
        Debug.Log(GameController.Instance.useProfile.OnMusic);
        Debug.Log(GameController.Instance.useProfile.OnSound);
        Debug.Log(UseProfile.OnVibration);
        musicToggleButton.GetComponent<Image>().sprite = (GameController.Instance.useProfile.OnMusic) ? GameAssets.Instance.musicOn : GameAssets.Instance.musicOff;
        soundToggleButton.GetComponent<Image>().sprite = (GameController.Instance.useProfile.OnSound) ? GameAssets.Instance.soundOn : GameAssets.Instance.soundOff;
        vibrationToggleButton.GetComponent<Image>().sprite = (UseProfile.OnVibration) ? GameAssets.Instance.vibrationOn : GameAssets.Instance.vibrationOff;

        cheatAdText.text = String.Format("CheatAD:<color={0}>{1}</color>", (UseProfile.IsCheatAd) ? "green" : "red", (UseProfile.IsCheatAd) ? "ON" : "OFF");

        UpdatePowerup();

        SetupAdditionalPowerups();

        enablePowerUpInteraction = true;
    }

    private void Start()
    {
        powerupEffectCoroutine = PowerUpEffectAnimate();
        Initialize();
    }

    private void Update()
    {
        timeSincePlayerNotActive += Time.deltaTime;

        if (timeSincePlayerNotActive > 5f && isPowerupEffectOn == false)
        {
            Debug.Log("Enter powerup coroutine");
            isPowerupEffectOn = true;
            StartCoroutine(powerupEffectCoroutine);
        }

        if (Input.GetMouseButtonDown(0))
        {
            isPowerupEffectOn = false;
            StopCoroutine(powerupEffectCoroutine);
            timeSincePlayerNotActive = 0;
            UpdatePowerup();
        }
    }

    private IEnumerator powerupEffectCoroutine;
    private bool isPowerupEffectOn = false;
    private bool enableHintPowerupEffect = true;

    private IEnumerator PowerUpEffectAnimate()
    {
        if (UseProfile.CurrentLevel < 3)
        {
            yield break;
        }
        while (isPowerupEffectOn)
        {
            randomizeButton.GetComponent<RectTransform>().DOScale(1.2f, .15f).OnComplete(() =>
            {
                randomizeButton.GetComponent<RectTransform>().DOScale(.8f, .25f).OnComplete(() =>
                {
                    randomizeButton.GetComponent<RectTransform>().DOScale(1f, .15f);
                });
            });
            if (enableHintPowerupEffect)
            {
                hintButton.GetComponent<RectTransform>().DOScale(1.2f, .15f).OnComplete(() =>
                {
                    hintButton.GetComponent<RectTransform>().DOScale(.8f, .25f).OnComplete(() =>
                    {
                        hintButton.GetComponent<RectTransform>().DOScale(1f, .15f);
                    });
                });
            }
            yield return new WaitForSecondsRealtime(.75f);
        }
    }

    private void ResetPowerupScale()
    {
        randomizeButton.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        hintButton.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    private void OnRandomizeButtonClicked()
    {
        Debug.Log("mouse click on randomize btn");
        if (!enablePowerUpInteraction) return;
        GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);
        if (GameController.Instance.useProfile.CurrentRandomizePowerup > 0)
        {
            bool isRandomizeSuccess = GameplayController.Instance.level.board.RandomizeBoard();
            if (isRandomizeSuccess)
            {
                GameController.Instance.useProfile.CurrentRandomizePowerup -= 1;
                ToggleHint(true);
            }
            else
            {
                GuidelinePopup.Setup("You cannot randomize board\nThere is only one type of cat remains.").Show();
                Debug.Log("Current board remains same cats, cannot randomize !");
            }
        }
        else
        {
            if (UseProfile.IsCheatAd)
            {
                GameController.Instance.useProfile.CurrentRandomizePowerup += 3;
                if(GameController.Instance.useProfile.CurrentRandomizePowerup > 3)
                {
                    GameController.Instance.useProfile.CurrentRandomizePowerup = 3;
                }
                UpdatePowerup();
                return;
            }
            GameController.Instance.admobAds.ShowVideoReward(
                actionReward: () =>
                {
                    GameController.Instance.useProfile.CurrentRandomizePowerup += 3;
                    if (GameController.Instance.useProfile.CurrentRandomizePowerup > 3)
                    {
                        GameController.Instance.useProfile.CurrentRandomizePowerup = 3;
                    }
                    UpdatePowerup();
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
                    UpdatePowerup();
                },
                actionClose: null,
                ActionWatchVideo.None,
                GameController.Instance.useProfile.CurrentLevelPlay.ToString()
            );
        }
        UpdatePowerup();
    }

    private void OnHintButtonClicked()
    {
        Debug.Log("mouse click on hint btn");
        if (!enablePowerUpInteraction) return;
        GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);
        if (GameController.Instance.useProfile.CurrentHintPowerup > 0)
        {
            Vector2Int p1, p2;
            GameplayController.Instance.level.board.GetTwoCellsCanBeConnectPriotizeBomb(out p1, out p2);
            if(p1 == new Vector2Int(-1, -1) || p2 == new Vector2Int(-1, -1))
            {
                GameplayController.Instance.level.board.GetTwoCellsCanBeConnect(out p1, out p2);
            }
            //if found
            if (p1 != new Vector2Int(-1, -1) && p2 != new Vector2Int(-1, -1))
            {
                GameController.Instance.useProfile.CurrentHintPowerup -= 1;
                DoHint(p1, p2);
            }
            else
            {
                GuidelinePopup.Setup("There is not hint available\nYou can use random powerup or retry !").Show();
                Debug.Log("Current board exists no hints !");
            }
        }
        else
        {
            if (UseProfile.IsCheatAd)
            {
                GameController.Instance.useProfile.CurrentHintPowerup += 3;
                if(GameController.Instance.useProfile.CurrentHintPowerup > 3)
                {
                    GameController.Instance.useProfile.CurrentHintPowerup = 3;
                }
                UpdatePowerup();
                return;
            }
            GameController.Instance.admobAds.ShowVideoReward(
                actionReward: () =>
                {
                    GameController.Instance.useProfile.CurrentHintPowerup += 3;
                    if (GameController.Instance.useProfile.CurrentHintPowerup > 3)
                    {
                        GameController.Instance.useProfile.CurrentHintPowerup = 3;
                    }
                    UpdatePowerup();
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
                    UpdatePowerup();
                },
                actionClose: null,
                ActionWatchVideo.None,
                GameController.Instance.useProfile.CurrentLevelPlay.ToString()
            );
        }
        UpdatePowerup();
    }

    private void DoHint(Vector2Int p1, Vector2Int p2)
    {
        existHintOnBoard = true;

        SetupHintEffect(p1, p2);
        /*GameplayController.Instance.level.board.GetCellFromPosition(p1).ToggleCatZoomEffect(true);
        GameplayController.Instance.level.board.GetCellFromPosition(p2).ToggleCatZoomEffect(true);
        RectTransform rectTransform = GameplayController.Instance.level.board.cellGameObjectsInScene[p1.x, p1.y].transform.Find("Cat").GetChild(0).Find("root").Find("SHADOW").GetComponent<RectTransform>();
        var scale = rectTransform.localScale;
        scale.x = 1;
        scale.y = 1;
        rectTransform.localScale = scale;
        RectTransform rectTransform2 = GameplayController.Instance.level.board.cellGameObjectsInScene[p2.x, p2.y].transform.Find("Cat").GetChild(0).Find("root").Find("SHADOW").GetComponent<RectTransform>();
        scale = rectTransform2.localScale;
        scale.x = 1;
        scale.y = 1;
        rectTransform2.localScale = scale;*/
    }

    public bool enableHintCoroutine = true;

    private void SetupHintEffect(Vector2Int p1, Vector2Int p2)
    {
        Vector3 pos1 = GameplayController.Instance.level.board.cellWorldPointPositions[p1.x, p1.y];
        Vector3 pos2 = GameplayController.Instance.level.board.cellWorldPointPositions[p2.x, p2.y];
        enableHintCoroutine = true;
        hand.DOKill();
        hand.gameObject.SetActive(true);
        HandleHand(p1, p2, pos1, pos2);
    }

    public void DeleteHintEffect()
    {
        hand.gameObject.SetActive(false);
        enableHintCoroutine = false;
        hand.DOKill();
    }

    private bool moveDir = true;

    private void HandleHand(Vector2Int p1, Vector2Int p2, Vector3 pos1, Vector3 pos2)
    {
        if (enableHintCoroutine)
        {
            hand.transform.position = (moveDir) ? pos1 : pos2;

            float handSpeed = 3f;
            float moveTime = Vector2.Distance(pos1, pos2) / handSpeed;
            Mathf.Clamp(moveTime, 4f, 6f);
            if (moveDir)
            {
                GameplayController.Instance.level.board.GetCellFromPosition(p1).ToggleCatZoomEffect(true);
                GameplayController.Instance.level.board.GetCellFromPosition(p2).ToggleCatZoomEffect(false);
                GameplayController.Instance.level.board.GetCellFromPosition(p2).ResetCellScale();
            }
            else
            {
                GameplayController.Instance.level.board.GetCellFromPosition(p2).ToggleCatZoomEffect(true);
                GameplayController.Instance.level.board.GetCellFromPosition(p1).ToggleCatZoomEffect(false);
                GameplayController.Instance.level.board.GetCellFromPosition(p1).ResetCellScale();
            }
            hand.transform.DOMove((moveDir) ? pos2 : pos1, moveTime).OnComplete(delegate
            {
                if (enableHintCoroutine)
                {
                    moveDir = !moveDir;
                    HandleHand(p1, p2, pos1, pos2);
                }
            });
        }
    }

    public void ToggleHint(bool value)
    {
        if(value == true)
        {
            hintButton.interactable = true;

            hintContainer.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            hintButton.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            hintAdIcon.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            hintCircle.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            hintCircle.Find("Text").GetComponent<Text>().color = new Color(1f, 1f, 1f, 1f);

            enableHintPowerupEffect = true;

        }
        else
        {
            hintButton.interactable = false;

            hintContainer.GetComponent<Image>().color = new Color(0.55f, 0.55f, 0.55f, 1f);
            hintButton.GetComponent<Image>().color = new Color(0.55f, 0.55f, 0.55f, 1f);
            hintAdIcon.GetComponent<Image>().color = new Color(0.55f, 0.55f, 0.55f, 1f);
            hintCircle.GetComponent<Image>().color = new Color(0.55f, 0.55f, 0.55f, 1f);
            hintCircle.Find("Text").GetComponent<Text>().color = new Color(0.55f, 0.55f, 0.55f, 1f);

            enableHintPowerupEffect = false;
        }
    }

    private void SetupAdditionalPowerups()
    {
        int currentPowerup = 0;
        if (GameplayController.Instance.level.board.CheckBoardHasThisItem(CELL_ITEM_TYPE.Bomb))
        {
            SetupPowerupUI(CELL_ITEM_TYPE.Bomb, 1);
            currentPowerup++;
        }
        if (GameplayController.Instance.level.board.CheckBoardHasThisItem(CELL_ITEM_TYPE.Cage))
        {
            SetupPowerupUI(CELL_ITEM_TYPE.Cage, (currentPowerup == 0) ? 1 : 2);
            currentPowerup++;
        }
        if (currentPowerup <= 1 && GameplayController.Instance.level.board.CheckBoardHasThisItem(CELL_ITEM_TYPE.Box))
        {
            SetupPowerupUI(CELL_ITEM_TYPE.Box, (currentPowerup == 0) ? 1 : 2);
            currentPowerup++;
        }

        if (currentPowerup == 0)
        {
            SetupPowerupUI(CELL_ITEM_TYPE.None, 1);
            SetupPowerupUI(CELL_ITEM_TYPE.None, 2);
            powerup1Button.enabled = false;
            powerup2Button.enabled = false;
        }
        else if (currentPowerup == 1)
        {
            SetupPowerupUI(CELL_ITEM_TYPE.None, 2);
            powerup2Button.enabled = false;
        }
    }

    private void SetupPowerupUI(CELL_ITEM_TYPE itemType, int index)
    {
        switch (itemType)
        {
            case CELL_ITEM_TYPE.Bomb:
                {
                    Button btn = (index == 1) ? powerup1Button : powerup2Button;
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(OnClickDestroyBombPowerup);
                    btn.transform.GetComponent<Image>().sprite = GameAssets.Instance.destroyBombPowerup;
                    break;
                }
            case CELL_ITEM_TYPE.Box:
                {
                    Button btn = (index == 1) ? powerup1Button : powerup2Button;
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(OnClickDestroyBoxPowerup);
                    btn.transform.GetComponent<Image>().sprite = GameAssets.Instance.destroyBoxPowerup;
                    break;
                }
            case CELL_ITEM_TYPE.Cage:
                {
                    Button btn = (index == 1) ? powerup1Button : powerup2Button;
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(OnClickDestroyCagePowerup);
                    btn.transform.GetComponent<Image>().sprite = GameAssets.Instance.destroyCagePowerup;
                    break;
                }
            case CELL_ITEM_TYPE.None:
                {
                    Button btn = (index == 1) ? powerup1Button : powerup2Button;
                    btn.onClick.RemoveAllListeners();
                    btn.transform.GetComponent<Image>().sprite = GameAssets.Instance.lockPowerup;
                    break;
                }
        }
        if(itemType == CELL_ITEM_TYPE.None)
        {
            return;
        }
        if(index == 1)
        {
            powerupOpt1AdIcon.enabled = true;
        }
        else
        {
            powerupOpt2AdIcon.enabled = true;
        }
    }

    private void OnClickDestroyBombPowerup()
    {
        if (!enablePowerUpInteraction) return;
        GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);

        if (UseProfile.IsCheatAd)
        {
            if (GameplayController.Instance.level.board.CheckBoardHasThisItem(CELL_ITEM_TYPE.Bomb))
            {
                Debug.Log("Destroy all bombs");
                GameplayController.Instance.level.board.DestroyAllBombs();
            }
            return;
        }

        if (GameplayController.Instance.level.board.CheckBoardHasThisItem(CELL_ITEM_TYPE.Bomb))
        {
            GameController.Instance.admobAds.ShowVideoReward(
            actionReward: () =>
            {
                if (GameplayController.Instance.level.board.CheckBoardHasThisItem(CELL_ITEM_TYPE.Bomb))
                {
                    Debug.Log("Destroy all bombs");
                    GameplayController.Instance.level.board.DestroyAllBombs();
                }
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
                UpdatePowerup();
            },
            actionClose: null,
            ActionWatchVideo.None,
            GameController.Instance.useProfile.CurrentLevelPlay.ToString()
        );
        }
    }

    private void OnClickDestroyBoxPowerup()
    {
        if (!enablePowerUpInteraction) return;
        GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);

        if (UseProfile.IsCheatAd)
        {
            if (GameplayController.Instance.level.board.CheckBoardHasThisItem(CELL_ITEM_TYPE.Box))
            {
                Debug.Log("Open all boxes");
                GameplayController.Instance.level.board.OpenAllBoxes();
            }
            return;
        }

        if (GameplayController.Instance.level.board.CheckBoardHasThisItem(CELL_ITEM_TYPE.Box))
        {
            GameController.Instance.admobAds.ShowVideoReward(
            actionReward: () =>
            {
                if (GameplayController.Instance.level.board.CheckBoardHasThisItem(CELL_ITEM_TYPE.Box))
                {
                    Debug.Log("Open all boxes");
                    GameplayController.Instance.level.board.OpenAllBoxes();
                }
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
                UpdatePowerup();
            },
            actionClose: null,
            ActionWatchVideo.None,
            GameController.Instance.useProfile.CurrentLevelPlay.ToString()
        );
        }
    }

    private void OnClickDestroyCagePowerup()
    {
        if (!enablePowerUpInteraction) return;
        GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);

        if (UseProfile.IsCheatAd)
        {
            if (GameplayController.Instance.level.board.CheckBoardHasThisItem(CELL_ITEM_TYPE.Cage))
            {
                Debug.Log("Open all cages");
                GameplayController.Instance.level.board.OpenAllCages();
            }
            return;
        }

        if (GameplayController.Instance.level.board.CheckBoardHasThisItem(CELL_ITEM_TYPE.Cage))
        {
            GameController.Instance.admobAds.ShowVideoReward(
            actionReward: () =>
            {
                if (GameplayController.Instance.level.board.CheckBoardHasThisItem(CELL_ITEM_TYPE.Cage))
                {
                    Debug.Log("Destroy all cages");
                    GameplayController.Instance.level.board.OpenAllCages();
                }
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
                UpdatePowerup();
            },
            actionClose: null,
            ActionWatchVideo.None,
            GameController.Instance.useProfile.CurrentLevelPlay.ToString()
            );
        }
    }

    private void UpdatePowerup()
    {
        if (UseProfile.CurrentLevel < 3)
        {
            hintButton.transform.GetComponent<Image>().sprite = GameAssets.Instance.lockPowerup;
            hintButton.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(122, 122);
            hintButton.enabled = false;
            hintAdIcon.enabled = false;
            hintCircle.gameObject.SetActive(false);
            randomizeButton.transform.GetComponent<Image>().sprite = GameAssets.Instance.lockPowerup;
            randomizeButton.enabled = false;
            randomizeAdIcon.enabled = false;
            randomizeCircle.gameObject.SetActive(false);
        }
        randomizeCountText.text = (GameController.Instance.useProfile.CurrentRandomizePowerup <= 0) ? "+" : GameController.Instance.useProfile.CurrentRandomizePowerup.ToString();
        hintCountText.text = (GameController.Instance.useProfile.CurrentHintPowerup <= 0) ? "+" : GameController.Instance.useProfile.CurrentHintPowerup.ToString();
        randomizeAdIcon.enabled = (GameController.Instance.useProfile.CurrentRandomizePowerup <= 0) ? true : false;
        hintAdIcon.enabled = (GameController.Instance.useProfile.CurrentHintPowerup <= 0) ? true : false;
    }

    public void SetUpLevelText(string levelName)
    {
        levelText.text = "Level " + levelName;
    }

    public void OnMoveCompleted()
    {
        if (existHintOnBoard)
        {
            RemoveAllHintUIOnCells();
            existHintOnBoard = false;
        }
    }

    public void RemoveAllHintUIOnCells()
    {
        for (int i = 1; i <= GameplayController.Instance.level.board.boardHeight; i++)
        {
            for (int j = 1; j <= GameplayController.Instance.level.board.boardWidth; j++)
            {
                if (!GameplayController.Instance.level.board.GetCellFromPosition(new Vector2Int(i, j)).IsCellEmpty())
                {
                    if (!GameplayController.Instance.level.board.GetCellFromPosition(new Vector2Int(i, j)).CheckHasBlockItemsInThisCell())
                    {
                        GameplayController.Instance.level.board.GetCellFromPosition(new Vector2Int(i, j)).ToggleCatZoomEffect(false);
                    }
                    if (new Vector2Int(i, j) != GameplayController.Instance.boardUserInput.firstCell)
                    {
                        GameplayController.Instance.level.board.GetCellFromPosition(new Vector2Int(i, j)).ResetCellScale();
                        GameplayController.Instance.boardUserInput.UnhighlightCat(new Vector2Int(i, j));
                    }
                }
            }
        }
    }

    private void OnClickSettingButton()
    {
        if (UseProfile.IsCheatAd)
        {
            GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);
            if (!isSettingPanelOpening)
            {
                settingPanel.GetComponent<Animator>().runtimeAnimatorController = GameAssets.Instance.openPanelAnimator;
                isSettingPanelOpening = true;
            }
            else
            {
                settingPanel.GetComponent<Animator>().runtimeAnimatorController = GameAssets.Instance.closePanelAnimator;
                isSettingPanelOpening = false;
            }
            return;
        }
       // GameController.Instance.admobAds.ShowInterstitial(actionIniterClose: () =>
       // {
            GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);
            if (!isSettingPanelOpening)
            {
                settingPanel.GetComponent<Animator>().runtimeAnimatorController = GameAssets.Instance.openPanelAnimator;
                isSettingPanelOpening = true;
            }
            else
            {
                settingPanel.GetComponent<Animator>().runtimeAnimatorController = GameAssets.Instance.closePanelAnimator;
                isSettingPanelOpening = false;
            }
       // });
    }

    private void OnClickToggleMusicButton()
    {
        if (UseProfile.IsCheatAd)
        {
            GameController.Instance.soundController.ToggleMusic();
            musicToggleButton.GetComponent<Image>().sprite = (GameController.Instance.useProfile.OnMusic) ? GameAssets.Instance.musicOn : GameAssets.Instance.musicOff;
            GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);
            return;
        }
        //GameController.Instance.admobAds.ShowInterstitial(actionIniterClose: () =>
       // {
            GameController.Instance.soundController.ToggleMusic();
            musicToggleButton.GetComponent<Image>().sprite = (GameController.Instance.useProfile.OnMusic) ? GameAssets.Instance.musicOn : GameAssets.Instance.musicOff;
            GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);
       // });
    }
    private void OnClickToggleSoundButton()
    {
        if (UseProfile.IsCheatAd)
        {
            GameController.Instance.soundController.ToggleSound();
            soundToggleButton.GetComponent<Image>().sprite = (GameController.Instance.useProfile.OnSound) ? GameAssets.Instance.soundOn : GameAssets.Instance.soundOff;
            GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);
            return;
        }
       // GameController.Instance.admobAds.ShowInterstitial(actionIniterClose: () =>
        //{
            GameController.Instance.soundController.ToggleSound();
            soundToggleButton.GetComponent<Image>().sprite = (GameController.Instance.useProfile.OnSound) ? GameAssets.Instance.soundOn : GameAssets.Instance.soundOff;
            GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);
        //});
    }

    private void OnClickToggleVibrationButton()
    {
        if (UseProfile.IsCheatAd)
        {
            GameController.Instance.soundController.ToggleVibration();
            vibrationToggleButton.GetComponent<Image>().sprite = (UseProfile.OnVibration) ? GameAssets.Instance.vibrationOn : GameAssets.Instance.vibrationOff;
            GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);
            return;
        }
       // GameController.Instance.admobAds.ShowInterstitial(actionIniterClose: () =>
       // {
            GameController.Instance.soundController.ToggleVibration();
            vibrationToggleButton.GetComponent<Image>().sprite = (UseProfile.OnVibration) ? GameAssets.Instance.vibrationOn : GameAssets.Instance.vibrationOff;
            GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);
        //});
    }

    private void OnClickReplayButton()
    {
        if (UseProfile.IsCheatAd)
        {
            Debug.Log("Replay button clicked");
            GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            return;
        }
        GameController.Instance.admobAds.ShowInterstitial(actionIniterClose: () =>
        {
            Debug.Log("Replay button clicked");
            GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        });
    }

    public void Toggle_DISABLE_PANEL()
    {
        DISBALE_PANEL.SetActive(true);
    }

    #region Debug Buttons

    private void OnResetToLv1ButtonClicked()
    {
        GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);

        UseProfile.CurrentLevel = 1;

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    private void OnSkipLevelButton()
    {
        GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.ButtonNormal);
        GameplayController.Instance.level.board.OnWinLevel(true);
    }

    private void OnPrintBoardButton()
    {
        Debug.Log("IS_DISABLE: " + GameplayController.Instance.level.board.isCompletelyDisableControl);
        Utility.DebugIntegerMatrix(GameplayController.Instance.level.board.boardIntergerMatrix);
    }

    private void OnClickCheatADButton()
    {
        bool currentAdCheat = UseProfile.IsCheatAd;
        currentAdCheat = !currentAdCheat;
        UseProfile.IsCheatAd = currentAdCheat;
        cheatAdText.text = String.Format("CheatAD:<color={0}>{1}</color>", (UseProfile.IsCheatAd) ? "green" : "red", (UseProfile.IsCheatAd) ? "ON" : "OFF");
    }

    private void OnClickPlayLevelButton()
    {
        try
        {
            int levelNum = 1;
            Int32.TryParse(levelInputField.text, out levelNum);
            UseProfile.CurrentLevel = levelNum;
            GameController.Instance.LoadScene("_gameplay");
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    #endregion
}

