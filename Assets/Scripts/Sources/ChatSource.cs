using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Data;
using Messages;
using UnityEngine;
using UnityEngine.Events;
using UsersSystem;

namespace Sources
{
    public abstract class ChatSource : MonoBehaviour
    {
        [SerializeField]
        protected UsersRepository usersRepository;
        
        protected UserData CurrentUser;
        protected ObservableCollection<ChatMessageData> Messages;
        protected ChatMessageEvent ReceiveMessageEvent;
        protected RemoveMessageEvent DeleteMessageEvent;
        
        
        public void AddMessageListener(IReceiveListener listener)
        {
            ReceiveMessageEvent.AddListener(listener.OnReceiveMessage);
        }

        public void AddDeleteRequestListener(IDeleteRequestListener listener)
        {
            DeleteMessageEvent.AddListener(listener.OnCatchDeleteMessageRequest);
        }

        public void RemoveMessageListener(IReceiveListener listener)
        {
            ReceiveMessageEvent.RemoveListener(listener.OnReceiveMessage);
        }

        public void RemoveDeleteRequestListener(IDeleteRequestListener listener)
        {
            DeleteMessageEvent.RemoveListener(listener.OnCatchDeleteMessageRequest);
        }

        public void ConnectUsersRepository(UsersRepository repository)
        {
            usersRepository = repository;
        }
        
        public virtual void Connect(Action<bool> result = null)
        {
            CurrentUser = usersRepository.GetChatOwner();
            SendWelcome();
            result?.Invoke(true);
        }

        public void SendNewMessage(string message)
        {
            var newChatMessage = ChatMessageData.Create(message, GetSignature());
            Messages.Add(newChatMessage);
        }

        public void RemoveMessage(int messageId)
        {
            var message = Messages.FirstOrDefault(x => x.id == messageId);
            Messages.Remove(message);
        }

        protected abstract UserData GetSignature();
       
        protected virtual void HandleCollectionChange(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems != null)
                foreach (var newItem in args.NewItems)
                    if (newItem is ChatMessageData message)
                        HandleMessage(message);


            if (args.OldItems != null)
                foreach (var deletedItem in args.OldItems)
                    if (deletedItem is ChatMessageData message)
                        DeleteMessageEvent.Invoke(message.id);
            
        }
        private void HandleMessage(ChatMessageData data)
        {
            var user = usersRepository.GetUserWithId(data.fromUserId);
            if (data.fromUserId == CurrentUser.id)
                ReceiveMessageEvent.Invoke(new OwnerMessage(data, user));
            else
                ReceiveMessageEvent.Invoke(new OtherUserMessage(data, user));
        }

        protected void SendWelcome()
        {
            var sender = usersRepository
                .GetAllUsers()
                .Find(x => x.id != usersRepository.GetChatOwner().id);
            var message = ChatMessageData.Create("Welcome to ChatSimulator!", sender);
            Messages.Add(message);
        }
        
        protected void Awake()
        {
            ReceiveMessageEvent = new ChatMessageEvent();
            DeleteMessageEvent = new RemoveMessageEvent();
            Messages = new ObservableCollection<ChatMessageData>();
            Messages.CollectionChanged += HandleCollectionChange;
        }

        
        protected class RemoveMessageEvent : UnityEvent<int>
        {
        }
    }
}