using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Regret : MonoBehaviour
{
    public GameObject player;
    public NavMeshAgent agent;
    public LayerMask Layer;
    public float RaycastLookYOffset = 1.2f;
    public float walkDisctance = 20f;
    public float MinCloseValueRoamPos = 2;
    public float HitDistace = 5f;
    public Vector3[] offsets;
    public bool Roaming = true;
    public bool FollowingPlayer = false;
    float maxDistance = 300f;
    Vector3 NewPosRoam;
    Vector3 Oldpos;
    Vector3 Olderpos;

    bool RoamTimerPlaying = false;
    float TimeUntilNewRoam = 0;
    public float StartTimeNewRoam = 1.5f;

    public float MaxTimeBetweenFrames = 2f;
    float MaxFrameTime;
    float currentFrameTime = 0;
    int CurrentFrame = 0;
    int LastFrame = 0;
    public Texture[] Frames;
    bool changingFrames = false;

    public float Highspeed = 10;
    public float lowspeed = 5;


    private void Start()
    {
        Oldpos = transform.position;
        player = FindObjectOfType<Player>().gameObject;

        NewFrame();
    }

    void NewFrame()
    {
        MaxFrameTime = Mathf.RoundToInt(Random.RandomRange(0, MaxTimeBetweenFrames));
        if (MaxFrameTime < 0.2f)
        {
            MaxFrameTime = 0.2f;
        }
        int tempNewFrame = Mathf.RoundToInt(Random.RandomRange(0, Frames.Length));
        while (tempNewFrame == CurrentFrame && tempNewFrame == LastFrame)
        {
            tempNewFrame = Mathf.RoundToInt(Random.RandomRange(0, Frames.Length));
            if (tempNewFrame != CurrentFrame && tempNewFrame != LastFrame)
            {
                break;
            }
        }
        LastFrame = CurrentFrame;
        CurrentFrame = tempNewFrame;

        gameObject.GetComponentInChildren<MeshRenderer>().material.SetTexture("_MainTex", Frames[CurrentFrame]);
        currentFrameTime = 0;
        changingFrames = false;
    }

    public void UpdateCurrSpeed(){
        if (FollowingPlayer)
        {
            if (agent.speed != Highspeed)
            {
                agent.speed = Highspeed;
            }
        }
        else
        {
            if (agent.speed != lowspeed)
            {
                agent.speed = lowspeed;
            }
        }
    }

    void Update()
    {
        if (RoamTimerPlaying = true)
        {
            TimeUntilNewRoam += Time.deltaTime;
            if (TimeUntilNewRoam > StartTimeNewRoam)
            {
                Roaming = true;
                RoamTimerPlaying = false;
                FollowingPlayer = false;
            }
        }

        if (!changingFrames)
        {
            currentFrameTime += Time.deltaTime;
            if (currentFrameTime >= MaxFrameTime)
            {
                changingFrames = true;
                NewFrame();
            }
        }

        Vector3 PlayerNoY = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        transform.LookAt(PlayerNoY);
        
        if (Vector3.Distance(transform.position, player.transform.position) < HitDistace)
        {
            AppHelper.Quit();
        }

        if (FollowingPlayer)
        {
            if (agent.speed != Highspeed)
            {
                agent.speed = Highspeed;
            }
            agent.SetDestination(player.transform.position);
        }
        else
        {
            Vector2 NoYRegretPos = new Vector2(transform.position.x, transform.position.z);
            //Debug.Log(NewPosRoam != null);
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
                if (agent.speed != lowspeed)
                {
                    agent.speed = lowspeed;
                }
                agent.SetDestination(NewPosRoam);
            }
        }

        bool[] Rayhits = new bool[offsets.Length];
        for (int i = 0; i < offsets.Length; i++)
        {
            RaycastHit hit;
            Vector3 Dir = ((player.transform.position + new Vector3(0, RaycastLookYOffset, 0)) - (transform.position + offsets[i])).normalized;
            Debug.DrawRay(transform.position + offsets[i], Dir * maxDistance, Color.red);
            if (Physics.Raycast(transform.position + offsets[i], Dir, out hit, maxDistance, Layer))
            {
                if (hit.transform.parent.name == player.name)
                {
                    Rayhits[i] = true;
                }
                else
                {
                    Rayhits[i] = false;
                }
            }
        }
        bool AllFalse = true;
        for (int i = 0; i < Rayhits.Length; i++)
        {
            if (Rayhits[i] == true)
            {
                AllFalse = false;
            }
        }
        if (AllFalse)
        {
            if (RoamTimerPlaying == false)
            {
                NewPosRoam = NewRoamPos();
                RoamTimerPlaying = true;
                TimeUntilNewRoam = 0;
            }
        }
        else
        {
            FollowingPlayer = true;
            Roaming = false;
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
