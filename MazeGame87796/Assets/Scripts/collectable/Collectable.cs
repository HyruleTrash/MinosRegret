using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectable : MonoBehaviour
{
    public Text PlayerTextPlane;
    public int LevelOfUnlock = 0;
    public string LockedMessage = "This doesn't seem moveable until later";
    GameVariables gamevars;
    public List<GameObject> EventGameStateChange;

    public int TimeMax = 2;
    public Color NpcColor;

    int currentchar = 0;
    bool playing = false;
    bool TimerPlaying = false;
    float TimeLeft = 0;
    float OriginalAlpha;

    public GameObject Fountain;
    Animator animator;

    public string Kind = "Lever";
    public GameObject Door;
    public int ID;
    public bool[] holdings;
    public bool state = false;

    private void Start()
    {
        OriginalAlpha = PlayerTextPlane.color.a;
        gamevars = FindObjectOfType<GameVariables>();
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
        if (LevelOfUnlock <= gamevars.Gamestage)
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


                    if (ID == -1)
                    {
                        int TotalPieces = 0;
                        for (int i = 0; i < holdings.Length; i++)
                        {
                            if (holdings[i] == true)
                            {
                                TotalPieces++;
                            }
                        }

                        if (TotalPieces == holdings.Length)
                        {
                            animator.SetBool("EndReached", true);
                            FindObjectOfType<AudioManager>().Play("Door");
                        }
                        if (TotalPieces > gamevars.Gamestage)
                        {
                            gamevars.Gamestage = TotalPieces;
                            for (int i = 0; i < EventGameStateChange.Count; i++)
                            {
                                EventGameStateChange[i].GetComponent<GameChanged>().StateUpdate(gamevars.Gamestage);
                            }
                        }
                    }
                    break;
            }
        }
        else
        {
            if (!playing)
            {
                PlayerTextPlane.text = "";
                TimerPlaying = false;
                playing = true;
                currentchar = 0;
                PlayerTextPlane.color = NpcColor;
                PlayerTextPlane.color = new Color(NpcColor.r, NpcColor.g, NpcColor.b, OriginalAlpha);
            }
        }
    }

    private void Update()
    {
        if (TimerPlaying)
        {
            TimeLeft += Time.deltaTime;
            float Alpha = (1 - (((100 / TimeMax) * TimeLeft) / 100)) * OriginalAlpha;
            PlayerTextPlane.color = new Color(PlayerTextPlane.color.r, PlayerTextPlane.color.g, PlayerTextPlane.color.b, Alpha);
            if (TimeLeft > TimeMax)
            {
                TimerPlaying = false;
            }
        }

        if (playing)
        {
            PlayerTextPlane.text += LockedMessage[currentchar];
            currentchar++;
            if (currentchar >= LockedMessage.Length)
            {
                playing = false;
                TimerPlaying = true;
                TimeLeft = 0;
            }
            PlayerTextPlane.text = PlayerTextPlane.text.Replace("@", System.Environment.NewLine);
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
