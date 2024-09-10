using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class Facturacion
    {
        public int IdFacturacion { get; set; }
        public int? CitaId { get; set; }
        public DateTime FechaFacturacion { get; set; }
        public decimal TotalFactura { get; set; }
        public string MetodoPago { get; set; } = null!;
        public string EstadoFactura { get; set; } = null!;

        public virtual Citum? Cita { get; set; }
    }
}
