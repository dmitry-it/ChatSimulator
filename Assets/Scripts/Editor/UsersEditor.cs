using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Data;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UsersSystem;

namespace Editor
{
    public class UsersEditor : EditorWindow
    {
        private const string ConfigFilePath = "/Resources/users_configuration.json";
        private const string AvatarsPath = "/Resources/Avatars/";
        private const int AvatarsEditorWidth = 3;
        private readonly Dictionary<string, Texture> _avatarTextures = new Dictionary<string, Texture>();
        private UsersConfiguration _configuration;
        private UserData _currentUser;
        private Vector2 _scrollPosition = Vector2.zero;

        private ReorderableList _usersList;

        private void OnEnable()
        {
            _configuration = LoadConfiguration();
            CreateUsersList();
            LoadAvatars();
        }

        private void OnGUI()
        {
            DrawButtons();
            DrawOwner();
            DrawUsers();
        }

        [MenuItem("Tools/Chat Users Editor", false, 0)]
        private static void Init()
        {
            var window = GetWindow(typeof(UsersEditor));
            window.titleContent = new GUIContent("Chat Users Editor");
        }

        private void DrawButtons()
        {
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("New", GUILayout.Width(100), GUILayout.Height(50)))
                _configuration = new UsersConfiguration();

            if (GUILayout.Button("Save", GUILayout.Width(100), GUILayout.Height(50))) SaveConfiguration();

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void DrawOwner()
        {
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(
                new GUIContent("Chat Owner is user # " + _configuration.chatOwnerId, "Choose chat owner."),
                GUILayout.Width(EditorGUIUtility.labelWidth));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void DrawUsers()
        {
            GUILayout.Space(15);
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(300));
            _usersList.DoLayoutList();
            GUILayout.EndVertical();
            if (_currentUser != null)
            {
                GUILayout.Space(20);
                GUILayout.BeginVertical();
                DrawUserData(_currentUser, true);
                DrawAvatarEditor();
                GUILayout.EndVertical();
            }

            GUILayout.BeginVertical();
            GUILayout.Space(15);
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }

        private void DrawUserData(UserData userData, bool allowEdit = false)
        {
            if (allowEdit)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Make an Owner", GUILayout.Width(150), GUILayout.Height(30)))
                    _configuration.chatOwnerId = userData.id;

                GUILayout.EndHorizontal();
            }

            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent($"User : {userData.id}"),
                GUILayout.Width(EditorGUIUtility.labelWidth));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Name"),
                GUILayout.Width(EditorGUIUtility.labelWidth));
            userData.name = EditorGUILayout.TextField(userData.name, GUILayout.Width(70));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(new GUIContent("Avatar"),
                GUILayout.Width(EditorGUIUtility.labelWidth));
            var texture = _avatarTextures.ContainsKey(userData.avatarName)
                ? _avatarTextures[userData.avatarName]
                : _avatarTextures.First().Value;

            GUILayout.Box(texture, GUILayout.Width(100), GUILayout.Height(100));


            GUILayout.EndHorizontal();
        }

        private void DrawAvatarEditor()
        {
            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(
                new GUIContent("Avatar Editor:" + _configuration.chatOwnerId, "Choose chat owner."),
                GUILayout.Width(EditorGUIUtility.labelWidth));
            GUILayout.EndHorizontal();
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, GUILayout.MaxWidth(350));
            GUILayout.BeginHorizontal();
            var counter = 0;
            foreach (var pair in _avatarTextures)
            {
                if (GUILayout.Button(pair.Value, GUILayout.Width(100), GUILayout.Height(100)))
                    _currentUser.avatarName = pair.Key;

                counter++;
                if (counter != AvatarsEditorWidth) continue;
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                counter = 0;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
        }

        private static ReorderableList SetupReorderableList<T>(
            string headerText,
            List<T> elements,
            ref T currentElement,
            Action<Rect, T> drawElement,
            Action<T> selectElement,
            Action createElement,
            Action<T> removeElement)
        {
            var list = new ReorderableList(elements, typeof(T), false, true, true, true)
            {
                drawHeaderCallback = rect => { EditorGUI.LabelField(rect, headerText); },
                drawElementCallback = (rect, index, isActive, isFocused) =>
                {
                    var element = elements[index];
                    drawElement(rect, element);
                }
            };

            list.onSelectCallback = l =>
            {
                var selectedElement = elements[list.index];
                selectElement(selectedElement);
            };

            if (createElement != null) list.onAddDropdownCallback = (buttonRect, l) => { createElement(); };

            list.onRemoveCallback = l =>
            {
                if (!EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete this item?", "Yes",
                    "No")) return;

                var element = elements[l.index];
                removeElement(element);
                ReorderableList.defaultBehaviours.DoRemoveButton(l);
            };

            return list;
        }

        private void CreateUsersList()
        {
            _usersList = SetupReorderableList("Users", _configuration.users,
                ref _currentUser,
                (rect, x) =>
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 350, EditorGUIUtility.singleLineHeight),
                        $"{x.id} # {x.name}");
                },
                x => { _currentUser = x; },
                () =>
                {
                    var newItem = new UserData
                        {id = _configuration.users.Count, avatarName = _avatarTextures.First().Key};
                    _configuration.users.Add(newItem);
                },
                x => { _currentUser = null; });
        }

        private void LoadAvatars()
        {
            var path = Application.dataPath + AvatarsPath;
            var editorImagesPath = new DirectoryInfo(path);
            var files = editorImagesPath.GetFiles("*.png", SearchOption.TopDirectoryOnly);
            foreach (var file in files)
            {
                var filename = Path.GetFileNameWithoutExtension(file.Name);
                var bytes = File.ReadAllBytes(path + file.Name);
                var texture = new Texture2D(150, 150);
                texture.LoadImage(bytes);
                _avatarTextures[filename] = texture;
            }
        }

        private static UsersConfiguration LoadConfiguration()
        {
            var fullPath = Application.dataPath + ConfigFilePath;
            if (File.Exists(fullPath) == false) return new UsersConfiguration();
            var data = File.ReadLines(fullPath).FirstOrDefault(x => string.IsNullOrEmpty(x) == false);
            return JsonUtility.FromJson<UsersConfiguration>(data) ?? new UsersConfiguration();
        }

        private void SaveConfiguration()
        {
            var fullPath = Application.dataPath + ConfigFilePath;
            var data = JsonUtility.ToJson(_configuration);
            File.WriteAllLines(fullPath, new[] {data});
            AssetDatabase.Refresh();
        }
    }
}