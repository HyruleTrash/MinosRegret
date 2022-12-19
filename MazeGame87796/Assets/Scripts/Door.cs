using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool[] Triggers;
    bool soundPlayed = false;

    void Update()
    {
        bool allTrue = true;
        for (int i = 0; i < Triggers.Length; i++)
        {
            if (Triggers[i] == false)
            {
                allTrue = false;
            }
        }
        GetComponent<MeshCollider>().enabled = !allTrue;
        GetComponent<MeshRenderer>().enabled = !allTrue;
        if (allTrue == true)
        {
            if (soundPlayed == false)
            {
                FindObjectOfType<AudioManager>().Play("Door");
                soundPlayed = true;
            }
        }
        if (soundPlayed == true && allTrue == false)
        {
            FindObjectOfType<AudioManager>().Play("Door2");
            soundPlayed = false;
        }
    }
}
