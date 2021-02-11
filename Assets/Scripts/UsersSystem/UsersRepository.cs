using System.Collections.Generic;
using Data;
using UnityEngine;

namespace UsersSystem
{
    public abstract class UsersRepository : MonoBehaviour
    {
        public abstract List<UserData> GetAllUsers();
        public abstract UserData GetChatOwner();
    }
}