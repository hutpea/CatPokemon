using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRotate : MonoBehaviour
{
    Vector3 mousePosition = Vector3.zero;
    void Start()
    {
        
    }

    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 dir = mousePosition - this.transform.position;

        this.transform.Rotate(dir);
    }
}
