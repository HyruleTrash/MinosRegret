using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bull : MonoBehaviour
{
    public GameObject Player;

    public GameObject LookAtTarget;
    public GameObject[] FootTargets;

    public GameObject BullMesh;
    public GameObject AI;
    NavMeshAgent agent;

    public bool Following = true;
    bool OldFollowingState = true;


    public float StepDistance = 1.5f;
    public float StepHeight = 0.4f;
    public float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        agent = AI.GetComponent<NavMeshAgent>();
        for (int i = 0; i < FootTargets.Length; i++)
        {
            FootTargets[i].GetComponent<IKFootSolver>().StepDistance = StepDistance;
            FootTargets[i].GetComponent<IKFootSolver>().StepHeight = StepHeight;
            FootTargets[i].GetComponent<IKFootSolver>().speed = speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 PlayerPos = Player.transform.position;
        if (OldFollowingState != Following)
        {
            OldFollowingState = Following;
            if (Following)
            {
                for (int i = 0; i < FootTargets.Length; i++)
                {
                    FootTargets[i].GetComponent<IKFootSolver>().Moving = true;
                }
            }
            else
            {
                for (int i = 0; i < FootTargets.Length; i++)
                {
                    FootTargets[i].GetComponent<IKFootSolver>().speed = speed;
                }
            }
        }
        if (Following)
        {
            agent.SetDestination(PlayerPos);
            BullMesh.transform.position = AI.transform.position;

            for (int i = 0; i < FootTargets.Length; i++)
            {
                FootTargets[i].GetComponent<IKFootSolver>().Moving = true;
            }
        }
        float HeadDir = Vector2.Dot(new Vector2(transform.position.x, transform.position.y).normalized, new Vector2(PlayerPos.x, PlayerPos.z).normalized);
        if (HeadDir >= 0.6)
        {
            LookAtTarget.transform.position = new Vector3(PlayerPos.x, this.transform.position.y + 7.45f, PlayerPos.z);
        }
    }
}
