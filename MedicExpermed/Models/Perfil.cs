using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class Perfil
    {
        public Perfil()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int IdPerfil { get; set; }
        public string NombrePerfil { get; set; } = null!;
        public string DescripcionPerfil { get; set; } = null!;
        public DateTime FechacreacionPerfil { get; set; }
        public int EstadoPerfil { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
