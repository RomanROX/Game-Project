using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum DialogueState
{
    IntroText,
    QuestText,
    WaitingText,
    FinishedText
}


public class ForDialogue : MonoBehaviour
{
    //public Sprite dialogueBoxSprite;
    public GameObject dialogueBox;
    
    public bool IsDialogueActive { get; set; }
    public bool IsInRange { get; private set; }

    public List<Dialogue> dialogueAtDifferentStates = new List<Dialogue>();
    public DialogueState neededDialogue = DialogueState.IntroText;

    [SerializeField] Vector2 playerCheckRange;


    
    public void TriggerDialogue()
    {
        //Debug.Log(dialogs[0] + " --- " + dialogs[1] + " --- " + dialogs[2] + " --- " + dialogs[3]);

        //if (gameObject.CompareTag("Baker"))
        //{
        //    FindObjectOfType<DialogueManager>().StartDialogue(dialogs[(int)GameManager.instance.bakerNPCState]/*dialogue*/);
        //}
        //else if (gameObject.CompareTag("Grinder"))
        //{
        //    FindObjectOfType<DialogueManager>().StartDialogue(dialogs[(int)GameManager.instance.wheatNPCState]/*dialogue*/);
        //}
        //else if (gameObject.CompareTag("NPCBundle"))
        //{
        //    MultipleDialogueManager temp = GameObject.Find("MultipleDialogueManager").GetComponent<MultipleDialogueManager>();
        //    temp.StartDialogue(dialogs[(int)GameManager.instance.NPCQuestState[gameObject.name]]);
        //    //FindObjectOfType<MultipleDialogueManager>().StartDialogue(dialogs[(int)FindObjectOfType<MultipleDialogueManager>().NPCQuestState[gameObject.name]]);
        //}
        //else if (gameObject.CompareTag("Oil"))
        //{
        //    FindObjectOfType<DialogueManager>().StartDialogue(dialogs[(int)GameManager.instance.oilNPCState]/*dialogue*/);
        //}
        //else
        //{
            FindObjectOfType<DialogueManager>().StartDialogue(dialogueAtDifferentStates[(int)neededDialogue]/*dialogue*/);
        //}
        
    }

    public bool CheckForObject(LayerMask layerMask)
    {
        bool result = Physics2D.OverlapBox(transform.position, playerCheckRange, 0, layerMask);
        if (!result)
        {
            IsInRange = false;

            FindObjectOfType<DialogueManager>().animator.SetBool("isActive", false);

            IsDialogueActive = false;
        }
        return Physics2D.OverlapBox(transform.position, playerCheckRange, 0, layerMask);
    }
}
