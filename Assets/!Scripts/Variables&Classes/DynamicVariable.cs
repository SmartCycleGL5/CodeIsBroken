using System;

namespace Terminal.Language
{
    [Serializable]
    public class Variable
    {
        public string name;

        public Type type;

        public float Float = 0;
        public int Int = 0;
        public string String = "";
        public bool Bool = false;

        public Variable(string name, Type type)
        {
            this.name = name;
            this.type = type;
        }
        public void SetValue(string variable)
        {
            switch (type)
            {
                case Type.String:
                    {
                        String = variable;
                        return;
                    }
                case Type.Bool:
                    {
                        if (variable == "true")
                        {
                            Bool = true;
                        }
                        else if (variable == "false")
                        {
                            Bool = false;
                        }
                        return;
                    }
                case Type.Float:
                    {
                        Float = float.Parse(variable);
                        return;
                    }
                case Type.Int:
                    {
                        Int = int.Parse(variable);
                        return;
                    }
            }
        }
    }
}
