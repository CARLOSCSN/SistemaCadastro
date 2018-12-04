using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SistemaCadastro.Models.DB;
using SistemaCadastro.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static SistemaCadastro.Pages.PageModelExtensions;

namespace SistemaCadastro.Pages.Itens
{
    public class Edit : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Command Data { get; set; }

        public Edit(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync(Query query)
            => Data = await _mediator.Send(query);

        public async Task<ActionResult> OnPostAsync(int id)
        {
            await _mediator.Send(Data);

            string url = "/Itens/Index";
            return Redirect(url);

            //return this.RedirectToPageJson("Index");
        }

        public class Query : IRequest<Command>
        {
            public int Id { get; set; }
        }

        public class Command : IRequest
        {
            public int Id { get; set; }

            public string Nome { get; set; }

            public string Tipo { get; set; }

            public decimal Valor { get; set; }

            public DateTime DataInclusao { get; set; }

            public int? LoginID { get; set; }

            public byte[] RowVersion { get; set; }

            public Login Login { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(m => m.Nome).NotNull().Length(3, 50);
                RuleFor(m => m.Tipo).NotNull();
                RuleFor(m => m.Valor).NotNull();
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Item, Command>().ReverseMap();
        }

        public class QueryHandler : IRequestHandler<Query, Command>
        {
            private readonly db_coreloginContext _db;
            private readonly IConfigurationProvider _configuration;

            public QueryHandler(db_coreloginContext db, 
                IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<Command> Handle(Query message, 
                CancellationToken token) => await _db
                .Item
                .Where(d => d.Id == message.Id)
                .ProjectTo<Command>(_configuration)
                .SingleOrDefaultAsync(token);
        }

        public class CommandHandler : IRequestHandler<Command>
        {
            private readonly db_coreloginContext _db;
            private readonly IMapper _mapper;
            private readonly AspNetDataTempMessageInSession _tempDataSessionAux;

            public CommandHandler(db_coreloginContext db, IMapper mapper, AspNetDataTempMessageInSession tempDataSessionAux)
            {
                _db = db;
                _mapper = mapper;
                _tempDataSessionAux = tempDataSessionAux;
            }

            public async Task<Unit> Handle(Command message, 
                CancellationToken token)
            {
                try
                {
                    // Select por ID
                    var item =
                        await _db.Item.FindAsync(message.Id);
                    //item.DataInclusao = DateTime.Now;

                    message.Login =
                        await _db.Login.FindAsync(message.LoginID);


                    _mapper.Map(message, item);

                    _db.SaveChanges();

                    // Mensagem após submit
                    _tempDataSessionAux.InsertDataTempInSession(true, "warning", "300", "3500", "400", "O Item " + message.Nome + " foi editado!");
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