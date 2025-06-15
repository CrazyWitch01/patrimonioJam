using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class VidaPlayer : MonoBehaviour
{
    //vida
    public Transform Spawner;
    public int MaxSalud = 5;
    [SerializeField] int VidaActual;
    public Transform Player;
    public ThirdPer Camara;
    public Text SaludUIText;

    private float Inmunidad;

    public bool Regen;

    public float RegenCooldown = 5f;

    public Animator AnimacionMesh;
    

    private Movimiento Movimiento;
    private Dash Dash;
    private bool ISDEAD = false;

    private bool Inmunidad2 = false;
    public float TimeInmunidad2 = 0.2f;

    //Camara
    public Volume VolumenCamara;
    private Vignette Vignette;
    public float CamaraTransicion = 1f;
    public AudioSource VocesAudioSource;

    public AudioClip[] VozHit;
    public AudioClip VozMuerte;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VidaActual = MaxSalud;
        VidaUI();
        Movimiento = Player.GetComponent<Movimiento>();
        Dash = Player.GetComponent<Dash>();
        VolumenCamara.profile.TryGet(out Vignette vignette);
        Vignette = vignette;
        Vignette.intensity.value = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (VidaActual > 0 && Time.time - Inmunidad >= RegenCooldown && !Regen)
        {
            StartCoroutine(RegenHealth());
        }
    }
    private void VidaUI()
    {
        SaludUIText.text = VidaActual + " / " + MaxSalud;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(1);
        }
    }
    public void TakeDamage(int amount)
    {

        if (Inmunidad2 || ISDEAD)
        {
            return;
        }
        VidaActual -= amount;
        VidaActual = Mathf.Clamp(VidaActual,0,MaxSalud);
        Inmunidad = Time.time;
        Regen = false;
        if (VidaActual > 0)
        {
            AudioClip Clipdaño = VozHit[Random.Range(0, VozHit.Length)];
            VocesAudioSource.PlayOneShot(Clipdaño);
        }


        VidaUI();

        if (VidaActual <= 0)
        {
            Muerte();
            VocesAudioSource.PlayOneShot(VozMuerte);
        }
        else
        {
            StartCoroutine(ActivarInmunidad());
        }
    }
    private IEnumerator ActivarInmunidad()
    {
        Inmunidad2 = true;
        yield return new WaitForSeconds(TimeInmunidad2);
        Inmunidad2 = false;
    }
    public void Muerte()
    {

        if (ISDEAD)
        {
            return;
        }

        ISDEAD = true;

        AnimacionMesh.SetBool("IsDead", true);

        //Movimiento mov = Player.GetComponentInChildren<Movimiento>();

        //Dash dash = Player.GetComponentInChildren<Dash>();


        Movimiento.enabled = false;
        Dash.enabled = false;
        if(Camara != null)
        {
            Camara.GameActive = false;
        }

        StartCoroutine(TranscicionVignette(0f,1f));
        StartCoroutine(Respawn());
    }
    IEnumerator RegenHealth()
    {
        Regen = true;
        while (VidaActual < MaxSalud)
        {
            VidaActual++;
            VidaUI();
            yield return new WaitForSeconds(RegenCooldown);
        }
        Regen = false;
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(7f);

        Player.position = Spawner.position;
        VidaActual = MaxSalud;
        VidaUI();
        Regen = false;

        AnimacionMesh.SetBool("IsDead", false);

        Movimiento.enabled = true;
        Dash.enabled = true;

        Camara.GameActive = true;

        ISDEAD = false;
        StartCoroutine(TranscicionVignette(1f, 0f));

    }

    IEnumerator TranscicionVignette( float start, float end)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * CamaraTransicion;
            float value = Mathf.Lerp(start, end, t);
            Vignette.intensity.value = value;
            yield return null;
        }
    }
}
