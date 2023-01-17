using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bull : MonoBehaviour
{
    public SkinnedMeshRenderer meshRenderer;
    public MeshCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Mesh colliderMesh = new Mesh();
        meshRenderer.BakeMesh(colliderMesh);
        collider.sharedMesh = null;
        collider.sharedMesh = colliderMesh;
    }
}
