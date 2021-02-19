//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using WebSocketSharp;
//using UnityEngine.UI;

//namespace ProgramChat
//{
//    public class WebSocketConnection : MonoBehaviour
//    {
//        private WebSocket webSocket;

//        public GameObject loginPanel;
//        public Text localInput;
//        public Text portInput;

//        public GameObject mainPanel;
//        public Text inputText;
//        public Text textPanel;

//        // Start is called before the first frame update
//        void Start()
//        {       
//            //webSocket.Send("I'm comming here");
//        }

//        // Update is called once per frame
//        void Update()
//        {

//        }
//        private void OnDestroy()
//        {
//            if(webSocket != null)
//            {
//                webSocket.Close();
//            }
//        }    

//        private void CreateRoom()
//        {
//            if(webSocket.ReadyState == WebSocketState.Open)
//            {

//            }
//        }

//        private void OnMessage(object sender, MessageEventArgs messageEventArgs)
//        {           
//                textPanel.text += messageEventArgs.Data + "\n";    
//        }
//        public void SendInputMessage()
//        {
//            if (webSocket.ReadyState == WebSocketState.Open)
//            {
//                webSocket.Send(inputText.text);
//            }

//        }
//        public void ConnectToChat()
//        {
//            if (localInput.text == "127.0.0.1" && portInput.text == "1623")
//            {
//                webSocket = new WebSocket("ws://127.0.0.1:1623/");
//                webSocket.Connect();
//                webSocket.OnMessage += OnMessage;
//                loginPanel.SetActive(false);
//                mainPanel.SetActive(true);
//            }
//        }
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System;
using UnityEngine.UI;

namespace ChatWebSocket
{
    public class WebSocketConnection : MonoBehaviour
    {
        public struct SocketEvent
        {
            public string eventName;
            public string data;

            public SocketEvent(string eventName, string data)
            {
                this.eventName = eventName;
                this.data = data;
            }
        }

        private WebSocket ws;

        private string tempMessageString;

        public delegate void DelegateHandle(SocketEvent result);
        public DelegateHandle OnCreateRoom;
        public DelegateHandle OnJoinRoom;
        public DelegateHandle OnLeaveRoom;

        public Text userName;
        public GameObject lobby;
        public GameObject login;
        public GameObject create;
        public Text roomNameTextInCreate;
        public GameObject room;
        public Text roomNameText;
        public GameObject join;
        public Text roomNameTextInJoin;      

        public GameObject failCreatePanel;
        public GameObject fail2CreatePanel;
        public bool know = false;
        private void Update()
        {
            UpdateNotifyMessage();
        }

        public void Connect()
        {
            string url = "ws://127.0.0.1:1623/";

            ws = new WebSocket(url);

            ws.OnMessage += OnMessage;

            lobby.SetActive(true);
            login.SetActive(false);
            ws.Connect();
        }

        public void CreateRoom(string roomName)
        {
            roomName = roomNameTextInCreate.text; 
            SocketEvent socketEvent = new SocketEvent("CreateRoom", roomName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
        }

        public void GoToCreate()
        {
            lobby.SetActive(false);
            create.SetActive(true);
        }

        public void GoToJoin()
        {
            lobby.SetActive(false);
            join.SetActive(true);
        }

        public void BackToCreateFromError()
        {
            failCreatePanel.SetActive(false);
            create.SetActive(true);
            know = true;
        }

        public void BackToJoinFromError()
        {
            fail2CreatePanel.SetActive(false);
            join.SetActive(true);
            know = true;
        }

        public void BackToCreate()
        {
            lobby.SetActive(true);
            create.SetActive(false);
            know = true;
        }

        public void BackToJoin()
        {
            lobby.SetActive(true);
            join.SetActive(false);
            know = true;
        }
        public void JoinRoom(string roomName)
        {
            roomName = roomNameTextInJoin.text;
            SocketEvent socketEvent = new SocketEvent("JoinRoom", roomName);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            ws.Send(toJsonStr);
        }

        public void LeaveRoom()
        {
            SocketEvent socketEvent = new SocketEvent("LeaveRoom", roomNameText.text);

            string toJsonStr = JsonUtility.ToJson(socketEvent);

            lobby.SetActive(true);
            room.SetActive(false);
            create.SetActive(false);
            join.SetActive(false);
            ws.Send(toJsonStr);
        }

        public void Disconnect()
        {
            if (ws != null)
                ws.Close();
        }

        public void SendMessage(string message)
        {

        }

        private void OnDestroy()
        {
            Disconnect();
        }

        private void UpdateNotifyMessage()
        {
            if (string.IsNullOrEmpty(tempMessageString) == false)
            {
                SocketEvent receiveMessageData = JsonUtility.FromJson<SocketEvent>(tempMessageString);

                if (receiveMessageData.eventName == "CreateRoom")
                {
                    if (OnCreateRoom != null)
                        OnCreateRoom(receiveMessageData);
                    know = false;
                    if (receiveMessageData.data == "fail" && know == false)
                    {
                        failCreatePanel.SetActive(true);
                    }
                    else if(receiveMessageData.data != "fail")
                    {
                        roomNameText.text = roomNameTextInCreate.text;
                        create.SetActive(false);
                        room.SetActive(true);
                    }                                            
                }
                else if (receiveMessageData.eventName == "JoinRoom")
                {
                    if (OnJoinRoom != null)
                        OnJoinRoom(receiveMessageData);
                    know = false;
                    if(receiveMessageData.data == "fail" && know == false)
                    {
                        fail2CreatePanel.SetActive(true);
                    }
                    else if (receiveMessageData.data != "fail")
                    {
                        roomNameText.text = roomNameTextInJoin.text;
                        join.SetActive(false);
                        room.SetActive(true);
                    }                   
                }
                else if (receiveMessageData.eventName == "LeaveRoom")
                {
                    if (OnLeaveRoom != null)
                        OnLeaveRoom(receiveMessageData);
                }

                tempMessageString = "";
            }
        }

        private void OnMessage(object sender, MessageEventArgs messageEventArgs)
        {
            Debug.Log(messageEventArgs.Data);

            tempMessageString = messageEventArgs.Data;
        }
    }
}
