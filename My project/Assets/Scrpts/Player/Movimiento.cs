using UnityEditor.Experimental.GraphView;
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
    }

    public void IsAttack()
    {
        GetComponentInChildren<Animator>().SetTrigger("IsAttack");
        puedeAtacar = false;
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
        }
        else
        {
            rb.linearDamping = 0;
        }

        bool isRunning = (horizontalInput != 0 || verticalInput != 0) && grounded;
        animator.SetBool("IsRunning", isRunning);

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
    }

    private void ResetJump()
    {
        canJump = true;
        animator.SetBool("IsJumping", false);
    }
}
