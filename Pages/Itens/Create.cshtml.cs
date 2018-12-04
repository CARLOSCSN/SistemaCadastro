using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using SistemaCadastro.Models;
using SistemaCadastro.Models.DB;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using static SistemaCadastro.Pages.PageModelExtensions;
using Newtonsoft.Json;
using SistemaCadastro.Interfaces;

namespace SistemaCadastro.Pages.Itens
{
    public class Create : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public Command Data { get; set; }

        public Create(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActionResult> OnPostAsync()
        {
            await _mediator.Send(Data);

            string url = "/Itens/Index";
            return Redirect(url);

            //return this.RedirectToPageJson("Index");
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

        public class MappingProfiler : Profile
        {
            public MappingProfiler() => CreateMap<Command, Item>(MemberList.Source);
        }

        public class Command : IRequest<int>
        {
            [Column("ItemID")]
            public int Id { get; set; }

            [StringLength(50, MinimumLength = 3)]
            public string Nome { get; set; }

            [StringLength(50, MinimumLength = 3)]
            public string Tipo { get; set; }

            [DataType(DataType.Currency)]
            [Column(TypeName = "money")]
            public decimal Valor { get; set; }

            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            [Display(Name = "Start Date")]
            public DateTime DataInclusao { get; set; }

            public int? LoginID { get; set; }

            [Timestamp]
            public byte[] RowVersion { get; set; }

            public Login Login { get; set; }
        }

        public class CommandHandler : IRequestHandler<Command, int>
        {
            private readonly db_coreloginContext _context;
            private readonly IMapper _mapper;
            private readonly AspNetDataTempMessageInSession _tempDataSessionAux;
            private readonly IUser _user;

            public CommandHandler(db_coreloginContext context, IMapper mapper, IUser user, AspNetDataTempMessageInSession tempDataSessionAux)
            {
                _context = context;
                _mapper = mapper;
                _user = user;
                _tempDataSessionAux = tempDataSessionAux;
            }

            public async Task<int> Handle(Command message, CancellationToken token)
            {
                var returnId = 0;

                try
                {
                    var loginAux = new LoginByUsernamePassword();
                    loginAux = await _context.LoginByUsernameMethodAsync(_user.Nome);

                    var login = new Login
                    {
                        Id = loginAux.LoginId,
                        Username = loginAux.Username,
                        Password = loginAux.Password,
                    };

                    var item = _mapper.Map<Command, Item>(message);
                    item.DataInclusao = DateTime.Now;
                    item.LoginID = login.Id;
                    item.LoginNome = login.Username;

                    _context.Item.Add(item);
                    await _context.SaveChangesAsync(token);
                    returnId = item.Id;

                    // Mensagem após submit
                    _tempDataSessionAux.InsertDataTempInSession(true, "success", "300", "3500", "400", "O Item " + message.Nome + " foi cadastrado com sucesso!");

                }
                catch (Exception ex)
                {
                    var erro = ex.Message;

                    // Mensagem após submit
                    _tempDataSessionAux.InsertDataTempInSession(true, "failure", "300", "3500", "400", erro);
                }

                return returnId;
            }
        }
    }
}