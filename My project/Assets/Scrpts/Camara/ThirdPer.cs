using UnityEngine;
using Unity.Cinemachine;

public class ThirdPer : MonoBehaviour
{
    public bool GameActive = true;
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotSpeed;

    //Cosas a Desactivar el puntero este activo xd

    public CinemachineCamera Camara;
    private CinemachineOrbitalFollow OrbFollow;
    private Movimiento Movimiento;
    private Dash Dash;
    private Animator playerAnimator;
    private Quaternion playerSavedRotation;

    private bool IdlePlay = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Movimiento = player.GetComponent<Movimiento>();
        Dash = player.GetComponent<Dash>();
        OrbFollow = Camara.GetComponent<CinemachineOrbitalFollow>();
        playerAnimator = playerObj.GetComponent<Animator>();

    }
    void Update()
    {
        if (!GameActive)
        {
            return;
        }

        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotSpeed);
        }

        if (!GameActive)
        {
            Cursor.visible=false;
            Cursor.lockState=CursorLockMode.Locked;
            Movimiento.enabled = false;
            Dash.enabled = false;
            OrbFollow.enabled = false;
            return;
        }

        GameObject CURSORUNLOCKER = GameObject.FindGameObjectWithTag("CURSORUNLOCKER");

        if (CURSORUNLOCKER !=null && CURSORUNLOCKER.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Movimiento.enabled = false;
            Dash.enabled = false;
            OrbFollow.enabled = false;
            playerObj.rotation = playerSavedRotation;

        }

        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Movimiento.enabled = true;
            Dash.enabled = true;
            OrbFollow.enabled = true;
            playerSavedRotation = playerObj.rotation;
            

        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (playerObj.CompareTag("ConversationTrigger"))
        {
            playerAnimator.Play("idle");
            playerAnimator.SetBool("isRunning", false);
            playerAnimator.SetTrigger("IdleTrigger");            //si esto no Funciona me meto un pepazo

        }
    }

}
