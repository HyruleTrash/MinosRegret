using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Regret : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent agent;
    public float walkDisctance = 20f;
    public float HitDistace = 5f;
    public float MaxTimeBetweenFrames = 2f;

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

        if (FollowingPlayer)
        {
            agent.SetDestination(player.transform.position);
        }
        else
        {
            if (NewPosRoam != transform.position)
            {
                Vector3 RandomDir = Random.insideUnitCircle * walkDisctance;
                RaycastHit hit2;
                NewPosRoam = Vector3.zero;
                if (Physics.Raycast(transform.position, RandomDir, out hit2, walkDisctance))
                {
                    NewPosRoam = hit2.point;
                }
            }
            agent.SetDestination(NewPosRoam);
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, (PlayerNoY - transform.position).normalized, out hit, maxDistance))
        {
            if (hit.transform.gameObject == player)
            {
                FollowingPlayer = true;
            }
            else
            {
                FollowingPlayer = false;
            }
        }
    }
}
