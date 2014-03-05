using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHttpJson
{
    public class Comment
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Url { get; private set; }
        public DateTime Date { get; private set; }
        public string Content { get; private set; } // !!!! HTML !!!!

        /*
         * public Author author;
         */
    }
}
