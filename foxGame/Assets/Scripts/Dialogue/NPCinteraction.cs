using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class NPCinteraction : ForDialogue
{

    //public AudioSource audioSource;

    //public AudioClip dialogueSound;

    //public UnityEvent OnStartOfConversation;

   

    


    private void Start()
    {
        //audioSource = GetComponent<AudioSource>();

        //dialogueBox.GetComponent<Animator>().SetBool("isActive", false);
        
    }

    private void Update()
    {
        if (IsInRange)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //OnStartOfConversation.Invoke();


                if (IsDialogueActive)
                {
                    FindObjectOfType<DialogueManager>().DisplayNextSentence();
                }
                else
                {
                   // dialogueBox.transform.GetComponent<Image>().sprite = dialogueBoxSprite;
                    TriggerDialogue();
                    IsDialogueActive = true;
                }
            }
        }

    }

            //audioSource.Stop();    
}
