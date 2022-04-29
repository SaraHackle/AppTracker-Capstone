using AppTrackerMVC.Models;
using System.Collections.Generic;

namespace AppTrackerMVC.Repositories
{
    public interface IApplicationRepository
    {
        List<Application> GetAllApplicationsByUser(int userId);
        Application GetById(int id);

        void Add(Application application);

        void Update(Application application);


    }
}