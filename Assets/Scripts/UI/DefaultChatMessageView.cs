using System;
using DG.Tweening;
using Messages;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    public class DefaultChatMessageView : ChatMessageView
    {
        [SerializeField] private GameObject deleteButtonPanel;
        [SerializeField] private GameObject avatarCorner;
       
        private CanvasGroup _canvasGroup;

        protected new void Awake()
        {
            Assert.IsNotNull(deleteButtonPanel);
            _canvasGroup = gameObject.AddComponent<CanvasGroup>() ?? gameObject.GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0f;
            deleteButtonPanel.transform.DOScale(Vector3.zero, 0f);
            base.Awake();
        }

        protected override Sprite GetAvatarSprite(string spriteName)
        {
            return Resources.Load<Sprite>("Avatars/" + spriteName);
        }

        public override void ShowRemoveButton()
        {
            deleteButtonPanel.SetActive(true);
            deleteButtonPanel.transform.DOScale(Vector3.one, 0.5f);
        }

        public override void HideRemoveButton()
        {
            deleteButtonPanel.transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
            {
                deleteButtonPanel.SetActive(false);
            });
        }

        public override void ShowAvatar()
        {
            avatarCorner.SetActive(true);
            base.ShowAvatar();
        }

        public override void HideAvatar()
        {
            avatarCorner.SetActive(false);
            base.HideAvatar();
        }

        public override void ShowWithAnimation(Action callback = null)
        {
            gameObject.SetActive(true);
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(0.3f);
            sequence.Append(_canvasGroup.DOFade(1f, 1f));
            sequence.OnComplete(() => { callback?.Invoke(); });
        }

        public override void HideWithAnimation(Action callback)
        {
            var vector2 = Vector2.zero;
            switch (MessageData)
            {
                case OtherUserMessage _:
                    vector2 = new Vector2(-1000f, transform.localPosition.y);
                    break;
                case OwnerMessage _:
                    vector2 = new Vector2(1000f, transform.localPosition.y);
                    break;
            }

            transform.DOLocalMove(vector2, 0.5f).OnComplete(callback.Invoke);
        }
    }
}