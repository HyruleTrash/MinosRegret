using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnd : MonoBehaviour
{
    public List<GameObject> Particles;
    public GameObject Exit;

    public void StartGame()
    {
        for (int i = 0; i < Particles.Count; i++)
        {
            Particles[i].GetComponent<ParticleSystem>().Play();
        }
        Exit.GetComponent<Animator>().SetTrigger("TutorialEnd");
    }
}
