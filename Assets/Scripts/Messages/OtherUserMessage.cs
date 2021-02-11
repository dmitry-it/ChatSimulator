using Data;

namespace Messages
{
    public class OtherUserMessage : ChatMessage
    {
        public OtherUserMessage(ChatMessageData data, UserData userData) : base(data, userData)
        {
        }
    }
}