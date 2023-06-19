using MisCanchas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisCanchas.Contracts.Services
{
    public interface IFieldService
    {
        Task<Field> Get();
        Task Update(int openHour, int closeHour, string name, decimal price);
    }
}
