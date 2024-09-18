using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class ConsultaImagen
    {
        public ConsultaImagen()
        {
            Consulta = new HashSet<Consultum>();
            Imagen = new Imagen();
        }

        public int IdConsultaImagen { get; set; }
        public int? ImagenId { get; set; }
        public int? ConsultaImagenId { get; set; }
        public string? ObservacionImagen { get; set; }
        public int? CantidadImagen { get; set; }
        public int? SecuencialImagen { get; set; }
        public int EstadoImagen { get; set; }

        public virtual Imagen? Imagen { get; set; }
        public virtual ICollection<Consultum> Consulta { get; set; }
    }
}
