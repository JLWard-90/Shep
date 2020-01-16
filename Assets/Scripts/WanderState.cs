using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : FSMState
{
    private float shepDistanceLimit = 50;
    GameObject[] sheeples;
    private SheepController sheep;
    bool avoiding = false;

    public WanderState(Transform npc)
    {
        sheep = npc.GetComponent<SheepController>();
        shepDistanceLimit = sheep.shepDistLimit;
        sheep.InitRandomWalk();
        stateID = FSMStateID.Wandering;
        GameObject[] sheeples = GameObject.FindGameObjectsWithTag("sheep");
    }

    public override void Reason(Transform player, Transform npc)
    {
        //throw new System.NotImplementedException();
        //Check the distance between the player and this npc
        if (sheep.inPen == true)
        {
            Debug.Log("switch to standingState");
            sheep.SetTransition(Transition.InPen);
        }
        if (Vector3.Distance(npc.position, player.position) < shepDistanceLimit)
        {
            float noticeNum = Random.Range(0, 2f);
            if (noticeNum > 1) //50% chance of the sheep noticing the player
            {
                Debug.Log("Switch to Fleeing state");
                sheep.SetTransition(Transition.SawPlayer);
            }
        }
        else if (Random.Range(0, 2) > 0)//if on coin flip
        {
            if (sheeples == null)
            {
                sheeples = GameObject.FindGameObjectsWithTag("sheep");
            }
            if (sheeples.Length > 0) //This should always be true
            {
                foreach(GameObject sh in sheeples)
                {
                    
                    if (Vector3.Distance(npc.position, sh.transform.position) < sheep.friendDistance && npc.gameObject != sh.gameObject)
                    {
                        Debug.Log(Vector3.Distance(npc.position, sh.transform.position));
                        //Debug.Log("Switch to Boiding state");
                        sheep.SetTransition(Transition.FoundFriend);
                    }
                }
            }
        }
        
    }

    public override void Act(Transform player, Transform npc)
    {
        RaycastHit hit;
        //Collision avoidance:
        if (Physics.Raycast(sheep.transform.position + (Vector3.forward * 2), Vector3.forward, out hit))
        {
            //Debug.Log("collisionDetector hit");
            float collisionDistance = (sheep.speed * Time.deltaTime) * 50;
            Debug.Log(collisionDistance);
            Debug.Log(hit.distance);
            if (hit.distance < collisionDistance) //If sheep will collide with something
            {
                if (!avoiding)//If not already avoiding
                {
                    Debug.Log("avoiding collision");
                    sheep.turnDir = TurnDir();
                    avoiding = true;
                }
                int leftRight = 0;
                if (sheep.turnDir == 1)
                {
                    leftRight = 1;
                }
                else
                {
                    leftRight = -1;
                }
                npc.rotation = Quaternion.Euler(0, npc.eulerAngles.y + (sheep.turnSpeed * leftRight * Time.deltaTime), 0);
                npc.position += sheep.speed * npc.forward * Time.deltaTime;

            }

            return;//Only do collision avoidance on this Act
        }
        else
        {
            avoiding = false;
        }
        //throw new System.NotImplementedException();
        //The behaviour associated with this state:
        sheep.SimpleSheepMovement();
    }

    public override void BeforeEnter()
    {
        //throw new System.NotImplementedException();
    }

    public override void BeforeExit()
    {
        //throw new System.NotImplementedException();
    }

    int TurnDir()
    {
        if (sheep.turnDir == 1) //Turn positive
            return 1;
        else if (sheep.turnDir == 2) //Turn negative
            return 2;
        else
        {
            int dir = Random.Range(1, 3);
            return dir;
        }
    }
}
