using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CatHomeController : MonoBehaviour
{
    public Transform catHome1;
    public Transform catHome2;

    public RectTransform heartUI;

    private int catInHome;

    private void Awake()
    {
        catInHome = 0;
    }

    public void AddCat()
    {
        Debug.Log("Cat home add cat, cat in home: " + catInHome);
        catInHome++;
        UpdateHeart();
    }

    public void RemoveCat()
    {
        Debug.Log("Cat home remove cat, cat in home: " + catInHome);
        catInHome--;
        UpdateHeart();
    }

    public void UpdateHeart()
    {
        if(catInHome > 0)
        {
            heartUI.DOScale(1.2f, .75f).SetEase(Ease.InOutBack);
        }
        else
        {
            heartUI.DOScale(0f, 1f);
        }
    }
}
