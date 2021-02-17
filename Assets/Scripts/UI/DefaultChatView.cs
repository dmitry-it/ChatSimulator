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

        private MessageViewsPool _ownerPool;
        private MessageViewsPool _usualPool;

        private new void Awake()
        {
            _ownerPool = scrollViewContent.gameObject.AddComponent<MessageViewsPool>();
            _ownerPool.Create(ownerMessagePrefab, 50);
            _usualPool = scrollViewContent.gameObject.AddComponent<MessageViewsPool>();
            _usualPool.Create(usualMessagePrefab, 50);
            base.Awake();
        }

        private void AddMessage<T>(T messageData, MessageViewsPool pool) where T : ChatMessage
        {
            var messageView = pool.GetChatView();
            messageView.FillWithInfo(messageData);
            messageView.onRemoveButtonClickEvent.AddListener(() => { DeleteMessageCall.Invoke(messageData.Id); });
            messageView.ShowWithAnimation();
            MessageViews.Add(messageView);
            CheckMessagesBlock(MessageViews.IndexOf(messageView));
            messageView.transform.SetAsLastSibling();
        }

        protected override void RemoveMessage(int messageId)
        {
            var index = MessageViews.FindIndex(x => x.Id == messageId);
            if (index < 0) return;
            var message = MessageViews[index];
            message.HideWithAnimation(() =>
            {
                message.gameObject.GetComponent<MessageViewsPool.PolledObject>()?
                    .ReturnToPool();
                MessageViews.Remove(message);
                CheckMessagesBlock(index);
            });
        }

        public override void OnReceiveMessage(ChatMessage message)
        {
            switch (message)
            {
                case OwnerMessage ownerMessage:
                    AddMessage(ownerMessage, _ownerPool);
                    break;
                case OtherUserMessage otherUserMessage:
                    AddMessage(otherUserMessage, _usualPool);
                    break;
            }
        }

        private void CheckMessagesBlock(int index)
        {
            if (index < 1) return;
            var previous = MessageViews[index - 1];
            if (previous == null) return;
            if (previous.FromUserId == MessageViews[index].FromUserId)
                previous.HideAvatar();
            else
            {
                previous.ShowAvatar();
            }
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
            foreach (var messageView in MessageViews.Where(x => x.IsOwnersMessage)) messageView.ShowRemoveButton();
        }

        public void OnFinishEditButtonClick()
        {
            inputPanel.SetActive(true);
            finishEditButton.SetActive(false);
            foreach (var messageView in MessageViews.Where(x => x.IsOwnersMessage)) messageView.HideRemoveButton();
        }
    }
}