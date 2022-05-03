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
    public class InterviewController : Controller
    {
        private readonly IInterviewRepository _interviewRepo;
        private readonly IApplicationRepository _appRepo;
        public InterviewController(IInterviewRepository interviewRepo, IApplicationRepository appRepo)
        {
            _interviewRepo = interviewRepo;
            _appRepo = appRepo;
        }
        // GET: InterviewController
        public ActionResult Index()
        {
            int userId = GetCurrentUserId();

            List<Interview> interview = _interviewRepo.GetAllInterviewsByUser(userId);
            return View(interview);
        }

        // GET: InterviewController/Details/5
        public ActionResult Details(int id)
        {
            Application application = _appRepo.GetById(id);
            List<Interview> interviews = _interviewRepo.GetInterviewsByApplicationId(application.Id);
            return View(interviews);
        }

        // GET: InterviewController/Create
        public ActionResult Create()
        {
            int userId = GetCurrentUserId();
            var vm = new InterviewViewModel();
            vm.Applications = _appRepo.GetAllApplicationsByUser(userId);

            return View(vm);
        }

        // POST: InterviewController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InterviewViewModel vm)
        {
            try
            {
                _interviewRepo.Add(vm.Interview);
                return RedirectToAction("Index", "Interview", new { id = vm.Interview.Id });
            }
            catch
            {
                return View();
            }
        }

        // GET: InterviewController/Edit/5
        public ActionResult Edit(int id)
        {
            int userId = GetCurrentUserId();
            Interview interview = _interviewRepo.GetById(id);
            List<Application> applications = _appRepo.GetAllApplicationsByUser(userId);
            InterviewViewModel ivm = new InterviewViewModel()
            {
                Interview = interview,
                Applications = applications
            };
            return View(ivm);
        }

        // POST: InterviewController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, InterviewViewModel ivm)
        {
            try
            {
                _interviewRepo.Update(ivm.Interview);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Edit), new { id });

            }
        }

        // GET: InterviewController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InterviewController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
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
