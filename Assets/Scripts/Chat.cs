using Sources;
using UI;
using UnityEngine;


public class Chat : MonoBehaviour
{
    [SerializeField] private ChatView chatView;
    
    [SerializeField] private ChatSource source;
    
    private void Start()
    {
        source.AddMessageListener(chatView);
        source.AddDeleteRequestListener(chatView);
        chatView.InitListeners(
            dataForSend => { source.SendNewMessage(dataForSend); },
            dataForDelete => { source.RemoveMessage(dataForDelete); });
        source.Connect();
    }
}