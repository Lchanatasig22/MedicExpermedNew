using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class Imagen
    {
        public Imagen()
        {
            ConsultaImagens = new HashSet<ConsultaImagen>();
        }

        public int IdImagen { get; set; }
        public string NombreImagen { get; set; } = null!;
        public string? DescripcionImagen { get; set; }
        public string? CategoriaImagen { get; set; }
        public string UuidImagen { get; set; } = null!;
        public int EstadoImagen { get; set; }

        public virtual ICollection<ConsultaImagen> ConsultaImagens { get; set; }
    }
}
