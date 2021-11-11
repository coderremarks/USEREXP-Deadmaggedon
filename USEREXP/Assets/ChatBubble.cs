using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ChatBubble : MonoBehaviour
{
    
    
    [SerializeField] private SpriteRenderer backgroundSR;
    [SerializeField] private TextMeshPro contentTMP;

    //public static void CreateTemp(Transform p_parent, Vector3 p_localPosition, string p_text, float p_duration)
    //{
    //    //Transform chatBubbleTransform Instantiate(GameAssets.i.pfChatBubble, parent); ;
    //    GameObject chatBubble = Instantiate(GameAssets.instance.chatBubble, p_parent);
    //    Transform chatBubbleTransform = chatBubble.transform;
    //    chatBubbleTransform.localPosition = p_localPosition;
    //   // Debug.Log("WOOOOOOORKS");
    //    chatBubbleTransform.GetComponent<ChatBubble>().SetUp(p_text);
    //    //Destroy(chatBubble, p_duration);
    //}
    public static ChatBubble Create(Transform p_parent, Vector3 p_localPosition, string p_text, float p_duration)
    {
        //Transform chatBubbleTransform Instantiate(GameAssets.i.pfChatBubble, parent); ;
        GameObject chatBubble = Instantiate(GameAssets.instance.chatBubble, p_parent);
        Transform chatBubbleTransform = chatBubble.transform;
        //chatBubbleTransform.localPosition = p_localPosition;
        //Debug.Log("WOOOOOOORKS");
        chatBubbleTransform.GetComponent<ChatBubble>().SetUp(chatBubbleTransform, p_text);
        Vector3 offset = new Vector3(0f, p_parent.gameObject.GetComponent<BoxCollider2D>().size.y - 0.4f + chatBubbleTransform.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().size.y / 2f);
        chatBubbleTransform.position = chatBubbleTransform.position + offset;
        return chatBubble.GetComponent<ChatBubble>();
        //Destroy(chatBubble, p_duration);
    }

    private void Awake()
    {
        //backgroundSR = transform.Find("Background").GetComponent<SpriteRenderer>();
        //contentTMP = transform.Find("Text").GetComponent<TextMeshPro>();
    }
    private void Start()
    {
       // SetUp("Hello World! Say Hello to my little friend!");
    }
    public void SetUp(Transform chatBubbleTransform, string p_text)
    {

        //bool satisfactory = false;
        //int row = 1;
        //while (satisfactory == false)
        //{
        //    contentTMP.SetText(p_text);
        //    contentTMP.ForceMeshUpdate();

        //    Vector2 textSize = contentTMP.GetRenderedValues(false);
        //    Debug.Log("TEXT SIZZZZZZE:   " + textSize);
        //    Vector2 padding = new Vector2(4f, 1f);
        //    if (backgroundSR.size.x > 20)
        //    {
        //        backgroundSR.size = textSize + padding;
        //        row++;
        //        satisfactory = false;
        //    }
        //    else
        //    {
        //        satisfactory = true;
        //    }

        //}
        contentTMP.SetText(p_text);
        contentTMP.ForceMeshUpdate();
        Vector2 textSize = contentTMP.GetRenderedValues(false);
        //Debug.Log("TEXT SIZZZZZZE:   " + textSize);
        Vector2 padding = new Vector2(2f, 1f);
        backgroundSR.size = textSize + padding;
        // Debug.Log("BG:             " + backgroundSR.size);
        //Vector3 offset = new Vector3(0f, chatBubbleTransform.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().size.y / 2f);
        //chatBubbleTransform.position = chatBubbleTransform.position + offset;

        //backgroundSR.transform.localPosition = new Vector3(backgroundSR.size.x / 2f, 0f);


    }
}
