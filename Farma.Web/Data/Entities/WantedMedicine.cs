﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Farma.Web.Data.Entities
{
    public class WantedMedicine: IEntity
    {
        public int Id { get; set; }

        // Relacion con tabla Medicinas
        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }
        //---------------------------------------------

        // Relacion con tabla Usuarios
        public string UserId { get; set; }
        //public User User { get; set; }
        //---------------------------------------------
    }
}
