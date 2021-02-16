using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class ChatMessageData
    {
        public int id;
        public string text;
        public string date;
        public int fromUserId;

        private ChatMessageData()
        {
        }

        public static ChatMessageData Create(string text, UserData sender)
        {
            var message = new ChatMessageData
            {
                text = text,
                fromUserId = sender.id,
                date =  DateTime.Now.ToString("HH:mm:ss")
            };
            message.id = message.GetHashCode();
            return message;
        }
    }
}