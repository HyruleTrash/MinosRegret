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

    void Update()
    {
        Vector3 PlayerNoY = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        transform.LookAt(PlayerNoY);
        
        if (Vector3.Distance(transform.position, player.transform.position) < HitDistace)
        {
            AppHelper.Quit();
        }

        Debug.Log(NewPosRoam);
        if (FollowingPlayer)
        {
            agent.SetDestination(player.transform.position);
        }
        else
        {
            Vector3 NoYRegretPos = new Vector3(transform.position.x, 0, transform.position.z);
            if (Vector3.Distance(NoYRegretPos, NewPosRoam) < MinCloseValueRoamPos)
            {
                NewPosRoam = NewRoamPos();
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
