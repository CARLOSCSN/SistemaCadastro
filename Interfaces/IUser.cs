using System.Collections.Generic;
using System.Security.Claims;

namespace SistemaCadastro.Interfaces
{
    public interface IUser
    {
        string Nome { get; }
        bool IsAuthenticated();
        IEnumerable<Claim> GetClaimsIdentity();
    }
}