using UnityEngine;

public class RestoreAnimationToIdle : MonoBehaviour
{
    public GameObject DesactivarEsteObjetodspsQueSeActive;
    public GameObject playerObj;
    private Animator playerAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAnimator = playerObj.GetComponent<Animator>();
        playerAnimator.Play("idle");


        DesactivarEsteObjetodspsQueSeActive.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
