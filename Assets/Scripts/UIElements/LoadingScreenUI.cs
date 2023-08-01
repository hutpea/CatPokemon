using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenUI : MonoBehaviour
{
    public Text loadingText;

    private void Start()
    {
        StartCoroutine(LoadingTextController());
    }

    private IEnumerator LoadingTextController()
    {
        int socham = 1;
        while (true)
        {
            loadingText.text = "Loading ";
            for (int i = 1; i <= socham; i++)
            {
                loadingText.text += ".";
            }

            if (socham >= 3) socham = 1;
            else socham++;
            yield return new WaitForSeconds(.4f);
        }
    }
}
