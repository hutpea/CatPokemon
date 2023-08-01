using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SkinManager : MonoBehaviour
{
    public SkeletonGraphic body;
    [SpineSkin] [SerializeField] protected List<string> _skinMixData;
    [SpineSkin] [SerializeField] public List<string> _skinMix;

    private void Start()
    {
        //_skinMix.Add(_skinMixData[2]);
        //body.SetAnimation("1-idle", _skinMix, true);
    }

    public void ChangeSkin(int id, string animationName, bool isLoop)
    {
        _skinMix.Add(_skinMixData[id - 1]);
        body.SetAnimation(animationName, _skinMix, isLoop);
    }
}
