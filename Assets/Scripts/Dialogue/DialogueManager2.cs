using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogueManager2 : MonoBehaviour
{
    public static DialogueManager2 Instance { get; set; }

    private Queue<string> sentences;

    [SerializeField] private GameObject _panelDialogue;

    [SerializeField] private Text nameText;
    [SerializeField] private Text dialogueText;
    [SerializeField] private Image NPCImage;

    //[SerializeField] private Animator animator;

    [HideInInspector] public bool isInDialogue = false;
    private bool isSentenceDone = true;
    private string currentSentence;
    public bool isDialogueOver = false;




    private void Awake()
    {
        Instance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sentences = new Queue<string>();
    }


    public void StartDialogue (Dialogue2 dialogue)
    {
        _panelDialogue.SetActive(true);
       StartCoroutine(StartingDialogue(dialogue));
    }

    IEnumerator StartingDialogue(Dialogue2 dialogue)
    {
        isDialogueOver = false;
        sentences.Clear();
        isInDialogue = true;
        nameText.text = dialogue.name;
        Image image = NPCImage.GetComponent<Image>();
        image.sprite = dialogue.sprite;
        dialogueText.text = "";


        //animator.SetBool("isOpen", true);

        yield return new WaitForSeconds(1);

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (Time.timeScale != 0)
        {
            if (sentences.Count == 0 && isSentenceDone)
            {
                EndDialogue();
                return;
            }

            StopAllCoroutines();

            if (isSentenceDone)
            {
                string sentence = sentences.Dequeue();
                StartCoroutine(TypeSentence(sentence));
            }
            else
            {
                dialogueText.text = currentSentence;
                isSentenceDone = true;
            }
        }
        

       
    }

    IEnumerator TypeSentence(string sentence)
    {
        isSentenceDone = false;
        currentSentence = sentence;
        string displayedText = "";
        string[] words = sentence.Split(' ');

        foreach (string word in words)
        {
            // Ajoute le mot à la phrase en cours
            string tempText = displayedText + word + " ";
            dialogueText.text = tempText;  // Met à jour le texte pour vérifier sa largeur

            // Vérifie si le texte dépasse la largeur du conteneur
            if (dialogueText.preferredWidth > 876)
            {
                displayedText += "\n";  // Si oui, commence une nouvelle ligne
            }

            bool containsTag = word.Contains("<") && word.Contains(">"); // Vérifie si le mot a une balise complète

            if (containsTag)
            {
                // Si le mot contient une balise complète, on l'affiche d'un coup sans animation
                displayedText += word + " ";
                dialogueText.text = displayedText;
            }
            else
            {
                // Sinon, on affiche lettre par lettre
                foreach (char letter in word)
                {
                    displayedText += letter;
                    dialogueText.text = displayedText;
                    yield return new WaitForSeconds(0.03f);
                }
                displayedText += " "; // Ajoute l'espace après le mot
                dialogueText.text = displayedText;
            }
        }

        isSentenceDone = true;
    }

    void EndDialogue()
    {
        isDialogueOver = true;
        isInDialogue = false;
        _panelDialogue.SetActive(false);
        //animator.SetBool("isOpen", false);
    }

}
