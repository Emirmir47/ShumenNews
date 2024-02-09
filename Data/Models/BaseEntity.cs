namespace ShumenNews.Data.Models
{
    public class BaseEntity<T>
    {
        private bool isDeleted = false;
        public BaseEntity()
        {
            CreatedOn = DateTime.UtcNow;
        }
        public bool IsDeleted 
        {
            get => isDeleted;
            set
            {
                if (isDeleted == false && value == true)
                {
                    isDeleted = true;
                    DeletedOn = DateTime.UtcNow;
                }
                else if (isDeleted == true && value == false)
                {
                    isDeleted = false;
                    DeletedOn = null;
                }
            }
        }
        public DateTime CreatedOn { get; set; }
        public DateTime? DeletedOn { get; private set; } = null;
    }
}
