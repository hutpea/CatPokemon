using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using DG.Tweening.Core;
using DG.Tweening;
using EventDispatcher;

public class CatMoveEffectManager : MonoBehaviour
{
    private SkeletonGraphic skeletonGraphic;

    public AnimationReferenceAsset idle;
    public AnimationReferenceAsset jumping;
    public AnimationReferenceAsset grounding;

    private Transform mainCanvas;
    private Vector2 beginPoint;
    private Vector2 endPoint;
    private float catSpeed;
    private CatAnimationData animationData;

    private Transform itemContainer;
    private GameObject bomb;
    private GameObject key;

    Transform home;

    public static CatMoveEffectManager Create(Transform cellTransform, CAT_TYPE catType, Vector2 beginPoint, Vector2 endPoint, float catSpeed, Transform home)
    {
        Transform catMoveEffectManagerTransform = Instantiate(GameAssets.Instance.catMoveEffectPrefab, beginPoint, Quaternion.identity, cellTransform);

        CatMoveEffectManager catMoveEffectManager = catMoveEffectManagerTransform.GetComponent<CatMoveEffectManager>();
        catMoveEffectManager.Setup(beginPoint, endPoint, catSpeed, Utility.GetCatAnimationData(catType), home);
        return catMoveEffectManager;
    }

    private void Awake()
    {
        skeletonGraphic = GetComponent<SkeletonGraphic>();
        mainCanvas = GameObject.FindGameObjectWithTag("GameplayCanvas").transform;
        itemContainer = transform.Find("Items");
        bomb = itemContainer.Find("Bomb").gameObject;
        key = itemContainer.Find("Key").gameObject;
        bomb.SetActive(false);
        key.SetActive(false);
    }

    private void Setup(Vector2 beginPoint, Vector2 endPoint, float catSpeed, CatAnimationData animationData, Transform home)
    {
        this.animationData = animationData;
        this.beginPoint = beginPoint;
        this.endPoint = endPoint;
        this.catSpeed = catSpeed;
        this.home = home;

        skeletonGraphic.skeletonDataAsset = animationData.skeletonDataAsset;
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();

        this.idle = animationData.idle;
        this.jumping = animationData.jumping;
        this.grounding = animationData.grounding;

        if (beginPoint.x > endPoint.x)
        {
            //Reverse transform
            var tempScale = this.transform.localScale;
            tempScale.x *= -1;
            this.transform.localScale = tempScale;
        }

        Move();
    }

    private void Move()
    {
        this.transform.DOKill();
        float moveTime = Vector2.Distance(this.transform.position, endPoint) / catSpeed;

        SetAnimation(idle, false, 1);

        this.transform.DOJump(endPoint, 0.25f, 1, moveTime, false).SetDelay(0.12f).OnStart(() => OnMoveStart()).OnComplete(() =>
        {
            SetAnimation(grounding, false, 1);
            transform.SetParent(this.home);
            GameplayController.Instance.catHomeController.AddCat();
            //StartCoroutine(DelayFunction());
            StartCoroutine(DestroyAfter(grounding.Animation.Duration + 0.4f));
        });
    }

    IEnumerator DelayFunction()
    {
        yield return new WaitForSeconds(1.5f);
    }

    private void OnMoveStart()
    {
        SetAnimation(jumping, false, 1);
    }

    private void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        skeletonGraphic.AnimationState.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }

    private IEnumerator DestroyAfter(float time)
    {
        yield return new WaitForSeconds(time);
        GameplayController.Instance.catHomeController.RemoveCat();
        transform.GetChild(0).gameObject.SetActive(false);
        GetComponent<SkeletonGraphic>().enabled = false;
        yield return new WaitForSeconds(1.25f);
        Debug.Log("ENABLE-POWER-UP ON");
        GameplayController.Instance.gameplayUIController.enablePowerUpInteraction = true;
        Destroy(this.gameObject);
    }

    public void DisplayBomb(int counter)
    {
        bomb.SetActive(true);
        bomb.transform.Find("Counter").GetComponent<Text>().text = counter.ToString();
    }
    public void DisplayKey(int id)
    {
        Sprite keySprite;
        switch (id)
        {
            case 1:
                {
                    keySprite = GameAssets.Instance.key1;
                    break;
                }
            case 2:
                {
                    keySprite = GameAssets.Instance.key2;
                    break;
                }
            case 3:
                {
                    keySprite = GameAssets.Instance.key3;
                    break;
                }
            default:
                {
                    keySprite = GameAssets.Instance.key4;
                    break;
                }
        }
        key.GetComponent<Image>().sprite = keySprite;
        key.SetActive(true);
    }

    public void FlipItems()
    {
        var scale = itemContainer.GetComponent<RectTransform>().localScale;
        scale.x *= -1;
        itemContainer.GetComponent<RectTransform>().localScale = scale;
    }
}
