using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaf : MonoBehaviour
{
    public float Radius1 = 0.1f;
    public float TextureSpeed = 0.6f;
    Material StillWaterMat;

    float TimeCounter;

    private void Awake()
    {
        StillWaterMat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        //Vector2 Offset = StillWaterMat.GetTextureOffset("_MainTex");
        TimeCounter += Time.deltaTime * TextureSpeed;
        StillWaterMat.SetTextureOffset("_MainTex", new Vector2(Mathf.Cos(TimeCounter) * Radius1, Mathf.Sin(TimeCounter) * Radius1));
    }
}
