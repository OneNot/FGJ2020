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
    private SpriteRenderer lowerBodySR, upperBodySR;
    private CharacterController cc;

    public static Animator lb_animator, ub_animator;
    private float animationBaseSpeed;

    public enum UpperBodyModes
    {
        Empty,
        Melee,
        Pistol,
        Shotgun
    }

    public static UpperBodyModes UpperBodyMode {get; private set;}

    private BulletSpawner bulletSpawner;

    // Start is called before the first frame update
    void Awake()
    {
        upperBody = transform.Find("UpperBody");
        bulletSpawner = upperBody.GetComponentInChildren<BulletSpawner>();
        lowerBody = transform.Find("LowerBody");
        lb_animator = lowerBody.GetComponentInChildren<Animator>();
        ub_animator = upperBody.GetComponentInChildren<Animator>();
        lowerBodySR = lb_animator.transform.GetComponent<SpriteRenderer>();
        upperBodySR = ub_animator.transform.GetComponent<SpriteRenderer>();
        cc = GetComponent<CharacterController>();
        lb_animator.SetInteger("walk", 0);
        ub_animator.SetBool("arms", false);
        ub_animator.SetBool("ActionInProgress", false);
        animationBaseSpeed = MoveSpeed / 80f;
        lb_animator.speed = ub_animator.speed = animationBaseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //todo: for controller we need to replace normalization
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
        bool shouldRun = CanRun && Input.GetButton("Run");

        WeaponSwitchChecks();
        Move(moveInput, shouldRun);
        WalkingAnimationChecks(moveInput.magnitude, shouldRun);
        LookAim(moveInput);
        WeaponFireChecks();
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

        #region feet
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
        #endregion

        #region upper body
        ub_animator.SetInteger("mode", (int)UpperBodyMode); //set animator mode

        if(UpperBodyMode == UpperBodyModes.Empty && movementSpeed > 0f)
            ub_animator.SetBool("arms", true);
        else
            ub_animator.SetBool("arms", false);
        #endregion
    }
    private void LookAim(Vector3 input)
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
        point = new Vector3(point.x, transform.position.y, point.z);

        Debug.DrawLine(Camera.main.transform.position, point, Color.red, 1f);

        
        if(UpperBodyMode == UpperBodyModes.Empty)
        {
            lowerBody.transform.LookAt(lowerBody.transform.position + input);
            upperBody.transform.LookAt(lowerBody.transform.position + input);
        }
        else
        {
            lowerBody.transform.LookAt(lowerBody.transform.position + input);
            upperBody.transform.LookAt(point);
        }
    }
    private void WeaponSwitchChecks()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(scroll > 0f)
            NextWeapon();
        else if(scroll < 0f)
            PrevWeapon();
        
        if(Input.GetButtonDown("weapon1"))
            UpperBodyMode = UpperBodyModes.Empty;
        else if(Input.GetButtonDown("weapon2"))
            UpperBodyMode = UpperBodyModes.Melee;
        else if(Input.GetButtonDown("weapon3"))
            UpperBodyMode = UpperBodyModes.Pistol;
        else if(Input.GetButtonDown("weapon4"))
            UpperBodyMode = UpperBodyModes.Shotgun;
    }
    private void WeaponFireChecks()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            switch(UpperBodyMode)
            {
                case UpperBodyModes.Melee:
                {
                    if(!ub_animator.GetBool("ActionInProgress"))
                        ub_animator.SetTrigger("melee_attack");
                    break;
                }
                case UpperBodyModes.Pistol:
                {
                    bulletSpawner.Shoot();
                    break;
                }
            }
        }
    }

    private void NextWeapon()
    {
        //if last mode
        if(UpperBodyMode == UpperBodyModes.Shotgun)
            UpperBodyMode = UpperBodyModes.Empty; //first mode
        else
            UpperBodyMode++; //next mode
    }
    private void PrevWeapon()
    {
        //if first mode
        if(UpperBodyMode == UpperBodyModes.Empty)
            UpperBodyMode = UpperBodyModes.Shotgun; //last mode
        else
            UpperBodyMode--; //prev mode
    }

}
