using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

namespace Coding.SharpCube
{
    public interface IMethodContainer : IContainer<UserMethod>
    {
        public SerializedDictionary<string, UserMethod> userMethods { get; set; }
        public Method GetMethod(string toGet);
    }
}
