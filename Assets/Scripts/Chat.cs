using Sources;
using UI;
using UnityEngine;
using UsersSystem;

public class Chat : MonoBehaviour
{ 
    
    [SerializeField] private ChatView chatView;
    [SerializeField] private UsersRepository users;
    private IChatSource _source;
   
    private void Awake()
    {
        _source = gameObject.AddComponent<LocalFileSource>();
    }

    private void Start()
    {
        _source.AddMessageListener(chatView);
        _source.AddDeleteRequestListener(chatView);
        
        chatView.InitListeners(
            dataForSend => { _source.SendNewMessage(dataForSend); },
            dataForDelete => { _source.RemoveMessage(dataForDelete); });
        
        _source.Login(users.GetChatOwner(), result => { Debug.Log("Login Result = " + result); });
    }
}