//-----------------------------------------------------------------------
// <copyright file="Index.cshtml.cs" company="None">
//     Copyright (c) Allow to distribute this code and utilize this code for personal or commercial purpose.
// </copyright>
// <author>Asma Khalid</author>
//-----------------------------------------------------------------------

namespace SistemaCadastro.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using SistemaCadastro.Models;
    using SistemaCadastro.Models.DB;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using static SistemaCadastro.Pages.PageModelExtensions;

    /// <summary>
    /// Index page model class.
    /// </summary>
    public class IndexModel : PageModel
    {            
        #region Private Properties.

        /// <summary>
        /// Database Manager property.
        /// </summary>
        private readonly db_coreloginContext databaseManager;
        private readonly AspNetDataTempMessageInSession _tempDataSessionAux;

        #endregion

        #region Default Constructor method.

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexModel"/> class.
        /// </summary>
        /// <param name="databaseManagerContext">Database manager context parameter</param>
        public IndexModel(db_coreloginContext databaseManagerContext, AspNetDataTempMessageInSession tempDataSessionAux)
        {
            try
            {
                _tempDataSessionAux = tempDataSessionAux;

                // Settings.
                this.databaseManager = databaseManagerContext;
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets login model property.
        /// </summary>
        [BindProperty]
        public Login LoginModel { get; set; }

        #endregion
        
        #region On Get method.

        /// <summary>
        /// GET: /Index
        /// </summary>
        /// <returns>Returns - Appropriate page </returns>
        public IActionResult OnGet()
        {
            try
            {
                // Verification.
                if (this.User.Identity.IsAuthenticated)
                {
                    // Home Page.
                    return this.RedirectToPage("/Home/Index");
                }
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Info.
            return this.Page();
        }

        #endregion

        #region On Post Login method.

        /// <summary>
        /// POST: /Index/LogIn
        /// </summary>
        /// <returns>Returns - Appropriate page </returns>
        public async Task<IActionResult> OnPostLogIn()
        {
            try
            {
                // Verification.
                if (ModelState.IsValid)
                {
                    // Initialization.
                    var loginInfo = await this.databaseManager.LoginByUsernamePasswordMethodAsync(this.LoginModel.Username, this.LoginModel.Password);

                    // Verification.
                    if (loginInfo != null && loginInfo.Count() > 0)
                    {
                        // Initialization.
                        var logindetails = loginInfo.First();

                        // Login In.
                        await this.SignInUser(logindetails.Username, false);

                        // Info.
                        return this.RedirectToPage("/Home/Index");
                    }
                    else
                    {
                        // Setting.
                        ModelState.AddModelError(string.Empty, "Nome ou Senha estão incorretos.");
                    }
                }
            }
            catch (Exception ex)
            {
                // teste error
                ModelState.AddModelError(string.Empty, "Nome ou Senha estão incorretos.");

                // Info
                Console.Write(ex);
            }

            // Info.
            return this.Page();
        }

        #endregion

        #region On Post Create Login method.

        /// <summary>
        /// POST: /Index/CreateLogIn
        /// </summary>
        /// <returns>Returns - Appropriate page </returns>
        public async Task<IActionResult> OnPostCreateLogin()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                databaseManager.Login.Add(LoginModel);
                await databaseManager.SaveChangesAsync();

                // Mensagem após submit
                _tempDataSessionAux.InsertDataTempInSession(true, "success", "300", "3500", "400", "O Login " + LoginModel.Username + " foi cadastrado com sucesso!");

            }
            catch (Exception ex)
            {
                var erro = ex.Message;

                // Mensagem após submit
                _tempDataSessionAux.InsertDataTempInSession(true, "failure", "300", "3500", "400", erro);
            }


            // Home Page.
            return Page();
        }

        #endregion

        // ************ Helpers

        #region Helpers

        #region Sign In method.

        /// <summary>
        /// Sign In User method.
        /// </summary>
        /// <param name="username">Username parameter.</param>
        /// <param name="isPersistent">Is persistent parameter.</param>
        /// <returns>Returns - await task</returns>
        private async Task SignInUser(string username, bool isPersistent)
        {
            // Initialization.
            var claims = new List<Claim>();

            try
            {
                // Setting
                claims.Add(new Claim(ClaimTypes.Name, username));
                var claimIdenties = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimIdenties);
                var authenticationManager = Request.HttpContext;

                // Sign In.
                await authenticationManager.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, new AuthenticationProperties() { IsPersistent = isPersistent });
            }
            catch (Exception ex)
            {
                // Info
                throw ex;
            }
        }

        #endregion

        #endregion
    }
}
