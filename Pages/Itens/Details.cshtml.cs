using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SistemaCadastro.Models.DB;
using SistemaCadastro.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace SistemaCadastro.Pages.Itens
{
    public class CommandResult<T>
    {
        private CommandResult(string reason)
            => FailureReason = reason;

        private CommandResult(T payload)
            => Payload = payload;

        public T Payload { get; }
        public string FailureReason { get; }
        public bool IsSuccess => FailureReason != null;

        public static CommandResult<T> Fail(string reason)
            => new CommandResult<T>(reason);

        public static CommandResult<T> Success(T payload)
            => new CommandResult<T>(payload);

        public static implicit operator bool(CommandResult<T> result)
            => result.IsSuccess;
    }


    public class Details : PageModel
    {
        private readonly IMediator _mediator;

        public Model Data { get; private set; }

        public Details(IMediator mediator) => _mediator = mediator;

        public async Task OnGetAsync(Query query)
            => Data = await _mediator.Send(query);

        public class Query : IRequest<Model>
        {
            public int Id { get; set; }
        }

        public class Model
        {
            public int Id { get; set; }

            public string Nome { get; set; }

            public string Tipo { get; set; }

            public decimal Valor { get; set; }

            public DateTime DataInclusao { get; set; }

            public string LoginNome { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<Item, Model>();
        }
        
        public class QueryHandler : IRequestHandler<Query, Model>
        {
            private readonly  db_coreloginContext _context;
            private readonly IConfigurationProvider _configuration;

            public QueryHandler(db_coreloginContext context, IConfigurationProvider configuration)
            {
                _context = context;
                _configuration = configuration;
            }

            public Task<Model> Handle(Query message, 
                CancellationToken token) => 
                _context.Item
                .FromSql(@"SELECT * FROM Item WHERE ItemID = {0}", 
                        message.Id)
                .ProjectTo<Model>(_configuration)
                .SingleOrDefaultAsync(token);
        }
    }
}