using Data;

namespace Messages
{
    public abstract class ChatMessage
    {
        protected ChatMessage(ChatMessageData data, UserData userData)
        {
            Id = data.id;
            Text = data.text;
            Date = data.date;
            FromUser = userData;
        }

        public int Id { get; }
        public string Text { get; }
        public string Date { get; }
        public UserData FromUser { get; }
    }
}