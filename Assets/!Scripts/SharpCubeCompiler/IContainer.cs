namespace SharpCube
{
    public interface IContainer
    {
        public Encapsulation encapsulation { get; set; }
        public Memory<IReference> containedVarialbes { get; set; }
        public void StartCompile();
    }

}