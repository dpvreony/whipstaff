// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Example.WebMvcApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Splat;

namespace Dhgms.AspNetCoreContrib.Example.WebMvcApp.Controllers
{
    /// <summary>
    /// Sample home controller.
    /// </summary>
    public sealed class HomeController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="authorizationService">.NET core authorization service.</param>
        /// <param name="logger">Logging framework instance.</param>
        public HomeController(
            IAuthorizationService authorizationService,
            ILogger<HomeController> logger)
        {
            ArgumentNullException.ThrowIfNull(authorizationService);
            ArgumentNullException.ThrowIfNull(logger);
            _authorizationService = authorizationService;
            _logger = logger;
        }

        /// <summary>
        /// Serves the home page.
        /// </summary>
        /// <returns>View.</returns>
        public async Task<IActionResult> GetAsync()
        {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
            _logger.LogInformation("Home page requested.");
#pragma warning restore CA1848 // Use the LoggerMessage delegates
            var authResult = await _authorizationService.AuthorizeAsync(User, "HomePageView");
            if (!authResult.Succeeded)
            {
                return NotFound();
            }

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
