using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Spine.Unity;

public class TutorialKeyLock : MonoBehaviour
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
    public SkeletonGraphic cageSkeletonGraphic;

    public Transform key;

    Spine.Animation lockAnim;
    Spine.Animation unlockAnim;

    public Button playButton;

    public bool isAnim = true;

    private void Start()
    {
        lockAnim = GameAssets.Instance.cagekeySkeletonDataAsset.GetAnimationStateData().SkeletonData.FindAnimation("1-idle");
        unlockAnim = GameAssets.Instance.cagekeySkeletonDataAsset.GetAnimationStateData().SkeletonData.FindAnimation("2-unlock");
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
            cageSkeletonGraphic.transform.gameObject.SetActive(true);
            cageSkeletonGraphic.AnimationState.SetAnimation(0, lockAnim, true);
            key.gameObject.SetActive(true);
            cat3.gameObject.SetActive(false);
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
                                    hand.DOScale(1f, .25f).OnComplete(() =>
                                    {
                                        key.gameObject.SetActive(false);
                                    });
                                    cat1Rect.DOScale(0, .75f).SetDelay(.75f);
                                    cat2Rect.DOScale(0, .75f).SetDelay(.75f).OnComplete(() =>
                                    {
                                        hand.DOScale(0f, .5f);
                                        SkinManager cageSkinManager = cageSkeletonGraphic.transform.GetComponent<SkinManager>();
                                        var track = cageSkeletonGraphic.AnimationState.SetAnimation(0, unlockAnim, false);
                                        track.Complete += (s) =>
                                        {
                                            cageSkeletonGraphic.transform.gameObject.SetActive(false);
                                            cat3.gameObject.SetActive(true);

                                            hand.DOScale(0f, .75f).OnComplete(() =>
                                            {
                                                hand.DOKill();
                                                cat1.DOKill();
                                                cat2.DOKill();
                                                cat1ShadowRect.DOKill();
                                                cat2ShadowRect.DOKill();
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
        }
    }

    public IEnumerator SpawnHearts(float time)
    {
        for (int i = 0; i < pathList.Count; i++)
        {
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
