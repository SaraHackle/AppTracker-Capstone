using AppTrackerMVC.Models;
using System.Collections.Generic;

namespace AppTrackerMVC.Repositories
{
    public interface ITagRepository
    {
        public List<Tag> GetAllTags();
        List<Tag> GetTagsByApplicationId(int id);
        public ApplicationTag GetApplicationTagById(int id);
        public List<ApplicationTag> GetApplicationTagsByApplicationId(int id);
        void AddApplicationTag(ApplicationTag appTag);
        void DeleteApplicationTag(int id);
    }
}