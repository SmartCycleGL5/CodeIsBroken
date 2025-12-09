using System;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PersistentData<T>
{
    [field: SerializeField] public string name { get; private set; }
    [field: SerializeField] public string folder  { get; private set; }
    [field: SerializeField] public T data { get; private set; }

    public Action onChanged;
    
    public string filePath => folderPath + name + ".json";
    public string folderPath => Application.persistentDataPath + $"{folder}/";
    
    public PersistentData(string name, string folder, T data = default)
    {
        this.name = name;
        this.folder = "/"+folder;
        this.data = data;

        if (File.Exists(filePath))
        {
            Load();
        }
        else
        {
            Save();   
        }
    }

    public void UpdateData(T data)
    {
        this.data = data;
        Save();
    }
    
    void Save()
    {
        Debug.Log($"Saving to: {folderPath + name}.txt");
        string jsonText = JsonUtility.ToJson(this);

        if(!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
        File.WriteAllText(filePath, jsonText);
        
        onChanged?.Invoke();
    }
    public void Load()
    {
        Debug.Log($"Loading: {folderPath}");
        string jsonToRead = File.ReadAllText(filePath);
        JsonUtility.FromJsonOverwrite(jsonToRead, this);
    }

    public void Delete()
    {
        throw  new System.NotImplementedException();
    }
}
