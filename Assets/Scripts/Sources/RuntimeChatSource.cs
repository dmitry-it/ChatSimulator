using Data;
using UnityEngine;

namespace Sources
{
    public class RuntimeChatSource : ChatSource
    {
        protected override UserData GetSignature()
        {
            var users = usersRepository.GetAllUsers();
            var random = Random.Range(0, users.Count);
            return users[random];
        }
    }
}