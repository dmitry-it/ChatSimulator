using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Data;
using Messages;
using UnityEngine;
using UnityEngine.Events;

namespace Sources
{
    public class LocalFileSource : MonoBehaviour, IChatSource
    {
        private UserData _currentUser;
        private string _filePath;

        private ObservableCollection<ChatMessageData> _messages;
        private ChatMessageEvent ReceiveMessageEvent { get; set; }
        private RemoveMessageEvent DeleteMessageEvent { get; set; }

        private void Awake()
        {
            _filePath = Application.persistentDataPath + "/chat_log.txt";
            ReceiveMessageEvent = new ChatMessageEvent();
            DeleteMessageEvent = new RemoveMessageEvent();
            _messages = new ObservableCollection<ChatMessageData>();
            _messages.CollectionChanged += HandleCollectionChange;
        }

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


        public void Login(UserData user, Action<bool> result)
        {
            _currentUser = user;
            StartCoroutine(LoadLocalHistory(() => { result.Invoke(true); }));
        }

        public void SendNewMessage(string message)
        {
            var newChatMessage = ChatMessageData.Create(message, _currentUser);
            _messages.Add(newChatMessage);
        }

        public void RemoveMessage(int messageId)
        {
            var message = _messages.FirstOrDefault(x => x.id == messageId);
            _messages.Remove(message);
        }

        private IEnumerator LoadLocalHistory(Action isDone)
        {
            if (File.Exists(_filePath) == false) File.Create(_filePath).Close();

            var messages = File.ReadAllLines(_filePath)
                .Where(x => string.IsNullOrWhiteSpace(x) == false)
                .Select(JsonUtility.FromJson<ChatMessageData>).ToList();
            foreach (var message in messages)
            {
                _messages.Add(message);
                yield return null;
            }

            isDone?.Invoke();
        }

        private void HandleMessage(ChatMessageData data)
        {
            if (data.fromUser.id == _currentUser.id)
                ReceiveMessageEvent.Invoke(new OwnerMessage(data));
            else
                ReceiveMessageEvent.Invoke(new OtherUserMessage(data));
        }

        private void SaveToLocalHistory()
        {
            var lines = _messages.Select(JsonUtility.ToJson);
            File.WriteAllLines(_filePath, lines);
        }

        private void HandleCollectionChange(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems != null)
                foreach (var newItem in args.NewItems)
                    if (newItem is ChatMessageData message)
                        HandleMessage(message);


            if (args.OldItems != null)
                foreach (var deletedItem in args.OldItems)
                    if (deletedItem is ChatMessageData message)
                        DeleteMessageEvent.Invoke(message.id);


            SaveToLocalHistory();
        }

        private class RemoveMessageEvent : UnityEvent<int>
        {
        }
    }
}