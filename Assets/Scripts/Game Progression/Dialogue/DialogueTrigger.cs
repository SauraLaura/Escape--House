using NUnit.Framework;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour 
{
    public DialogueData dialogueToPlay;   
    public string newObjectiveText;       
    public bool hasPlayed = false;       

    [Header("Sequence Settings")]
    [SerializeField] int requiredStep; // This trigger only works if GameProgress is this number
    [SerializeField] int stepToSetAfter; // The number to set GameProgress to after this

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player") && !hasPlayed && GameProgress.currentStoryStep == requiredStep) 
        {
            GameProgress.currentStoryStep = stepToSetAfter;  
            FindFirstObjectByType<DialogueManager>().StartDialogue(dialogueToPlay, this);
            // Debug.Log("I am step " + requiredStep + ". The game is currently on step " + GameProgress.currentStoryStep);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && hasPlayed)
        {
            FindFirstObjectByType<ObjectiveUI>().UpdateObjective(newObjectiveText);   
            // Debug.Log("I am step " + requiredStep + ". The game is currently on step " + GameProgress.currentStoryStep);
        }
    }
}