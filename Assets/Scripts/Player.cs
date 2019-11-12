using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    float speed = 5;
    [SerializeField]
    float turnSpeed = 5;
    Animator animator;
    float padding = 0.2f;
    float animationTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.gameObject.GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float haxis = Input.GetAxis("Horizontal");
        float vaxis = Input.GetAxis("Vertical");
        if (vaxis != 0)
        {
            animator.SetBool("Walking",true);
            transform.position += speed * transform.forward * vaxis * Time.deltaTime;
            animationTime = 0;
        }
        else if (animationTime > padding)
        {
            animator.SetBool("Walking",false);
        }
        else
        {
            animationTime += Time.deltaTime;
        }
        if (haxis != 0)
        {
            transform.rotation = Quaternion.Euler(0,transform.eulerAngles.y + (turnSpeed * haxis * Time.deltaTime),0);
        }
    }
}
