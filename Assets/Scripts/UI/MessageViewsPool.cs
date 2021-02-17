using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace UI
{
    public class MessageViewsPool : MonoBehaviour
    {
        private readonly Stack<ChatMessageView> _instances = new Stack<ChatMessageView>();
        private GameObject _prefab;
        private int _size;

        public void Create(GameObject prefab, int size)
        {
            _prefab = prefab;
            _size = size;
            for (int i = 0; i < size; i++)
            {
                _instances.Push(CreateInstance(prefab));
            }
        }

        public ChatMessageView GetChatView()
        {
            var view = _instances.Count > 0 ? _instances.Pop() : CreateInstance(_prefab);
            return view;
        }

        private ChatMessageView CreateInstance(GameObject prefab)
        {
            var go = Instantiate(prefab, transform, false);
            var polledObject = go.AddComponent<PolledObject>();
            polledObject.LinkToPool(this);
            var view = go.GetComponent<ChatMessageView>();
            go.SetActive(false);
            return view;
        }

        public void ReturnToPool(GameObject go)
        {
            Assert.IsTrue(go.GetComponent<PolledObject>()?.Pool == this);
            go.SetActive(false);
            go.transform.SetParent(transform);
        }

        public class PolledObject : MonoBehaviour
        {
            public MessageViewsPool Pool { get; private set; }

            public void LinkToPool(MessageViewsPool pool)
            {
                Pool = pool;
            }

            public void ReturnToPool()
            {
                Pool.ReturnToPool(this.gameObject);
            }
        }
    }
}