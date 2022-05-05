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
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepo;
        private readonly IApplicationRepository _appRepo;

        public TagController(ITagRepository tagRepo, IApplicationRepository appRepo)
        {
           
            _tagRepo = tagRepo;
            _appRepo = appRepo;

        }
        // GET: TagController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TagController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TagController/Create
        public ActionResult Create()
        {
            int userId = GetCurrentUserId();
            var avm = new ApplicationDetailViewModel();
            avm.Tags = _tagRepo.GetAllTags();
            avm.Applications = _appRepo.GetAllApplicationsByUser(userId);
        
            return View(avm);
        }

        // POST: TagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ApplicationDetailViewModel avm)
        {
            try
            {
                _tagRepo.AddApplicationTag(avm.ApplicationTag);
                
                return RedirectToAction("Index", "Application");
            }
            catch (Exception ex)
            {
                var exception = ex;
                return View();
            }
        }

        // GET: TagController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: TagController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TagController/Delete/5
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
