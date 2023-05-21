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
    public class FieldService : IFieldService
    {
        private readonly MisCanchasDbContext misCanchasDbContext;
        public FieldService(MisCanchasDbContext misCanchasDbContext)
        {
            this.misCanchasDbContext = misCanchasDbContext;
        }
        public async Task<Field> Get()
        {
            var field = await misCanchasDbContext.Fields.FirstOrDefaultAsync();
            return field;
        }

        public async Task Update(int openHour, int closeHour)
        {
            var field = await Get();
            if (field != null)
            {
                field.OpenHour = openHour;
                field.CloseHour = closeHour;
                misCanchasDbContext.Fields.Update(field);
                await misCanchasDbContext.SaveChangesAsync();
            }

            //var field1 = new Field()
            //{
            //    OpenHour = openHour,
            //    CloseHour = closeHour,
            //};
            //await misCanchasDbContext.AddAsync(field1);
            //await misCanchasDbContext.SaveChangesAsync();

        }
    }
}
