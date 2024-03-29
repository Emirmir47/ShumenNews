﻿using Microsoft.AspNetCore.Mvc.Rendering;
using ShumenNews.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace ShumenNews.Models.BindingModels
{
    public class ArticleUpdateBindingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishedOn { get; set; }
        public string CategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IEnumerable<string> Images { get; set; }
        public ShumenNewsUser Author { get; set; }
        public bool IsDeleted { get; set; }
    }
}
