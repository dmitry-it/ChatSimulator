using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace UsersSystem
{
    public class LocalUsersRepository : UsersRepository
    {
        [SerializeField] private TextAsset configFile;
        private UsersConfiguration _configuration;

        private void Awake()
        {
            Assert.IsNotNull(configFile);
            _configuration = JsonUtility.FromJson<UsersConfiguration>(configFile.ToString());
            Assert.IsTrue(_configuration.users?.Count > 0, "No Users in configuration file.");
        }

        public override List<UserData> GetAllUsers()
        {
            return _configuration.users;
        }

        public override UserData GetChatOwner()
        {
            return _configuration.users.Find(x => x.id.Equals(_configuration.chatOwnerId));
        }
    }
}