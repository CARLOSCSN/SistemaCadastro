using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SistemaCadastro.Models;
using SistemaCadastro.Models.DB;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace SistemaCadastro.Pages.Itens
{
    public class Index : PageModel
    {
        private readonly IMediator _mediator;

        public Index(IMediator mediator) => _mediator = mediator;

        public List<Model> Data { get; private set; }

        public async Task OnGetAsync()
            => Data = await _mediator.Send(new Query());

        public class Query : IRequest<List<Model>>
        {
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

        public class QueryHandler : IRequestHandler<Query, List<Model>>
        {
            private readonly db_coreloginContext _context;
            private readonly IConfigurationProvider _configuration;

            public QueryHandler(db_coreloginContext context, 
                IConfigurationProvider configuration)
            {
                _context = context;
                _configuration = configuration;
            }

            public Task<List<Model>> Handle(Query message, 
                CancellationToken token) => _context
                .Item
                .ProjectTo<Model>(_configuration)
                .ToListAsync(token);
        }
    }
}