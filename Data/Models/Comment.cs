namespace ShumenNews.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
     
        //TODO !!! 
        public int Likes { get; set; }
        public int Dislikes { get; set; }
       
        public string AppUserId { get; set; }
        public virtual AppUser Commenter { get; set; }
        
        public int ArticleId { get; set; }
        public virtual UserArticleAttitude Article { get; set; }
       
        //Optional
        public int? ParentCommentId { get; set; }
        public virtual Comment? ParentComment { get; set; }
        
        //Optional TODO
        public virtual ICollection<Comment>? Kids { get; set; }
    }
}
