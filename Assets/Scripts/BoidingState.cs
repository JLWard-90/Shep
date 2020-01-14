using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidingState : FSMState
{
    public override void BeforeEnter()
    {
        throw new System.NotImplementedException();
    }

    public override void BeforeExit()
    {
        throw new System.NotImplementedException();
    }

    public override void Reason(Transform player, Transform npc)
    {
        throw new System.NotImplementedException();
    }

    public override void Act(Transform player, Transform npc)
    {
        throw new System.NotImplementedException();

        //Boiding is flock-like behaviour. In principle it follows three rules:
        //1) Boids will avoid colliding with each other
        //2) Boids will orientate to face the average direction of the flock
        //3) Boids will attempt to reach the centre of the flock


    }

}
