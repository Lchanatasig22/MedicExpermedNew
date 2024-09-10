using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class Catalogo
    {
        public Catalogo()
        {
            AntecedentesFamiliareParentescocatalogoCancerNavigations = new HashSet<AntecedentesFamiliare>();
            AntecedentesFamiliareParentescocatalogoCardiopatiaNavigations = new HashSet<AntecedentesFamiliare>();
            AntecedentesFamiliareParentescocatalogoDiabetesNavigations = new HashSet<AntecedentesFamiliare>();
            AntecedentesFamiliareParentescocatalogoEnfcardiovascularNavigations = new HashSet<AntecedentesFamiliare>();
            AntecedentesFamiliareParentescocatalogoEnfinfecciosaNavigations = new HashSet<AntecedentesFamiliare>();
            AntecedentesFamiliareParentescocatalogoEnfmentalNavigations = new HashSet<AntecedentesFamiliare>();
            AntecedentesFamiliareParentescocatalogoHipertensionNavigations = new HashSet<AntecedentesFamiliare>();
            AntecedentesFamiliareParentescocatalogoMalformacionNavigations = new HashSet<AntecedentesFamiliare>();
            AntecedentesFamiliareParentescocatalogoOtroNavigations = new HashSet<AntecedentesFamiliare>();
            AntecedentesFamiliareParentescocatalogoTuberculosisNavigations = new HashSet<AntecedentesFamiliare>();
            ConsultaAlergia = new HashSet<ConsultaAlergia>();
            ConsultaCirugia = new HashSet<ConsultaCirugia>();
            PacienteEstadocivilPacientesCaNavigations = new HashSet<Paciente>();
            PacienteFormacionprofesionalPacientesCaNavigations = new HashSet<Paciente>();
            PacienteSegurosaludPacientesCaNavigations = new HashSet<Paciente>();
            PacienteSexoPacientesCaNavigations = new HashSet<Paciente>();
            PacienteTipodocumentoPacientesCaNavigations = new HashSet<Paciente>();
            PacienteTiposangrePacientesCaNavigations = new HashSet<Paciente>();
        }

        public int IdCatalogo { get; set; }
        public DateTime? FechacreacionCatalogo { get; set; }
        public string? UsuariocreacionCatalogo { get; set; }
        public DateTime? FechamodificacionCatalogo { get; set; }
        public string? UsuariomodificacionCatalogo { get; set; }
        public string? DescripcionCatalogo { get; set; }
        public string? CategoriaCatalogo { get; set; }
        public Guid UuidCatalogo { get; set; }
        public int EstadoCatalogo { get; set; }

        public virtual ICollection<AntecedentesFamiliare> AntecedentesFamiliareParentescocatalogoCancerNavigations { get; set; }
        public virtual ICollection<AntecedentesFamiliare> AntecedentesFamiliareParentescocatalogoCardiopatiaNavigations { get; set; }
        public virtual ICollection<AntecedentesFamiliare> AntecedentesFamiliareParentescocatalogoDiabetesNavigations { get; set; }
        public virtual ICollection<AntecedentesFamiliare> AntecedentesFamiliareParentescocatalogoEnfcardiovascularNavigations { get; set; }
        public virtual ICollection<AntecedentesFamiliare> AntecedentesFamiliareParentescocatalogoEnfinfecciosaNavigations { get; set; }
        public virtual ICollection<AntecedentesFamiliare> AntecedentesFamiliareParentescocatalogoEnfmentalNavigations { get; set; }
        public virtual ICollection<AntecedentesFamiliare> AntecedentesFamiliareParentescocatalogoHipertensionNavigations { get; set; }
        public virtual ICollection<AntecedentesFamiliare> AntecedentesFamiliareParentescocatalogoMalformacionNavigations { get; set; }
        public virtual ICollection<AntecedentesFamiliare> AntecedentesFamiliareParentescocatalogoOtroNavigations { get; set; }
        public virtual ICollection<AntecedentesFamiliare> AntecedentesFamiliareParentescocatalogoTuberculosisNavigations { get; set; }
        public virtual ICollection<ConsultaAlergia> ConsultaAlergia { get; set; }
        public virtual ICollection<ConsultaCirugia> ConsultaCirugia { get; set; }
        public virtual ICollection<Paciente> PacienteEstadocivilPacientesCaNavigations { get; set; }
        public virtual ICollection<Paciente> PacienteFormacionprofesionalPacientesCaNavigations { get; set; }
        public virtual ICollection<Paciente> PacienteSegurosaludPacientesCaNavigations { get; set; }
        public virtual ICollection<Paciente> PacienteSexoPacientesCaNavigations { get; set; }
        public virtual ICollection<Paciente> PacienteTipodocumentoPacientesCaNavigations { get; set; }
        public virtual ICollection<Paciente> PacienteTiposangrePacientesCaNavigations { get; set; }
    }
}
