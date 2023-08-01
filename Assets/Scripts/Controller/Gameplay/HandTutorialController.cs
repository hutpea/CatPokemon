using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandTutorialController : MonoBehaviour
{
    int level;

    public Transform hand;

    bool isTutorial = false;

    public void Setup(int level)
    {
        hand.gameObject.SetActive(true);
        this.level = level;
        isTutorial = true;
        isScaleAnim = true;
        HandScaleAnim();
        switch (level)
        {
            case 1:
                {
                    StartCoroutine(Coroutine1());
                    break;
                }
            case 2:
                {
                    StartCoroutine(Coroutine2());
                    break;
                }
        }
    }

    public void InvokeHighlightCat(Vector2Int cellPos)
    {
        switch (level)
        {
            case 1:
                {
                    if (isTutorial)
                    {
                        if (cellPos == new Vector2Int(1, 1))
                        {
                            hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[2, 1], .4f);
                        }
                        else if (cellPos == new Vector2Int(2, 1))
                        {
                            hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[1, 1], .4f);
                        }
                        else if (cellPos == new Vector2Int(1, 2))
                        {
                            hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[2, 2], .4f);
                        }
                        else if (cellPos == new Vector2Int(2, 2))
                        {
                            hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[1, 2], .4f);
                        }
                    }
                    break;
                }
            case 2:
                {
                    if (isTutorial)
                    {
                        if (cellPos == new Vector2Int(1, 1))
                        {
                            hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[3, 1], .4f);
                        }
                        else if (cellPos == new Vector2Int(3, 1))
                        {
                            hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[1, 1], .4f);
                        }
                        else if (cellPos == new Vector2Int(2, 2))
                        {
                            hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[3, 2], .4f);
                        }
                        else if (cellPos == new Vector2Int(3, 2))
                        {
                            hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[2, 2], .4f);
                        }
                        else if (cellPos == new Vector2Int(1, 2))
                        {
                            if (lv2_condition_1 || lv2_condition_3)
                            {
                                hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[2, 1], .4f);
                            }
                            else
                            {
                                hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[2, 2], .4f);
                            }
                        }
                        else if (cellPos == new Vector2Int(2, 1))
                        {
                            if (lv2_condition_1 || lv2_condition_3)
                            {
                                hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[1, 2], .4f);
                            }
                            else
                            {
                                hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[2, 2], .4f);
                            }
                        }
                    }
                    break;
                }
        }
    }

    private bool lv1_condition_1 = false;
    private bool lv1_condition_2 = false;

    private bool lv2_condition_1 = false;
    private bool lv2_condition_2 = false;
    private bool lv2_condition_3 = false;

    public void InvokeTwoCellsConnected(Vector2Int p1, Vector2Int p2)
    {
        switch (level)
        {
            case 1:
                {
                    if (p1 == new Vector2Int(1, 1) && p2 == new Vector2Int(2, 1) || p1 == new Vector2Int(2, 1) && p2 == new Vector2Int(1, 1))
                    {
                        lv1_condition_1 = true;
                    }
                    if (p1 == new Vector2Int(1, 2) && p2 == new Vector2Int(2, 2) || p1 == new Vector2Int(2, 2) && p2 == new Vector2Int(1, 2))
                    {
                        lv1_condition_2 = true;
                    }
                    if (lv1_condition_1)
                    {
                        hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[1, 2], .4f);
                    }
                    if (lv1_condition_2)
                    {
                        hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[1, 1], .4f);
                    }
                    if (lv1_condition_1 && (p1 == new Vector2Int(1, 2) && p2 == new Vector2Int(2, 2) || p1 == new Vector2Int(2, 2) && p2 == new Vector2Int(1, 2)))
                    {
                        hand.DOScale(0f, .5f).OnComplete(() =>
                        {
                            hand.gameObject.SetActive(true);
                            hand.DOKill();
                            isScaleAnim = false;
                            isTutorial = false;
                        });
                    }
                    if (lv1_condition_2 && (p1 == new Vector2Int(1, 1) && p2 == new Vector2Int(2, 1) || p1 == new Vector2Int(2, 1) && p2 == new Vector2Int(1, 1)))
                    {
                        hand.DOScale(0f, .3f).OnComplete(() =>
                        {
                            hand.gameObject.SetActive(true);
                            hand.DOKill();
                            isScaleAnim = false;
                            isTutorial = false;
                        });
                    }
                    break;
                }
            case 2:
                {
                    if ((p1 == new Vector2Int(1, 1) && p2 == new Vector2Int(3, 1)) || (p1 == new Vector2Int(3, 1) && p2 == new Vector2Int(1, 1)))
                    {
                        Debug.Log("Pha o (1,1) va (3,1)");
                        lv2_condition_1 = true;
                        if (!lv2_condition_2)
                        {
                            hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[1, 2], .4f);
                        }
                        else
                        {
                            hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[2, 2], .4f);
                        }
                    }
                    if ((p1 == new Vector2Int(1, 2) && p2 == new Vector2Int(2, 1)) || (p1 == new Vector2Int(2, 1) && p2 == new Vector2Int(1, 2)))
                    {
                        Debug.Log("Pha o (1,2) va (2,1)");
                        lv2_condition_2 = true;
                        if (!lv2_condition_1)
                        {
                            hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[1, 1], .4f);
                        }
                        else if (!lv2_condition_3)
                        {
                            hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[2, 2], .4f);
                        }
                    }
                    if ((p1 == new Vector2Int(2, 2) && p2 == new Vector2Int(3, 2)) || (p1 == new Vector2Int(3, 2) && p2 == new Vector2Int(2, 2)))
                    {
                        Debug.Log("Pha o (2,2) va (3,2)");
                        lv2_condition_3 = true;
                        if (!lv2_condition_1)
                        {
                            hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[1, 1], .4f);
                        }
                        else
                        {
                            hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[1, 2], .4f);
                        }
                    }

                    if(lv2_condition_1 && lv2_condition_2 && lv2_condition_3)
                    {
                        hand.DOScale(0f, .3f).OnComplete(() =>
                        {
                            hand.gameObject.SetActive(true);
                            hand.DOKill();
                            isScaleAnim = false;
                            isTutorial = false;
                            GameController.Instance.AnalyticsController.LogTutLevelEnd(UseProfile.CurrentLevel);
                        });
                    }

                    break;
                }
        }
    }

    bool isScaleAnim = false;

    private void HandScaleAnim()
    {
        if (isScaleAnim)
        {
            hand.DOScale(.23f, .5f).OnComplete(() =>
            {
                hand.DOScale(.17f, .5f).OnComplete(() =>
                {
                    HandScaleAnim();
                });
            });
        }
    }

    private IEnumerator Coroutine1()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log(GameplayController.Instance.level.board.cellWorldPointPositions[1, 1]);
        hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[1, 1], .2f);
    }

    private IEnumerator Coroutine2()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log(GameplayController.Instance.level.board.cellWorldPointPositions[1, 1]);
        hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[1, 1], .2f);
    }
}
