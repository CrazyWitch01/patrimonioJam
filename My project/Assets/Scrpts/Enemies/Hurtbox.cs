using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public EnemyAI EnemyAI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Aguja"))
        {
            EnemyAI.TakeDamage(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
