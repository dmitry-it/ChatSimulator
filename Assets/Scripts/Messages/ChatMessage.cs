using Data;

namespace Messages
{
    public abstract class ChatMessage
    {
        public int Id { get; }
        public string Text { get; }
        public string Date { get; }
        public UserData FromUser { get; }

        protected ChatMessage(ChatMessageData data)
        {
            Id = data.id;
            Text = data.text;
            Date = data.date;
            FromUser = data.fromUser;
        } 
    }
}