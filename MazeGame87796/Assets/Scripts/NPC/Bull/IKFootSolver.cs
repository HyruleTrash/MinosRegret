using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolver : MonoBehaviour
{
    public bool Moving = true;

    public GameObject body;
    public float FootSpacing;

    public float StepDistance = 1.5f;
    public float StepHeight = 0.4f;
    public float speed = 2f;

    Vector3 currentPosition;
    Vector3 oldPosition;
    Vector3 newPosition;

    float lerp = 0;

    void Update()
    {
        if (Moving)
        {
            transform.position = currentPosition;

            Ray ray = new Ray(body.transform.position + (body.transform.right * FootSpacing), Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit info, 10))
            {
                if (Vector3.Distance(newPosition, info.point) > StepDistance)
                {
                    lerp = 0;
                    newPosition = info.point;
                }
            }
            if (lerp < 1)
            {
                Vector3 Footposition = Vector3.Lerp(oldPosition, newPosition, lerp);
                Footposition.y += Mathf.Sin(lerp * Mathf.PI) * StepHeight;
                currentPosition = Footposition;

                lerp += Time.deltaTime * speed;
            }
            else
            {
                oldPosition = newPosition;
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 pos = body.transform.position + (body.transform.right * FootSpacing);
        Gizmos.DrawLine(pos, pos + (Vector3.down * 10));
    }
}
