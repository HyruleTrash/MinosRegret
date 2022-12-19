using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCam : MonoBehaviour
{
    public Animator cam;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(camam());
    }



    IEnumerator camam()
    {
        cam.SetTrigger("open");
        yield return new WaitForSeconds(70);
        camam();
    }
}
