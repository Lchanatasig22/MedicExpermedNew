using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class ConsultaMedicamento
    {
        public ConsultaMedicamento()
        {
            Consulta = new HashSet<Consultum>();
        }

        public int IdConsultaMedicamento { get; set; }
        public DateTime? FechacreacionMedicamento { get; set; }
        public int? MedicamentoId { get; set; }
        public int? ConsultaMedicamentosId { get; set; }
        public string? DosisMedicamento { get; set; }
        public string? ObservacionMedicamento { get; set; }
        public int? SecuencialMedicamento { get; set; }
        public int EstadoMedicamento { get; set; }

        public virtual Medicamento? Medicamento { get; set; }
        public virtual ICollection<Consultum> Consulta { get; set; }
    }
}
