using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialController : MonoBehaviour
{
    #region olds
    /*int level;
    public Transform hand;

    public Vector2Int firstCell;

    private void Start()
    {
        StartCoroutine(ScaleEffect());    
    }

    public void Setup(int level)
    {
        Debug.Log("Show tutorial level " + level);
        this.level = level;
        ShowTutorial();
    }

    public void ShowTutorial()
    {
        hand.gameObject.SetActive(true);
        switch (level)
        {
            case 1:
                {
                    Debug.Log("XXX");
                    hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[1, 1], 0.1f);
                    break;
                }
            default: break;
        }
    }

    private IEnumerator ScaleEffect()
    {
        while (true)
        {
            hand.DOScale(0.25f, 0.5f).OnComplete(() =>
            {
                hand.DOScale(0.2f, 0.5f).OnComplete(() =>
                {

                });
            });
            yield return new WaitForSeconds(1.25f);
        }
    }

    public void InvokeFirstCell(Vector2Int cell)
    {
        Debug.Log("Invoke");
        firstCell = cell;
        switch (level)
        {
            case 1:
                {
                    Debug.Log("move " + firstCell);
                    if(firstCell == new Vector2Int(1, 1))
                    {
                        hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[2, 1], .5f);
                    }
                    if (firstCell == new Vector2Int(1, 2))
                    {
                        hand.DOMove(GameplayController.Instance.level.board.cellWorldPointPositions[2, 2], .5f);
                    }
                    //hand.transform.position = 
                    break;
                }
            default: break;
        }
    }*/
    #endregion

    public Transform tutorial_LV1;
    public Transform tutorial_LV2;
    public Transform tutorial_Move;
    public Transform tutorial_Boom;
    public Transform tutorial_Box;
    public Transform tutorial_KeyLock;
    public Transform tutorial_SpyCat;
         
    public void SetupTutorial(int level)
    {
        switch (level)
        {
            case 1:
                {
                    Instantiate(tutorial_LV1);
                    break;
                }
            case 2:
                {
                    Instantiate(tutorial_LV2);
                    break;
                }
            case 7:
                {
                    Instantiate(tutorial_Move);
                    break;
                }
            case 15:
                {
                    Instantiate(tutorial_Boom);
                    break;
                }
            case 30:
                {
                    Instantiate(tutorial_Box);
                    break;
                }
            case 45:
                {
                    Instantiate(tutorial_KeyLock);
                    break;
                }
            case 60:
                {
                    Instantiate(tutorial_SpyCat);
                    break;
                }
        }
    }
}
