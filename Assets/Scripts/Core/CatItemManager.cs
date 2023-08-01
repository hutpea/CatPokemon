using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using DG.Tweening;

public class CatItemManager : MonoBehaviour
{
    private Dictionary<CELL_ITEM_TYPE, string> items;

    public Transform key;
    public Transform cage;
    public Transform bomb;
    public Transform box;
    public Transform spy;

    private int keyID;
    private int cageID;
    private int bombCounter = 99;
    private string spyHatDirection = "";
    public void InitializeItemTransform()
    {
        items = new Dictionary<CELL_ITEM_TYPE, string>();
        items.Clear();
        key = Instantiate(GameAssets.Instance.keyPrefab, this.transform);
        cage = Instantiate(GameAssets.Instance.cagePrefab, this.transform);
        bomb = Instantiate(GameAssets.Instance.bombPrefab, this.transform);
        box = Instantiate(GameAssets.Instance.boxPrefab, this.transform);
        spy = Utility.FindDeepChild(this.transform.parent, "SpyHat");
    }

    public void GetItemsFromListCellItem(List<CellItem> cellItems)
    {
        foreach (var item in cellItems)
        {
            items.Add(item.cellItemType, item.itemString);
        }
    }

    public void ShowCurrentItems()
    {
        foreach (var item in items)
        {
            switch (item.Key)
            {
                case CELL_ITEM_TYPE.Key:
                    {
                        keyID = Int32.Parse(item.Value);
                        SetKeySkin(keyID);
                        ShowItem(CELL_ITEM_TYPE.Key);
                        break;
                    }
                case CELL_ITEM_TYPE.Cage:
                    {
                        cageID = Int32.Parse(item.Value);
                        SetCageSkin(cageID);
                        GetThisCell().HideCat();
                        ShowItem(CELL_ITEM_TYPE.Cage);
                        break;
                    }
                case CELL_ITEM_TYPE.Bomb:
                    {
                        bombCounter = Int32.Parse(item.Value);
                        SetBombCounter(bombCounter);
                        ShowItem(CELL_ITEM_TYPE.Bomb);

                        break;
                    }
                case CELL_ITEM_TYPE.Box:
                    {
                        GetThisCell().HideCat();
                        GetThisCell().SetCatPositionInPlaceBox();
                        ShowItem(CELL_ITEM_TYPE.Box);
                        break;
                    }
                case CELL_ITEM_TYPE.Spy:
                    {
                        spyHatDirection = item.Value;
                        SetSpyHat(spyHatDirection);
                        ShowItem(CELL_ITEM_TYPE.Spy);
                        break;
                    }
            }
        }
    }

    public void HideAllItems()
    {
        HideItem(CELL_ITEM_TYPE.Key);
        HideItem(CELL_ITEM_TYPE.Cage);
        HideItem(CELL_ITEM_TYPE.Bomb);
        HideItem(CELL_ITEM_TYPE.Box);
        HideItem(CELL_ITEM_TYPE.Spy);
    }

    public void FlipAllItems()
    {
        if(CheckHasItemInList(CELL_ITEM_TYPE.Key))
            FlipItem(ref key);
        if (CheckHasItemInList(CELL_ITEM_TYPE.Spy))
            FlipItem(ref spy);
        if (CheckHasItemInList(CELL_ITEM_TYPE.Bomb))
            FlipItem(ref bomb);
    }

    private void FlipItem(ref Transform t)
    {
        //Debug.LogError("process flip " + t.name);
        var scale = t.GetComponent<RectTransform>().localScale;

        //scale = new Vector3(-scale.x, CheckHasItemInList(CELL_ITEM_TYPE.Key) ? -scale.y : 1f, 1f);
        bool isKey = CheckHasItemInList(CELL_ITEM_TYPE.Key); 
        Transform cat = t.parent.parent;
        float xValue = 0f, yValue = 0f;
        if (isKey)
        {
            xValue = .5f;
            yValue = .5f;
        }
        else
        {
            xValue = 1f;
            yValue = 1f;
        }
        if (cat)
        {
            //Debug.LogError("cat-rect-scale-x=" + cat.GetComponent<RectTransform>().localScale.x);
            scale = new Vector3((cat.GetComponent<RectTransform>().localScale.x < 0) ? -xValue : xValue, yValue, 1f);
        }
        //Debug.LogError("n-scale:" + scale);
        t.GetComponent<RectTransform>().localScale = scale;
    }

    public void HideItem(CELL_ITEM_TYPE itemType)
    {
        switch (itemType)
        {
            case CELL_ITEM_TYPE.Bomb:
                {
                    bomb.gameObject.SetActive(false);
                    break;
                }
            case CELL_ITEM_TYPE.Cage:
                {
                    cage.gameObject.SetActive(false);
                    break;
                }
            case CELL_ITEM_TYPE.Key:
                {
                    key.gameObject.SetActive(false);
                    break;
                }
            case CELL_ITEM_TYPE.Box:
                {
                    box.gameObject.SetActive(false);
                    break;
                }
            case CELL_ITEM_TYPE.Spy:
                {
                    spy.gameObject.SetActive(false);
                    break;
                }
        }
    }

    public void ShowItem(CELL_ITEM_TYPE itemType)
    {
        switch (itemType)
        {
            case CELL_ITEM_TYPE.Bomb:
                {
                    bomb.gameObject.SetActive(true);
                    break;
                }
            case CELL_ITEM_TYPE.Cage:
                {
                    cage.gameObject.SetActive(true);
                    break;
                }
            case CELL_ITEM_TYPE.Key:
                {
                    key.gameObject.SetActive(true);
                    break;
                }
            case CELL_ITEM_TYPE.Box:
                {
                    box.gameObject.SetActive(true);
                    break;
                }
            case CELL_ITEM_TYPE.Spy:
                {
                    spy.gameObject.SetActive(true);
                    break;
                }
        }
    }

    public void RemoveItem(CELL_ITEM_TYPE itemType)
    {
        HideItem(itemType);
        items.Remove(itemType);
    }

    public bool CheckHasItemInList(CELL_ITEM_TYPE itemType)
    {
        if (items.ContainsKey(itemType))
        {
            return true;
        }
        return false;
    }

    //Item: CAGE
    private void SetCageSkin(int id)
    {
        SkinManager cageSkinManager = cage.GetComponent<SkinManager>();
        cageSkinManager.ChangeSkin(id, "1-idle", true);
    }

    public void OpenCage()
    {
        GameplayController.Instance.level.board.DisableClickControlAllCellsOnBoard();
        GameplayController.Instance.level.board.isCompletelyDisableControl = true;
        GameplayController.Instance.gameplayUIController.enablePowerUpInteraction = false;

        var keyMove = Instantiate(GameAssets.Instance.keyPrefab, GameplayController.Instance.level.board.mainCanvas);

        keyMove.GetComponent<SkeletonGraphic>().timeScale = .1f;
        SkinManager keySkinManager = keyMove.GetComponent<SkinManager>();
        keySkinManager.ChangeSkin(cageID, "key", true);

        keyMove.position = GameplayController.Instance.level.board.keyPositions[cageID];

        float keySpeed = 4f;
        float distance = Vector2.Distance(keyMove.position, this.transform.position);

        keyMove.DOMove(this.transform.position, distance / keySpeed).OnComplete(() =>
        {
            keyMove.DOScale(0f, .25f).OnComplete(() =>
            {
                Destroy(keyMove.gameObject);
                SkinManager cageSkinManager = cage.GetComponent<SkinManager>();
                Spine.Animation unlockAnim = GameAssets.Instance.cagekeySkeletonDataAsset.GetAnimationStateData().SkeletonData.FindAnimation("2-unlock");
                var skeletonGraphic = cage.GetComponent<SkeletonGraphic>();
                var track = skeletonGraphic.AnimationState.SetAnimation(0, unlockAnim, false);
                track.Complete += (s) =>
                {
                    //GetThisCell().SetCatPositionInPlaceOld();
                    GetThisCell().DisplayCat();
                    RemoveItem(CELL_ITEM_TYPE.Cage);
                    GameplayController.Instance.level.board.isCompletelyDisableControl = false;
                    GameplayController.Instance.level.board.EnableClickControlAllCellsOnBoard();
                    Debug.Log("ENABLE-POWER-UP ON");
                    GameplayController.Instance.gameplayUIController.enablePowerUpInteraction = true;
                };
            });
        });
    }

    public void PowerupDestroyCage()
    {
        SkinManager cageSkinManager = cage.GetComponent<SkinManager>();
        Spine.Animation attackAnim = GameAssets.Instance.cagekeySkeletonDataAsset.GetAnimationStateData().SkeletonData.FindAnimation("attack");
        var skeletonGraphic = cage.GetComponent<SkeletonGraphic>();
        var track = skeletonGraphic.AnimationState.SetAnimation(0, attackAnim, false);
        track.Complete += (s) =>
        {
            GetThisCell().DisplayCat();
            RemoveItem(CELL_ITEM_TYPE.Cage);
            GameplayController.Instance.level.board.isCompletelyDisableControl = false;
            GameplayController.Instance.level.board.EnableClickControlAllCellsOnBoard();
            Debug.Log("ENABLE-POWER-UP ON");
            GameplayController.Instance.gameplayUIController.enablePowerUpInteraction = true;
        };
    }

    //Item: KEY
    private void SetKeySkin(int id)
    {
        var keyRect = key.GetComponent<RectTransform>();
        keyRect.localScale = new Vector3(0.5f, 0.5f, 1f);
        key.GetComponent<SkeletonGraphic>().timeScale = .1f;
        SkinManager keySkinManager = key.GetComponent<SkinManager>();
        keySkinManager.ChangeSkin(id, "key", true);
    }

    //Item: BOMB
    public void SetBombCounter(int value)
    {
        this.bombCounter = value;
        bomb.GetChild(0).GetChild(0).Find("Counter").GetComponent<Text>().text = bombCounter.ToString();
        if (bombCounter <= 0)
        {
            GameplayController.Instance.gameplayUIController.Toggle_DISABLE_PANEL();
            GameplayController.Instance.gameplayUIController.enablePowerUpInteraction = false;
            BombExplode();
        }
    }

    private void BombExplode()
    {
        GameplayController.Instance.level.board.DisableClickControlAllCellsOnBoard();
        GameplayController.Instance.level.board.isCompletelyDisableControl = true;

        Spine.Animation exploseAnim = GameAssets.Instance.bombSkeletonDataAsset.GetAnimationStateData().SkeletonData.FindAnimation("explose");
        var skeletonGraphic = bomb.GetComponent<SkeletonGraphic>();
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();

        var track = skeletonGraphic.AnimationState.SetAnimation(0, exploseAnim, false);
        track.Complete += (s) =>
        {
            Debug.Log("Bomb exploded");
            RemoveItem(CELL_ITEM_TYPE.Bomb);
            GameplayController.Instance.level.board.isCompletelyDisableControl = false;
            StartCoroutine(WaitLose(1f));
        };
    }

    public void PowerupRemoveBomb()
    {
        Spine.Animation attackAnim = GameAssets.Instance.bombSkeletonDataAsset.GetAnimationStateData().SkeletonData.FindAnimation("attack");
        var skeletonGraphic = bomb.GetComponent<SkeletonGraphic>();
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
        bomb.GetChild(0).gameObject.SetActive(false);
        var track = skeletonGraphic.AnimationState.SetAnimation(0, attackAnim, false);
        track.Complete += (s) =>
        {
            RemoveItem(CELL_ITEM_TYPE.Bomb);
            GameplayController.Instance.level.board.isCompletelyDisableControl = false;
            GameplayController.Instance.level.board.EnableClickControlAllCellsOnBoard();
            Debug.Log("ENABLE-POWER-UP ON");
            GameplayController.Instance.gameplayUIController.enablePowerUpInteraction = true;
        };
    }

    private IEnumerator WaitLose(float time)
    {
        yield return new WaitForSeconds(time);
        GameplayController.Instance.level.board.OnLoseLevel();
    }
    //Item: BOX
    public void OpenBox()
    {
        GameplayController.Instance.level.board.DisableClickControlAllCellsOnBoard();
        GameplayController.Instance.level.board.isCompletelyDisableControl = true;
        GameplayController.Instance.gameplayUIController.enablePowerUpInteraction = false;

        Spine.Animation openingAnim = GameAssets.Instance.boxSkeletonDataAsset.GetAnimationStateData().SkeletonData.FindAnimation("2-opening");
        Spine.Animation openAnim = GameAssets.Instance.boxSkeletonDataAsset.GetAnimationStateData().SkeletonData.FindAnimation("3-open");
        var skeletonGraphic = box.GetComponent<SkeletonGraphic>();
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
        var track = skeletonGraphic.AnimationState.SetAnimation(0, openingAnim, false);
        track.Complete += (s) =>
        {
            var track2 = skeletonGraphic.AnimationState.SetAnimation(0, openAnim, false);
            track2.Complete += (s) =>
            {
                GetThisCell().SetCatPositionInPlaceOld();
                GetThisCell().DisplayCat();
                RemoveItem(CELL_ITEM_TYPE.Box);
                GameplayController.Instance.level.board.isCompletelyDisableControl = false;
                GameplayController.Instance.level.board.EnableClickControlAllCellsOnBoard();
                Debug.Log("ENABLE-POWER-UP ON");
                GameplayController.Instance.gameplayUIController.enablePowerUpInteraction = true;
            };
        };
    }

    public void PowerupDestroyBox()
    {
        Spine.Animation attackAnim = GameAssets.Instance.boxSkeletonDataAsset.GetAnimationStateData().SkeletonData.FindAnimation("4-attack");
        var skeletonGraphic = box.GetComponent<SkeletonGraphic>();
        skeletonGraphic.Initialize(true);
        skeletonGraphic.SetMaterialDirty();
        var track = skeletonGraphic.AnimationState.SetAnimation(0, attackAnim, false);
        track.Complete += (s) =>
        {
            GetThisCell().SetCatPositionInPlaceOld();
            GetThisCell().DisplayCat();
            RemoveItem(CELL_ITEM_TYPE.Box);
            GameplayController.Instance.level.board.isCompletelyDisableControl = false;
            GameplayController.Instance.level.board.EnableClickControlAllCellsOnBoard();
            Debug.Log("ENABLE-POWER-UP ON");
            GameplayController.Instance.gameplayUIController.enablePowerUpInteraction = true;
        };
    }

    //Item: SPY
    private void SetSpyHat(string direction)
    {
        Sprite spyHatSprite;
        switch (direction)
        {
            case "l":
                {
                    spyHatSprite = GameAssets.Instance.spyHatLeft;
                    break;
                }
            case "r":
                {
                    spyHatSprite = GameAssets.Instance.spyHatRight;
                    break;
                }
            case "u":
                {
                    spyHatSprite = GameAssets.Instance.spyHatUp;
                    break;
                }
            default:
                {
                    spyHatSprite = GameAssets.Instance.spyHatDown;
                    break;
                }
        }
        spy.GetComponent<Image>().sprite = spyHatSprite;
    }

    private Cell GetThisCell()
    {
        return this.transform.parent.parent.GetComponent<Cell>();
    }

    public int GetCageID()
    {
        return this.cageID;
    }

    public int GetKeyID()
    {
        return this.keyID;
    }

    public int GetBombCounter()
    {
        return this.bombCounter;
    }

    #region Event Methods

    public void OnThisCellRemovedOutOfBoard()
    {
        if (CheckHasItemInList(CELL_ITEM_TYPE.Key))
        {
            var cellPos = GetThisCell().GetCellPosition();
            GameplayController.Instance.level.board.keyPositions[keyID] = GameplayController.Instance.level.board.cellWorldPointPositions[cellPos.x, cellPos.y];
            GameplayController.Instance.level.board.DisableClickControlAllCellsOnBoard();
            GameplayController.Instance.level.board.isCompletelyDisableControl = true;
            GameplayController.Instance.level.board.EnqueueActionToGameplayActions(new GameplayAction(GAMEPLAY_ACTION_TYPE.OpenCage, keyID.ToString()));
        }
        else if (CheckHasItemInList(CELL_ITEM_TYPE.Spy))
        {
            GameplayController.Instance.level.board.DisableClickControlAllCellsOnBoard();
            GameplayController.Instance.level.board.isCompletelyDisableControl = true;
            GameplayController.Instance.gameplayUIController.enablePowerUpInteraction = false;
            /*GameplayController.Instance.level.board.rowModifier.Clear();
            GameplayController.Instance.level.board.columnModifier.Clear();
            switch (spyHatDirection)
            {
                case "l":
                    {
                        for (int i = 1; i <= GameplayController.Instance.level.board.boardHeight; i++)
                        {
                            GameplayController.Instance.level.board.rowModifier.Add(i, LINE_MODIFIER_TYPE.Left);
                        }
                        for (int i = 1; i <= GameplayController.Instance.level.board.boardHeight; i++)
                        {
                            GameplayController.Instance.level.board.CheckRowModifier(i);
                        }
                        break;
                    }
                case "r":
                    {
                        for (int i = 1; i <= GameplayController.Instance.level.board.boardHeight; i++)
                        {
                            GameplayController.Instance.level.board.rowModifier.Add(i, LINE_MODIFIER_TYPE.Right);
                        }
                        for (int i = 1; i <= GameplayController.Instance.level.board.boardHeight; i++)
                        {
                            GameplayController.Instance.level.board.CheckRowModifier(i);
                        }
                        break;
                    }
                case "u":
                    {
                        for (int i = 1; i <= GameplayController.Instance.level.board.boardWidth; i++)
                        {
                            GameplayController.Instance.level.board.columnModifier.Add(i, LINE_MODIFIER_TYPE.Up);
                        }
                        for (int i = 1; i <= GameplayController.Instance.level.board.boardWidth; i++)
                        {
                            GameplayController.Instance.level.board.CheckColumnModifier(i);
                        }
                        break;
                    }
                //down
                default:
                    {
                        for (int i = 1; i <= GameplayController.Instance.level.board.boardWidth; i++)
                        {
                            GameplayController.Instance.level.board.columnModifier.Add(i, LINE_MODIFIER_TYPE.Down);
                        }
                        for (int i = 1; i <= GameplayController.Instance.level.board.boardWidth; i++)
                        {
                            GameplayController.Instance.level.board.CheckColumnModifier(i);
                        }
                        break;
                    }
            }*/
            GameplayController.Instance.level.board.EnqueueActionToGameplayActions(new GameplayAction(GAMEPLAY_ACTION_TYPE.ChangeBoardLineModifiers, spyHatDirection));
        }
        //Find all next to cells and invoke their OnCellNextToThisCellRemovedOutOfBoard()
        Vector2Int thisCellPosition = GetThisCell().GetCellPosition();
        if (thisCellPosition.y > 1)
        {
            Cell cell = GameplayController.Instance.level.board.GetCellFromPosition(new Vector2Int(thisCellPosition.x, thisCellPosition.y - 1));
            if (cell != null)
            {
                if (cell.GetCatItemManager() != null)
                {
                    cell.GetCatItemManager().OnCellNextToThisCellRemovedOutOfBoard();
                }
            }
        }
        if (thisCellPosition.y < GameplayController.Instance.level.board.boardWidth)
        {
            Cell cell = GameplayController.Instance.level.board.GetCellFromPosition(new Vector2Int(thisCellPosition.x, thisCellPosition.y + 1));
            if (cell != null)
            {
                if (cell.GetCatItemManager() != null)
                {
                    cell.GetCatItemManager().OnCellNextToThisCellRemovedOutOfBoard();
                }
            }
        }
        if (thisCellPosition.x > 1)
        {
            Cell cell = GameplayController.Instance.level.board.GetCellFromPosition(new Vector2Int(thisCellPosition.x - 1, thisCellPosition.y));
            if (cell != null)
            {
                if (cell.GetCatItemManager() != null)
                {
                    cell.GetCatItemManager().OnCellNextToThisCellRemovedOutOfBoard();
                }
            }
        }
        if (thisCellPosition.x < GameplayController.Instance.level.board.boardHeight)
        {
            Cell cell = GameplayController.Instance.level.board.GetCellFromPosition(new Vector2Int(thisCellPosition.x + 1, thisCellPosition.y));
            if (cell != null)
            {
                if (cell.GetCatItemManager() != null)
                {
                    cell.GetCatItemManager().OnCellNextToThisCellRemovedOutOfBoard();
                }
            }
        }
    }

    public void OnCellNextToThisCellRemovedOutOfBoard()
    {
        if (CheckHasItemInList(CELL_ITEM_TYPE.Box))
        {
            Vector2Int thisCellPosition = GetThisCell().GetCellPosition();
            GameplayController.Instance.level.board.DisableClickControlAllCellsOnBoard();
            GameplayController.Instance.level.board.isCompletelyDisableControl = true;
            GameplayController.Instance.level.board.EnqueueActionToGameplayActions(new GameplayAction(GAMEPLAY_ACTION_TYPE.OpenBox, thisCellPosition.x.ToString() + "," + thisCellPosition.y.ToString()));
        }
    }

    #endregion

#if UNITY_EDITOR

    public void DebugAllItems()
    {
        string res = "";
        if (items.Count == 0)
        {
            Debug.Log("There is no item at this cell " + GetThisCell().GetCellPosition());
            return;
        }
        foreach (var item in items)
        {
            res += item.Key.ToString() + " " + item.Value + "\n";
        }
        Debug.Log("Item at " + GetThisCell().GetCellPosition() + ":" + res);
    }

#endif
}
