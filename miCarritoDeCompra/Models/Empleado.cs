using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace miCarritoDeCompra.Models
{
    public class Empleado : Usuario
    {
        public override Rol Rol => Rol.Empleado;
    }
}
