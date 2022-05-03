using AppTrackerMVC.Models;
using AppTrackerMVC.Models.ViewModels;
using AppTrackerMVC.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AppTrackerMVC.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IApplicationRepository _appRepo;
        private readonly IInterviewRepository _interviewRepo;

        public ApplicationController(IApplicationRepository appRepo, IInterviewRepository interviewRepo)
        {
            _appRepo = appRepo;
            _interviewRepo = interviewRepo;
           
        }
        // GET: ApplicationController
        public ActionResult Index()

        {

            int userId = GetCurrentUserId();
          
            List<Application> applications = _appRepo.GetAllApplicationsByUser(userId);
            return View(applications);
        }

        // GET: ApplicationController/Details/5
        public ActionResult Details(int id)
        {
            Application application = _appRepo.GetById(id);
            List<Interview> interviews = _interviewRepo.GetInterviewsByApplicationId(application.Id);

            ApplicationDetailViewModel avm = new ApplicationDetailViewModel()
            {
                Application = application,
                Interviews = interviews,
                
            };

            return View(avm);
        }

        // GET: ApplicationController/Create
        public ActionResult Create()
        {
                int userId = GetCurrentUserId();
                List<Application> applications = _appRepo.GetAllApplicationsByUser(userId);
                return View();
            
         
        }

        // POST: ApplicationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Application application)
        {
            try
            {
                application.UserId = GetCurrentUserId();
                _appRepo.Add(application);
                return RedirectToAction("Details", "Application", new { id = application.Id });
            }
            catch
            {

                return View(application);
            }
        }

        // GET: ApplicationController/Edit/5
        public ActionResult Edit(int id)
        {
            Application application = _appRepo.GetById(id);
            return View(application);
        }

        // POST: ApplicationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Application application)
        {
            try
            {
                application.UserId = GetCurrentUserId();
                _appRepo.Update(application);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ApplicationController/Delete/5
        public ActionResult Delete(int id)

        {
            Application application = _appRepo.GetById(id);
            return View(application);
        }

        // POST: ApplicationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Application application)
        {
            try
            {
                _appRepo.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(application);
            }
        }
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null)
            { id = "0"; }
            return int.Parse(id);

        }
    }
}
