namespace SharpCube
{
    public interface IContainer
    {
        public Encapsulation encapsulation { get; set; }
        public Memory<Variable> containedVarialbes { get; set; }
        public void StartCompile();
    }

}