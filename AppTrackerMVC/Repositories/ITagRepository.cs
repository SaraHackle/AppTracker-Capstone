using AppTrackerMVC.Models;
using System.Collections.Generic;

namespace AppTrackerMVC.Repositories
{
    public interface ITagRepository
    {
        public List<Tag> GetAllTags();
        List<Tag> GetTagsByApplicationId(int id);
    }
}