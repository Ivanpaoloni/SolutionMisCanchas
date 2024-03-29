﻿using Microsoft.EntityFrameworkCore;
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
        public async Task Update(int openHour, int closeHour, string name, decimal price)
        {
            var field = await Get();
            if (field != null)
            {
                field.OpenHour = openHour;
                field.CloseHour = closeHour;
                field.Name = name;
                field.Price = price;
                misCanchasDbContext.Fields.Update(field);
                await misCanchasDbContext.SaveChangesAsync();
            }
        }
    }
}
