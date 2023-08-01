using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class TutorialBoom : MonoBehaviour
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

    public RectTransform cat3Rect;
    public RectTransform cat4Rect;
    public RectTransform cat3ShadowRect;
    public RectTransform cat4ShadowRect;

    public List<Transform> pathList;
    public List<Transform> pathList2;

    public SkeletonGraphic bombSkeletonGraphic;
    public Text bombCounter;
    private Spine.Animation idle;
    private Spine.Animation exploseAnim;

    public RectTransform failImageRect;

    public Button playButton;

    public bool isAnim = true;

    private void Start()
    {
        idle = GameAssets.Instance.bombSkeletonDataAsset.GetAnimationStateData().SkeletonData.FindAnimation("idle");
        exploseAnim = GameAssets.Instance.bombSkeletonDataAsset.GetAnimationStateData().SkeletonData.FindAnimation("explose");
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

            cat3Rect.localScale = new Vector3(1, 1, 1);
            cat4Rect.localScale = new Vector3(1, 1, 1);
            cat3ShadowRect.localScale = new Vector3(0, 0, 1);
            cat4ShadowRect.localScale = new Vector3(0, 0, 1);

            failImageRect.localScale = new Vector3(0, 0, 1);

            bombCounter.text = "2";
            bombSkeletonGraphic.transform.GetChild(0).gameObject.SetActive(true);
            bombSkeletonGraphic.Initialize(true);
            bombSkeletonGraphic.SetMaterialDirty();
            var track = bombSkeletonGraphic.AnimationState.SetAnimation(0, idle, true);

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
                                        bombCounter.text = "1";
                                        hand.DOKill();
                                        //Phase 2
                                        hand.DOMove(cat3.position, 1f).OnComplete(() =>
                                        {
                                            hand.DOScale(1.2f, .25f).OnComplete(() =>
                                            {
                                                cat3Rect.DOScale(1.15f, .5f);
                                                cat3ShadowRect.localScale = new Vector3(1, 1, 1);
                                                hand.DOScale(1f, .25f).OnComplete(() =>
                                                {
                                                    hand.DOScale(1f, .25f).OnComplete(() =>
                                                    {
                                                        hand.DOMove(cat4.position, 1f).OnComplete(() =>
                                                        {
                                                            hand.DOScale(1.2f, .25f).OnComplete(() =>
                                                            {
                                                                cat4Rect.DOScale(1.15f, .5f);
                                                                cat4ShadowRect.localScale = new Vector3(1, 1, 1);
                                                                hand.DOScale(1f, .25f).OnComplete(() =>
                                                                {
                                                                    StopAllCoroutines();
                                                                    StartCoroutine(SpawnHearts(pathList2, 0.075f));
                                                                    cat3Rect.DOScale(0, .75f).SetDelay(.75f);
                                                                    cat4Rect.DOScale(0, .75f).SetDelay(.75f).OnComplete(() =>
                                                                    {
                                                                        bombCounter.text = "0";

                                                                        failImageRect.DOScale(0, 0f).SetDelay(.75f).OnComplete(() =>
                                                                        {
                                                                            bombSkeletonGraphic.transform.GetChild(0).gameObject.SetActive(false);
                                                                            bombSkeletonGraphic.Initialize(true);
                                                                            bombSkeletonGraphic.SetMaterialDirty();
                                                                            bombSkeletonGraphic.AnimationState.SetAnimation(0, exploseAnim, false);
                                                                            Debug.Log("Bomb explodes");
                                                                            failImageRect.DOScale(1.15f, .5f).SetDelay(1.5f).OnComplete(() =>
                                                                            {
                                                                                failImageRect.DOScale(1, .75f).SetDelay(1.5f).OnComplete(() =>
                                                                                {
                                                                                    hand.DOKill();
                                                                                    cat1.DOKill();
                                                                                    cat2.DOKill();
                                                                                    cat1ShadowRect.DOKill();
                                                                                    cat2ShadowRect.DOKill();

                                                                                    cat3.DOKill();
                                                                                    cat4.DOKill();
                                                                                    cat3ShadowRect.DOKill();
                                                                                    cat4ShadowRect.DOKill();
                                                                                    AnimLoop();
                                                                                });
                                                                            });
                                                                        });
                                                                    });
                                                                });
                                                            });
                                                        });
                                                    });
                                                });
                                            });
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
