using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class Consultum
    {
        public Consultum()
        {
            Cita = new HashSet<Citum>();
            ConsultaAntecedentesFamiliares = new AntecedentesFamiliare();
            ConsultaOrganosSistemas = new OrganosSistema();
            ConsultaExamenFisico = new ExamenFisico();
            ConsultaDiagnostico = new ConsultaDiagnostico();
            Especialidad = new Especialidad();
            PacienteConsultaPNavigation = new Paciente();
            MedicoConsultaDNavigation = new Usuario();
            ConsultaAlergiasIntNavigation = new ConsultaAlergia();
            ConsultaMedicamentos = new ConsultaMedicamento();



        }

        public int IdConsulta { get; set; }
        public DateTime? FechacreacionConsulta { get; set; }
        public string? UsuariocreacionConsulta { get; set; }
        public string HistorialConsulta { get; set; } = null!;
        public int? SecuencialConsulta { get; set; }
        public int? PacienteConsultaP { get; set; }
        public string? MotivoConsulta { get; set; }
        public string? EnfermedadConsulta { get; set; }
        public string? NombreparienteConsulta { get; set; }
        public string? SignosalarmaConsulta { get; set; }
        public string? Reconofarmacologicas { get; set; }
        public int? TipoparienteConsulta { get; set; }
        public string? TelefonoParienteConsulta { get; set; }
        public string TemperaturaConsulta { get; set; } = null!;
        public string FrecuenciarespiratoriaConsulta { get; set; } = null!;
        public string PresionarterialsistolicaConsulta { get; set; } = null!;
        public string PresionarterialdiastolicaConsulta { get; set; } = null!;
        public string PulsoConsulta { get; set; } = null!;
        public string PesoConsulta { get; set; } = null!;
        public string TallaConsulta { get; set; } = null!;
        public string? PlantratamientoConsulta { get; set; }
        public string? ObservacionConsulta { get; set; }
        public string? AntecedentespersonalesConsulta { get; set; }
        public int? DiasincapacidadConsulta { get; set; }
        public int? MedicoConsultaD { get; set; }
        public int? EspecialidadId { get; set; }
        public int? TipoConsultaC { get; set; }
        public string? NotasevolucionConsulta { get; set; }
        public string? ConsultaprincipalConsulta { get; set; }
        public int EstadoConsultaC { get; set; }
        public int? ConsultaMedicamentosId { get; set; }
        public int? ConsultaLaboratorioId { get; set; }
        public int? ConsultaAntecedentesFamiliaresId { get; set; }
        public int? ConsultaOrganosSistemasId { get; set; }
        public int? ConsultaExamenFisicoId { get; set; }
        public int? ConsultaImagenId { get; set; }
        public int? ConsultaDiagnosticoId { get; set; }
        public int? ConsultaAlergiasInt { get; set; }
        public int? ConsultaCirugiasId { get; set; }

        public virtual ConsultaAlergia? ConsultaAlergiasIntNavigation { get; set; }
        public virtual AntecedentesFamiliare? ConsultaAntecedentesFamiliares { get; set; }
        public virtual ConsultaCirugia? ConsultaCirugias { get; set; }
        public virtual ConsultaDiagnostico? ConsultaDiagnostico { get; set; }
        public virtual ExamenFisico? ConsultaExamenFisico { get; set; }
        public virtual ConsultaImagen? ConsultaImagen { get; set; }
        public virtual ConsultaLaboratorio? ConsultaLaboratorio { get; set; }
        public virtual ConsultaMedicamento? ConsultaMedicamentos { get; set; }
        public virtual OrganosSistema? ConsultaOrganosSistemas { get; set; }
        public virtual Especialidad? Especialidad { get; set; }
        public virtual Usuario? MedicoConsultaDNavigation { get; set; }
        public virtual Paciente? PacienteConsultaPNavigation { get; set; }
        public virtual ICollection<Citum> Cita { get; set; }
    }
}
