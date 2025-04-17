using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Text")]
public class DialogueLine : ScriptableObject
{
    public string NameText;
    public Sprite CharImage;
    [TextArea(2, 5)]
    public string DialogueText;
}
