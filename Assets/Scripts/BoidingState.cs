using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math; //Necessary to compute mean angles
using System.Linq; //Necessary to compute mean angles

public class BoidingState : FSMState
{
    private float shepDistanceLimit = 70;
    private SheepController sheep;
    bool avoiding = false;
    List<Transform> otherSheepInFlock; //Each sheep will keep track of the other sheep in its flock. A flock will therefore be a construct of each sheep's view of the world around it.
    public BoidingState(Transform npc)
    {
        stateID = FSMStateID.Boiding;
        sheep = npc.GetComponent<SheepController>();
        shepDistanceLimit = sheep.shepDistLimit;
        otherSheepInFlock = new List<Transform>();
    }
    public override void BeforeEnter()
    {
        //throw new System.NotImplementedException();
        Debug.Log("Enter BoidingState");
    }

    public override void BeforeExit()
    {
        //throw new System.NotImplementedException();
        //Debug.Log("Exit BoidingState");
    }

    public override void Reason(Transform player, Transform npc)
    {
        //throw new System.NotImplementedException();
        //In our reasoning state we need to be able to add or remove sheep from the flock
        //Add sheep if they become close enough
        //Remove sheep if they are far away enough
        RaycastHit[] hit;
        hit = Physics.SphereCastAll(npc.position, sheep.friendDistance, npc.forward);
        //Debug.Log(hit);
        if (sheep.inPen)
        {
            Debug.Log("transition to stand");
            sheep.SetTransition(Transition.InPen);
        }
        else if (Vector3.Distance(npc.position, player.position)<sheep.shepDistLimit)
        {
            sheep.SetTransition(Transition.SawPlayer);
        }
        else if (hit.Length > 0)
        {
            //Add sheep to flock
            //
            foreach (RaycastHit q in hit)
            {
                if (q.transform.gameObject.tag == "sheep" && !otherSheepInFlock.Contains(q.transform) && q.transform != npc && q.distance > 2)
                {
                    otherSheepInFlock.Add(q.transform);
                }
            }
        }
        else if (Vector3.Distance(npc.position, player.position) < sheep.shepDistLimit)
        {
            Debug.Log("Switching to fleeing state");
            sheep.SetTransition(Transition.SawPlayer);
        }
        else if (otherSheepInFlock.Count > 0) //If the sheep even has a flock
        {
            CullFlock(npc);
        }
        else
        {
            //If there is no flock then the sheep should not be boiding at all. Set to WanderState
            Debug.Log("Switch to Walking state");
            sheep.SetTransition(Transition.LostPlayer);//Lost player pretty much just sets the sheep to wander so I might as well just use that.
        }
    }

    public override void Act(Transform player, Transform npc)
    {
        //throw new System.NotImplementedException();

        //Boiding is flock-like behaviour. In principle it follows three rules:
        //1) Boids will avoid colliding with each other
        //2) Boids will orientate to face the average direction of the flock
        //3) Boids will attempt to reach the centre of the flock

        //First identify whether any other sheep are in the sheep's path:
        Vector3 forwardDir = Vector3.forward;
        //Raycast forward:
        RaycastHit hit;
        //Debug.Log("boiding...");
        if (Physics.Raycast(sheep.transform.position+(npc.forward*2), npc.forward, out hit))
        {
            //Debug.Log("collisionDetector hit");
            float collisionDistance = (sheep.speed * Time.deltaTime) * 50;
            //Debug.Log(collisionDistance);
            //Debug.Log(hit.distance);
            if (hit.distance < collisionDistance) //If sheep will collide with something
            {
                if(!avoiding)//If not already avoiding
                {
                    Debug.Log("avoiding collision");
                    sheep.turnDir = TurnDir();
                    avoiding = true;
                }
                int leftRight = 0;
                if(sheep.turnDir == 1)
                {
                    leftRight = 1;
                }
                else
                {
                    leftRight = -1;
                }
                npc.rotation = Quaternion.Euler(0, npc.eulerAngles.y + (sheep.turnSpeed * 10 * leftRight * Time.deltaTime), 0);
                npc.position += sheep.speed * npc.forward * Time.deltaTime;
                return;//Only do collision avoidance on this Act
            }
            else
            {
                avoiding = false;
            }
            
        }

        //Debug.Log(otherSheepInFlock.Count);
            sheep.turnDir = 0; //If no collision detected set the turning direction to 0
            //Now We have delt with avoiding immediate collisions, we can do the flocking
            //So first we need a component to deal with aligning the flock. This should be the dominant component, say 0.75 of the turn
            if(otherSheepInFlock.Count > 0) //Only try flocking behaviour if there is a flock
            {
                float averageFlockAngle = 0;
                Vector3 averageFlockPosition = new Vector3(0, 0, 0);
                foreach (Transform sheepTrans in otherSheepInFlock)
                {
                    averageFlockAngle += sheepTrans.eulerAngles.y; //For the moment, we are assuming that the game always takes place on a flat surface
                    averageFlockPosition.x += sheepTrans.position.x;
                    averageFlockPosition.y += sheepTrans.position.y;
                    averageFlockPosition.z += sheepTrans.position.z;
                }
                averageFlockAngle = averageFlockAngle / otherSheepInFlock.Count;
                averageFlockPosition.x = averageFlockPosition.x / otherSheepInFlock.Count;
                averageFlockPosition.y = 0;// averageFlockPosition.y / otherSheepInFlock.Count;
                averageFlockPosition.z = averageFlockPosition.z / otherSheepInFlock.Count;
                Vector3 towardsFlockCentre = averageFlockPosition - npc.transform.position;
                //0.75 weighting to flock average direction
                npc.rotation = Quaternion.Euler(0, npc.eulerAngles.y + (sheep.turnSpeed *0.1f* Time.deltaTime * (averageFlockAngle - npc.eulerAngles.y)), 0);
                //0.25 weighting to flock centre
                Vector3 newDir = Vector3.RotateTowards(npc.forward, towardsFlockCentre, sheep.turnSpeed*0.01f, 0.0f);
                npc.rotation = Quaternion.LookRotation(newDir);
                npc.position += sheep.speed * npc.forward * Time.deltaTime;
            }
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
    /* The following is no longer used in this script but is certainly useful to know!
    static double MeanAngle(double[] angles)
    {
        var x = angles.Sum(a => Cos(a * PI / 180)) / angles.Length;
        var y = angles.Sum(a => Sin(a * PI / 180)) / angles.Length;
        return Atan2(y, x) * 180 / PI;
    }*/

    void CullFlock(Transform npc)
    {
        List<int> sheepToRemove = new List<int>();
        for (int i = 0; i < otherSheepInFlock.Count; i++)
        {
            Transform singleSheep = otherSheepInFlock[i];
            if (Vector3.Distance(npc.position, singleSheep.position) > sheep.friendDistance)//If sheep are too far away to be friends
            {
                sheepToRemove.Add(i);//Add those sheep to a list and then...
            }
        }
        if (sheepToRemove.Count > 0)
        {
            foreach (int i in sheepToRemove)
            {
                otherSheepInFlock.RemoveAt(i); //... remove those sheep from the flock
            }
        }
    }
}
