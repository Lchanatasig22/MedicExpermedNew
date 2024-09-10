using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class ConsultaLaboratorio
    {
        public ConsultaLaboratorio()
        {
            Consulta = new HashSet<Consultum>();
        }

        public int IdLaboratorioConsulta { get; set; }
        public int? CantidadLaboratorio { get; set; }
        public int? ConsultaLaboratorioId { get; set; }
        public string? Observacion { get; set; }
        public int? CatalogoLaboratorioId { get; set; }
        public int? SecuencialLaboratorio { get; set; }
        public int EstadoLaboratorio { get; set; }

        public virtual Laboratorio? CatalogoLaboratorio { get; set; }
        public virtual ICollection<Consultum> Consulta { get; set; }
    }
}
