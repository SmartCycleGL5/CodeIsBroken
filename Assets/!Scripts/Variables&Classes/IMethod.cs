using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

namespace Coding.Language
{
    public interface IMethod
    {
        public SerializedDictionary<string, UserMethod> methods { get; set; }

        public void AddMethod(UserMethod method);
    }
}
