using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Board board;

    public void Init()
    {
        board.Initialize();
    }
}
