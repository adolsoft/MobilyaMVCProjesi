using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogApp.Data.Concrete.EfCore
{
    public class BlogContext:DbContext
    {
        //dışardan connection string vermemiz gerekiyor o yuzden yapıcı bir metod yapaıyoruz.
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {

        }
        
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
