using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueLine DialogueLine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.ShowDialogue(DialogueLine);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.HideDialogue();
        }
    }
}
