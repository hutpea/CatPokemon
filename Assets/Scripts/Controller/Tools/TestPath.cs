using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPath : MonoBehaviour
{
    public PathMoveController pathMoveController;

    private void Start()
    {
        List<Vector3> path = new List<Vector3>();
        path.Add(new Vector3(0, 0, 0));
        path.Add(new Vector3(0, 1, 0));
        path.Add(new Vector3(1, 1, 0));
        path.Add(new Vector3(1, 0, 0));
        pathMoveController.DoKill();
        pathMoveController.Setup(path, .5f, () => {
            Debug.Log("Complete");
        });
    }
}
