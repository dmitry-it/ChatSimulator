using System;
using System.Collections.Generic;
using Data;

namespace UsersSystem
{
   [Serializable]
    public class UsersConfiguration
    {
        public int chatOwnerId;
        public List<UserData> users = new List<UserData>();
    }
}