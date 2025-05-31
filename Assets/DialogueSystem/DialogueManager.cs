using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Image CharImage;
    public Text NameText;
    public Text DialogueText;

    public float typingSpeed = 0.05f;
    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private string FullText = "";


    private static DialogueManager _instance;
    public static DialogueManager Instance => _instance;


    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

    }

    public void ShowDialogue(DialogueLine line)
    {
        dialoguePanel.SetActive(true);
        NameText.text = line.NameText;
        CharImage.sprite = line.CharImage;

        if (typingCoroutine != null) 
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeText(line.DialogueText));
    }

    IEnumerator TypeText(string text)
    {
        isTyping = true;
        FullText = text;
        DialogueText.text = "";

        foreach (char letter in FullText.ToCharArray())
        {
            DialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void Update()
    {
        if (dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                DialogueText.text = FullText;
                isTyping = false;
            }
            else
            {
                HideDialogue();
            }
        }
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        DialogueText.text = "";
        NameText.text = "";
        CharImage.sprite = null;
    }
}
