using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PersistentData<T>
{
    public string name { get; private set; }
    public string filePath { get; private set; }
    [field: SerializeField] private T data { get; set; }

    public Action onChanged;
    
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
    
    private PersistentData(string name, string filePath)
    {
        this.name = name;
        this.filePath = filePath;
    }

    public static PersistentData<T> NewFile(string name, string folder, string fileType = "json", T data = default)
    {
        PersistentData<T> newData = new PersistentData<T>(name, Application.persistentDataPath + $"/{folder}/{name}.{fileType}");
        
        newData.data = data;
        string folderPath = Application.persistentDataPath + $"/{folder}";
        
        Debug.Log(name);
        if(!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
        
        newData.Save(); 
        return newData;
    }

    public static PersistentData<T> LoadFile(string filePath)
    {
        PersistentData<T> loadedData = new PersistentData<T>(Path.GetFileNameWithoutExtension(filePath), filePath);
        
        loadedData.Load(filePath);
        return loadedData;
    }

    public void UpdateData(T data)
    {
        this.data = data;
        onChanged?.Invoke();
        Save();
    }

    public T GetData()
    {
        Load(filePath);
        return data;    
    }
    
    void Save()
    {
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
        File.Delete(filePath);
    }
}
