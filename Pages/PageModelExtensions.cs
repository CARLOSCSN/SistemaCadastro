using SistemaCadastro.Interfaces;
using SistemaCadastro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Security.Claims;
using Newtonsoft.Json;

namespace SistemaCadastro.Pages
{
    public static class PageModelExtensions
    {
        public static ActionResult RedirectToPageJson<TPage>(this TPage controller, string pageName)
            where TPage : PageModel
        {
            return controller.JsonNet(new
                {
                    redirect = controller.Url.Page(pageName)
                }
            );
        }

        public static ContentResult JsonNet(this PageModel controller, object model)
        {
            var serialized = JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return new ContentResult
            {
                Content = serialized,
                ContentType = "application/json"
            };
        }

        // Pegar o Usuario logado
        public class AspNetUser : IUser
        {
            private readonly IHttpContextAccessor _accessor;

            public AspNetUser(IHttpContextAccessor accessor)
            {
                _accessor = accessor;
            }

            public string Nome => _accessor.HttpContext.User.Identity.Name;

            public bool IsAuthenticated()
            {
                return _accessor.HttpContext.User.Identity.IsAuthenticated;
            }

            public IEnumerable<Claim> GetClaimsIdentity()
            {
                return _accessor.HttpContext.User.Claims;
            }
        }

        // Acessar context http para lidar com session, recuperar dados e etc
        public class AspNetAcessContextHTTP
        {
            private readonly IHttpContextAccessor _accessor;

            public AspNetAcessContextHTTP(IHttpContextAccessor accessor)
            {
                _accessor = accessor;
            }

            public void InsertStringSession()
            {
                _accessor.HttpContext.Session.SetString("teste", "teste");
            }
        }

        // Gerar ou Atualizar DataTemp
        public class AspNetDataTempMessageInSession : PageModel
        {
            private readonly IHttpContextAccessor _accessor;

            // CTOR
            public AspNetDataTempMessageInSession(IHttpContextAccessor accessor)
            {
                _accessor = accessor;
            }

            public void InsertDataTempInSession(bool ok, string className, string fadeIn, string delay, string fadeOut, string message)
            {
                if (ok)
                {
                    var item = new MessageViewModel
                    {
                        ClassName = className,
                        FadeIn = fadeIn,
                        Delay = delay,
                        fadeOut = fadeOut,
                        Message = className == "failure" ? "Erro:" + " " + message : message
                    };

                    var itemJson = JsonConvert.SerializeObject(item);

                    // Mensagem após submit
                    _accessor.HttpContext.Session.SetString("UserMessage", itemJson);
                }
            }
        }

        // AddItem Json in session
        public class AddItemInSessionJson : PageModel
        {
            private readonly IHttpContextAccessor _accesso;

            // CTOR
            public AddItemInSessionJson(IHttpContextAccessor accesso)
            {
                _accesso = accesso;
            }

            public void InsertItemInSessionJson(object obj, string nameSessionItem)
            {
                var itemJson = JsonConvert.SerializeObject(obj);

                // Mensagem após submit
                _accesso.HttpContext.Session.SetString(nameSessionItem, itemJson);
            }
        }

        // Get item Session
        public class GetItemInSessionJson : PageModel
        {
            private readonly IHttpContextAccessor _accesso;

            // CTOR
            public GetItemInSessionJson(IHttpContextAccessor accesso)
            {
                _accesso = accesso;
            }

            public string getItemInSessionJson(string nameSessionItem)
            {
                // Mensagem após submit
               var item = _accesso.HttpContext.Session.GetString(nameSessionItem);

                return item;
            }
        }
    }
}