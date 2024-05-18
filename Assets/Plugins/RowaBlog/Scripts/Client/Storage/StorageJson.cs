using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Rowa.Blog.Client.Storage
{
    public class StorageJson : IStorage
    {
        private readonly string _filePath = Application.persistentDataPath + "/storage.json";
        private Dictionary<string, string> _dictinary = new Dictionary<string, string>();
        public void Save(string key, string value, Action<bool> onComplete = null)
        {
            if (_dictinary.ContainsKey(key))
            {
                _dictinary[key] = value;
            }
            else
            {
                _dictinary.Add(key, value);
            }
            Save(onComplete);
        }
        public void Load(string key, Action<bool, string> onComplete)
        {
            Load();
            if (_dictinary.ContainsKey(key))
            {
                onComplete.Invoke(true, _dictinary[key]);
            }
            else
            {
                onComplete.Invoke(false, "");
            }
        }
        public void Delete(string key, Action<bool> onComplete = null)
        {
            if (_dictinary.ContainsKey(key))
            {
                _dictinary.Remove(key);
                Save();
                onComplete.Invoke(true);
            }
            else
            {
                Save();
                onComplete.Invoke(false);
            }
        }
        private void Load(Action<bool> onComplete = null)
        {
            if (!File.Exists(_filePath))
            {
                onComplete?.Invoke(false);
                return;
            }
            string fileContents = File.ReadAllText(_filePath);
            StorageJsonList storageJsonList = JsonUtility.FromJson<StorageJsonList>(fileContents);
            if (storageJsonList == null)
            {
                onComplete?.Invoke(false);
                return;
            }
            _dictinary.Clear();
            foreach (StorageJsonElement storageJsonElement in storageJsonList.items)
            {
                if (!_dictinary.ContainsKey(storageJsonElement.key))
                {
                    _dictinary.Add(storageJsonElement.key, storageJsonElement.value);
                }
            }
            onComplete?.Invoke(true);
        }
        private void Save(Action<bool> onComplete = null)
        {
            StorageJsonList storageJsonList = new StorageJsonList();
            foreach (KeyValuePair<string, string> pair in _dictinary)
            {
                storageJsonList.items.Add(new StorageJsonElement()
                {
                    key = pair.Key,
                    value = pair.Value
                });
            }
            string json = JsonUtility.ToJson(storageJsonList);
            File.WriteAllText(_filePath, json);
            onComplete?.Invoke(true);
        }
    }
}