using Sources;
using UI;
using UnityEngine;


public class Chat : MonoBehaviour
{
    [SerializeField] private ChatView chatView;

    [SerializeField] private ChatSource source;

    private void Start()
    {
        SubscribeChatViewToSource();
        
        chatView.InitListeners(
            source.SendNewMessage,
            source.RemoveMessage);
        
        chatView.OnClose.AddListener(UnSubscribeChatViewFromSource);
        
        source.Connect();
    }

    private void SubscribeChatViewToSource()
    {
        source.AddMessageListener(chatView);
        source.AddDeleteRequestListener(chatView);
    }

    private void UnSubscribeChatViewFromSource()
    {
        source.RemoveMessageListener(chatView);
        source.RemoveDeleteRequestListener(chatView);
    }
}