// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Example.WebMvcApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dhgms.AspNetCoreContrib.Example.WebMvcApp.Controllers
{
    /// <summary>
    /// Sample home controller.
    /// </summary>
    [Microsoft.AspNetCore.Authorization.Authorize]
    public class HomeController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="authorizationService">.NET core authorization service.</param>
        public HomeController(IAuthorizationService authorizationService)
        {
            ArgumentNullException.ThrowIfNull(authorizationService);
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// Serves the home page.
        /// </summary>
        /// <returns>View.</returns>
        public async Task<IActionResult> Get()
        {
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
