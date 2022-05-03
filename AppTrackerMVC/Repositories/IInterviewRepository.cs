using AppTrackerMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppTrackerMVC.Repositories
{
    public interface IInterviewRepository
    {
        public List<Interview> GetAllInterviewsByUser(int userId);
        public List<Interview> GetInterviewsByApplicationId(int id);
        public Interview GetById(int id);
        public void Add(Interview interview);
        public void Update(Interview interview);
        public void Delete(int id);

    }
}
