using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SistemaCadastro.Models;
using SistemaCadastro.Models.DB;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using SistemaCadastro.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using static SistemaCadastro.Pages.PageModelExtensions;

namespace SistemaCadastro.Pages.Itens
{
    public class Delete : PageModel
    {
        private readonly IMediator _mediator;

        public Delete(IMediator mediator) => _mediator = mediator;

        [BindProperty]
        public Command Data { get; set; }

        public async Task OnGetAsync(Query query)
            => Data = await _mediator.Send(query);

        public async Task<ActionResult> OnPostAsync()
        {
            await _mediator.Send(Data);

            string url = "/Itens/Index";
            return Redirect(url);

            //return Page();
            //return this.RedirectToPageJson("/Itens/Index");
        }

        public class Query : IRequest<Command>
        {
            public int Id { get; set; }
        }

        public class Command : IRequest
        {
            public string Nome { get; set; }

            public string Tipo { get; set; }

            public decimal Valor { get; set; }

            public DateTime DataInclusao { get; set; }

            public int Id { get; set; }

            public string LoginNome { get; set; }

            public byte[] RowVersion { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Item, Command>();
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly db_coreloginContext _db;
            private readonly IConfigurationProvider _configuration;

            public QueryHandler(db_coreloginContext db, IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<Command> Handle(Query message, CancellationToken token) => await _db
                .Item
                .Where(d => d.Id == message.Id)
                .ProjectTo<Command>(_configuration)
                .SingleOrDefaultAsync(token);
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly db_coreloginContext _db;
            private readonly AspNetDataTempMessageInSession _tempDataSessionAux;

            public CommandHandler(db_coreloginContext db, AspNetDataTempMessageInSession tempDataSessionAux)
            {
                _db = db;
                _tempDataSessionAux = tempDataSessionAux;
            }

            public async Task<Unit> Handle(Command message, CancellationToken token)
            {
                var item = await _db.Item.FindAsync(message.Id);

                try
                {
                    _db.Item.Remove(item);
                    _db.SaveChanges();

                    // Mensagem após submit
                    _tempDataSessionAux.InsertDataTempInSession( true, "warning", "300", "3500", "400", "O Item " + message.Nome + " foi excluido!");
                }
                catch (Exception ex)
                {
                    var erro = ex.Message;

                    // Mensagem após submit
                    _tempDataSessionAux.InsertDataTempInSession(true, "failure", "300", "3500", "400", erro);
                }

                return default;
            }
        }
    }
}