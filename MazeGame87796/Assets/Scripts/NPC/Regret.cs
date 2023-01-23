using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Regret : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent agent;
    public LayerMask Layer;
    public float RaycastLookYOffset = 5f;
    public float walkDisctance = 20f;
    public float MinCloseValueRoamPos = 2;
    public float HitDistace = 5f;
    public float MaxTimeBetweenFrames = 2f;
    bool Roaming = false;
    bool FollowingPlayer = false;
    float maxDistance = 300f;
    Vector3 NewPosRoam;
    Vector3 Oldpos;
    Vector3 Olderpos;

    private void Start()
    {
        Oldpos = transform.position;
    }

    void Update()
    {
        Vector3 PlayerNoY = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        transform.LookAt(PlayerNoY);
        
        if (Vector3.Distance(transform.position, player.transform.position) < HitDistace)
        {
            AppHelper.Quit();
        }

        if (FollowingPlayer)
        {
            agent.SetDestination(player.transform.position);
        }
        else
        {
            Vector2 NoYRegretPos = new Vector2(transform.position.x, transform.position.z);
            Debug.Log(NewPosRoam != null);
            if (Vector3.Distance(NoYRegretPos, new Vector2(NewPosRoam.x, NewPosRoam.z)) < MinCloseValueRoamPos)
            {
                NewPosRoam = NewRoamPos();
            }
            else
            {
                if (Olderpos == transform.position)
                {
                    NewPosRoam = NewRoamPos();
                }
                if (Oldpos == transform.position)
                {
                    Olderpos = transform.position;
                }
            }
            if (NewPosRoam != null)
            {
                agent.SetDestination(NewPosRoam);
            }
        }

        RaycastHit hit;
        Debug.DrawRay(transform.position + new Vector3(0, RaycastLookYOffset, 0), (PlayerNoY + new Vector3(0, 2, 0) - transform.position).normalized * maxDistance, Color.red);
        if (Physics.Raycast(transform.position + new Vector3(0, RaycastLookYOffset, 0), (PlayerNoY + new Vector3(0, 2, 0) - transform.position).normalized, out hit, maxDistance, Layer))
        {
            if (hit.transform.parent.name == player.name)
            {
                FollowingPlayer = true;
                Roaming = false;
            }
            else
            {
                FollowingPlayer = false;
                if (Roaming == false)
                {
                    NewPosRoam = NewRoamPos();
                    Roaming = true;
                }
            }
        }

        Oldpos = transform.position;
    }

    Vector3 NewRoamPos()
    {
        Vector3 RandomDir1 = Random.insideUnitCircle;
        Vector3 RandomDir = new Vector3(RandomDir1.x, RandomDir1.z, RandomDir1.y);
        RaycastHit hit2;
        Vector3 NewPosRoamTemp = RandomDir * walkDisctance;
        Debug.DrawRay(transform.position, RandomDir * walkDisctance, Color.red);
        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), RandomDir, out hit2, walkDisctance, Layer))
        {
            NewPosRoamTemp = hit2.point;
        }

        return NewPosRoamTemp;
    }
}
