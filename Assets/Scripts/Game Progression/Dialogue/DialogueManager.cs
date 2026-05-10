using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{   
    #region NewDialogueSystem

    InputSystem_Actions inputActions;
    InputAction interactAction;

    void OnEnable()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        interactAction = inputActions.Player.Interact;
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

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
    // [SerializeField] Animator animator;
    [SerializeField] public bool dialogueActive;
    [SerializeField] GameObject dialogueUI;

    DialogueTrigger activeTrigger;

    void Update()
    {
        if(dialogueActive && interactAction.WasPressedThisFrame())
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
            // animator.SetBool("Dissappear", false);
        }
    }
    #endregion NewDialogueSystem
}