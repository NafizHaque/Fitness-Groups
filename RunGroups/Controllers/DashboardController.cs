﻿using Microsoft.AspNetCore.Mvc;

namespace RunGroups.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
