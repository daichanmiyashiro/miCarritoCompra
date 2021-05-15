using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace miCarritoDeCompra.Models
{   
    // Administrador hereda de Usuario
    public class Administrador: Usuario
    {
        public override Rol Rol => Rol.Administrador;

    }
}
