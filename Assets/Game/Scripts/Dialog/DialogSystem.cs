using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using Dialogues.Dialogue;

namespace Dialogues.System
{
    /// <summary>
    /// Организует чтение диалогов из .json-файла в класс <see cref="JSONDialog"/>,
    /// получение нужного набора по ключу и запуск диалога.
    /// </summary>
    public class DialogSystem : MonoBehaviour
    {
        [SerializeField] private DialogSettings _dialogSettings;
        [SerializeField] private RectTransform _placeForDialogues;

        private Dialog _dialogPrefab;
        private JSONDialog _dialog;


        public void StartDialog(string key, string path)
        {
            var find = GameObject.FindAnyObjectByType<Dialog>();
            if (find)
            {
                find.StartDialog(key, path, _dialogSettings);
                return;
            }
            GameObject obj = Instantiate(_dialogPrefab.gameObject, _placeForDialogues);
            var txt = obj.GetComponent<Dialog>();
            txt.StartDialog(key, path, _dialogSettings);
        }
        public bool ReadDialogJSON(string path)
        {
            _dialog = new();
            TextAsset json = Resources.Load(path) as TextAsset;
            _dialog = JsonConvert.DeserializeObject<JSONDialog>(json.text);

            if (_dialog.text.Count <= 0)
            {
                GameUtilities.Debug.GameUtilities.DisplayWarning("Path is empty");
                json = Resources.Load(_dialogSettings.DefaultPath) as TextAsset;
                _dialog = JsonConvert.DeserializeObject<JSONDialog>(json.text);
                return false;
            }

            return true;
        }
        public Dictionary<string, List<string>> GetDialog(string key)
        {
            if (_dialog.text.TryGetValue(key, out var dialog)) return dialog;
            GameUtilities.Debug.GameUtilities.DisplayWarning("Key is empty");
            _dialog.text.TryGetValue(_dialogSettings.DefaultKey, out var dialogNull);
            return dialogNull;
        }

        private void Awake()
        {
            if (!_dialogSettings) return;
            _dialogPrefab = _dialogSettings.PrefabDialogFull;
        }


        public static DialogSystem Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = GameObject.FindAnyObjectByType<DialogSystem>();
                    if (!_instance) return null;
                    DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }
        private static DialogSystem _instance;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeOnLoadMethod()
        {
            _instance = null;
        }
    }
}