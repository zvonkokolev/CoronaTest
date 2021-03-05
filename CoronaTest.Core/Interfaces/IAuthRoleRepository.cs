using CoronaTest.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoronaTest.Core.Interfaces
{
    public interface IAuthRoleRepository
    {
        Task AddAsync(AuthRole role);
    }
}
