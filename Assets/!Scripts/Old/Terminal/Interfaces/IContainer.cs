using AYellowpaper.SerializedCollections;
using Coding.SharpCube;
using UnityEngine;
namespace Coding.SharpCube
{
    public interface IContainer<T>
    {
        public void Add(T toAdd);
    }
}
