using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : FSM //The sheep controller inherits from the FSM class
{
    public float shepDistLimit = 10;
    public float speed = 5;
    public float turnSpeed = 45;
    public bool inPen = false;
    float decisionTimer = 0;
    float decisionTimeLimit = 5; //Time between decisions in seconds
    bool decisionMade = false;
    float leftRight;
    bool turning;
    float turningTime;
    [SerializeField]
    bool AvoidingShep = false;
    Transform shepTransform;
    public float maxSpeed = 20;
    [SerializeField]
    float shepDistance;
    Animator animator;
    bool moving;
    private void Awake() 
    {
        shepTransform = GameObject.Find("Player").transform;
        animator = this.gameObject.GetComponentInChildren<Animator>();
        fsmStates = new List<FSMState>();
        ConstructFSM(); //Construct the finite state machine
    }
    // Start is called before the first frame update
    void Start()
    {
        decisionMade = false;
        decisionTimeLimit = Random.Range(0,8);
        AvoidingShep = false;
        animator.SetBool("moving", true);
        moving = true; //Set the sheep moving
        SetInitialState();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        /*Vector3 awayFromShep = transform.position - shepTransform.position;
        shepDistance = Vector3.Magnitude(awayFromShep);
        checkForShep();
        if (AvoidingShep && moving) //Here we implement a simple finite state machine where the sheep has two three states: Avoiding shep, Simple sheep movement, stopped
        {
            AvoidShepWalk();
        }
        else if (moving)
        {
            SimpleSheepMovement();
        }
        else
        {
            Debug.Log("sheep not moving");
        }*/
        CurrentState.Reason(shepTransform, transform);
        CurrentState.Act(shepTransform, transform);
        
    }

    private void RandomSheepWalk()
    {
        if (turning)
        {
            transform.rotation = Quaternion.Euler(0,transform.eulerAngles.y + (turnSpeed * leftRight * Time.deltaTime),0);
            transform.position += speed * transform.forward * Time.deltaTime;
        }
        else
        {
            transform.position += speed * transform.forward * Time.deltaTime;
        }
        if (!decisionMade)
        {
            leftRight = Random.Range(-1.0f,1.0f);
            decisionMade = true;
            turning = true;
            turningTime = Random.Range(0.0f,1.5f);
        }
    }
    public void AvoidShepWalk()
    {
        Vector3 awayFromShep = transform.position - shepTransform.position;
        float step = turnSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, awayFromShep, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
        float runSpeed = (maxSpeed - speed) * (shepDistance / shepDistLimit) + speed;
        transform.position += runSpeed * transform.forward * Time.deltaTime;
    }

    void checkForShep() //Checks if the sheep has noticed shep
    {
        if (Vector3.Magnitude(transform.position - shepTransform.position) < shepDistLimit)
        {
            float noticeNum = Random.Range(0,2f);
            if (noticeNum > 1)
            {
                AvoidingShep = true;
            }
        }
        else
        {
            AvoidingShep = false;
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Pen")
        {
            moving = false;
            animator.SetBool("moving",false);
        }
    }

    public void SimpleSheepMovement()
    {
        RandomSheepWalk();
        decisionTimer += Time.deltaTime;
        if (!turning && decisionTimer > turningTime)
        {
            turning = false;
        }
        if (decisionTimer > decisionTimeLimit)
        {
            decisionTimer = 0;
            decisionTimeLimit = Random.Range(0, 8);
            Debug.Log(decisionTimeLimit);
            decisionMade = false;
        }
    }

    public void SetTransition(Transition t)
    {
        if (t==Transition.SawPlayer)
        {
            PerformTransition(t);
        }
        if (t==Transition.LostPlayer)
        {
            PerformTransition(t);   
        }
        if (t==Transition.InPen)
        {
            PerformTransition(t);
        }
    }

    private void ConstructFSM()
    {
        WanderState wandering = new WanderState(transform);
        wandering.AddTransition(Transition.SawPlayer, FSMStateID.Fleeing);
        wandering.AddTransition(Transition.InPen, FSMStateID.Standing);

        FleeingState fleeing = new FleeingState(transform);
        fleeing.AddTransition(Transition.LostPlayer, FSMStateID.Wandering);
        fleeing.AddTransition(Transition.InPen, FSMStateID.Standing);

        StandingState standing = new StandingState();

        AddFSMState(wandering);
        AddFSMState(fleeing);
        AddFSMState(standing);
    }
    
    public void InitRandomWalk()
    {
        decisionMade = false;
        decisionTimeLimit = Random.Range(0, 8);
        AvoidingShep = false;
        animator.SetBool("moving", true);
        moving = true; //Set the sheep moving
    }

    public void TurnOffWalkAnimation()
    {
        animator.SetBool("moving", false);
    }
}
