using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class Medicamento
    {
        public Medicamento()
        {
            ConsultaMedicamentos = new HashSet<ConsultaMedicamento>();
        }

        public int IdMedicamento { get; set; }
        public string NombreMedicamento { get; set; } = null!;
        public string? DescripcionMedicamento { get; set; }
        public string? CategoriaMedicamento { get; set; }
        public string? DistintivoMedicamento { get; set; }
        public string? ConcentracionMedicamento { get; set; }
        public string UuidMedicamento { get; set; } = null!;
        public int EstadoMedicamento { get; set; }

        public virtual ICollection<ConsultaMedicamento> ConsultaMedicamentos { get; set; }
    }
}
