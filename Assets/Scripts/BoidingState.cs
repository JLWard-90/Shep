using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidingState : FSMState
{
    private float shepDistanceLimit = 70;
    private SheepController sheep;
    bool avoiding = false;
    List<Transform> otherSheepInFlock;
    public BoidingState(Transform npc)
    {
        stateID = FSMStateID.Fleeing;
        sheep = npc.GetComponent<SheepController>();
        shepDistanceLimit = sheep.shepDistLimit;
    }
    public override void BeforeEnter()
    {
        //throw new System.NotImplementedException();
        Debug.Log("Enter BoidingState");
    }

    public override void BeforeExit()
    {
        //throw new System.NotImplementedException();
        Debug.Log("Exit BoidingState");
    }

    public override void Reason(Transform player, Transform npc)
    {
        //throw new System.NotImplementedException();

        //In our reasoning state we need to be able to add or remove sheep from the flock
        //Add sheep if they become close enough
        //Remove sheep if they are far away enough
        //Switch to wandering state if no more sheep in flock on a coin flip
        //Switch to fleeing state if player too close
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
        if (Physics.Raycast(sheep.transform.position, Vector3.forward, out hit))
        {
            float collisionDistance = (sheep.speed / Time.deltaTime) * 3;
            if (hit.distance < collisionDistance) //If sheep will collide with something
            {
                if(!avoiding)//If not already avoiding
                {
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
                npc.rotation = Quaternion.Euler(0, npc.eulerAngles.y + (sheep.turnSpeed * leftRight * Time.deltaTime), 0);
                npc.position += sheep.speed * npc.forward * Time.deltaTime;
            }
        }
        else
        {
            sheep.turnDir = 0; //If no collision detected set the turning direction to 0
            //Now We have delt with avoiding immediate collisions, we can do the flocking
            //So first we need a component to deal with aligning the flock. This should be the dominant component, say 0.75 of the turn
            if(otherSheepInFlock.Count > 0) //Only try flocking behaviour if there is a flock
            {

            }
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

}
