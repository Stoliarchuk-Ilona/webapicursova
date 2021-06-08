using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace firstWebAPI.Model
{
    public class Class1
    {
        public Artist Artist { get; set; }
    }
    public class Artist
    {
        public string Name { get; set; }
        public Bio Bio { get; set; }

    }
    public class Bio
    {
        public Links Links { get; set; }
        public string Summary { get; set; }

    }
    public class Links
    {
        public Link Link { get; set; }
        
    }
    public class Link
    {
        public string Href { get; set; }
    }
    
}
