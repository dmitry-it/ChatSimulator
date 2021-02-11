using System.Linq;
using Messages;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DefaultChatView : ChatView
    {
        [SerializeField] protected Transform scrollViewContent;
        [SerializeField] protected GameObject ownerMessagePrefab;
        [SerializeField] protected GameObject usualMessagePrefab;
        [SerializeField] private TMP_InputField inputFiend;
        [SerializeField] private GameObject inputPanel;
        [SerializeField] private GameObject finishEditButton;


        protected override void AddMessage<T>(T messageData, GameObject prefab)
        {
            var go = Instantiate(prefab);
            var messageView = go.GetComponent<ChatMessageView>();
            messageView.FillWithInfo(messageData);
            messageView.onRemoveButtonClickEvent.AddListener(() => { DeleteMessageCall.Invoke(messageData.Id); });
            CheckMessagesBlock(messageData.FromUser.id);
            MessageViews.Add(messageView);
            go.transform.SetParent(scrollViewContent, false);
        }

        protected override void RemoveMessage(int messageId)
        {
            var message = MessageViews.Find(x => x.Id == messageId);
            MessageViews.Remove(message);
            message.DestroyWithAnimation();
        }

        public override void OnReceiveMessage(ChatMessage message)
        {
            switch (message)
            {
                case OwnerMessage ownerMessage:
                    AddMessage(ownerMessage, ownerMessagePrefab);
                    break;
                case OtherUserMessage otherUserMessage:
                    AddMessage(otherUserMessage, usualMessagePrefab);
                    break;
            }
        }

        private void CheckMessagesBlock(int userId)
        {
            var lastMessage = MessageViews.LastOrDefault();
            if (lastMessage != null && lastMessage.FromUserId == userId)
                lastMessage.HideAvatar();
        }

        public void OnSendButtonClick()
        {
            if (inputFiend.text.Length <= 0) return;
            SendMessageCall.Invoke(inputFiend.text);
            inputFiend.text = "";
        }

        public void OnDeleteMessagesButtonClick()
        {
            inputPanel.SetActive(false);
            finishEditButton.SetActive(true);
            foreach (var messageView in MessageViews.FindAll(x=>x.IsOwnersMessage)) messageView.ShowRemoveButton();
        }

        public void OnFinishEditButtonClick()
        {
            inputPanel.SetActive(true);
            finishEditButton.SetActive(false);
            foreach (var messageView in MessageViews.FindAll(x=>x.IsOwnersMessage)) messageView.HideRemoveButton();
        }
    }
}