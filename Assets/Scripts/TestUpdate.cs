using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUpdate : MonoBehaviour
{
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        text.text = (GameplayController.Instance.gameplayUIController.enablePowerUpInteraction) ? "<color=green>enable</color>" : "<color=red>disable</color>";
    }
}
