using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class GameDataProcess
{
    public static BoardData GetBoardData(string fileName)
    {
        BoardData boardData = new BoardData();
        if (File.Exists(Application.persistentDataPath + "/" + fileName + ".json"))
        {
            var data = File.ReadAllText(Application.persistentDataPath + "/" + fileName + ".json");
            boardData = JsonUtility.FromJson<BoardData>(data);
        }
        return boardData;
    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper);
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
