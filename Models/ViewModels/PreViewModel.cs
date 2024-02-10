namespace ShumenNews.Models.ViewModels
{
    public class PreViewModel
    {
        public IEnumerable<ArticleViewModel> WeekArticles { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
    }
}
