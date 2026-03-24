using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{   
    #region NewDialogueSystem
    public static DialogueManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI textDisplay;
    private DialogueData currentDialogue;
    [SerializeField] int index = 0;
    [SerializeField] Animator animator;
    [SerializeField] public bool dialogueActive;
    [SerializeField] GameObject dialogueUI;

    DialogueTrigger activeTrigger;

    void Update()
    {
        if(dialogueActive && Input.GetKeyDown(KeyCode.E)) 
        {
            DisplayNextLine();
        }
    }

    public void StartDialogue(DialogueData data, DialogueTrigger trigger) 
    {
        dialogueUI.SetActive(true);
        currentDialogue = data;
        activeTrigger = trigger;
        index = 0;
        dialogueActive = true;
        DisplayNextLine();
    }

    public void DisplayNextLine() 
    {
        // Check if we ran out of lines
        if (index < currentDialogue.lines.Length) 
        {
            nameDisplay.text = currentDialogue.lines[index].characterName;
            textDisplay.text = currentDialogue.lines[index].text;
            nameDisplay.color = currentDialogue.lines[index].nameColor;

            index++;
        } 
        else 
        {
            if(activeTrigger != null)
            {
                Debug.Log(activeTrigger.gameObject.name + " dialogue ended.");
                activeTrigger.hasPlayed = true;
                activeTrigger = null;
            }
            index = 0;
            dialogueActive = false;
            dialogueUI.SetActive(false);
            animator.SetBool("Dissappear", false);
        }
    }
    #endregion NewDialogueSystem
}