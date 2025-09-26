using AYellowpaper.SerializedCollections;
using Coding.Language;
using UnityEngine;
namespace Coding.Language
{
    public interface IContainer<T>
    {
        public void Add(T toAdd);
    }
}
