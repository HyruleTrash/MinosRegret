using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public GameObject Fountain;
    Animator animator;

    public string Kind = "Lever";
    public GameObject Door;
    public int ID;
    public bool[] holdings;
    public bool state = false;

    private void Start()
    {
        if (ID == -1)
        {
            animator = Fountain.GetComponent<Animator>();
        }

        switch (Kind)
        {
            case "PuzzlePieceHolder":
                holdings = new bool[4];
                for (int i = 0; i < holdings.Length; i++)
                {
                    holdings[i] = false;
                }
                if (ID > -1 && ID < holdings.Length)
                {
                    holdings[ID] = true;
                }

                ReloadPuzzlePieces();

                break;
        }
    }

    public void activate()
    {
        switch (Kind)
        {
            case "Lever":
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                state = !state;
                Door.GetComponent<Door>().Triggers[ID] = state;
                break;
            case "PuzzlePieceHolder":
                int CurrItemInHandID = GameObject.Find("Player").GetComponent<Player>().currentlyholding;
                if (CurrItemInHandID != -1)
                {
                    Item CurrItemInHand = GameObject.Find("Player").GetComponent<Player>().Items[CurrItemInHandID];
                    if (CurrItemInHand.name.Contains("PuzzlePiece") == true)
                    {
                        string ItemBool = CurrItemInHand.name.Split('|')[1];
                        holdings[int.Parse(ItemBool)] = true;
                        ReloadPuzzlePieces();
                        if (CurrItemInHandID == GameObject.Find("Player").GetComponent<Player>().Items.Count - 1)
                        {
                            GameObject.Find("Player").GetComponent<Player>().currentlyholding = GameObject.Find("Player").GetComponent<Player>().Items.Count - 2;
                            GameObject.Find("Player").GetComponent<Player>().currentlyholdingTemp = GameObject.Find("Player").GetComponent<Player>().Items.Count - 2f;
                        }
                        GameObject.Find("Player").GetComponent<Player>().Items.RemoveAt(CurrItemInHandID);
                        GameObject.Find("Player").GetComponent<Player>().UpdateInventory();
                    }
                    else
                    {
                        int ToGet = -1;
                        for (int i = 0; i < holdings.Length; i++)
                        {
                            if (holdings[i] == true)
                            {
                                ToGet = i;
                            }
                        }
                        if (ToGet != -1 && GameObject.Find("Player").GetComponent<Player>().Items.Count < 8)
                        {
                            GameObject.Find("Player").GetComponent<Player>().Items.Add(new Item("PuzzlePiece" + "|" + ToGet, 0, false));
                            holdings[ToGet] = false;
                            ReloadPuzzlePieces();
                            GameObject.Find("Player").GetComponent<Player>().UpdateInventory();
                        }
                    }
                }
                else
                {
                    int ToGet = -1;
                    for (int i = 0; i < holdings.Length; i++)
                    {
                        if (holdings[i] == true){
                            ToGet = i;
                        }
                    }
                    if (ToGet != -1 && GameObject.Find("Player").GetComponent<Player>().Items.Count < 8)
                    {
                        GameObject.Find("Player").GetComponent<Player>().Items.Add(new Item("PuzzlePiece" + "|" + ToGet, 0, false));
                        holdings[ToGet] = false;
                        ReloadPuzzlePieces();
                        GameObject.Find("Player").GetComponent<Player>().UpdateInventory();
                    }
                }

                bool allPuzzlePieces = true;
                for (int i = 0; i < holdings.Length; i++)
                {
                    if (holdings[i] == false)
                    {
                        allPuzzlePieces = false;
                    }
                }

                if (ID == -1 && allPuzzlePieces == true)
                {
                    animator.SetBool("EndReached", true);
                    FindObjectOfType<AudioManager>().Play("Door");
                }
                break;
        }
    }

    private void ReloadPuzzlePieces()
    {
        for (int i = 0; i < holdings.Length; i++)
        {
            this.transform.GetChild(i + 1).GetComponent<MeshRenderer>().enabled = holdings[i];
        }
    }
}
