using UnityEngine;
using UnityEngine.InputSystem;

public class Movimiento : MonoBehaviour
{
    [SerializeField] float velocidad = 14f;
    [SerializeField] float WalkingVelocidad = 14f;
    [SerializeField] GameObject AgujaArma;
    [SerializeField] GameObject Agujaespalda;


    bool ArmaAtaque = false;
    bool puedeAtacar = true;
    bool estabaAtacando = false;
    Animator animator;

    public Transform Orientation;
    public float PlayerHeight;
    public LayerMask Ground;
    public bool grounded;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool canJump;
    private KeyCode jumpKey = KeyCode.Space;

    public float dashSpeed;
    public MovementState state;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    //Pisadas
    public AudioClip[] pisadasTierra;
    public AudioClip[] pisadasPiedra;
    public AudioSource PisadasAudioSource;

    public float PisadasIntervalo = 0.5f;
    private float pisadasTimer = 0f;

    private Collider Coli;
    private PhysicsMaterial friccion;

    //Voz

    public AudioClip VozSalto;
    public AudioClip VozAttackDash;
    public AudioSource VocesAudioSource;

    public enum MovementState
    {
        dashing,
        walking
    }

    public bool dashing;

    private void StateHandler()
    {
        if (dashing)
        {
            state = MovementState.dashing;
            velocidad = dashSpeed;
            
        }
        else if (grounded)
        {
            state = MovementState.walking;
            velocidad = WalkingVelocidad;
        }
    }

    void Start()
    {
        canJump = true;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        animator = GetComponentInChildren<Animator>();
        Coli = GetComponentInChildren<Collider>();
        friccion = Coli.material;
    }

    public void IsAttack()
    {
        GetComponentInChildren<Animator>().SetTrigger("IsAttack");
        puedeAtacar = false;
        VocesAudioSource.PlayOneShot(VozAttackDash);
    }

    private void InputFunc()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && canJump && grounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    public void Update()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 1f;
        float rayLength = PlayerHeight * 0.5f + 0.2f;
        Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.red);
        grounded = Physics.Raycast(rayOrigin, Vector3.down, rayLength, Ground);
        InputFunc();
        SpeedControl();
        StateHandler();

        if (grounded)
        {
            rb.linearDamping = groundDrag;
            friccion.dynamicFriction = 1f;

        }
        else
        {
            rb.linearDamping = 0;
            friccion.dynamicFriction = 0f;
        }

        bool isRunning = (horizontalInput != 0 || verticalInput != 0) && grounded;
        animator.SetBool("IsRunning", isRunning);

        if (isRunning)
        {
            pisadasTimer -= Time.deltaTime;
            if (pisadasTimer < 0f)
            {
                PasosSonido();
                pisadasTimer = PisadasIntervalo;
            }
        }
        else
        {
            pisadasTimer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && puedeAtacar)
        {
            IsAttack();
        }

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        bool estaAtacando = stateInfo.IsName("golpear");

        if (estaAtacando && !estabaAtacando)
        {
            AgujaArma.SetActive(true);
            Agujaespalda.SetActive(false);
            ArmaAtaque = true;
            estabaAtacando = true;
        }
        else if (!estaAtacando && estabaAtacando)
        {
            AgujaArma.SetActive(false);
            ArmaAtaque = false;
            puedeAtacar = true;
            estabaAtacando = false;
            Agujaespalda.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        moveDirection = Orientation.forward * verticalInput + Orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * velocidad * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * velocidad * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude > velocidad)
        {
            Vector3 limitedvel = flatVel.normalized * velocidad;
            rb.linearVelocity = new Vector3(limitedvel.x, rb.linearVelocity.y, limitedvel.z);
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        animator.SetBool("IsJumping", true);
        VocesAudioSource.PlayOneShot(VozSalto);

    }

    private void ResetJump()
    {
        canJump = true;
        animator.SetBool("IsJumping", false);
    }

    public string TagPasos()
    {
        Vector3 inicio = transform.position + Vector3.up * 0.5f;
        RaycastHit Pasoshit;
        if (Physics.Raycast(inicio,Vector3.down, out Pasoshit, 2f))
        {
            return Pasoshit.collider.tag;
        }
        return "Default";

    }

    public void PasosSonido()
    {
        string tagsuelo = TagPasos();
        AudioClip[] clips = null;

        if (tagsuelo == "Tierra")
        {
            clips = pisadasTierra;
        }

        else if (tagsuelo == "Piedra")
        {
            clips = pisadasPiedra;
        }

        if (clips != null && clips.Length > 0)
        {
            int index = Random.Range(0, clips.Length);
            PisadasAudioSource.PlayOneShot(clips[index]);
        }

    }
}
