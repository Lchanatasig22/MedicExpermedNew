using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class Diagnostico
    {
        public Diagnostico()
        {
            ConsultaDiagnosticos = new HashSet<ConsultaDiagnostico>();
        }

        public int IdDiagnostico { get; set; }
        public string NombreDiagnostico { get; set; } = null!;
        public string? DescripcionDiagnostico { get; set; }
        public string? CategoriaDiagnostico { get; set; }
        public string UuidDiagnostico { get; set; } = null!;
        public int EstadoDiagnostico { get; set; }

        public virtual ICollection<ConsultaDiagnostico> ConsultaDiagnosticos { get; set; }
    }
}
