using UnityEngine;

// This line lets you right-click in your folder to create a new Dialogue file!
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Story/Dialogue")]
public class DialogueData : ScriptableObject {
    public DialogueLine[] lines; 
}

[System.Serializable] // This makes the data show up in the Inspector window
public struct DialogueLine {
    public string characterName; 
    [TextArea(3, 10)] public string text; 
    public Color nameColor; 
}