using AppTrackerMVC.Models;
using AppTrackerMVC.Models.ViewModels;
using AppTrackerMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AppTrackerMVC.Controllers
{
    [Authorize]
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
        public ActionResult Create(int id)
        {
            int userId = GetCurrentUserId();
            var avm = new ApplicationDetailViewModel();
            avm.Tags = _tagRepo.GetAllTags();
            avm.Application = _appRepo.GetById(id);
          
              

        
            return View(avm);
        }

        // POST: TagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, ApplicationDetailViewModel avm)
        {
            try
            {
                avm.ApplicationTag.ApplicationId = id;
                _tagRepo.AddApplicationTag(avm.ApplicationTag);

                return RedirectToAction("Details", "Application", new { id = id });
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

            ApplicationTag applicationTag = _tagRepo.GetApplicationTagById(id);
            
            return View();
        }

        // POST: TagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, ApplicationDetailViewModel avm)
        {
            try
            {
                
                _tagRepo.DeleteApplicationTag(id);
                return RedirectToAction("Index", "Application");
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
