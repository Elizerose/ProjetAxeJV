using UnityEngine;

[System.Serializable]
public class Dialogue2
{
    public string name;

    public Sprite sprite;

    [TextArea(3, 10)]
    public string[] sentences;
}
