using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _30JoursDeBD.testmodel
{
    public class Category
    {
        public int id { get; set; }
        public string slug { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int parent { get; set; }
        public int post_count { get; set; }
    }

    public class Author
    {
        public int id { get; set; }
        public string slug { get; set; }
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string nickname { get; set; }
        public string url { get; set; }
        public string description { get; set; }
    }

    public class Author2
    {
        public int id { get; set; }
        public string slug { get; set; }
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string nickname { get; set; }
        public string url { get; set; }
        public string description { get; set; }
    }

    public class Comment
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string date { get; set; }
        public string content { get; set; }
        public int parent { get; set; }
        public Author2 author { get; set; }
    }

    public class Attachment
    {
        public int id { get; set; }
        public string url { get; set; }
        public string slug { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string caption { get; set; }
        public int parent { get; set; }
        public string mime_type { get; set; }
        public List<object> images { get; set; }
    }

    public class CustomFields
    {
        public List<string> lightboxoff { get; set; }
        public List<string> views { get; set; }
    }

    public class Post
    {
        public int id { get; set; }
        public string type { get; set; }
        public string slug { get; set; }
        public string url { get; set; }
        public string status { get; set; }
        public string title { get; set; }
        public string title_plain { get; set; }
        public string content { get; set; }
        public string excerpt { get; set; }
        public string date { get; set; }
        public string modified { get; set; }
        public List<Category> categories { get; set; }
        public List<object> tags { get; set; }
        public Author author { get; set; }
        public List<Comment> comments { get; set; }
        public List<Attachment> attachments { get; set; }
        public int comment_count { get; set; }
        public string comment_status { get; set; }
        public object thumbnail { get; set; }
        public CustomFields custom_fields { get; set; }
        public string thumbnail_size { get; set; }
        public List<object> thumbnail_images { get; set; }
    }

    public class RootObject
    {
        public string status { get; set; }
        public int count { get; set; }
        public int count_total { get; set; }
        public int pages { get; set; }
        public List<Post> posts { get; set; }
    }
}
