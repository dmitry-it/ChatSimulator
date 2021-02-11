using System.Collections.Generic;
using Messages;
using Sources;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public abstract class ChatView : MonoBehaviour, IReceiveListener, IDeleteRequestListener
    {
        
        /// <summary>
        /// Called when user try to send Message
        /// </summary>
        protected SendMessageEvent SendMessageCall;

        /// <summary>
        /// Called when user try to delete Message
        /// </summary>
        protected DeleteMessageEvent DeleteMessageCall;
        protected readonly List<ChatMessageView>
            MessageViews = new List<ChatMessageView>();

        protected abstract void AddMessage<T>(T messageData, GameObject prefab) where T : ChatMessage;

        protected abstract void RemoveMessage(int messageId);

        public void InitListeners(UnityAction<string> sendCallback, UnityAction<int> deleteCallback)
        {
            SendMessageCall.AddListener(sendCallback);
            DeleteMessageCall.AddListener(deleteCallback);
        }


        protected void Awake()
        {
            DeleteMessageCall = new DeleteMessageEvent();
            SendMessageCall = new SendMessageEvent();
        }

        public abstract void OnReceiveMessage(ChatMessage message);

        public void OnCatchDeleteMessageRequest(int messageId)
        {
            RemoveMessage(messageId);
        }

        protected class SendMessageEvent : UnityEvent<string>
        {
        }

        protected class DeleteMessageEvent : UnityEvent<int>
        {
        }
    }
}