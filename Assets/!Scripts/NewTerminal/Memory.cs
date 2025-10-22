using AYellowpaper.SerializedCollections;
using System;

public enum Privilege
{
    Public = 0,
    Private = 1,
}

[Serializable]
public class Memory<T>
{
    [SerializedDictionary("Name", "Value")]
    public SerializedDictionary<string, T> @public = new();

    [SerializedDictionary("Name", "Value")]
    public SerializedDictionary<string, T> @private = new();


    public T Get(Privilege privilege, string name)
    {
        if (privilege == Privilege.Private && @private.ContainsKey(name)) return @private[name];


        else if (privilege >= Privilege.Public && @public.ContainsKey(name)) return @public[name];

        return default(T);
    }

    public void Add(string name, T toAdd, Privilege privilege)
    {
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
        @public.Clear(); @public.Clear();
    }
}
