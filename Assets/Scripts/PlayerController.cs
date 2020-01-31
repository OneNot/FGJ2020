using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //todo: smoothing?
    public float MoveSpeed;
    public bool CanRun;
    public bool GravityOn;

    private Transform upperBody, lowerBody;
    private SpriteRenderer lowerBodySR;
    private CharacterController cc;

    private Animator lb_animator;
    private float animationBaseSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        upperBody = transform.Find("UpperBody");
        lowerBody = transform.Find("LowerBody");
        lb_animator = lowerBody.GetComponentInChildren<Animator>();
        lowerBodySR = lb_animator.transform.GetComponent<SpriteRenderer>();
        cc = GetComponent<CharacterController>();
        lb_animator.SetInteger("walk", 0);
        animationBaseSpeed = MoveSpeed / 80f;
        lb_animator.speed = animationBaseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //todo: for controller we need to replace normalization
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
        bool shouldRun = CanRun && Input.GetButton("Run");

        Move(moveInput, shouldRun);
        WalkingAnimationChecks(moveInput.magnitude, shouldRun);
        LookAim(moveInput);
    }

    private void Move(Vector3 input, bool run)
    {
        cc.Move(((input * MoveSpeed * (run ? 2 : 1)) + Vector3.down * (GravityOn && !cc.isGrounded ? 9.81f : 0f)) * Time.deltaTime);
    }

    private void WalkingAnimationChecks(float movementSpeed, bool run)
    {
        float upperY = upperBody.rotation.eulerAngles.y;
        float lowerY = lowerBody.rotation.eulerAngles.y;
        float d = Mathf.DeltaAngle(upperY, lowerY);

        //float angle = Quaternion.Angle(upperBody.rotation, lowerBody.rotation);
        if(d > 135f || d < -135f)
            lowerBodySR.flipY = true;
        else
            lowerBodySR.flipY = false;

        if(run)
            movementSpeed *= 2;

        lb_animator.speed = animationBaseSpeed * movementSpeed;

        if(movementSpeed == 0f)
            lb_animator.SetInteger("walk", 0); //standing
        else if(d > 135f || d < -135f)
        {
            
            lb_animator.SetInteger("walk", 2); //walking backwards
        }
        else if(d < -45f)
        {
            
            lb_animator.SetInteger("walk", 4); //walking left
        }
        else if(d > 45f)
        {
            
            lb_animator.SetInteger("walk", 3); //walking right
        }
        else
            lb_animator.SetInteger("walk", 1); //walking forwards  3: right
    }

    private void LookAim(Vector3 input)
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
        point = new Vector3(point.x, transform.position.y, point.z);

        Debug.DrawLine(Camera.main.transform.position, point, Color.red, 1f);
        upperBody.transform.LookAt(point);
        lowerBody.transform.LookAt(lowerBody.transform.position + input);
    }
}
