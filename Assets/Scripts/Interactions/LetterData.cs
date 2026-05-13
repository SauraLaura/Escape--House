using UnityEngine;

// This line lets you right-click in your folder to create a new Letter file!
[CreateAssetMenu(fileName = "NewLetter", menuName = "Story/Letter")]
public class LetterData : ScriptableObject {
    public LetterLine[] lines; 
}

[System.Serializable] // This makes the data show up in the Inspector window
public struct LetterLine {
    [TextArea(3, 10)] public string text; 
}