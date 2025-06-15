using DialogueEditor;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogueInicio : MonoBehaviour
{
   
    [SerializeField] private NPCConversation Conversation;
    public GameObject DesactivarEsteObjetodspsQueSeActive;
    public GameObject playerObj;
    public GameObject PlayerMain;
    private Rigidbody rbPlayer;

    private Animator playerAnimator;
    public GameObject PantallaNegra;

    private void Awake()
    {
        PantallaNegra.SetActive(true);
    }
    void Start()
    {
        
            //if (Input.GetKeyDown(KeyCode.Mouse0))
            // {

            playerAnimator = playerObj.GetComponent<Animator>();
            playerAnimator.SetBool("IsRunning", false);
            rbPlayer = PlayerMain.GetComponent<Rigidbody>();
            //rbPlayer.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
            rbPlayer.linearVelocity = Vector3.zero;
            rbPlayer.angularVelocity = Vector3.zero;
            ConversationManager.Instance.StartConversation(Conversation);
        StartCoroutine(PantallaNegraFAdeout());
            //playerAnimator.SetTrigger("IdleTrigger");            //wasap

            //}
        
    }

    IEnumerator PantallaNegraFAdeout()
    {
        Image pantallanegra = PantallaNegra.GetComponent<Image>();
        Color colornegro = pantallanegra.color;
        float duracion = 2f;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            float t = tiempo / duracion;
            pantallanegra.color = new Color(colornegro.r,colornegro.g,colornegro.b, Mathf.Lerp(1f,0f,t));
            tiempo += Time.deltaTime;
            yield return null;
            
        }
        pantallanegra.color = new Color(colornegro.r,colornegro.b,colornegro.b,0f);
        PantallaNegra.SetActive(false);
        gameObject.SetActive(false);

    }
}

