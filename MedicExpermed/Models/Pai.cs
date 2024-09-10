using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class Pai
    {
        public Pai()
        {
            Localidads = new HashSet<Localidad>();
            Pacientes = new HashSet<Paciente>();
        }

        public int IdPais { get; set; }
        public string NombrePais { get; set; } = null!;
        public string IsoPais { get; set; } = null!;
        public string CodigoPais { get; set; } = null!;
        public string GentilicioPais { get; set; } = null!;
        public int EstadoPais { get; set; }

        public virtual ICollection<Localidad> Localidads { get; set; }
        public virtual ICollection<Paciente> Pacientes { get; set; }
    }
}
