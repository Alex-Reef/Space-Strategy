using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    public class SaveManager
    {
        public static List<SaveData> Saves { get; private set; }
        private const string SaveFolder = "Saves";
        private const string SaveFileExtension = "gsdata";

        public static async void Load(Action onCompleted)
        {
            Saves = new List<SaveData>();
            
            DirectoryInfo directoryInfo = new DirectoryInfo($"{Application.persistentDataPath}/{SaveFolder}");
            if (!Directory.Exists(directoryInfo.FullName))
            {
                onCompleted?.Invoke();
                return;
            }
            
            FileInfo[] files = directoryInfo.GetFiles($"*.{SaveFileExtension}");

            for (int i = 0; i < files.Length; i++)
            {
                var text = await File.ReadAllTextAsync(files[i].FullName);
                if(text.Length == 0)
                    continue;
                try
                {
                    SaveData data = JsonConvert.DeserializeObject<SaveData>(Decrypt(text));
                    Saves.Add(data);
                }
                catch (JsonException e)
                {
#if UNITY_EDITOR
                    Debug.LogError(e.Message + " | File: " + files[i].FullName);    
#endif
                }
            }
            
            onCompleted?.Invoke();
        }

        public static async void Save(SaveData data, Action<string> onCompleted)
        {
            var settings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(data, Formatting.Indented, settings);
            
            string directoryPath = $"{Application.persistentDataPath}/{SaveFolder}";
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            string savePath = $"{directoryPath}/{data.dateTime}.{SaveFileExtension}";
            await File.WriteAllTextAsync(savePath, Encrypt(json));
            
            onCompleted?.Invoke(savePath);
        }

        private static string Encrypt(string data)
        {
            return data;
        }

        private static string Decrypt(string data)
        {
            return data;
        }
    }
}