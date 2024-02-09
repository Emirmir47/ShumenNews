namespace ShumenNews.Models.ViewModels
{
    public class AdminViewModel
    {
        public IEnumerable<UserViewModel> Authors { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IEnumerable<ArticleViewModel> Articles { get; set; }
    }
}
