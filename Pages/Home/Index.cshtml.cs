//-----------------------------------------------------------------------
// <copyright file="Index.cshtml.cs" company="None">
//     Copyright (c) Allow to distribute this code and utilize this code for personal or commercial purpose.
// </copyright>
// <author>Asma Khalid</author>
//-----------------------------------------------------------------------

namespace SistemaCadastro.Pages.Home
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    /// <summary>
    /// Index page model class.
    /// </summary>
    [Authorize]
    public class IndexModel : PageModel
    {
        #region On Get method.

        /// <summary>
        /// On Get method.
        /// </summary>
        public void OnGet()
        {
            try
            {
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }
        }

        #endregion

        #region Log Out method.

        /// <summary>
        /// POST: /Home/Index/LogOff
        /// </summary>
        /// <returns>Return log off action</returns>
        public async Task<IActionResult> OnPostLogOff()
        {
            try
            {
                // Setting.
                var authenticationManager = Request.HttpContext;

                // Sign Out.
                await authenticationManager.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }

            // Info.
            return this.RedirectToPage("/Index");
        }

        #endregion
    }
}