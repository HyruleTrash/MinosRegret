using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class UIElement
{
    public GameObject Item;
    public GameObject ItemBox;
    public GameObject Selectionbox;
    public GameObject Equip;

    public UIElement(GameObject ItemConstructer, GameObject ItemBoxConstructer, GameObject SelectionboxConstructer, GameObject EquipConstructer)
    {
        this.Item = ItemConstructer;
        this.ItemBox = ItemBoxConstructer;
        this.Selectionbox = SelectionboxConstructer;
        this.Equip = EquipConstructer;
    }
}
public class UIscript : MonoBehaviour
{
    public bool GamePaused = false;

    public Text CoinCounter;
    public List<UIElement> Inventory;
    public List<Texture> Items;
    public Color transparent;
    public Color SelectionColor;

    public int Selection = -1;
    public GameObject PausePanel;
    GameObject[] pauseobjs;

    // Start is called before the first frame update
    void Start()
    {
        pauseobjs = GameObject.FindGameObjectsWithTag("PauseButton");
        foreach (GameObject button in pauseobjs)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (i == Selection)
            {
                Inventory[i].Selectionbox.GetComponent<RawImage>().color = SelectionColor;
            }
            else
            {
                Inventory[i].Selectionbox.GetComponent<RawImage>().color = transparent;
            }
        }
    }

    public void change(int id, int itemID, bool equiped)
    {
        Inventory[id].Item.GetComponent<RawImage>().texture = Items[itemID];
        Inventory[id].Equip.GetComponent<RawImage>().enabled = equiped;
        if (itemID == 0)
        {
            Inventory[id].Item.GetComponent<RawImage>().color = transparent;
        }
        else
        {
            Inventory[id].Item.GetComponent<RawImage>().color = Color.white;
        }
    }

    public void ActivatePauseMenu()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PausePanel.GetComponent<CanvasGroup>().alpha = 1;

        pauseobjs = GameObject.FindGameObjectsWithTag("PauseButton");
        foreach (GameObject button in pauseobjs)
        {
            button.GetComponent<Button>().interactable = true;
        }
    }

    public void Continue()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GamePaused = false;
        Time.timeScale = 1;
        PausePanel.GetComponent<CanvasGroup>().alpha = 0;
        pauseobjs = GameObject.FindGameObjectsWithTag("PauseButton");
        foreach (GameObject button in pauseobjs)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
    }
}
