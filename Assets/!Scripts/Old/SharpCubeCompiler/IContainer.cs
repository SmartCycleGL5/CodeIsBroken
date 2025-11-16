namespace SharpCube
{
    public interface IContainer
    {
        public Encapsulation encapsulation { get; set; }
        public Keywords keywords { get; set; }
        public Keywords allKeywords { get; }
        
        public void StartCompile();
    }

}