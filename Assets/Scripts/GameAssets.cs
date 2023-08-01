using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;
    public static GameAssets Instance
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    private void Start()
    {
    }

    #region "Game Resources"
    public GameObject cellPrefab;
    public Transform catMoveEffectPrefab;
    public Transform heartEffectPrefab;
    public Transform cutOutCirclePrefab;
    public Transform lineHolderPrefab;
    public Transform pathMoveControllerPrefab;
    public Transform heartParticlePrefab;
    public Transform heartParticleUIPrefab;
    public Transform hintHand;

    public Transform cellBackground;
    public Sprite grid_0;
    public Sprite grid_1;

    public Transform bombPrefab;
    public Transform cagePrefab;
    public Transform keyPrefab;
    public Transform boxPrefab;

    public SkeletonDataAsset bombSkeletonDataAsset;
    public SkeletonDataAsset boxSkeletonDataAsset;
    public SkeletonDataAsset cagekeySkeletonDataAsset;

    public Sprite destroyBombPowerup;
    public Sprite destroyBoxPowerup;
    public Sprite destroyCagePowerup;
    public Sprite lockPowerup;

    public Sprite musicOn;
    public Sprite musicOff;
    public Sprite soundOn;
    public Sprite soundOff;
    public Sprite vibrationOn;
    public Sprite vibrationOff;

    public Sprite lock1;
    public Sprite lock2;
    public Sprite lock3;
    public Sprite lock4;
    public Sprite key1;
    public Sprite key2;
    public Sprite key3;
    public Sprite key4;
    public Sprite spyHatLeft;
    public Sprite spyHatRight;
    public Sprite spyHatUp;
    public Sprite spyHatDown;

    public ParticleSystem cellDestroyedEffect;

    public List<CatAnimationData> catAnimationDatas;

    public RuntimeAnimatorController openPanelAnimator;
    public RuntimeAnimatorController closePanelAnimator;

    public List<GameObject> catPrefabs;
    public int i;
    #endregion
}
