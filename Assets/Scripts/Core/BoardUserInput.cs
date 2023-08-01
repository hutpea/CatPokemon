using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventDispatcher;
using Spine.Unity;

public class BoardUserInput : MonoBehaviour
{
    public Vector2Int firstCell = new Vector2Int(-1, -1);
    public Vector2Int secondCell = new Vector2Int(-1, -1);
    private bool isNoCellSelected = true;
    private bool enableClickOnCell = true;

    public void Initialize()
    {
        this.RegisterListener(EventID.CAT_MOVE_ON_BOARD_BEGIN, (param) => OnCatMoveOnBoardBegin());
        this.RegisterListener(EventID.CAT_MOVE_ON_BOARD_COMPLETED, (param) => OnCatMoveOnBoardCompleted());
    }

    //Khi 1 cell click se invoke phuong thuc nay
    public void InvokeCellClicked(Vector2Int cellClicked)
    {
        if (enableClickOnCell)
        {
            //Chưa chọn ô nào => chọn ô đó làm ô đầu tiên, highlight
            if (isNoCellSelected)
            {
                firstCell = cellClicked;
                isNoCellSelected = false;

                GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.CatMewSound, 1);
                HighlightCat(cellClicked);

                GameplayController.Instance.handTutorialController.InvokeHighlightCat(cellClicked);
                return;
            }
            //Chọn vào chính ô vừa được chọn => Bỏ chọn ô đó, un-highlight
            if (!isNoCellSelected && cellClicked == firstCell)
            {
                UnhighlightCat(firstCell);
                firstCell = new Vector2Int(-1, -1);
                isNoCellSelected = true;
                return;
            }
            //Chọn vào 1 ô khác loại với ô ban đầu được chọn => Chọn ô vừa click làm ô đầu, highlight ô click
            if (!isNoCellSelected && !GameplayController.Instance.level.board.CheckTwoCellsSameAnimalType(firstCell, cellClicked))
            {
                UnhighlightCat(firstCell);
                firstCell = cellClicked;
                isNoCellSelected = false;

                GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.CatMewSound, 1);
                HighlightCat(cellClicked);

                GameplayController.Instance.handTutorialController.InvokeHighlightCat(cellClicked);
                return;
            }
            //Chọn vào 1 ô cùng loại với ô ban đầu được chọn => check nối, được thì xóa 2 ô => hủy chọn cả hai, un-highlight
            if (!isNoCellSelected && GameplayController.Instance.level.board.CheckTwoCellsSameAnimalType(firstCell, cellClicked))
            {
                secondCell = cellClicked;

                //Neu co the ket noi
                if (GameplayController.Instance.level.board.CheckTwoCellsCanBeConnected(firstCell, secondCell))
                {
                    if (GameplayController.Instance.level.board.CheckBoardHasRowModifiers())
                    {
                        GameplayController.Instance.level.board.DisableClickControlAtLine("row", firstCell.x);
                        GameplayController.Instance.level.board.DisableClickControlAtLine("row", secondCell.x);
                    }
                    else if (GameplayController.Instance.level.board.CheckBoardHasColModifiers())
                    {
                        GameplayController.Instance.level.board.DisableClickControlAtLine("col", firstCell.y);
                        GameplayController.Instance.level.board.DisableClickControlAtLine("col", secondCell.y);
                    }

                    //GameplayController.Instance.level.board.DisableClickControlAllCellsOnBoard();
                    
                    GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.CatMewSound, 2);
                    GameplayController.Instance.gameplayUIController.hand.gameObject.SetActive(false);
                    HighlightCat(secondCell);
                    GameplayController.Instance.handTutorialController.InvokeTwoCellsConnected(firstCell, secondCell);

                    GameplayController.Instance.level.board.NewConnectTwoPointsWithLines(firstCell, secondCell,
                        Utility.IntToCatType(GameplayController.Instance.level.board.GetCatTypeFromCell(firstCell)));

                    GameplayController.Instance.level.board.DisableClickControlMatrixAt(firstCell);
                    GameplayController.Instance.level.board.DisableClickControlMatrixAt(secondCell);
                    //GameplayController.Instance.level.board.DisableClickControlAllCellsOnBoard();
                }
                else
                {
                    GameController.Instance.soundController.PlaySound(AUDIO_CLIP_TYPE.CannotConnect);
                    UnhighlightCat(firstCell);

                }
                secondCell = new Vector2Int(-1, -1);
                isNoCellSelected = true;

                return;
            }
            Debug.LogError("Not fall into any cases, maybe error !");
        }
    }

    private void RemoveTwoSelectedCells(Vector2Int cell1, Vector2Int cell2)
    {
        GameplayController.Instance.level.board.RemoveCell(cell1);
        GameplayController.Instance.level.board.RemoveCell(cell2);
    }

    public void HighlightCat(Vector2Int cellPosition)
    {
        if (GameplayController.Instance.level.board.GetCellFromPosition(cellPosition).IsCellEmpty())
        {
            return;
        }
        Debug.Log("Highlight this cell");
        //Debug.Log(GameplayController.Instance.level.board.cellGameObjectsInScene[cellPosition.x, cellPosition.y].transform.Find("Cat").GetComponent<SkeletonGraphic>().skeletonDataAsset.name);
        RectTransform rectTransform = GameplayController.Instance.level.board.cellGameObjectsInScene[cellPosition.x, cellPosition.y].transform.Find("Cat").GetChild(0).Find("root").Find("SHADOW").GetComponent<RectTransform>();
        
        var scale = rectTransform.localScale;
        scale.x = 1;
        scale.y = 1;
        rectTransform.localScale = scale;

        if(!GameplayController.Instance.level.board.GetCellFromPosition(cellPosition).isCellZooming)
        {
            GameplayController.Instance.level.board.GetCellFromPosition(cellPosition).ToggleCellScale(true);
        }
    }

    public void UnhighlightCat(Vector2Int cellPosition)
    {
        if (GameplayController.Instance.level.board.GetCellFromPosition(cellPosition).IsCellEmpty())
        {
            return;
        }
        RectTransform rectTransform = GameplayController.Instance.level.board.cellGameObjectsInScene[cellPosition.x, cellPosition.y].transform.Find("Cat").GetChild(0).Find("root").Find("SHADOW").GetComponent<RectTransform>();
        var scale = rectTransform.localScale;
        scale.x = 0;
        scale.y = 0;
        rectTransform.localScale = scale;

        GameplayController.Instance.level.board.GetCellFromPosition(cellPosition).ToggleCellScale(false);
    }

    public void OnCatMoveOnBoardBegin()
    {
        enableClickOnCell = false;
    }

    public void OnCatMoveOnBoardCompleted()
    {
        enableClickOnCell = true;
    }
}

