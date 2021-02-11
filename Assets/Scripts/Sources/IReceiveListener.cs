using Messages;


namespace Sources
{
    public interface IReceiveListener
    {
        void OnReceiveMessage(ChatMessage message);
       
    }
}