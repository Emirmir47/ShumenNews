namespace ShumenNews.Models.ViewModels
{
    public class SearchViewModel
    {
        public bool IsAuthor { get; set; }
        public UserViewModel User { get; set; }
        public IEnumerable<ArticleViewModel> Articles { get; set; }
    }
}
