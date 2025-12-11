using System;
using System.Collections.Generic;
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
    
    public static List<string> disallowedNames = new()
    {
        "",
        //----- Our classes
        "Painter",
        "Assembler",
        "Furnace",
        "Laser",
        "Crane",
        "Machine",
        "MaterialTube",
        "Saw",
        "Console",
        "Material",
        "Random",
        //----- C# key words
        "abstract",
        "as",
        "base",
        "bool",
        "break",
        "byte",
        "case",
        "catch",
        "char",
        "checked",
        "class",
        "const",
        "continue",
        "decimal",
        "default",
        "delegate",
        "do",
        "double",
        "else",
        "enum",
        "event",
        "explicit",
        "extern",
        "false",
        "finally",
        "fixed",
        "float",
        "for",
        "foreach",
        "goto",
        "if",
        "implicit",
        "in",
        "int",
        "interface",
        "internal",
        "is",
        "lock",
        "long",
        "namespace",
        "new",
        "null",
        "object",
        "operator",
        "out",
        "override",
        "params",
        "private",
        "protected",
        "public",
        "readonly",
        "ref",
        "return",
        "sbyte",
        "sealed",
        "short",
        "sizeof",
        "stackalloc",
        "static",
        "string",
        "struct",
        "switch",
        "this",
        "throw",
        "true",
        "try",
        "typeof",
        "uint",
        "ulong",
        "unchecked",
        "unsafe",
        "ushort",
        "using",
        "virtual",
        "void",
        "volatile",
        "while",
    };
    public static List<char> disallowedCharacters = new()
    {
        ' ',
        '\t',
        '\n',
        ';',
        ':',
        ',',
        '.',
        '&',
        '@',
        '$',
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '"',
        '#',
        '%',
        '/',
        '=',
        '?',
        '+',
        '-',
        '*',
        '\'',
        '>',
        '<'
    };
    

    public static PersistentData<T> NewFile(string name, string folder, T data = default)
    {
        PersistentData<T> newData = new PersistentData<T>();
        
        newData.name = name;
        newData.folder = "/"+folder;
        newData.data = data;
        
        if(!Directory.Exists(newData.folderPath)) Directory.CreateDirectory(newData.folderPath);
        
        newData.Save(); 
        return newData;
    }

    public static PersistentData<T> LoadFile(string filePath)
    {
        PersistentData<T> loadedData = new PersistentData<T>();
        
        loadedData.Load(filePath);
        
        return loadedData;
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
        
        File.WriteAllText(filePath, jsonText);
        
        onChanged?.Invoke();
    }
    void Load(string filePath)
    {
        string jsonToRead = File.ReadAllText(filePath);
        JsonUtility.FromJsonOverwrite(jsonToRead, this);
    }

    public void Delete()
    {
        throw  new System.NotImplementedException();
    }
}
