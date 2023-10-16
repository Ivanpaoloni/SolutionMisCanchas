using MisCanchas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisCanchas.Contracts.Services
{
    public interface ICashService
    {
        Task<Cash> Get();
        Task Update(decimal amount);
    }
}
