using System.Collections.Generic;

namespace Homepage.Config
{
    public class Configuration
    {
        public string BasePath { get; set; }

        public List<Folder> Folders { get; set; }
    }
}