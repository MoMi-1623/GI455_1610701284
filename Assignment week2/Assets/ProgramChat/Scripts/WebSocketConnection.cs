using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.UI;

namespace ProgramChat
{
    public class WebSocketConnection : MonoBehaviour
    {
        private WebSocket webSocket;

        public GameObject loginPanel;
        public Text localInput;
        public Text portInput;

        public GameObject mainPanel;
        public Text inputText;
        public Text textPanel;

        // Start is called before the first frame update
        void Start()
        {       
            //webSocket.Send("I'm comming here");
        }

        // Update is called once per frame
        void Update()
        {
            //if(Input.GetKeyDown(KeyCode.Space))
            //{
            //    if(webSocket.ReadyState == WebSocketState.Open)
            //    {
            //        webSocket.Send("Random number : " + Random.Range(0, 999999));
            //    }
            //}
        }
        private void OnDestroy()
        {
            if(webSocket != null)
            {
                webSocket.Close();
            }
        }    

        private void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {           
                textPanel.text += messageEventArgs.Data + "\n";    
        }
        public void SendInputMessage()
        {
            if (webSocket.ReadyState == WebSocketState.Open)
            {
                webSocket.Send(inputText.text);
            }

        }
        public void ConnectToChat()
        {
            if (localInput.text == "127.0.0.1" && portInput.text == "1623")
            {
                webSocket = new WebSocket("ws://127.0.0.1:1623/");
                webSocket.Connect();
                webSocket.OnMessage += OnMessage;
                loginPanel.SetActive(false);
                mainPanel.SetActive(true);
            }
        }
    }
}
