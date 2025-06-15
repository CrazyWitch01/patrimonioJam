using UnityEngine;

public class CuchiTransform : MonoBehaviour
{
    [SerializeField] Transform TpCuchi;
    [SerializeField] GameObject CuchiNPC;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void TransformCuchi()
    {
        CuchiNPC.transform.position = TpCuchi.transform.position;
    }
}
