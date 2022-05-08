using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;


public class DialogueManager : MonoBehaviour
{
    public Animator animator;

    public Queue<string> text;
    public TMP_Text nameText;
    public TMP_Text dialogueText;

    //public UnityEvent DuringDisplaySentence;

    //public OilTask OilTaskInstance;

    private void Start()
    {
        text = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //FindObjectOfType<NPCinteraction>().AfterFinishTalking = AfterFinishTalking;
        

        animator.SetBool("isActive", true); 

        nameText.text = dialogue.name;

        text.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            text.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (text.Count == 0)                                            
        {
            FindObjectOfType<NPCinteraction>().IsDialogueActive = false;                  
                                                                                        
            animator.SetBool("isActive", false);
            //DuringDisplaySentence.Invoke();
            return;
        }

        //var sentence is first in line
        string sentence = text.Dequeue();
        StopAllCoroutines();

        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";

        //FindObjectOfType<NPCinteraction>().audioSource.Play();     

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }

        //FindObjectOfType<NPCinteraction>().audioSource.Stop();
    }

}
