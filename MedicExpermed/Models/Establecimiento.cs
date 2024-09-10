using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class Establecimiento
    {
        public Establecimiento()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int IdEstablecimiento { get; set; }
        public DateTime? FechacreacionEstablecimiento { get; set; }
        public DateTime? FechamodificacionEstablecimiento { get; set; }
        public string? DescripcionEstablecimiento { get; set; }
        public string? DireccionEstablecimiento { get; set; }
        public string? CiudadEstablecimiento { get; set; }
        public string? ProvinciaEstablecimiento { get; set; }
        public int EstadoEstablecimiento { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
