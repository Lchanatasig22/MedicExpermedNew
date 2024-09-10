using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class Especialidad
    {
        public Especialidad()
        {
            Consulta = new HashSet<Consultum>();
            Usuarios = new HashSet<Usuario>();
        }

        public int IdEspecialidad { get; set; }
        public string NombreEspecialidad { get; set; } = null!;
        public string? DescripcionEspecialidad { get; set; }
        public string? CategoriaEspecialidad { get; set; }
        public Guid UuidEspecialidad { get; set; }
        public int EstadoEspecialidad { get; set; }

        public virtual ICollection<Consultum> Consulta { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
