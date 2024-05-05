using MisCanchas.Contracts.Dtos.Cash;

namespace MisCanchas.Contracts.Services
{
    public interface ICashService
    {
        CashDto GetDto();
        void UpdateDto(CashUpdateDto dto, bool saveChanges);
    }
}
