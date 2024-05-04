using Microsoft.EntityFrameworkCore;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MisCanchas.Services
{
    public class CashService : ICashService
    {
        private readonly MisCanchasDbContext misCanchasDbContext;
        public CashService(MisCanchasDbContext misCanchasDbContext)
        {
            this.misCanchasDbContext = misCanchasDbContext;
        }


        public async Task<Cash> Get()
        {
            var cash = await misCanchasDbContext.Cash.FirstOrDefaultAsync();
            if (cash == null)
            {
                throw new ArgumentException("Error al obtener la consulta");
            }
            return cash;
        }

        public async Task Update(decimal amount)
        {
            var cash = await misCanchasDbContext.Cash.FirstOrDefaultAsync();
            if (cash != null)
            {
                cash.Amount += amount;
                misCanchasDbContext.Cash.Update(cash);   
                await misCanchasDbContext.SaveChangesAsync();
            }
        }
    }
}
