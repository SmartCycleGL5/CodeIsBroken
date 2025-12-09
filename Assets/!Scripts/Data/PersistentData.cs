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
    
    public string dataPath => Application.persistentDataPath + $"{folder}/";
    
    public PersistentData(string name, string folder, T data = default)
    {
        this.name = name;
        this.folder = "/"+folder;
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
        Debug.Log($"Saving to: {dataPath + name}.txt");
        string jsonText = JsonUtility.ToJson(this);

        if(!Directory.Exists(dataPath)) Directory.CreateDirectory(dataPath);
        File.WriteAllText(dataPath + name+ ".txt", jsonText);
        
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
