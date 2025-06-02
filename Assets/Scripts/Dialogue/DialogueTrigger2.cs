using UnityEngine;

public class DialogueTrigger2 : MonoBehaviour
{
    public Dialogue2 dialogue;
    public static DialogueTrigger2 Instance { get; set; }


    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (dialogue != null)
            {
                DialogueManager2.Instance.StartDialogue(dialogue);
                //Destroy(gameObject);
                //gameObject.SetActive(false);
            }
        }
        
    }

}
