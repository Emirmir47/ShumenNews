﻿namespace ShumenNews.Data.Models
{
    public class Category
    {
        public Category()
        {
            Articles= new HashSet<Article>();      
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}
