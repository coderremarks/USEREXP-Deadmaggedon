using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Conversation
{
    public TutorialPurpose conversationName;
    public List<string> messages = new List<string>();
}
[System.Serializable]
public class Speaker
{
    public GameObject speakerObject;
    public List<Conversation> conversations = new List<Conversation>();

    
 
}
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public List<Speaker> speakers = new List<Speaker>();
    public int currentMessageIndex = 0;
    public int currentSpeakerIndex = 0;
    public bool isTalking = false;
    public TutorialPurpose currentTutorialPurpose;
    public int currentConversationIndex = 0;
    public ChatBubble currentChat;
    public bool shook = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Update()
    {
        if (isTalking == true)
        {
            if (Input.anyKeyDown)
            {
                
                if (currentMessageIndex < speakers[currentSpeakerIndex].conversations[currentConversationIndex].messages.Count)
                {
                    if(currentChat != null)
                    {
                        Destroy(currentChat.gameObject);
                        
                    }
                    currentChat = ChatBubble.Create(speakers[currentSpeakerIndex].speakerObject.transform, new Vector3(0, 0f), speakers[currentSpeakerIndex].conversations[currentConversationIndex].messages[currentMessageIndex], 5f);

                    currentMessageIndex++;
                }
                else
                {
                    Destroy(currentChat.gameObject);
                    currentChat = null;
                    isTalking = false;
                    MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().dialogue.SetActive(false);
                    TutorialManager.instance.StartEndFunction(currentTutorialPurpose);

                  
                }

               
            }
            if (shook == false)
            {
                if (TutorialManager.instance.mayorGate.GetComponent<Door>().shakable == true)
                {
                    shook = true;
                    StartCoroutine(RepeatingShake());
                }

            }
        }
    }
    public IEnumerator RepeatingShake()
    {

        yield return new WaitForSeconds(3f);
        PlayerManager.instance.playerCamera.CameraShake(0.5f, 0.5f, 10f);//SHAKE TUTORIAL
        shook = false;
    }
    public void ActivateConversation(GameObject p_desiredSpeaker, TutorialPurpose p_purpose = TutorialPurpose.none)
    {
        
        currentMessageIndex = 0;

        currentSpeakerIndex = GetSpeakerIndex(p_desiredSpeaker);

        if (p_purpose != TutorialPurpose.none)
        {
            currentTutorialPurpose = p_purpose;
           // MenuManager.instance.GetCanvas(MenuType.Tutorial).GetComponent<TutorialCanvas>().Choose(p_purpose);
        }
        currentConversationIndex = GetConversationIndex(p_purpose);
        currentChat = ChatBubble.Create(speakers[currentSpeakerIndex].speakerObject.transform, new Vector3(0, 0f), speakers[currentSpeakerIndex].conversations[currentConversationIndex].messages[currentMessageIndex], 5f);
        currentMessageIndex++;
        MenuManager.instance.GetCanvas(MenuType.Transition).GetComponent<TransitionCanvas>().dialogue.SetActive(true);
        isTalking = true;


    }
    public int GetSpeakerIndex(GameObject p_desiredSpeaker)
    {
        for (int i = 0; i < speakers.Count; i++)
        {
            if (speakers[i].speakerObject == p_desiredSpeaker)
            {

                return i;
            }
        }
        return -1;
    }

    public int GetConversationIndex(TutorialPurpose p_purpose)
    {
       // Debug.Log("CONVERSATION GOTTEN : " + p_purpose);
        for (int i = 0; i < speakers[currentSpeakerIndex].conversations.Count; i++)
        {
            if (speakers[currentSpeakerIndex].conversations[i].conversationName == currentTutorialPurpose)
            {
           //     Debug.Log("CONVERSATION GOTTEN : " + speakers[currentSpeakerIndex].conversations[i].conversationName);
                return i;
            }
        }
        return -1;
    }
    public void OnDisable()
    {
        StopAllCoroutines();
    }
    // ChatBubble.Create(transform, new Vector3(0, 2), "aa");
}
