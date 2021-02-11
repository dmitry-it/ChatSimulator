using System;
using Data;

namespace Sources
{
    public interface IChatSource
    {
        void AddMessageListener(IReceiveListener listener);
        void AddDeleteRequestListener(IDeleteRequestListener listener);
        void RemoveMessageListener(IReceiveListener listener);
        void RemoveDeleteRequestListener(IDeleteRequestListener listener);
        void Login(UserData user, Action<bool> result);
        void SendNewMessage(string message);
        void RemoveMessage(int messageId);
    }
}