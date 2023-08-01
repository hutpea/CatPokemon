using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class TutorialBox : MonoBehaviour
{
    public RectTransform hand;
    public Transform cat1;
    public Transform cat2;

    public RectTransform cat1Rect;
    public RectTransform cat2Rect;
    public RectTransform cat1ShadowRect;
    public RectTransform cat2ShadowRect;

    public List<Transform> pathList;

    public Transform cat3;
    public Transform cat4;

    public SkeletonGraphic box1SkeletonGraphic;
    public SkeletonGraphic box2SkeletonGraphic;
    Spine.Animation closeAnim;
    Spine.Animation openingAnim;
    Spine.Animation openAnim;

    public Button playButton;

    public bool isAnim = true;

    private void Start()
    {
        closeAnim = GameAssets.Instance.boxSkeletonDataAsset.GetAnimationStateData().SkeletonData.FindAnimation("1-close");
        openingAnim = GameAssets.Instance.boxSkeletonDataAsset.GetAnimationStateData().SkeletonData.FindAnimation("2-opening");
        openAnim = GameAssets.Instance.boxSkeletonDataAsset.GetAnimationStateData().SkeletonData.FindAnimation("3-open");
        playButton.onClick.AddListener(delegate { OnClickPlayButton(); });
        AnimLoop();
    }

    public void AnimLoop()
    {
        if (isAnim)
        {
            hand.transform.position = cat1.position;
            cat1Rect.localScale = new Vector3(1, 1, 1);
            cat2Rect.localScale = new Vector3(1, 1, 1);
            cat1ShadowRect.localScale = new Vector3(0, 0, 1);
            cat2ShadowRect.localScale = new Vector3(0, 0, 1);
            cat3.gameObject.SetActive(false);
            cat4.gameObject.SetActive(false);

            box1SkeletonGraphic.transform.gameObject.SetActive(true);
            box2SkeletonGraphic.transform.gameObject.SetActive(true);

            box1SkeletonGraphic.Initialize(true);
            box1SkeletonGraphic.SetMaterialDirty();
            box1SkeletonGraphic.AnimationState.SetAnimation(0, closeAnim, true);
            box2SkeletonGraphic.Initialize(true);
            box2SkeletonGraphic.SetMaterialDirty();
            box2SkeletonGraphic.AnimationState.SetAnimation(0, closeAnim, true);

            hand.DOScale(1.2f, .25f).OnComplete(() =>
            {
                cat1Rect.DOScale(1.15f, .5f);
                cat1ShadowRect.localScale = new Vector3(1, 1, 1);
                //cat1ShadowRect.DOScale(1.15f, .5f);
                hand.DOScale(1f, .25f).OnComplete(() =>
                {
                    hand.DOScale(1f, .25f).OnComplete(() =>
                    {
                        hand.DOMove(cat2.position, 1f).OnComplete(() =>
                        {
                            hand.DOScale(1.2f, .25f).OnComplete(() =>
                            {
                                cat2Rect.DOScale(1.15f, .5f);
                                cat2ShadowRect.localScale = new Vector3(1, 1, 1);
                                hand.DOScale(1f, .25f).OnComplete(() =>
                                {
                                    StopAllCoroutines();
                                    StartCoroutine(SpawnHearts(0.1f));
                                    cat1Rect.DOScale(0, .75f).SetDelay(.75f);
                                    cat2Rect.DOScale(0, .75f).SetDelay(.75f).OnComplete(() =>
                                    {
                                        hand.DOScale(0f, .75f);

                                        box1SkeletonGraphic.Initialize(true);
                                        box1SkeletonGraphic.SetMaterialDirty();
                                        var track = box1SkeletonGraphic.AnimationState.SetAnimation(0, openingAnim, false);
                                        track.Complete += (s) =>
                                        {
                                            var track2 = box1SkeletonGraphic.AnimationState.SetAnimation(0, openAnim, false);
                                            track2.Complete += (s) =>
                                            {
                                                box1SkeletonGraphic.transform.gameObject.SetActive(false);
                                                cat3.gameObject.SetActive(true);
                                            };
                                        };

                                        box2SkeletonGraphic.Initialize(true);
                                        box2SkeletonGraphic.SetMaterialDirty();
                                        var track2 = box2SkeletonGraphic.AnimationState.SetAnimation(0, openingAnim, false);
                                        track.Complete += (s) =>
                                        {
                                            var track2 = box2SkeletonGraphic.AnimationState.SetAnimation(0, openAnim, false);
                                            track2.Complete += (s) =>
                                            {
                                                box2SkeletonGraphic.transform.gameObject.SetActive(false);
                                                cat4.gameObject.SetActive(true);

                                                hand.DOScale(0f, 1f).OnComplete(() => {
                                                    hand.DOKill();
                                                    cat1.DOKill();
                                                    cat2.DOKill();
                                                    cat1ShadowRect.DOKill();
                                                    cat2ShadowRect.DOKill();
                                                    AnimLoop();
                                                });
                                            };
                                        };
                                    });
                                });
                            });
                        });
                    });
                });
            });
        }
    }

    public IEnumerator SpawnHearts(float time)
    {
        for (int i = 0; i < pathList.Count; i++)
        {
            //Instantiate(GameAssets.Instance.heartParticlePrefab, inputPath[i], Quaternion.identity);
            Instantiate(GameAssets.Instance.heartParticleUIPrefab, pathList[i]);
            yield return new WaitForSeconds(time);
        }
    }

    private void OnClickPlayButton()
    {
        GameController.Instance.AnalyticsController.LogTutLevelEnd(UseProfile.CurrentLevel);
        isAnim = false;
        Destroy(this.gameObject);
    }
}
