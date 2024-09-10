using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class ExamenFisico
    {
        public ExamenFisico()
        {
            Consulta = new HashSet<Consultum>();
        }

        public int IdExamenFisico { get; set; }
        public bool? Cabeza { get; set; }
        public string? ObserCabeza { get; set; }
        public bool? Cuello { get; set; }
        public string? ObserCuello { get; set; }
        public bool? Torax { get; set; }
        public string? ObserTorax { get; set; }
        public bool? Abdomen { get; set; }
        public string? ObserAbdomen { get; set; }
        public bool? Pelvis { get; set; }
        public string? ObserPelvis { get; set; }
        public bool? Extremidades { get; set; }
        public string? ObserExtremidades { get; set; }

        public virtual ICollection<Consultum> Consulta { get; set; }
    }
}
