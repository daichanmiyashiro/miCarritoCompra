using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using miCarritoDeCompra.DataBase;
using miCarritoDeCompra.Models;
using Microsoft.AspNetCore.Authorization;

namespace miCarritoDeCompra.Controllers
{
    public class ClientesController : Controller
    {
        private readonly CarritoDbContext _context;

        public ClientesController(CarritoDbContext context)
        {
            _context = context;
        }



        //SOLO AUTORIZADOS LOS EMPLEADOS Y ADM PARA VER LA LISTA DE CLIENTES
        [Authorize(Roles = nameof(Rol.Administrador))]
        [Authorize(Roles = nameof(Rol.Empleado))]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }



        [Authorize]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }




        // CLIENTES PUEDEN AUTOREGISTRARSE
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        

        // 1- NO PUEDEN TENER MISMO NOMBRE DE USUARIO 
        // 2- NO PUEDEN TENER MISMO MAIL
        // 3- PREGUTNAR POR LA PASSWORD?                                                   --------consulta ---------

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Cliente cliente)
        {   
            if (_context.Clientes.Any(c => c.Username == cliente.Username))
            {
                ModelState.AddModelError(nameof(cliente.Username), "El Nombre de Usuario ya existe");
            }

            if (_context.Clientes.Any(c => c.Email == cliente.Email))
            {
                ModelState.AddModelError(nameof(cliente.Email), "El Email ya existe");
            }


            // SI LOS ATRIBUTOS REQUERIDOS SON CORRECTOS Y SI NO TIENEN NINGUN MODEL ERROR AGREGADO =TRUE
            if (ModelState.IsValid)
            {
                cliente.Id = Guid.NewGuid();
                cliente.FechaAlta = DateTime.Now; 
                _context.Add(cliente);
                

                //El carrito se crea automáticamente en estado activo con la creación de un cliente
                //(todo cliente tendrá un carrito en estado activo cuando éste sea creado).

                Carrito carrito = new Carrito()// id - activo - clienteId - subtotal
                {
                    Id = Guid.NewGuid(),
                    Activo = true,
                    ClienteId = cliente.Id,
                    Subtotal = 0
                };
                _context.Add(carrito);

                await _context.SaveChangesAsync(); // guardames los cambios 
                return RedirectToAction(nameof(Index));

            }
            return View(cliente);
        }



        //El cliente puede actualizar sus datos de contacto tales como el teléfono, dirección, etc.,
        //pero no puede modificar sus datos básicos tales como DNI, Nombre, Apellido, etc.
        [Authorize(Roles = nameof(Rol.Administrador))]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }




        [Authorize(Roles = nameof(Rol.Administrador))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Cliente cliente)
        {   
            // 1- No pueden haber dos mail iguales 
            // busco en la BD -tabla de clientes el cliente que tenga igual mail que el mail del cliente q me pasan por parametro
            //si el mail es igual && si el nombre de los usuarios son diferentes- agregame el error al modelstate
            var auxCliente = _context.Clientes.FirstOrDefaultAsync(e => e.Email == cliente.Email).Result;
            if (_context.Clientes.Any(e => e.Email == cliente.Email) && auxCliente.Username != cliente.Username)
            {
                ModelState.AddModelError(nameof(cliente.Email), "El Email ya existe");
            }

            if (id != cliente.Id)
            {
                return NotFound();
            }
            // CONSULTA EN CASO DE QUERER QUE SOLO MODIFIQUE SOLO ALGUNOS DATOS?                              ------------CONSULTA ---------
            if (ModelState.IsValid)
            {
                try
                {
                    var clienteDatabase = _context.Clientes.Find(id);

                    clienteDatabase.Dni = cliente.Dni;
                    clienteDatabase.Nombre = cliente.Nombre;
                    clienteDatabase.Apellido = cliente.Apellido;
                    clienteDatabase.Telefono = cliente.Telefono;
                    clienteDatabase.Direccion = cliente.Direccion;
                    clienteDatabase.Email = cliente.Email;

                    await _context.SaveChangesAsync();

                    TempData["EditIn"] = true;
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        // EDIT POR PARTE DEL CLIENTE 
        [Authorize(Roles = nameof(Rol.Cliente))]
        [HttpGet]
        public IActionResult Editarme()
        {   
            // EL NOMBRE DEL USUARIO QUE ESTA LOGUEADO GUARDALO EN UNA VARIABLE
            //CONSULTAR EN LA BD- TRAEME EL CLIENTE CON ESE NOMBRE DE USUARIO

            var username = User.Identity.Name;
            var cliente = _context.Clientes.FirstOrDefault(c => c.Username == username);

            return View(cliente);
        }

        [Authorize(Roles = nameof(Rol.Cliente))]
        [HttpPost]
        public IActionResult Editarme(Cliente cliente)
        {
           
            var auxCliente = _context.Clientes.FirstOrDefaultAsync(e => e.Email == cliente.Email).Result;
            if (_context.Clientes.Any(e => e.Email == cliente.Email) && auxCliente.Username != cliente.Username)
            {
                ModelState.AddModelError(nameof(cliente.Email), "El Email ya existe");
            }

            if (ModelState.IsValid)
            {
                var username = User.Identity.Name;
                var clienteDatabase = _context.Clientes.FirstOrDefault(c => c.Username == username);

                clienteDatabase.Telefono = cliente.Telefono;
                clienteDatabase.Direccion = cliente.Direccion;
                clienteDatabase.Email = cliente.Email;

                _context.SaveChanges();

                return RedirectToAction(nameof(Details), new { cliente.Id });
            }

            return View(cliente);
        }



        [Authorize(Roles = nameof(Rol.Administrador))]
        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }




        [Authorize(Roles = nameof(Rol.Administrador))]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(Guid id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }
    }
}
