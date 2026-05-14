using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{   
    #region NewDialogueSystem

    InputSystem_Actions inputActions;
    InputAction DialogueInteractAction;

    void OnEnable()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        DialogueInteractAction = inputActions.Player.DialogueInput;
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
    [SerializeField] float typingSpeed = 0.03f;
    bool isTyping = false;
    Coroutine typingCoroutine;
    // [SerializeField] Animator animator;
    [SerializeField] public bool dialogueActive;
    [SerializeField] GameObject dialogueUI;

    DialogueTrigger activeTrigger;

    void Update()
    {
        if(dialogueActive && DialogueInteractAction.WasPressedThisFrame())
        {
            if (isTyping)
            {
                CompleteCurrentLine();
            }
            else
            {
                DisplayNextLine();
            }
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
            textDisplay.text = string.Empty;
            nameDisplay.color = currentDialogue.lines[index].nameColor;

            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }
            typingCoroutine = StartCoroutine(TypeSentence(currentDialogue.lines[index].text));
            index++;
        } 
        else 
        {
            if(activeTrigger != null)
            {
                // Debug.Log(activeTrigger.gameObject.name + " dialogue ended.");
                activeTrigger.hasPlayed = true;
                activeTrigger = null;
            }
            index = 0;
            dialogueActive = false;
            dialogueUI.SetActive(false);
            // animator.SetBool("Dissappear", false);
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        textDisplay.text = string.Empty;

        foreach (char letter in sentence.ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    void CompleteCurrentLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        if (currentDialogue != null && index > 0 && index - 1 < currentDialogue.lines.Length)
        {
            textDisplay.text = currentDialogue.lines[index - 1].text;
        }
        isTyping = false;
    }
    #endregion NewDialogueSystem
}