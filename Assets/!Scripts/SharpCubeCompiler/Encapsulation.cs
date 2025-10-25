using UnityEngine;
using SharpCube.Object;

namespace SharpCube
{
    public class Encapsulation : IContainer
    {
        [field: SerializeField] public Memory<Variable> variables { get; set; }
        
        public static void Create(int line, Properties properties)
        {
            
        }
    }   
}
