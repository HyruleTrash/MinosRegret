using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInfoLoader : MonoBehaviour
{
    void Start()
    {
        GameVariables GameVars = GameObject.Find("GameVariablesHolder").GetComponent<GameVariables>();
        string tekst = "";
        tekst += "Coins collected: " + GameVars.Coins + "\n";
        tekst += "Hats merged together: " + GameVars.HatsMerged + "\n";
        tekst += "Amount of powerups collected: " + GameVars.PowerUpsCollected + "\n";
        tekst += "Time played: " + GameVars.PlayTime + "\n";
        this.gameObject.GetComponent<Text>().text = tekst;
        Destroy(GameVars);
    }
}
