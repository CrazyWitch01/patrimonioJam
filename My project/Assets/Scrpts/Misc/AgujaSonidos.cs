using UnityEngine;
using System.Collections;

public class AgujaSonidos : MonoBehaviour
{
    [SerializeField] AudioSource AgujaSource;
    [SerializeField] AudioClip[] contactoEnemigo;
    [SerializeField] AudioClip[] NoContacto;
    public LayerMask WhatIsEnemy;
    [SerializeField] float Delay = 0.3f;

    [SerializeField] bool SonidoPlayed;
    [SerializeField] bool Collided;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        SonidoPlayed = false;
        Collided = false;

        StartCoroutine(CollisionDelay());
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (SonidoPlayed)
        {
            return;
        }
        if (((1 << other.gameObject.layer) & WhatIsEnemy.value) != 0)
        {
            SonidoPlayed = true;
            Collided = true;
            PlaySonidoAguja(contactoEnemigo);
            
        }
    }

    IEnumerator CollisionDelay() 
    {
        yield return new WaitForSeconds(Delay);

        if (!Collided && !SonidoPlayed)
        {
            SonidoPlayed = true;
            PlaySonidoAguja(NoContacto);
        }
    }

    private void PlaySonidoAguja(AudioClip[] clips)
    {
        if (clips.Length > 0)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            AgujaSource.PlayOneShot(clip);
        }
    }
}
