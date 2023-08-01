using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeartParticleUI : MonoBehaviour
{
    private float scale;
    private void Start()
    {
        scale = GetComponent<RectTransform>().localScale.x;

        GetComponent<RectTransform>().DOScale(scale * 1.2f, .2f).OnComplete(() =>
        {
            GetComponent<RectTransform>().DOScale(0f, .45f).OnComplete(() =>
            {
                Destroy(this.gameObject);
            }); ;
        });
    }
}
