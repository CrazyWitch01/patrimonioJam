using UnityEngine;
using DialogueEditor;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private NPCConversation Conversation;
    public GameObject DesactivarEsteObjetodspsQueSeActive;
    public GameObject playerObj;

    private Animator playerAnimator;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ConversationStarter"))
        {
            //if (Input.GetKeyDown(KeyCode.Mouse0))
            // {
            playerAnimator = playerObj.GetComponent<Animator>();
            ConversationManager.Instance.StartConversation(Conversation);
            playerAnimator.SetBool("IsRunning", false);
            gameObject.SetActive(false);
            //playerAnimator.SetTrigger("IdleTrigger");            //si esto no Funciona me meto un pepazo

            //}
        }
    }
}
