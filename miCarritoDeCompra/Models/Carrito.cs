using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;// validaciones 
using System.ComponentModel.DataAnnotations.Schema; // para las fk 
namespace miCarritoDeCompra.Models
{
    public class Carrito
    {   
        // key: Identificador unico Guid(tipo) 
        [Key]
        public Guid Id { get; set; }

        public bool Activo { get; set; }


        [ForeignKey(nameof(Cliente))] // fk del tipo cliente 
        public Guid ClienteId { get; set; }// ClienteId ? para saber a que alumno especifico nos referimos
        public Cliente Cliente { get; set; }//Propiedad navegacional 

        public List<CarritoItem> CarritosItems { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(0, 100000000, ErrorMessage = "El {0} debe estar entre {1} y {2} ")]
        public decimal Subtotal { get; set; }
    }
}

/*Foreignkey solo en un lado tiene q estar. Lo que tiene q estar en los dos es contacto contacto y alumno alumno , lo que nos permite desde un contacto ver un alumno y desde un alumno ver un contacto
Relación uno a uno 

Entity framework lo que me permite es desde una alumno por mas que en la tabla de bd no vea el contacto, me permite desde el código ver contacto.
Una instancia de alumno va a tener un objecto contacto que va a tener todos los datos de contacto.
ENTITY FRAMEWORDK LO QUE HACES ES  BUSCAR EN LA TABLA CONTACTO UNA TABLA DE ALUMNOID (Q ES DE TIPO ALUMNO) TRAEME UNA TABLA DE ALUMNO QUE TENGA ESE ID.
*/