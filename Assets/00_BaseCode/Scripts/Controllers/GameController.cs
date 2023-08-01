using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif


public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public MoneyEffectController moneyEffectController;
    public UseProfile useProfile;
    public DataContain dataContain;
    public MusicManager musicManager;
    public AdmobAds admobAds;
    public AnalyticsController AnalyticsController;
    public IapController iapController;
    public SoundController soundController;

    [HideInInspector] public SceneType currentScene;

    protected void Awake()
    {
        Instance = this;
        Init();
        DontDestroyOnLoad(this);
        //GameController.Instance.UseProfile.IsRemoveAds = true;

#if UNITY_IOS

    if(ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == 
    ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
    {

        ATTrackingStatusBinding.RequestAuthorizationTracking();

    }

#endif

    }

    private void Start()
    {
        //musicManager.PlayBGMusic();
    }

    public void Init()
    {
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = false;

        soundController.Init();
        soundController.PlaySound(AUDIO_CLIP_TYPE.BGMusic1);

        //UseProfile.IsRemoveAds = true;
        //UseProfile.CurrentLevelPlay = UseProfile.CurrentLevel;
        admobAds.Init();
        //musicManager.Init();
        iapController.Init();

        MMVibrationManager.SetHapticsActive(UseProfile.OnVibration);
        //GameController.Instance.admobAds.ShowBanner();

        dataContain.Initialize();
        // LoadScene("_gameplay");
    }

    public void LoadScene(string sceneName)
    {
        Initiate.Fade(sceneName.ToString(), Color.black, 2f);
    }

    public static void SetUserProperties()
    {
        if (UseProfile.IsFirstTimeInstall)
        {
            UseProfile.FirstTimeOpenGame = UnbiasedTime.Instance.Now();
            UseProfile.LastTimeOpenGame = UseProfile.FirstTimeOpenGame;
            UseProfile.IsFirstTimeInstall = false;
        }

        var lastTimeOpen = UseProfile.LastTimeOpenGame;
        UseProfile.RetentionD = (UseProfile.FirstTimeOpenGame - UnbiasedTime.Instance.Now()).Days;

        var dayPlayerd = (TimeManager.ParseTimeStartDay(UnbiasedTime.Instance.Now()) - TimeManager.ParseTimeStartDay(UseProfile.LastTimeOpenGame)).Days;
        if (dayPlayerd >= 1)
        {
            UseProfile.LastTimeOpenGame = UnbiasedTime.Instance.Now();
            UseProfile.DaysPlayed++;
        }

        AnalyticsController.SetUserProperties();
    }

    public bool IsShowRate()
    {
        if (!UseProfile.CanShowRate)
            return false;
        int X = UseProfile.CurrentLevel - 1;
        Debug.Log("CURRENT_LEVEL:" + X + ", " + RemoteConfigController.GetFloatConfig(FirebaseConfig.LEVEL_START_SHOW_RATE, 5).ToString() + ", " + RemoteConfigController.GetIntConfig(FirebaseConfig.MAX_LEVEL_SHOW_RATE, 30).ToString());
        if (X < RemoteConfigController.GetFloatConfig(FirebaseConfig.LEVEL_START_SHOW_RATE, 5))
            return false;
        if (X == RemoteConfigController.GetFloatConfig(FirebaseConfig.LEVEL_START_SHOW_RATE, 5) || (X <= RemoteConfigController.GetIntConfig(FirebaseConfig.MAX_LEVEL_SHOW_RATE, 30) && X % 10 == 0))
        {
            return true;
        }

        return false;
    }
}
public enum SceneType
{
    StartLoading = 0,
    MainHome = 1,
    GamePlay = 2
}