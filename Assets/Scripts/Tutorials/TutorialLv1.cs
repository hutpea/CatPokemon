using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialLv1 : MonoBehaviour
{
    public RectTransform hand;
    public Transform cat1;
    public Transform cat2;

    public RectTransform cat1Rect;
    public RectTransform cat2Rect;
    public RectTransform cat1ShadowRect;
    public RectTransform cat2ShadowRect;

    public List<Transform> pathList;

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
            cat1Rect.localScale = new Vector3(1, 1, 1);
            cat2Rect.localScale = new Vector3(1, 1, 1);
            cat1ShadowRect.localScale = new Vector3(0, 0, 1);
            cat2ShadowRect.localScale = new Vector3(0, 0, 1);
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
                                    StartCoroutine(SpawnHearts(0.1f));
                                    cat1Rect.DOScale(0, .75f).SetDelay(.75f);
                                    cat2Rect.DOScale(0, .75f).SetDelay(.75f).OnComplete(() =>
                                    {
                                        hand.DOKill();
                                        cat1.DOKill();
                                        cat2.DOKill();
                                        cat1ShadowRect.DOKill();
                                        cat2ShadowRect.DOKill();
                                        AnimLoop();
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
        isAnim = false;
        Destroy(this.gameObject);
    }
}
