using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class TutorialSpyCat : MonoBehaviour
{
    public RectTransform hand;
    public Transform cat1;
    public Transform cat2;

    public RectTransform cat1Rect;
    public RectTransform cat2Rect;
    public RectTransform cat1ShadowRect;
    public RectTransform cat2ShadowRect;

    public Transform cat3;
    public Transform cat4;
    public Transform cat7;

    public SkeletonGraphic cat3SkeletonGraphic;
    public SkeletonGraphic cat4SkeletonGraphic;
    public SkeletonGraphic cat7SkeletonGraphic;

    public AnimationReferenceAsset cat3idle;
    public AnimationReferenceAsset cat3jump;
    public AnimationReferenceAsset cat3grounding;
    public AnimationReferenceAsset cat4idle;
    public AnimationReferenceAsset cat4jump;
    public AnimationReferenceAsset cat4grounding;

    public Transform cell11;
    public Transform cell12;
    public Transform cell13;
    public Transform cell21;
    public Transform cell31;
    public Transform cell32;

    public Transform cat5;
    public Transform cat6;
    public RectTransform cat5Rect;
    public RectTransform cat6Rect;
    public RectTransform cat5ShadowRect;
    public RectTransform cat6ShadowRect;

    public List<Transform> pathList;
    public List<Transform> pathList2;

    public Button playButton;

    public bool isAnim = true;

    private void Start()
    {
        playButton.onClick.AddListener(delegate { OnClickPlayButton(); });
        AnimLoop();
    }

    public void AnimLoop()
    {
        if (isAnim)
        {
            hand.transform.position = cat1.position;
            hand.localScale = new Vector3(1, 1, 1);

            cat1Rect.localScale = new Vector3(1, 1, 1);
            cat2Rect.localScale = new Vector3(1, 1, 1);
            cat1ShadowRect.localScale = new Vector3(0, 0, 1);
            cat2ShadowRect.localScale = new Vector3(0, 0, 1);

            cat5Rect.localScale = new Vector3(1, 1, 1);
            cat6Rect.localScale = new Vector3(1, 1, 1);
            cat5ShadowRect.localScale = new Vector3(0, 0, 1);
            cat6ShadowRect.localScale = new Vector3(0, 0, 1);

            cat3.position = cell12.position;
            cat3SkeletonGraphic.AnimationState.SetAnimation(0, cat3idle, true);
            cat4.position = cell13.position;
            cat4SkeletonGraphic.AnimationState.SetAnimation(0, cat4idle, true);
            cat7.position = cell21.position;
            cat7SkeletonGraphic.AnimationState.SetAnimation(0, cat3idle, true);

            cat3.transform.localScale = new Vector3(1, 1, 1);
            cat4.transform.localScale = new Vector3(1, 1, 1);
            cat7.transform.localScale = new Vector3(1, 1, 1);

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
                                //cat2ShadowRect.DOScale(1.15f, .5f);
                                hand.DOScale(1f, .25f).OnComplete(() =>
                                {
                                    StopAllCoroutines();
                                    StartCoroutine(SpawnHearts(pathList, 0.075f));
                                    cat1Rect.DOScale(0, .75f).SetDelay(.75f);
                                    cat2Rect.DOScale(0, .75f).SetDelay(.75f).OnComplete(() =>
                                    {
                                        cat3.transform.localScale = new Vector3(-1, 1, 1);
                                        cat4.transform.localScale = new Vector3(-1, 1, 1);

                                        cat3.DOJump(cell11.position, 0.25f, 1, .7f, false).SetDelay(0.12f).OnStart(() =>
                                        {
                                            cat3SkeletonGraphic.timeScale = 1.5f;
                                            cat3SkeletonGraphic.AnimationState.SetAnimation(0, cat3jump, false);
                                        }).OnComplete(() =>
                                        {
                                            var track = cat3SkeletonGraphic.AnimationState.SetAnimation(0, cat3grounding, false);
                                            track.Complete += (s) =>
                                            {
                                                cat3SkeletonGraphic.timeScale = 1f;
                                                cat3SkeletonGraphic.AnimationState.SetAnimation(0, cat3idle, true);
                                                cat3SkeletonGraphic.AnimationState.ClearTracks();
                                                cat3SkeletonGraphic.Skeleton.SetToSetupPose();
                                                cat3SkeletonGraphic.Initialize(true);
                                                cat3SkeletonGraphic.SetMaterialDirty();
                                            };
                                        });

                                        cat4.DOJump(cell12.position, 0.25f, 1, .7f, false).SetDelay(0.12f).OnStart(() =>
                                        {
                                            cat4SkeletonGraphic.timeScale = 1.5f;
                                            cat4SkeletonGraphic.AnimationState.SetAnimation(0, cat4jump, false);
                                        }).OnComplete(() =>
                                        {
                                            var track = cat4SkeletonGraphic.AnimationState.SetAnimation(0, cat4grounding, false);
                                            track.Complete += (s) =>
                                            {
                                                cat4SkeletonGraphic.timeScale = 1f;
                                                cat4SkeletonGraphic.AnimationState.SetAnimation(0, cat4idle, true);
                                                cat4SkeletonGraphic.AnimationState.ClearTracks();
                                                cat4SkeletonGraphic.Skeleton.SetToSetupPose();
                                                cat4SkeletonGraphic.Initialize(true);
                                                cat4SkeletonGraphic.SetMaterialDirty();
                                                //Phase 2
                                                hand.DOMove(cat5.position, 1f).OnComplete(() => {
                                                    hand.DOScale(1.2f, .25f).OnComplete(() =>
                                                    {
                                                        cat5Rect.DOScale(1.15f, .5f);
                                                        cat5ShadowRect.localScale = new Vector3(1, 1, 1);
                                                        //cat1ShadowRect.DOScale(1.15f, .5f);
                                                        hand.DOScale(1f, .25f).OnComplete(() =>
                                                        {
                                                            hand.DOScale(1f, .25f).OnComplete(() =>
                                                            {
                                                                hand.DOMove(cat6.position, 1f).OnComplete(() =>
                                                                {
                                                                    hand.DOScale(1.2f, .25f).OnComplete(() =>
                                                                    {
                                                                        cat6Rect.DOScale(1.15f, .5f);
                                                                        cat6ShadowRect.localScale = new Vector3(1, 1, 1);
                                                                        //cat2ShadowRect.DOScale(1.15f, .5f);
                                                                        hand.DOScale(1f, .25f).OnComplete(() =>
                                                                        {
                                                                            StopAllCoroutines();
                                                                            StartCoroutine(SpawnHearts(pathList2, 0.1f));
                                                                            cat5Rect.DOScale(0, .75f).SetDelay(.75f);
                                                                            cat6Rect.DOScale(0, .75f).SetDelay(.75f).OnComplete(() =>
                                                                            {
                                                                                cat7.DOJump(cell31.position, 0.25f, 1, .7f, false).SetDelay(0.12f).OnStart(() =>
                                                                                {
                                                                                    cat7SkeletonGraphic.timeScale = 1.5f;
                                                                                    cat7SkeletonGraphic.AnimationState.SetAnimation(0, cat3jump, false);
                                                                                }).OnComplete(() =>
                                                                                {
                                                                                    var track = cat7SkeletonGraphic.AnimationState.SetAnimation(0, cat3grounding, false);
                                                                                    track.Complete += (s) =>
                                                                                    {
                                                                                        cat7SkeletonGraphic.timeScale = 1f;
                                                                                        cat7SkeletonGraphic.AnimationState.SetAnimation(0, cat3idle, true);
                                                                                        cat7SkeletonGraphic.AnimationState.ClearTracks();
                                                                                        cat7SkeletonGraphic.Skeleton.SetToSetupPose();
                                                                                        cat7SkeletonGraphic.Initialize(true);
                                                                                        cat7SkeletonGraphic.SetMaterialDirty();
                                                                                    };
                                                                                });

                                                                                cat3.DOJump(cell21.position, 0.25f, 1, .7f, false).SetDelay(0.12f).OnStart(() =>
                                                                                {
                                                                                    cat3SkeletonGraphic.timeScale = 1.5f;
                                                                                    cat3SkeletonGraphic.AnimationState.SetAnimation(0, cat3jump, false);
                                                                                }).OnComplete(() =>
                                                                                {
                                                                                    var track = cat3SkeletonGraphic.AnimationState.SetAnimation(0, cat3grounding, false);
                                                                                    track.Complete += (s) =>
                                                                                    {
                                                                                        cat3SkeletonGraphic.timeScale = 1f;
                                                                                        cat3SkeletonGraphic.AnimationState.SetAnimation(0, cat3idle, true);
                                                                                        cat3SkeletonGraphic.AnimationState.ClearTracks();
                                                                                        cat3SkeletonGraphic.Skeleton.SetToSetupPose();
                                                                                        cat3SkeletonGraphic.Initialize(true);
                                                                                        cat3SkeletonGraphic.SetMaterialDirty();
                                                                                    };
                                                                                });

                                                                                cat4.DOJump(cell32.position, 0.25f, 1, .7f, false).SetDelay(0.12f).OnStart(() =>
                                                                                {
                                                                                    cat4SkeletonGraphic.timeScale = 1.5f;
                                                                                    cat4SkeletonGraphic.AnimationState.SetAnimation(0, cat4jump, false);
                                                                                }).OnComplete(() =>
                                                                                {
                                                                                    var track = cat4SkeletonGraphic.AnimationState.SetAnimation(0, cat4grounding, false);
                                                                                    track.Complete += (s) =>
                                                                                    {
                                                                                        cat4SkeletonGraphic.timeScale = 1f;
                                                                                        cat4SkeletonGraphic.AnimationState.SetAnimation(0, cat4idle, true);
                                                                                        cat4SkeletonGraphic.AnimationState.ClearTracks();
                                                                                        cat4SkeletonGraphic.Skeleton.SetToSetupPose();
                                                                                        cat4SkeletonGraphic.Initialize(true);
                                                                                        cat4SkeletonGraphic.SetMaterialDirty();

                                                                                        hand.DOScale(1f, .5f).OnComplete(() => {
                                                                                            hand.DOKill();
                                                                                            cat1.DOKill();
                                                                                            cat2.DOKill();
                                                                                            cat3.DOKill();
                                                                                            cat4.DOKill();
                                                                                            cat5.DOKill();
                                                                                            cat6.DOKill();
                                                                                            cat7.DOKill();
                                                                                            cat1ShadowRect.DOKill();
                                                                                            cat2ShadowRect.DOKill();
                                                                                            cat5ShadowRect.DOKill();
                                                                                            cat6ShadowRect.DOKill();
                                                                                            AnimLoop();
                                                                                        });
                                                                                    };
                                                                                });
                                                                            });
                                                                        });
                                                                    });
                                                                });
                                                            });
                                                        });
                                                    });
                                                });

                                            };
                                        });
                                    });
                                });
                            });
                        });
                    });
                });
            });
        }
    }

    public IEnumerator SpawnHearts(List<Transform> path, float time)
    {
        for (int i = 0; i < path.Count; i++)
        {
            Instantiate(GameAssets.Instance.heartParticleUIPrefab, path[i]);
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
