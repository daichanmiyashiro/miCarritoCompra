using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;// validaciones 
using System.ComponentModel.DataAnnotations.Schema; // para las fk 

namespace miCarritoDeCompra.Models
{
    public class Compra
    {
        [Key]
        public Guid Id { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(0, 100000000, ErrorMessage = "El {0} debe estar entre {1} y {2} ")]
        public decimal Total { get; set; }


        [ForeignKey(nameof(Cliente))]
        public Guid ClienteId { get; set; }
        public Cliente Cliente { get; set; }


        [ForeignKey(nameof(Carrito))]
        public Guid CarritoId { get; set; }
        public Carrito Carrito { get; set; }
    }
}
