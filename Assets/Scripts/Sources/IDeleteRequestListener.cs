namespace Sources
{
    public interface IDeleteRequestListener
    {
        void OnCatchDeleteMessageRequest(int messageId);
    }
}