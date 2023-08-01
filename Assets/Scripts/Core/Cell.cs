using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine.Unity;
using EventDispatcher;

public class Cell : MonoBehaviour
{
    [Header("Basic Information")]
    [SerializeField] private Vector2Int cellPosition;
    [SerializeField] private int cellCatType;
    [SerializeField] private bool isEmpty;
    [SerializeField] private int faceDirection = 1;
    public Color cellColor;

    private Transform itemContainer;
    private Button button;
    [SerializeField] private Animator animator;
    [SerializeField] private Image raycastPaneImage;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => OnCellClicked());
        animator.enabled = false;
        if(transform.Find("RaycastPane") != null)
        {
            raycastPaneImage = transform.Find("RaycastPane").GetComponent<Image>();
        }
    }

    public void Initialize()
    {
        
    }

    private void OnCellClicked()
    {
       // Debug.LogError("CLICK CELL " + cellPosition);
        if(GameplayController.Instance.level.board.clickControlMatrix[cellPosition.x, cellPosition.y])
        {
            if (!GetCatItemManager().CheckHasItemInList(CELL_ITEM_TYPE.Box) && !GetCatItemManager().CheckHasItemInList(CELL_ITEM_TYPE.Cage))
            {
                GameplayController.Instance.boardUserInput.InvokeCellClicked(cellPosition);
            }
            else
            {
                Debug.Log("There is box not opened/cage at this position !");
            }
        }
    }

    public void RemoveCell()
    {
        Debug.Log("remove cell at " + cellPosition);
        button.enabled = false;
        transform.Find("Hint").Find("Shine").GetComponent<Image>().enabled = false;
        transform.Find("Hint").Find("Radial").GetComponent<Image>().enabled = false;
        if (GetCatItemManager() != null)
        {
            GetCatItemManager().OnThisCellRemovedOutOfBoard();
        }
        if (GetCat() != null)
        {
            Destroy(GetCat().gameObject);
        }
        isEmpty = true;
    }

    public void RemoveCellNotOnBoard()
    {
        Debug.Log("remove cell (wall) at " + cellPosition);
        button.enabled = false;
        transform.Find("Hint").Find("Shine").GetComponent<Image>().enabled = false;
        transform.Find("Hint").Find("Radial").GetComponent<Image>().enabled = false;
        if (GetCat() != null)
        {
            Destroy(GetCat().gameObject);
        }
        isEmpty = true;
    }

    private void ChangeBoardLineModifierDirection(string dir)
    {
        GameplayController.Instance.level.board.ChangeBoardLineModifiersDirection(dir);
    }

    public void EnableCell()
    {
        isEmpty = false;
        button.enabled = true;
    }

    public void DisableCell()
    {
        button.enabled = false;
    }

    public void DisplayCat()
    {
        if (GetCat() != null)
        {
            Debug.Log("Display cat at " + cellPosition);
            var rootTransform = GetCat().GetChild(0).Find("root");
            rootTransform.GetComponent<SkeletonUtilityBone>().mode = SkeletonUtilityBone.Mode.Override;
            RectTransform catRootRectTransform = rootTransform.GetComponent<RectTransform>();
            var scale = catRootRectTransform.localScale;
            scale.x = 1;
            scale.y = 1;
            catRootRectTransform.localScale = scale;
            EnableCell();
        }
    }

    public void HideCat()
    {
        if (GetCat() != null)
        {
            Debug.Log("Hide cat at " + cellPosition);
            var rootTransform = GetCat().GetChild(0).Find("root");
            rootTransform.GetComponent<SkeletonUtilityBone>().mode = SkeletonUtilityBone.Mode.Override;
            RectTransform catRootRectTransform = rootTransform.GetComponent<RectTransform>();
            var scale = catRootRectTransform.localScale;
            scale.x = 0;
            scale.y = 0;
            catRootRectTransform.localScale = scale;
            DisableCell();
        }
    }

    public void ResetCellScale()
    {
        RectTransform rect = this.GetComponent<RectTransform>();
        var scale = rect.localScale;
        scale.x = 1f;
        scale.y = 1f;
        rect.localScale = scale;
    }

    public bool isCatScale = false;

    public void ToggleCellScale(bool isScaleUp)
    {
        if (GetCat() != null)
        {
            isCatScale = isScaleUp;
            RectTransform rect = this.GetComponent<RectTransform>();
            var scale = rect.localScale;
            scale.x = (isScaleUp) ? 1.2f : 1f;
            scale.y = (isScaleUp) ? 1.2f : 1f;
            rect.localScale = scale;
        }
    }

    public bool isCellZooming = false;

    public void ToggleCatZoomEffect(bool value)
    {
        if (isCatScale)
        {
            ToggleCellScale(false);
        }
        isCellZooming = value;
        animator.enabled = value;
        if (value)
        {
            animator.Play("CatZoomEffect", -1, 0f);
        }
    }

    private Vector2 oldPivot = Vector2.zero;

    public void SetCatPositionInPlaceBox()
    {
        if (GetCat() != null)
        {
            RectTransform catRect = GetCat().GetComponent<RectTransform>();
            oldPivot = catRect.pivot;
            catRect.pivot = new Vector2(0.5f, 0.5f);
            catRect.anchoredPosition = new Vector2(0, 20);
        }
    }

    public void SetCatPositionInPlaceOld()
    {
        if (GetCat() != null)
        {
            RectTransform catRect = GetCat().GetComponent<RectTransform>();
            catRect.pivot = oldPivot;
            catRect.anchoredPosition = new Vector2(0, 0);
        }
    }

    public void FlipCatTo(int dir)
    {
        //bool changed = (faceDirection != dir) ? true : false;
        var scale = GetCat().GetComponent<RectTransform>().localScale;
        scale = new Vector3((dir == -1) ? -1f : 1f, 1f, 1f);
        GetCat().GetComponent<RectTransform>().localScale = scale;

        if(GetCatItemManager() != null)
        {
            Debug.Log("Flip all items");
            GetCatItemManager().FlipAllItems();
        }
    }

    #region ITEMS RELATEDs

    private List<CellItem> startCellItems = new List<CellItem>();

    public void AddStartCellItem(CellItem cellItem)
    {
        startCellItems.Add(cellItem);
    }

    public void InitializeAllCellItems()
    {
        if (GetCatItemManager())
        {
            Debug.Log("insert items");
            CatItemManager catItemManager = GetCatItemManager();
            catItemManager.InitializeItemTransform();
            catItemManager.GetItemsFromListCellItem(startCellItems);
            catItemManager.HideAllItems();
            catItemManager.ShowCurrentItems();
            FlipCatTo(faceDirection);
        }
        startCellItems.Clear();
    }

    public CatItemManager GetCatItemManager()
    {
        if(GetCat() == null || IsCellEmpty())
        {
            return null;
        }
        return GetCat().Find("Items").GetComponent<CatItemManager>();
    }

    public bool CheckHasItemInThisCell(CELL_ITEM_TYPE itemType)
    {
        if (IsCellEmpty())
        {
            return false;
        }
        if (GetCatItemManager() == null)
        {
            return false;
        }
        return GetCatItemManager().CheckHasItemInList(itemType);
    }

    public bool CheckHasBlockItemsInThisCell()
    {
        if(CheckHasItemInThisCell(CELL_ITEM_TYPE.Cage) || CheckHasItemInThisCell(CELL_ITEM_TYPE.Box))
        {
            return true;
        }
        return false;
    }

    #endregion

    public Transform GetCat()
    {
        if (IsCellEmpty())
        {
            Debug.Log("This cell is set to empty before " + cellPosition);
            return null;
        }
        return this.transform.Find("Cat");
    }

    public Vector2Int GetCellPosition()
    {
        return this.cellPosition;
    }

    public void SetCellPosition(Vector2Int value)
    {
        this.cellPosition = value;
    }

    public int GetCellCatType()
    {
        return this.cellCatType;
    }

    public void SetCellCatType(int value)
    {
        cellCatType = value;
    }

    public void SetCellBackground(Sprite sprite)
    {
        transform.Find("Background").GetComponent<Image>().sprite = sprite;
    }

    public bool IsCellEmpty()
    {
        return this.isEmpty;
    }

    //This function is call only one each level, set up begin cat
    public void SetCellCat()
    {

        if (transform.Find("Cat") != null)
        {
            Destroy(transform.Find("Cat").gameObject);
        }
        GameObject catPrefab = null;
        
        catPrefab = GameAssets.Instance.catPrefabs[cellCatType - 1];
        var catGo = Instantiate(catPrefab, this.transform);
        catGo.name = "Cat";

        itemContainer = transform.Find("Items");
        CatItemManager catItemManager = itemContainer.gameObject.AddComponent<CatItemManager>() as CatItemManager;
        itemContainer.SetParent(GetCat()); //make items always on top of cat
        RectTransform itemContainerRect = itemContainer.GetComponent<RectTransform>();
        itemContainerRect.anchoredPosition = new Vector2(0, 0);
    }

    private void SetCellColor()
    {
        Color color = new Color();
        switch (cellCatType)
        {
            case 1:
                color = new Color(255f / 255f, 140f / 255f, 0);
                break;
            case 2:
                color = new Color(139f / 255f, 69f / 255f, 19f / 255f);
                break;
            case 3:
                color = new Color(255f / 255f, 105f / 255f, 180f / 255f);
                break;
            case 4:
                color = new Color(153f / 255f, 51f / 255f, 255f / 255f);
                break;
            case 5:
                color = new Color(248f / 255f, 248f / 255f, 255f / 255f);
                break;
            case 6:
                color = new Color(51f / 255f, 102f / 255f, 0);
                break;
            case 7:
                color = new Color(255f / 255f, 215f / 255f, 0);
                break;
            case 8:
                color = new Color(127f / 255f, 255f / 255f, 212f / 255f);
                break;
        }
        cellColor = color;
        transform.Find("Background").GetComponent<Image>().color = color;
    }

    public void SetCatFaceDirection(int dir)
    {
        this.faceDirection = dir;
    }

    public Vector3 GetWorldPositionOfCellCat()
    {
        if (GetCat())
        {
            return GetCat().position;
        }
        else
        {
            return GameplayController.Instance.level.board.cellWorldPointPositions[cellPosition.x, cellPosition.y];
        }
    }

    public void ToggleRaycastPane(bool value)
    {
        raycastPaneImage.raycastTarget = value;
    }
}
