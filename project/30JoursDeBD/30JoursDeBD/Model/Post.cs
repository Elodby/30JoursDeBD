using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHttpJson
{
    public class Post
    {
        public int Id { get; private set; }
        public string Slug { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; } // !!!! HTML !!!!
        public DateTime Date { get; private set; }
        private List<Category> _categories = new List<Category>();
        public List<Category> Categories
        {
            get
            {
                return this._categories;
            }
        }
        public Author Author { get; set; }

        private List<Comment> _comments = new List<Comment>();
        public List<Comment> Comments
        {
            get
            {
                return this._comments;
            }
        }
        private List<Attachment> _attachments = new List<Attachment>();
        public List<Attachment> Attachments
        {
            get
            {
                return this._attachments;
            }
        }
    }
}
