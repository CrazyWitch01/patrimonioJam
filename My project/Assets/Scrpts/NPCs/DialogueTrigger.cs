using UnityEngine;
using DialogueEditor;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private NPCConversation Conversation;
    public GameObject DesactivarEsteObjetodspsQueSeActive;
    public GameObject playerObj;
    public GameObject PlayerMain;
    private Rigidbody rbPlayer;

    private Animator playerAnimator;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ConversationStarter"))
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
            
            gameObject.SetActive(false);
            //playerAnimator.SetTrigger("IdleTrigger");            //wasap

            //}
        }
    }
}
