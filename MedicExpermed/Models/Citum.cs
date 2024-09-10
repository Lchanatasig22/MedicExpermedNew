using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class Citum
    {
        public Citum()
        {
            Facturacions = new HashSet<Facturacion>();
        }

        public int IdCita { get; set; }
        public DateTime? FechacreacionCita { get; set; }
        public string? UsuariocreacionCita { get; set; }
        public DateTime? FechadelacitaCita { get; set; }
        public TimeSpan? HoradelacitaCita { get; set; }
        public int? UsuarioId { get; set; }
        public int? PacienteId { get; set; }
        public int? ConsultaId { get; set; }
        public string? Motivo { get; set; }
        public int? EstadoCita { get; set; }

        public virtual Consultum? Consulta { get; set; }
        public virtual Paciente? Paciente { get; set; }
        public virtual Usuario? Usuario { get; set; }
        public virtual ICollection<Facturacion> Facturacions { get; set; }
    }
}
