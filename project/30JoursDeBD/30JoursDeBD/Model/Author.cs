using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHttpJson
{
    public class Author
    {
        public int Id { get; private set; }
        public string Slug { get; private set; }
        public string Name { get; private set; }
        public string First_name { get; private set; }
        public string Last_name { get; private set; }
        public string Url { get; private set; }
        public string Description { get; private set; } // !! HTML !! 
    }
}
