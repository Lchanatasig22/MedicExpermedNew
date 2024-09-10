using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class Localidad
    {
        public Localidad()
        {
            Pacientes = new HashSet<Paciente>();
        }

        public int IdLocalidad { get; set; }
        public DateTime? FechacreacionLocalidad { get; set; }
        public string? UsuariocreacionLocalidad { get; set; }
        public DateTime? FechamodificacionLocalidad { get; set; }
        public string? UsuariomodificacionLocalidad { get; set; }
        public string? NombreLocalidad { get; set; }
        public string? GentilicioLocalidad { get; set; }
        public string? PrefijoLocalidad { get; set; }
        public string? CodigoLocalidad { get; set; }
        public string? IsoLocalidad { get; set; }
        public string? IsoadLocalidad { get; set; }
        public string? CiaLocalidad { get; set; }
        public int EstadoLocalidad { get; set; }
        public int? PaisId { get; set; }

        public virtual Pai? Pais { get; set; }
        public virtual ICollection<Paciente> Pacientes { get; set; }
    }
}
