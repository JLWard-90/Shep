using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    //Thois is the finite state maching (FSM) class
    // Start is called before the first frame update
    public enum Transition
    {
        None = 0,
        SawPlayer, //When the ai sees the player
        LostPlayer, //When the ai is out of range of the player
        InPen, //When the ai is in the pen
    }

    public enum FSMStateID
    {
        None = 0,
        Wandering,
        Standing,
        Fleeing,
    }
}
