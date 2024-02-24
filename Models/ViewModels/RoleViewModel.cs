namespace ShumenNews.Models.ViewModels
{
    public class RoleViewModel
    {
        public RoleViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
}
