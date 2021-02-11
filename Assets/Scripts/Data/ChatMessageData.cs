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
            var currentTime = DateTime.Now;
            var message = new ChatMessageData
            {
                text = text,
                fromUserId = sender.id,
                date = $"{currentTime.Hour} : {currentTime.Minute:D2} : {currentTime.Second:D2}"
            };
            message.id = message.GetHashCode();
            return message;
        }
    }
}