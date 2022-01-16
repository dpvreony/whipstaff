// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics;
using Dhgms.AspNetCoreContrib.Example.WebMvcApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dhgms.AspNetCoreContrib.Example.WebMvcApp.Controllers
{
    /// <summary>
    /// Sample home controller.
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="logger">Instance of logging framework.</param>
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Serves the home page.
        /// </summary>
        /// <returns>View.</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Serves the privacy page.
        /// </summary>
        /// <returns>View.</returns>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Serves the error page.
        /// </summary>
        /// <returns>View.</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
