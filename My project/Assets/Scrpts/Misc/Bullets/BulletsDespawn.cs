using UnityEngine;

public class BulletsDespawn : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float TimeToDestroy = 2f;


    void Start()
    {
        Destroy(gameObject, TimeToDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            Destroy(gameObject);
        }
        if (other.CompareTag("PLAYERHURTBOX"))
        {
            Destroy(gameObject);
        }
    }
}
