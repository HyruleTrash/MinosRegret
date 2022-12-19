using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public string Kind = "Start";

    private void Start()
    {
        Button knop = this.GetComponent<Button>();
        knop.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        switch (Kind)
        {
            case "Start":
                SceneManager.LoadScene("Game");
                break;
            case "Quit":
                AppHelper.Quit();
                break;
        }
    }
}
