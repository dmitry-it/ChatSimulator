using System;
using System.Collections.Generic;
using Messages;
using Sources;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public abstract class ChatView : MonoBehaviour, IReceiveListener, IDeleteRequestListener
    {
        protected readonly List<ChatMessageView>
            MessageViews = new List<ChatMessageView>();

        /// <summary>
        ///     Called when user try to delete Message
        /// </summary>
        protected DeleteMessageEvent DeleteMessageCall;

        /// <summary>
        ///     Called when user try to send Message
        /// </summary>
        protected SendMessageEvent SendMessageCall;

        public readonly UnityEvent OnClose = new UnityEvent();

        protected void Awake()
        {
            DeleteMessageCall = new DeleteMessageEvent();
            SendMessageCall = new SendMessageEvent();
        }

        public void OnCatchDeleteMessageRequest(int messageId)
        {
            RemoveMessage(messageId);
        }

        public abstract void OnReceiveMessage(ChatMessage message);
        
        protected abstract void RemoveMessage(int messageId);

        public void InitListeners(UnityAction<string> sendCallback, UnityAction<int> deleteCallback)
        {
            SendMessageCall.AddListener(sendCallback);
            DeleteMessageCall.AddListener(deleteCallback);
        }

        protected void OnDestroy()
        {
            OnClose.Invoke();
        }

        protected class SendMessageEvent : UnityEvent<string>
        {
        }

        protected class DeleteMessageEvent : UnityEvent<int>
        {
        }
    }
}