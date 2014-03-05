using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHttpJson
{
    public class Attachment
    {
        public int Id { get; private set; }
        public string Url { get; private set; }
        public string Slug { get; private set; }
        public string Title { get; private set; }
    }
}
