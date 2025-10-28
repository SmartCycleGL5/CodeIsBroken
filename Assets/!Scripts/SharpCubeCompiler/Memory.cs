using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;

public enum Privilege
{
    Private = 0,
    Public = 1,
}

[Serializable]
public class Memory<T>
{
    public Dictionary<string, T> inMemory = new();
    [SerializedDictionary("Name", "Value")]
    public SerializedDictionary<string, T> @public = new();

    [SerializedDictionary("Name", "Value")]
    public SerializedDictionary<string, T> @private = new();

    public bool Contains(string key)
    {
        if(@public.ContainsKey(key)) return true;
        if(@private.ContainsKey(key)) return true;

        return false;
    }
    public T Get(Privilege privilege, string name)
    {
        if (privilege == Privilege.Private && @private.ContainsKey(name)) return @private[name];


        else if (privilege >= Privilege.Public && @public.ContainsKey(name)) return @public[name];

        return default(T);
    }

    public void Add(string name, T toAdd, Privilege privilege)
    {
        inMemory.Add(name, toAdd);
        
        switch (privilege)
        {
            case Privilege.Private:
                {
                    @private.Add(name, toAdd);
                    break;
                }
            case Privilege.Public:
                {
                    @public.Add(name, toAdd);
                    break;
                }
        }
    }

    public void Clear()
    {
        @public.Clear(); @private.Clear(); inMemory.Clear();
    }
}
