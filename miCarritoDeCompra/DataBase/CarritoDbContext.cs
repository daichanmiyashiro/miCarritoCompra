using miCarritoDeCompra.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace miCarritoDeCompra.DataBase
{
    public class CarritoDbContext: DbContext
    {
        //CONSTRUCTOR 
        /*CADA VEZ QUE SE CREA UN ALUMNODBCONTEXT, SE LE VAN A PASAR OPCIONES 
        LAS OPCIONES NOS VAN A DECIR A QUE BASE DE DATO TIENE QUE IR 
        -VAMOS A USAR UN MOTOR DE BASE DE DATO : SQLITE 
        Y EL PREOGRAMA ES DB BROWSER SQLITE */

        public CarritoDbContext(DbContextOptions<CarritoDbContext> opciones) : base(opciones) 
        {

        }

        // TABLAS 
        public DbSet<Carrito> Carritos { get; set; }
        public DbSet<CarritoItem> CarritoItems { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<Sucursal> Sucursal { get; set; }
        public DbSet<Administrador> Administradores { get; set; }

    }
}
