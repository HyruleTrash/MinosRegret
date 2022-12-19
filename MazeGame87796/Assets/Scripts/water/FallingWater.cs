using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingWater : MonoBehaviour
{
    public float TextureSpeed = 0.6f;
    Material StillWaterMat;
    float TimeCounter;

    private void Awake()
    {
        StillWaterMat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        TimeCounter += Time.deltaTime * TextureSpeed;

        StillWaterMat.SetTextureOffset("_MainTex", new Vector2(0, TimeCounter));
    }
}
