using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Cita = new HashSet<Citum>();
            Consulta = new HashSet<Consultum>();
        }

        public int IdUsuario { get; set; }
        public int CiUsuario { get; set; }
        public string NombresUsuario { get; set; } = null!;
        public string ApellidosUsuario { get; set; } = null!;
        public string TelefonoUsuario { get; set; } = null!;
        public string EmailUsuario { get; set; } = null!;
        public DateTime? FechacreacionUsuario { get; set; }
        public DateTime? FechamodificacionUsuario { get; set; }
        public string DireccionUsuario { get; set; } = null!;
        public byte[]? FirmadigitalUsuario { get; set; }
        public byte[]? CodigoqrUsuario { get; set; }
        public string? CodigoSenecyt { get; set; }
        public string LoginUsuario { get; set; } = null!;
        public string ClaveUsuario { get; set; } = null!;
        public string? CodigoUsuario { get; set; }
        public int EstadoUsuario { get; set; }
        public int? PerfilId { get; set; }
        public int? EstablecimientoId { get; set; }
        public int? EspecialidadId { get; set; }

        public virtual Especialidad? Especialidad { get; set; }
        public virtual Establecimiento? Establecimiento { get; set; }
        public virtual Perfil? Perfil { get; set; }
        public virtual ICollection<Citum> Cita { get; set; }
        public virtual ICollection<Consultum> Consulta { get; set; }
    }
}
