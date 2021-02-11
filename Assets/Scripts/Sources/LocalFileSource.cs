using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sources
{
    public class LocalFileSource : ChatSource
    {
        private string _filePath;

        protected override UserData GetSignature()
        {
            return CurrentUser;
        }

        private new void Awake()
        {
            base.Awake();
            _filePath = Application.persistentDataPath + "/chat_log.txt";
        }

        public override void Connect(Action<bool> result)
        {
            CurrentUser = usersRepository.GetChatOwner();
            StartCoroutine(LoadLocalHistory(() => { result.Invoke(true); }));
        }


        private IEnumerator LoadLocalHistory(Action isDone)
        {
            if (File.Exists(_filePath) == false) File.Create(_filePath).Close();

            var messages = File.ReadAllLines(_filePath)
                .Where(x => string.IsNullOrWhiteSpace(x) == false)
                .Select(JsonUtility.FromJson<ChatMessageData>).ToList();

            if (messages.Count < 1)
            {
                SendWelcome();
            }
            else
            {
                foreach (var message in messages)
                {
                    Messages.Add(message);
                    yield return null;
                }
            }

            isDone?.Invoke();
        }

        protected override void HandleCollectionChange(object sender, NotifyCollectionChangedEventArgs args)
        {
            base.HandleCollectionChange(sender, args);
            SaveToLocalHistory();
        }

        private void SaveToLocalHistory()
        {
            var lines = Messages.Select(JsonUtility.ToJson);
            File.WriteAllLines(_filePath, lines);
        }

        
        
    }
}