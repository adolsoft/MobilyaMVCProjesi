using System;
using System.Collections.Generic;
using System.Text;

namespace BlogApp.Entity
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        //Bire çok ilişki// bir categorye birden fala blogu olabilir anlamındadır.
        public List<Blog> Blogs { get; set; }

    }
}
