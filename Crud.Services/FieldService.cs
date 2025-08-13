using Microsoft.EntityFrameworkCore;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;

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

            if (field == null)
                throw new Exception("No se encontró la cancha. Asegúrate de que exista al menos una en la base de datos.");
            else
                return field;
        }
        public async Task Update(int openHour, int closeHour, string name, decimal price, decimal deposit)
        {
            var field = await Get();
            if (field != null)
            {
                field.OpenHour = openHour;
                field.CloseHour = closeHour;
                field.Name = name;
                field.Price = price;
                field.Deposit = deposit;
                misCanchasDbContext.Fields.Update(field);
                await misCanchasDbContext.SaveChangesAsync();
            }
        }
    }
}
