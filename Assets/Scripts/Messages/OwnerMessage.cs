using Data;

namespace Messages
{
    public class OwnerMessage : ChatMessage
    {
        public OwnerMessage(ChatMessageData data, UserData userData) : base(data, userData)
        {
        }
    }
}