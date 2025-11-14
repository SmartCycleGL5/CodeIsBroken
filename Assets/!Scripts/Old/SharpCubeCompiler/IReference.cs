using UnityEngine;


namespace SharpCube
{
    public interface IReference
    {
        public string name { get; set; }
        public IContainer container { get; set; }
    }

}
