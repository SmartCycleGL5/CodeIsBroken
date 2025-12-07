using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PersistentData<T>
{
    public string name { get; private set; }
    public string folder  { get; private set; }
    public T data { get; private set; }

    public Action onChanged;
    
    public string dataPath => Application.persistentDataPath + $"/{folder}/{name}.json";

    public PersistentData(string name, string folder, T data = default)
    {
        this.name = name;
        this.folder = folder;
        this.data = data;
        
        Save();
    }

    public void UpdateData(T data)
    {
        this.data = data;
        Save();
    }
    
    void Save()
    {
        Debug.Log($"Saving to: {dataPath}");
        string jsonText = JsonUtility.ToJson(this);
        File.WriteAllText(dataPath, jsonText);
        
        onChanged?.Invoke();
    }
    public void Load()
    {
        Debug.Log($"Loading: {dataPath}");
        string jsonToRead = File.ReadAllText(dataPath);
        JsonUtility.FromJsonOverwrite(jsonToRead, this);
    }

    public void Delete()
    {
        throw  new System.NotImplementedException();
    }
}
