using UnityEngine;

public class Agua : MonoBehaviour
{
    [SerializeField] private Vector3 DireccionAgua = new Vector3 (1,0,0);
    [SerializeField] private float fuerzaAgua = 10f;
    [SerializeField] private LayerMask WhatIsPlayer;
    [SerializeField] Rigidbody PlayerRb;


    [SerializeField] public  Vector2 movimientoAgua = new Vector2(0.1f,0f);
    private Renderer RendererAgua;
    private Vector2 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        RendererAgua = GetComponent<Renderer>();
    }
    void OnTriggerStay(Collider other)
    {
        if (((1<<other.gameObject.layer) & WhatIsPlayer)!=0) 
        {
            PlayerRb.AddForce(DireccionAgua.normalized * fuerzaAgua, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        offset += movimientoAgua * Time.deltaTime;
        RendererAgua.material.mainTextureOffset = offset;
    }
}
