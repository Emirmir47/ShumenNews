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
        /// <summary>
        /// Determines whether the user is in this role or not
        /// </summary>
        public bool IsChecked { get; set; }
    }
}
