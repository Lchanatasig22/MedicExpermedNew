using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class Laboratorio
    {
        public Laboratorio()
        {
            ConsultaLaboratorios = new HashSet<ConsultaLaboratorio>();
        }

        public int IdLaboratorio { get; set; }
        public string NombreLaboratorio { get; set; } = null!;
        public string? DescripcionLaboratorio { get; set; }
        public string? CategoriaLaboratorios { get; set; }
        public string UuidLaboratorios { get; set; } = null!;
        public int EstadoLaboratorios { get; set; }

        public virtual ICollection<ConsultaLaboratorio> ConsultaLaboratorios { get; set; }
    }
}
