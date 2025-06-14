using UnityEngine;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class TPs : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject playerObj;
    public Transform TPObjectivo;
    public GameObject PlayerGnral;

    public Volume volumen;
    private ColorAdjustments colorAdjustments;
    public float fade = .1f;
    //public bool Telep = false;
    void Start()
    {
        volumen.profile.TryGet(out colorAdjustments);
        colorAdjustments.postExposure.overrideState = true;
        colorAdjustments.colorFilter.value = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerObj)
        {
            StartCoroutine(fadeTeleport());
            StartCoroutine(TPtoObjetivo());
        }

    }

    IEnumerator TPtoObjetivo()
    {
        PlayerGnral.transform.position = TPObjectivo.transform.position;
        yield return null;


    }
    IEnumerator fadeTeleport()
    {

        float t = 0f;
        while (t < fade)
        {
            t += Time.deltaTime;
            colorAdjustments.colorFilter.value = Color.Lerp(Color.white, Color.black, t / fade);
            yield return null;
        }
        

        yield return new WaitForSeconds(0f);
        t= 0f;
        while(t < fade)
        {
            t += Time.deltaTime;
            colorAdjustments.colorFilter.value = Color.Lerp(Color.black,Color.white ,t/fade);
            yield return null;
        }

    }
}
