using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StillWater : MonoBehaviour
{
    public float Radius1 = 0.1f;
    public float TextureSpeed = 0.6f;
    public float AnimationSpeed = 0.6f;
    public float WaveSpeed = 0.01f;
    public float WaveSize = 0.1f;

    public Texture[] frames;
    int currentFrame = 0;

    Material StillWaterMat;
    Mesh WaterMesh;

    Vector3[] DirectionVertices;
    int[] VerticeDirectionMoving;

    float TimeCounter;
    float TimeCounter1;

    private void Awake()
    {
        StillWaterMat = GetComponent<Renderer>().material;
        WaterMesh = GetComponent<MeshFilter>().mesh;
    }

    private void Start()
    {
        DirectionVertices = WaterMesh.vertices;
        VerticeDirectionMoving = new int[DirectionVertices.Length];
        for (int i = 0; i < DirectionVertices.Length; i++)
        {
            VerticeDirectionMoving[i] = 0;
        }
    }

    void Update()
    {
        TimeCounter += Time.deltaTime * TextureSpeed;
        TimeCounter1 += Time.deltaTime * AnimationSpeed;
        StillWaterMat.SetTextureOffset("_MainTex", new Vector2(Mathf.Cos(TimeCounter) * Radius1, Mathf.Sin(TimeCounter) * Radius1));
        if (TimeCounter1 > frames.Length - 1)
        {
            TimeCounter1 = 0;
        }
        currentFrame = (int)Math.Round(TimeCounter1, 0);
        StillWaterMat.SetTexture("_MainTex", frames[currentFrame]);

        Vector3[] vertices = WaterMesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            switch (VerticeDirectionMoving[i])
            {
                case 0:
                    DirectionVertices[i] = new Vector3(vertices[i].x, vertices[i].y, 0);
                    DirectionVertices[i] += new Vector3(0, 0, UnityEngine.Random.Range(0, WaveSize));
                    VerticeDirectionMoving[i] = 1;
                    break;

                case 1:
                    vertices[i] = Vector3.Lerp(vertices[i], DirectionVertices[i], WaveSpeed);
                    if (Vector3.Distance(vertices[i], DirectionVertices[i]) < WaveSpeed)
                    {
                        VerticeDirectionMoving[i] = 2;
                    }
                    break;

                case 2:
                    vertices[i] = Vector3.Lerp(vertices[i], new Vector3(vertices[i].x, vertices[i].y,0), WaveSpeed);
                    if (Vector3.Distance(new Vector3(vertices[i].x, vertices[i].y, 0), vertices[i]) < WaveSpeed)
                    {
                        VerticeDirectionMoving[i] = 0;
                    }
                    break;
            }
        }
        WaterMesh.vertices = vertices;
        WaterMesh.RecalculateNormals();
    }
}
