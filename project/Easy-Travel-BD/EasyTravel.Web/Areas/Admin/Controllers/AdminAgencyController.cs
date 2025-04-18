﻿using EasyTravel.Domain.Entites;
using EasyTravel.Domain.Services;
using EasyTravel.Web.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyTravel.Web.Areas.Admin.Controllers
{
    [Area("Admin"),Authorize(Roles ="admin,agencyManager")]
    public class AdminAgencyController : Controller
    {
        private readonly IAdminAgencyService _agencyService;
        public AdminAgencyController(IAdminAgencyService agencyService)
        {
            _agencyService = agencyService;
        }
        public IActionResult Index()
        {
            
            var agencies = _agencyService.GetAll();
            return View(agencies);
        }
        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create(Agency model)
        {
            
            if (ModelState.IsValid)
            {
                _agencyService.Create(model);
                TempData["success"] = "The agency has been created successfully";
                return RedirectToAction("Index", "AdminAgency", new { area = "Admin" });
            }
            return View();
        }
        [HttpGet]
        public IActionResult Update(Guid id)
        {
            
            if (id == Guid.Empty)
            {
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            var agency = _agencyService.Get(id);
            return View(agency);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(Agency model)
        {
            
            if (ModelState.IsValid)
            {
                _agencyService.Update(model);
                TempData["success"] = "The agency has been updated successfully";

                return RedirectToAction("Index", "AdminAgency", new { area = "Admin" });
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            
            if (id == Guid.Empty)
            {
                TempData["error"] = "The agency not found";
                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            var agency = _agencyService.Get(id);
            return View(agency);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Agency model)
        {
            
            if (model.Id == Guid.Empty)
            {
                TempData["error"] = "The agency not found";

                return RedirectToAction("Error", "Home", new { area = "Admin" });
            }
            _agencyService.Delete(model.Id);
            TempData["success"] = "The agency was deleted";

            return RedirectToAction("Index", "AdminAgency", new { area = "Admin" });
        }
    }
}