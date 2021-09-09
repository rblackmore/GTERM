namespace GTerm.NET.Configuration
{
    public class PortOptions
    {
        public const string ConfigurationName = "PortOptions";

        public string Name { get; set; }
        
        public int BaudRate { get; set; }
        
        public int DataBits { get; set; }
        
        public string HandShake { get; set; }

        public float StopBits { get; set; }
        
        public string Parity { get; set; }
    }
}
