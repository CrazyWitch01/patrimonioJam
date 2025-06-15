using UnityEngine;

public class DesactivarMusicaMusicos : MonoBehaviour
{
    [SerializeField] GameObject Musicos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Musicos.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
