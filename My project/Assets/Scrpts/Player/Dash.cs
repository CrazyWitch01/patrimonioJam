using UnityEngine;

public class Dash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform Orientation;
    public Transform playerCam;
    Rigidbody rb;
    Movimiento pm;

    public float dashForce;
    public float dashUpwardsForce;
    public float dashDuration;

    public float dashCD;
    private float dashCDTimer;
    public AudioSource VoiceAudioSource;
    public AudioClip[] DashClip;

    private KeyCode dashKey = KeyCode.LeftShift;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent <Movimiento>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(dashKey))
        {
            Dashing();
        }

        if (dashCDTimer > 0)
        {
            dashCDTimer -= Time.deltaTime;
        }
    }

    void Dashing()
    {
        if (dashCDTimer > 0) return;
        else dashCDTimer = dashCD;

            pm.dashing = true;
        Vector3 forceToApply = Orientation.forward * dashForce + Orientation.up * dashUpwardsForce;
        delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForce), 0.025f);
        Invoke(nameof(ResetDashing), dashDuration);
        int index = Random.Range(0, DashClip.Length);
        VoiceAudioSource.PlayOneShot(DashClip[index]);


    }

    private Vector3 delayedForceToApply;
    private void DelayedDashForce()
    {
        rb.AddForce(delayedForceToApply, ForceMode.Impulse);

    }

    void ResetDashing()
    {
        pm.dashing=false;
    }
}
