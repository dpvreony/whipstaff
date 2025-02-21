// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Diagnostics;
using Dhgms.AspNetCoreContrib.Example.WebMvcApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Dhgms.AspNetCoreContrib.Example.WebMvcApp.Controllers
{
    /// <summary>
    /// Sample home controller.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        public HomeController()
        {
        }

        /// <summary>
        /// Serves the home page.
        /// </summary>
        /// <returns>View.</returns>
        public IActionResult Get()
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
