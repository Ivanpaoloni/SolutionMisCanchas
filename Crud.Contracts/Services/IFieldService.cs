using MisCanchas.Domain.Entities;

namespace MisCanchas.Contracts.Services
{
    public interface IFieldService
    {
        Task<Field> Get();
        Task Update(int openHour, int closeHour, string name, decimal price);
    }
}
