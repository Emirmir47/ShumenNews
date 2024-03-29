﻿using ShumenNews.Data.Models;
using ShumenNews.Models.ViewModels;

namespace ShumenNews.Services
{
    public interface IArticleService
    {
        public ShumenNewsArticle GetArticleById(int id);
        public int GetLastArticleId();
        public List<ShumenNewsArticle> GetArticlesByAuthor(ShumenNewsUser author);
        public List<ShumenNewsArticle> GetArticlesByCategoryId(string categoryId, int wordsCount);
        public List<ShumenNewsArticle> GetAllArticlesWithShortContent();
        public UserArticleViewModel GetUserArticleAsDTOByUsername(string username, ShumenNewsArticle article);
        public ShumenNewsUser GetArticleAuthor(ShumenNewsArticle article);
        public List<ShumenNewsArticle> ArticlesWithShortContent(List<ShumenNewsArticle> articles, int wordsCount);
        public void AddAViewToArticle(ShumenNewsArticle article);
        public void CreateUserArticle(ArticleViewModel article, ShumenNewsUser user);
        public void SetAttitudeToArticle(ArticleViewModel articleViewModel);
        public void AddCommentToArticle(ArticleViewModel articleViewModel);
        public void DeleteCommentFromArticle(ArticleViewModel articleViewModel);
    }
}
