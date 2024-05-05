using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MisCanchas.Contracts.Dtos.Cash;
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
        private readonly MisCanchasDbContext _misCanchasDbContext;
        private readonly IMapper _mapper;
        public CashService(MisCanchasDbContext misCanchasDbContext, IMapper mapper)
        {
            this._misCanchasDbContext = misCanchasDbContext;
            this._mapper = mapper;
        }

        public CashDto GetDto()
        {
            return _mapper.Map<CashDto>(this.Get()?.FirstOrDefault());
        }

        public void UpdateDto(CashUpdateDto dto, bool saveChanges = false)
        {
            var cash = _mapper.Map<Cash>(dto);
            this.Update(cash, saveChanges);
        }

        //Metodos internal
        internal Cash Find(int id)
        {
            var cash = this.Get(id);
            if (cash == null) { throw new ArgumentException("Entity not found"); }
            return cash;
        }

        internal IQueryable<Cash>? Get()
        {
            var cashList = this._misCanchasDbContext.Cash;
            if (cashList == null) { throw new ArgumentException("Entities not found"); }
            return cashList;
        }
        internal Cash? Get(int id)
        {
            var cash = _misCanchasDbContext.Cash.FirstOrDefault(x => x.Id == id);
            if (cash == null) { throw new ArgumentException("Entity not found"); }
            return cash;
        }

        internal void Update(Cash cash, bool saveChanges)
        {
            var cashUpdate = this.Find(cash.Id);
            cashUpdate.Amount = cash.Amount;
            _misCanchasDbContext.Cash.Update(cashUpdate);
            if (saveChanges) this._misCanchasDbContext.SaveChanges();
        }
    }
}
