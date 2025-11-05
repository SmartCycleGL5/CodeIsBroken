namespace SharpCube
{
    public interface IContainer
    {
        public Encapsulation encapsulation { get; set; }
        public Keywords additionalKeywords { get; set; }
        
        public void StartCompile();
        
        
    }

}