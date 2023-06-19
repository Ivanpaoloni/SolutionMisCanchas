using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MisCanchas.Services
{
    public class TurnService : ITurnService
    {
        private readonly MisCanchasDbContext misCanchasDbContext;
        private readonly IFieldService fieldService;

        public TurnService(MisCanchasDbContext misCanchasDbContext, IFieldService fieldService)
        {
            this.misCanchasDbContext = misCanchasDbContext;
            this.fieldService = fieldService;
        }
        public async Task Add(DateTime dateTime, int id, decimal price)
        {
            var turn = new Turn()
            {
                TurnDateTime = dateTime,
                ClientId = id,
                Price = price
            };
            //var duplicateTurn = await misCanchasDbContext.Turns.Find();
            //var duplicateTurn = await misCanchasDbContext.Turns.FirstOrDefaultAsync(t => t.TurnDateTime == addTurnViewModel.TurnDateTime);
            await misCanchasDbContext.AddAsync(turn);
            await misCanchasDbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var turn = await misCanchasDbContext.Turns.FindAsync(id);
            if (turn != null)
            {
                misCanchasDbContext.Turns.Remove(turn);
                await misCanchasDbContext.SaveChangesAsync();
            }
        }

        public async Task<Turn> Get(int id)
        {
            var turn = await misCanchasDbContext.Turns.FirstOrDefaultAsync(t => t.TurnId == id);
            return turn;
        }

        public async Task<IQueryable<Turn>> GetByDateRange(DateTime startDateTime, DateTime endDateTime)
        {
            var turns = await misCanchasDbContext.Turns.Where(t => t.TurnDateTime >= startDateTime && t.TurnDateTime <= endDateTime).ToListAsync();
            var turnsq = turns.AsQueryable();
            return turnsq;
        }

        public async Task<IQueryable<Turn>> GetSingleTurnByDate(DateTime dateTime)
        {
            dateTime = dateTime.AddHours(-3); //UTC-3
            var turn = await misCanchasDbContext.Turns.Where(t => t.TurnDateTime.Date == dateTime.Date && t.TurnDateTime.Hour == dateTime.Hour).ToListAsync();
            var turnsq = turn.AsQueryable();
            return turnsq;
        }

        public async Task<IQueryable<Turn>> GetTurns()
        {
            var allTurns = await misCanchasDbContext.Turns.ToListAsync();
            var turnsq = allTurns.AsQueryable();
            return turnsq;
        }

        ////Turn Selector dbcontext
        //private Task TurnSelector(string i)
        //{
        //    return i switch
        //    {
        //        "1" => misCanchasDbContext.Turns,
        //        "2" => misCanchasDbContext.Turns2,
        //        "3" => misCanchasDbContext.Turns3,
        //        _ => null
        //    };
        //}
    }
}
