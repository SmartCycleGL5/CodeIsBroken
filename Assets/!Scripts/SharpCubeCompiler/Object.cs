
using SharpCube.Type;

namespace SharpCube
{
    public class Object
    {
        public IType value;

        public Object(IType value)
        {
            this.value = value;
        }
    }

}