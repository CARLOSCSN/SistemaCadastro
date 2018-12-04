using System.Collections.Generic;
using System.Security.Claims;

namespace SistemaCadastro.Interfaces
{
    public interface IStringSession
    {
        string Key { get; }
        string Value { get; }
    }
}