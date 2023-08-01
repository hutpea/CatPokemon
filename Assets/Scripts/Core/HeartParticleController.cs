using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HeartParticleController : MonoBehaviour
{
    private float scale;
    private void Start()
    {
        scale = transform.localScale.x;

        transform.DOScale(scale * 1.2f, .2f).OnComplete(() =>
        {
            transform.DOScale(0f, .45f);
        });
    }
}
