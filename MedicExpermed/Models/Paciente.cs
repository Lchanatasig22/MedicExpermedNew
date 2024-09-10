using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class Paciente
    {
        public Paciente()
        {
            Cita = new HashSet<Citum>();
            Consulta = new HashSet<Consultum>();
        }

        public int IdPacientes { get; set; }
        public DateTime? FechacreacionPacientes { get; set; }
        public string UsuariocreacionPacientes { get; set; } = null!;
        public DateTime? FechamodificacionPacientes { get; set; }
        public string UsuariomodificacionPacientes { get; set; } = null!;
        public int? TipodocumentoPacientesCa { get; set; }
        public int CiPacientes { get; set; }
        public string PrimernombrePacientes { get; set; } = null!;
        public string? SegundonombrePacientes { get; set; }
        public string PrimerapellidoPacientes { get; set; } = null!;
        public string? SegundoapellidoPacientes { get; set; }
        public int? SexoPacientesCa { get; set; }
        public DateTime? FechanacimientoPacientes { get; set; }
        public int? EdadPacientes { get; set; }
        public int? TiposangrePacientesCa { get; set; }
        public string? DonantePacientes { get; set; }
        public int? EstadocivilPacientesCa { get; set; }
        public int? FormacionprofesionalPacientesCa { get; set; }
        public string? TelefonofijoPacientes { get; set; }
        public string? TelefonocelularPacientes { get; set; }
        public string? EmailPacientes { get; set; }
        public int? NacionalidadPacientesPa { get; set; }
        public int? ProvinciaPacientesL { get; set; }
        public string? DireccionPacientes { get; set; }
        public string? OcupacionPacientes { get; set; }
        public string? EmpresaPacientes { get; set; }
        public int? SegurosaludPacientesCa { get; set; }
        public int EstadoPacientes { get; set; }

        public virtual Catalogo? EstadocivilPacientesCaNavigation { get; set; }
        public virtual Catalogo? FormacionprofesionalPacientesCaNavigation { get; set; }
        public virtual Pai? NacionalidadPacientesPaNavigation { get; set; }
        public virtual Localidad? ProvinciaPacientesLNavigation { get; set; }
        public virtual Catalogo? SegurosaludPacientesCaNavigation { get; set; }
        public virtual Catalogo? SexoPacientesCaNavigation { get; set; }
        public virtual Catalogo? TipodocumentoPacientesCaNavigation { get; set; }
        public virtual Catalogo? TiposangrePacientesCaNavigation { get; set; }
        public virtual ICollection<Citum> Cita { get; set; }
        public virtual ICollection<Consultum> Consulta { get; set; }
    }
}
