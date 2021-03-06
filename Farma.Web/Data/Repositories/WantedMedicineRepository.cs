﻿using Farma.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Farma.Web.Data.Repositories
{
    public class WantedMedicineRepository:GenericRepository<WantedMedicine>,IWantedMedicineRepository
    {
        private readonly DataContext context;
        
        public WantedMedicineRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<WantedMedicine> GetWantedByUserId(string uid)
        {
                var d = this.context.WantedMedicines
                .Include(m => m.Medicine)
                .Where(x => x.UserId == uid);

            return d;
        }
    }
}
