using System;

namespace Data
{
    [Serializable]
    public class ChatMessageData
    {
        public int id;
        public string text;
        public string date;
        public UserData fromUser = new UserData();

        private ChatMessageData()
        {
        }

        public static ChatMessageData Create(string text, UserData sender)
        {
            var currentTime = DateTime.Now;
            var message = new ChatMessageData
            {
                text = text,
                fromUser = sender,
                date = $"{currentTime.Hour} : {currentTime.Minute:D2} : {currentTime.Second}"
            };
            message.id = message.GetHashCode();
            return message;
        }
    }
}