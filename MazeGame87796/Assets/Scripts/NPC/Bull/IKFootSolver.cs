using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    public bool Moving = true;

    public LayerMask terrainLayer = default;
    public Transform body = default;
    public IKFootSolver otherFoot = default;
    public float speed = 1f;
    public float StepDistance = 4f;
    public float StepLenght = 4f;
    public float StepHeight = 1f;
    public Vector3 FootOffset = default;
    float FootSpacing;

    Vector3 currentPosition, oldPosition, newPosition;
    Vector3 currentNormal, oldNormal, newNormal;

    float lerp;

    private void Start()
    {
        FootSpacing = transform.localPosition.x;
        currentPosition = newPosition = oldPosition = transform.position;
        currentNormal = newNormal = oldNormal = transform.up;
        lerp = 1;
    }

    void Update()
    {
        if (Moving)
        {
            transform.position = currentPosition;
            transform.up = currentNormal;

            Ray ray = new Ray(body.position + (body.right * FootSpacing), Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit info, 10, terrainLayer.value))
            {
                if (Vector3.Distance(newPosition, info.point) > StepDistance && !otherFoot.IsMoving() && lerp >= 1)
                {
                    lerp = 0;
                    int direction = body.InverseTransformPoint(info.point).z > body.InverseTransformPoint(newPosition).z ? 1 : -1;
                    newPosition = info.point + (body.forward * StepLenght * direction) + FootOffset;
                    newNormal = info.normal;
                }
            }
            if (lerp < 1)
            {
                Vector3 TempPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
                TempPosition.y += Mathf.Sin(lerp * Mathf.PI) * StepHeight;
                currentPosition = TempPosition;
                currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);

                lerp += Time.deltaTime * speed;
            }
            else
            {
                oldPosition = newPosition;
                oldNormal = newNormal;
            }
        }
    }
    
    void OnDrawGizmos()
    {
        Ray ray = new Ray(body.position + (body.right * FootSpacing), -body.up);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(ray.origin, ray.origin + (ray.direction * 10));
    }

    public bool IsMoving()
    {
        return lerp < 1;
    }
}
