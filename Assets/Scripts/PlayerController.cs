using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //todo: smoothing?
    public float MaxHealth;
    public float MoveSpeed;
    public bool CanRun;
    public bool GravityOn;

    private float CurrentHealth;

    private Transform upperBody, lowerBody;
    private SpriteRenderer lowerBodySR, upperBodySR;
    private CharacterController cc;

    private AudioSource audioSource;
    public float MeleeDamage;
    public AudioClip MeleeSound;
    public float MeleeReachForwards, MeleeReachWidth;

    public float LowerSFXSoundLevel;

    public AudioClip[] DamageTakeSounds;
    public AudioClip[] DeathSounds;

    public static Animator lb_animator, ub_animator;
    private float animationBaseSpeed;

    private bool alive;

    public Slider HPSlider;
    public Text WeaponText;
    public GameObject InteractPrompt;

    public InteractableObj interactable {get; set;}

    public enum UpperBodyModes
    {
        Empty,
        Melee,
        Pistol,
        Shotgun,
        Rifle
    }

    public UpperBodyModes UpperBodyMode {get; private set;}

    private BulletSpawner bulletSpawner;

    // Start is called before the first frame update
    void Awake()
    {
        InteractPrompt.SetActive(false);
        alive = true;
        audioSource = GetComponent<AudioSource>();
        CurrentHealth = MaxHealth;
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
        ChangeUpperBodyMode(UpperBodyMode);
    }

    // Update is called once per frame
    void Update()
    {
        if(alive)
        {
            //todo: for controller we need to replace normalization
            Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized;
            bool shouldRun = CanRun && Input.GetButton("Run");

            if(interactable != null && Input.GetButtonDown("Interact"))
            {
                interactable.Interact();
            }

            WeaponSwitchChecks();
            Move(moveInput, shouldRun);
            AnimationChecks(moveInput.magnitude, shouldRun);
            LookAim(moveInput);
            WeaponFireChecks();
            HealthCheck();
        }
    }

    private void Move(Vector3 input, bool run)
    {
        cc.Move(((input * MoveSpeed * (run ? 2 : 1)) + Vector3.down * (GravityOn && !cc.isGrounded ? 9.81f : 0f)) * Time.deltaTime);
    }
    private void AnimationChecks(float movementSpeed, bool run)
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

        //Debug.DrawLine(Camera.main.transform.position, point, Color.red, 1f);

        
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
            ChangeUpperBodyMode(UpperBodyModes.Empty);
        else if(Input.GetButtonDown("weapon2"))
            ChangeUpperBodyMode(UpperBodyModes.Melee);
        else if(Input.GetButtonDown("weapon3"))
            ChangeUpperBodyMode(UpperBodyModes.Pistol);
        else if(Input.GetButtonDown("weapon4"))
            ChangeUpperBodyMode(UpperBodyModes.Shotgun);
        else if(Input.GetButtonDown("weapon5"))
            ChangeUpperBodyMode(UpperBodyModes.Rifle);
    }
    private void WeaponFireChecks()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(UpperBodyMode == UpperBodyModes.Melee && !ub_animator.GetBool("ActionInProgress"))
                ub_animator.SetTrigger("melee_attack");
            else if(UpperBodyMode == UpperBodyModes.Rifle)
                bulletSpawner.StartRepeatingFire();
            else
                bulletSpawner.Shoot(UpperBodyMode);
        }
        if(Input.GetButtonUp("Fire1"))
            bulletSpawner.EndRepeatingFire();

    }

    public void TakeDamage(float dmg)
    {
        if(alive)
        {
            CurrentHealth -= dmg;
            audioSource.PlayOneShot(DamageTakeSounds[Random.Range(0, DamageTakeSounds.Length-1)], LowerSFXSoundLevel);
            HealthCheck();
            print("player took " + dmg + " dmg");
        }
    }

    private void HealthCheck()
    {
        HPSlider.value = CurrentHealth / MaxHealth;
        if(alive && CurrentHealth <= 0f)
            Die();
    }
    private void Die()
    {
        print("Death");
        audioSource.PlayOneShot(DeathSounds[Random.Range(0, DeathSounds.Length-1)]);
        alive = false;
    }

    private void NextWeapon(  )
    {
        //if last mode
        if(UpperBodyMode == UpperBodyModes.Rifle)
            ChangeUpperBodyMode(UpperBodyModes.Empty); //first mode
        else
            ChangeUpperBodyMode(UpperBodyMode + 1); //next mode
    }
    private void PrevWeapon()
    {
        //if first mode
        if(UpperBodyMode == UpperBodyModes.Empty)
            ChangeUpperBodyMode(UpperBodyModes.Rifle); //last mode
        else
            ChangeUpperBodyMode(UpperBodyMode - 1); //prev mode
    }

    private void ChangeUpperBodyMode(UpperBodyModes mode)
    {
        if(mode != UpperBodyModes.Rifle)
            bulletSpawner.EndRepeatingFire();

        UpperBodyMode = mode;
        if(mode == UpperBodyModes.Pistol)
        {
            bulletSpawner.transform.localPosition = new Vector3(0.2f, 0f, 11.5f);
            WeaponText.text = "Pistol";
        }
        else if(mode == UpperBodyModes.Shotgun)
        {
            bulletSpawner.transform.localPosition = new Vector3(1.7f, 0f, 11f);
            WeaponText.text = "Shotgun";
        }
        else if(mode == UpperBodyModes.Rifle)
        {
            bulletSpawner.transform.localPosition = new Vector3(1.6f, 0f, 12.1f);
            WeaponText.text = "Rifle";
        }
        else if(mode == UpperBodyModes.Melee)
        {
            WeaponText.text = "Crowbar";
        }
        else
        {
            WeaponText.text = "Empty";
        }
    }

    public void PlayMeleeSound()
    {
        audioSource.PlayOneShot(MeleeSound, LowerSFXSoundLevel);
    }

    public void TryToDoMeleeDamage()
    {

        Debug.DrawRay(upperBody.position, upperBody.forward * MeleeReachForwards, Color.yellow, 200f);
        if(Physics.SphereCast(upperBody.position, MeleeReachWidth, upperBody.forward, out RaycastHit hit, MeleeReachForwards, LayerMask.GetMask("Enemy")))
        {
            EnemyAIRobot robot = hit.transform.gameObject.GetComponentInParent<EnemyAIRobot>();
            EnemyAI NotRobot = hit.transform.gameObject.GetComponentInParent<EnemyAI>();
            if(robot != null)
                robot.TakeDamage(MeleeDamage);
            else if(NotRobot != null)
                NotRobot.TakeDamage(MeleeDamage);
        }
    }
}
