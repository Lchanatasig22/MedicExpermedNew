using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace MedicExpermed.Models
{
    public class Consultation
    {
        public int Id { get; set; } // Identificador único de la consulta
        public DateTime FechaCreacion { get; set; } // Fecha de creación de la consulta
        public string UsuarioCreacion { get; set; } // Usuario que crea la consulta
        public string HistorialConsulta { get; set; } // Historial médico del paciente
        public int? SecuencialConsulta { get; set; } // Número secuencial de la consulta
        public int PacienteId { get; set; } // Id del paciente relacionado
        public string MotivoConsulta { get; set; } // Motivo de la consulta
        public string EnfermedadConsulta { get; set; } // Descripción de la enfermedad
        public string NombrePariente { get; set; } // Nombre del pariente de contacto
        public string SignosAlarma { get; set; } // Signos de alarma que presenta el paciente
        public string ReconocimientoFarmacologico { get; set; } // Medicamentos conocidos o reconocidos
        public int TipoPariente { get; set; } // Tipo de pariente
        public string TelefonoPariente { get; set; } // Teléfono del pariente
        public string Temperatura { get; set; } // Temperatura del paciente
        public string FrecuenciaRespiratoria { get; set; } // Frecuencia respiratoria
        public string PresionArterialSistolica { get; set; } // Presión arterial sistólica
        public string PresionArterialDiastolica { get; set; } // Presión arterial diastólica
        public string Pulso { get; set; } // Pulso del paciente
        public string Peso { get; set; } // Peso del paciente
        public string Talla { get; set; } // Talla del paciente
        public string PlanTratamiento { get; set; } // Plan de tratamiento
        public string? Observacion { get; set; } // Observaciones generales de la consulta
        public string AntecedentesPersonales { get; set; } // Antecedentes personales del paciente
        public int DiasIncapacidad { get; set; } // Días de incapacidad
        public int MedicoId { get; set; } // Id del médico que realiza la consulta
        public int EspecialidadId { get; set; } // Id de la especialidad médica
        public int? TipoConsultaId { get; set; } = null; // Tipo de consulta
        public string NotasEvolucion { get; set; } // Notas de evolución del paciente
        public string ConsultaPrincipal { get; set; } // Consulta principal de la visita
        public int EstadoConsulta { get; set; } // Estado de la consulta

        // Relaciones con otras tablas
        public List<ConsultaAlergiaDTO> Alergias { get; set; } // Lista de alergias asociadas a la consulta
        public List<ConsultaCirugiaDTO> Cirugias { get; set; } // Lista de cirugías asociadas a la consulta
        public List<ConsultaMedicamentoDTO> Medicamentos { get; set; } // Lista de medicamentos asociados
        public List<ConsultaLaboratorioDTO> Laboratorios { get; set; } // Lista de laboratorios asociadosss
        public List<ConsultaImagenDTO> Imagenes { get; set; } // Lista de imágenes asociadas
        public List<ConsultaDiagnosticoDTO> Diagnosticos { get; set; } // Lista de diagnósticos asociados
        public OrganosSistema OrganosSistemas { get; set; } // Órganos y sistemas asociados
        public ExamenFisico ExamenFisico { get; set; } // Examen físico asociado
        public AntecedentesFamiliare AntecedentesFamiliares { get; set; } // Antecedentes familiares asociados

        public virtual Usuario? MedicoConsultaDNavigation { get; set; }
        public virtual Paciente? PacienteConsultaPNavigation { get; set; }
        public virtual Catalogo? EstadocivilPacientesCaNavigation { get; set; }
        public virtual Catalogo? FormacionprofesionalPacientesCaNavigation { get; set; }
        public virtual Pai? NacionalidadPacientesPaNavigation { get; set; }
        public virtual Localidad? ProvinciaPacientesLNavigation { get; set; }
        public virtual Catalogo? SegurosaludPacientesCaNavigation { get; set; }
        public virtual Catalogo? SexoPacientesCaNavigation { get; set; }
        public virtual Catalogo? TipodocumentoPacientesCaNavigation { get; set; }
        public virtual Catalogo? TiposangrePacientesCaNavigation { get; set; }


        // Constructor que inicializa los objetos complejos
        public Consultation()
        {
            AntecedentesFamiliares = new AntecedentesFamiliare();

            Alergias = new List<ConsultaAlergiaDTO>();
            Cirugias = new List<ConsultaCirugiaDTO>();
            Medicamentos = new List<ConsultaMedicamentoDTO>();
            Laboratorios = new List<ConsultaLaboratorioDTO>();
            Imagenes = new List<ConsultaImagenDTO>();
            Diagnosticos = new List<ConsultaDiagnosticoDTO>();
            OrganosSistemas = new OrganosSistema();
            ExamenFisico = new ExamenFisico();
   
        }
    }

    public class ConsultaAlergiaDTO
    {
        public int? CatalogoalergiaId { get; set; } // Solo envías el ID
        public string? ObservacionAlergias { get; set; }
        public int EstadoAlergias { get; set; } = 1; // Valor predeterminado
    }
    public class ConsultaCirugiaDTO
    {
        public int? CatalogocirugiaId { get; set; } // Solo envías el ID
        public string? ObservacionCirugia { get; set; }
        public int EstadoCirugias { get; set; } = 1; // Valor predeterminado
    }
    public class ConsultaImagenDTO
    {
        public int? ImagenId { get; set; }
        public string? ObservacionImagen { get; set; }

        public int? CantidadImagen { get; set; }
        public int? SecuencialImagen { get; set; }

        public int EstadoImagen { get; set; }
    }
    public class ConsultaLaboratorioDTO
    {
        public int? CatalogoLaboratorioId { get; set; }
       
        public int? CantidadLaboratorio { get; set; }
        public string? Observacion { get; set; }
        public int? SecuencialLaboratorio { get; set; }

        public int EstadoLaboratorio { get; set; }
    }
    public class ConsultaMedicamentoDTO
    {
        public int? MedicamentoId { get; set; }
        public string? DosisMedicamento { get; set; }
        public string? ObservacionMedicamento { get; set; }
        public int? SecuencialMedicamento { get; set; }

        public int EstadoMedicamento { get; set; }
    }

    public class ConsultaDiagnosticoDTO
    {
        public int? DiagnosticoId { get; set; }
        public string? ObservacionDiagnostico { get; set; }
        public bool? PresuntivoDiagnosticos { get; set; }
        public bool? DefinitivoDiagnosticos { get; set; }
        public int? SecuencialDiagnostico { get; set; }

        public int EstadoDiagnostico { get; set; }
    }

}
