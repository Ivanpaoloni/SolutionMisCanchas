using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MisCanchas.Contracts.Services;
using MisCanchas.Data;
using MisCanchas.Domain.Entities;

namespace MisCanchas.Services
{
    public class ReportService : IReportService
    {
        private readonly MisCanchasDbContext misCanchasDbContext;
        public ReportService(MisCanchasDbContext misCanchasDbContext)
        {
            this.misCanchasDbContext = misCanchasDbContext;
        }

    }
}
