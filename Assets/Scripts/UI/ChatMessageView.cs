using System;
using Messages;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public abstract class ChatMessageView : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI nameText;
        [SerializeField] protected TextMeshProUGUI bodyText;
        [SerializeField] protected TextMeshProUGUI dateText;
        [SerializeField] protected Image avatarImage;
        [SerializeField] protected GameObject deleteButton;

        public UnityEvent onRemoveButtonClickEvent = new UnityEvent();

        protected ChatMessage _messageData;

        public int Id => _messageData.Id;
        public int FromUserId => _messageData.FromUser.id;

        protected void Awake()
        {
            Assert.IsNotNull(nameText);
            Assert.IsNotNull(bodyText);
            Assert.IsNotNull(dateText);
            Assert.IsNotNull(deleteButton);
        }

        protected void Start()
        {
            var button = deleteButton.AddComponent<Button>();
            button.onClick.AddListener(() => { onRemoveButtonClickEvent.Invoke(); });
            PlayAppearAnimation();
        }

        protected virtual void PlayAppearAnimation(Action callback = null)
        {
            callback?.Invoke();
        }

        protected virtual void PlayDestroyAnimation(Action callback)
        {
            callback.Invoke();
        }

        public void DestroyWithAnimation()
        {
            PlayDestroyAnimation(() => { Destroy(gameObject); });
        }

        public virtual void FillWithInfo(ChatMessage messageData)
        {
            _messageData = messageData;
            nameText.text = messageData.FromUser.name;
            bodyText.text = messageData.Text;
            dateText.text = messageData.Date;
            if (!(avatarImage is null)) avatarImage.sprite = GetAvatarSprite(messageData.FromUser.avatarName);
        }

        protected abstract Sprite GetAvatarSprite(string spriteName);

        public abstract void ShowRemoveButton();
        public abstract void HideRemoveButton();

        public virtual void ShowAvatar()
        {
            avatarImage.gameObject.SetActive(true);
        }

        public virtual void HideAvatar()
        {
            avatarImage.gameObject.SetActive(false);
        }
    }
}