using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;



public class MessagePanel : NetworkBehaviour
{
    public bool isInput;
    public InputField inputFiled;
    public Text messageText;
    public Transform Content;
    public ScrollRect sr;

    public Queue<Action> queue = new Queue<Action>();

    //public string message = "";
    [Serializable]
    public struct ChatMessage : NetworkMessage
    {
        public string user;
        public string message;
    }

    void OnReceiveChatMessgae(ChatMessage msg)
    {
        queue.Enqueue(() =>
        {
            Text text = Instantiate(messageText, Content);
            text.text = string.Format("{0} : {1}",msg.user , msg.message);
            queue.Enqueue(() =>
            {
                sr.verticalNormalizedPosition = 0;
            });
        });
    }

    [Command]
    void OnSendMessage(string info)
    {
        ChatMessage msg = new ChatMessage()
        {
            user = string.Format("Player{0}", 0),
            message = info
        };
        NetworkServer.SendToAll(msg);
    }

    private void Awake()
    {
        inputFiled = GameObject.Find("Canvas/Message/InputField").GetComponentInChildren<InputField>();
        Content = GameObject.Find("Canvas/Message/Scroll View/Viewport/Content").transform;
        // 自动滑动到尾部
        sr = GameObject.Find("Canvas/Message/Scroll View").GetComponent<ScrollRect>();
        //messageText = GameObject.Find("Canvas").transform.Find("Message").GetComponentInChildren<Text>();
        inputFiled.characterLimit = 20;
        HideAll();
    }

    private void Start()
    {
        NetworkClient.RegisterHandler<ChatMessage>(OnReceiveChatMessgae);
    }

    private void Update()
    {
        // 服务端发送rpc后
        if (queue.Count > 0)
        {
            queue.Dequeue().Invoke();
        }
        if (!isLocalPlayer) { return; }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!isInput)
            {
                //Debug.Log("! isInput!!!!!!!!!!!!!");
                ShowAll();
            }
            else
            {
                //Debug.Log("Send!!!!!!!!!!!!!");
                OnSendMessage(inputFiled.text);
                HideAll();
            }
        }
    }


    private void HideAll()
    {
        isInput = false;
        inputFiled.DeactivateInputField();
        inputFiled.gameObject.SetActive(false);
    }

    private void ShowAll()
    {
        inputFiled.text = "";
        isInput = true;
        inputFiled.gameObject.SetActive(true);
        inputFiled.Select();
        inputFiled.ActivateInputField();
    }
}

