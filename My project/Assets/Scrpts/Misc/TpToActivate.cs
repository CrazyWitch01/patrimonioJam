using UnityEngine;

public class TpToActivate : MonoBehaviour
{
    public GameObject Jefe;
    public GameObject playerObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == playerObj)
        {
            Jefe.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
