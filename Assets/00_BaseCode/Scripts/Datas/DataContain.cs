using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataContain : MonoBehaviour
{
    public GiftDatabase giftDatabase;
    public List<BoardData> boardDataList = new List<BoardData>();

    public void Initialize()
    {
        LoadAllBoardDataToList();
    }

    public void LoadAllBoardDataToList()
    {
        for (int i = 1; i <= Context.MAX_LEVEL; i++)
        {
            Debug.Log(i);
            var jsonTextFile = Resources.Load<TextAsset>("Levels/Level_" + i);
            BoardData boardData = new BoardData();
            Debug.Log(jsonTextFile);
            boardData = JsonUtility.FromJson<BoardData>(jsonTextFile.text);
            boardDataList.Add(boardData);
        }
    }
}

