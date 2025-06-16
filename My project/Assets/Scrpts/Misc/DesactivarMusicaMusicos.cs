using UnityEngine;

public class DesactivarMusicaMusicos : MonoBehaviour
{
    [SerializeField] GameObject Musicos;
    public GameObject MusicaDungeons;
    public GameObject AmbientalDungeon;
    public GameObject AmbientalRios;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Musicos.SetActive(false);
            MusicaDungeons.SetActive(true);
            AmbientalDungeon.SetActive(true);
            AmbientalRios.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
