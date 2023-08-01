using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GPlayStudio
{
    public class NewBehaviourScript : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            BoardData boardData = new BoardData();
            var data = JsonUtility.ToJson(boardData);
            File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", data);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
