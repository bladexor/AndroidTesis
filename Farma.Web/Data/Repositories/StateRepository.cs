using Farma.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farma.Web.Data.Repositories
{
    public class StateRepository : GenericRepository<State>, IStateRepository
    {
        private readonly DataContext context;

        public StateRepository(DataContext context) : base(context)
        {
            this.context = context;
        }

    }
}
