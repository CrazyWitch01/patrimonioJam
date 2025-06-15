using System.Collections;
using UnityEngine;

public class AudioAmbiente : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;
    public float TMin = 7f;
    public float TMax = 10f;
    private int NewIndice = -1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ReproducirAleatorio());
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator ReproducirAleatorio()
    {
        while (true)
        {
            while (audioSource.isPlaying)
            {
                yield return null;
            }

            int nuevoIndice;

            do
            {
                nuevoIndice = Random.Range(0, audioClips.Length);
            } while (nuevoIndice == NewIndice);

            NewIndice = nuevoIndice;

            audioSource.clip = audioClips[NewIndice];
            audioSource.Play();

            float waiting = Random.Range(TMin, TMax);
            yield return new WaitForSeconds(waiting);
        }
    }
}
