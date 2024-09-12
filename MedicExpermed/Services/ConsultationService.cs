using MedicExpermed.Controllers;
using MedicExpermed.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MedicExpermed.Services
{
    public class ConsultationService
    {
        private readonly medicossystembdIContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PatientService> _logger;

        public ConsultationService(medicossystembdIContext context, IHttpContextAccessor httpContextAccessor, ILogger<PatientService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;


        }
        /// <summary>
        /// Obtener todas las consultas
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Consultum>> GetAllConsultasAsync()
        {
            // Obtener el nombre de usuario de la sesión
            var loginUsuario = _httpContextAccessor.HttpContext.Session.GetString("UsuarioNombre");

            if (string.IsNullOrEmpty(loginUsuario))
            {
                throw new Exception("El nombre de usuario no está disponible en la sesión.");
            }

            // Filtrar las consultas por el usuario de creación y el estado igual a 0
            var consultas = await _context.Consulta
                .Where(c => c.UsuariocreacionConsulta == loginUsuario)
                .Include(c => c.ConsultaDiagnostico)
                .Include(c => c.ConsultaImagen)
                .Include(c => c.ConsultaLaboratorio)
                .Include(c => c.ConsultaMedicamentos)
                .Include(c => c.PacienteConsultaPNavigation)
                .OrderBy(c => c.FechacreacionConsulta) // Ordenar por fecha de la consulta Ocupar esto mismo para cualquier tabla 

                .ToListAsync();

            return consultas;
        }


        //
        public async Task<Consultation> ObtenerConsultaPorIdAsync(int id)
        {
            var consultum = await _context.Consulta
             .Include(c => c.PacienteConsultaPNavigation)  // Relación con la entidad Paciente
             .Include(c => c.ConsultaAlergiasIntNavigation)  // Relación con la entidad ConsultaAlergia
             .Include(c => c.ConsultaCirugias)  // Relación con la entidad ConsultaCirugia
             .Include(c => c.ConsultaMedicamentos)  // Relación con la entidad ConsultaMedicamento
             .Include(c => c.ConsultaLaboratorio)  // Relación con la entidad ConsultaLaboratorio (colección)
             .Include(c => c.ConsultaImagen)  // Relación con la entidad ConsultaImagen (colección)
             .Include(c => c.ConsultaDiagnostico)  // Relación con la entidad ConsultaDiagnostico (colección)
             .Include(c => c.ConsultaAntecedentesFamiliares)  // Relación con la entidad Antecedentes Familiares
             .Include(c => c.MedicoConsultaDNavigation)  // Relación con la entidad Antecedentes Familiares

             .FirstOrDefaultAsync(c => c.IdConsulta == id);

         


            // Verifica si la consulta fue encontrada
            if (consultum == null)
            {
                return null; // O maneja el caso de consulta no encontrada
            }

            // Mapeo manual de Consultum a Consultation
            var consultation = new Consultation
            {
                Id = consultum.IdConsulta,
                FechaCreacion = consultum.FechacreacionConsulta ?? DateTime.Now, // Usa DateTime.Now si es null
                UsuarioCreacion = consultum.UsuariocreacionConsulta,
                HistorialConsulta = consultum.HistorialConsulta,
                SecuencialConsulta = consultum.SecuencialConsulta,
                PacienteId = consultum.PacienteConsultaP ?? 0, // Usa 0 si es null
                MotivoConsulta = consultum.MotivoConsulta,
                EnfermedadConsulta = consultum.EnfermedadConsulta,
                NombrePariente = consultum.NombreparienteConsulta,
                SignosAlarma = consultum.SignosalarmaConsulta,
                ReconocimientoFarmacologico = consultum.Reconofarmacologicas,
                TipoPariente = consultum.TipoparienteConsulta ?? 0, // Usa 0 si es null
                TelefonoPariente = consultum.TelefonoParienteConsulta,
                Temperatura = consultum.TemperaturaConsulta,
                FrecuenciaRespiratoria = consultum.FrecuenciarespiratoriaConsulta,
                PresionArterialSistolica = consultum.PresionarterialsistolicaConsulta,
                PresionArterialDiastolica = consultum.PresionarterialdiastolicaConsulta,
                Pulso = consultum.PulsoConsulta,
                Peso = consultum.PesoConsulta,
                Talla = consultum.TallaConsulta,
                PlanTratamiento = consultum.PlantratamientoConsulta,
                Observacion = consultum.ObservacionConsulta,
                AntecedentesPersonales = consultum.AntecedentespersonalesConsulta,
                DiasIncapacidad = consultum.DiasincapacidadConsulta ?? 0,
                MedicoId = consultum.MedicoConsultaD ?? 0,
                EspecialidadId = consultum.EspecialidadId ?? 0,
                TipoConsultaId = consultum.TipoConsultaC ?? 0,
                NotasEvolucion = consultum.NotasevolucionConsulta,
                ConsultaPrincipal = consultum.ConsultaprincipalConsulta,
                EstadoConsulta = consultum.EstadoConsultaC,
              
            };

            // Mapea las listas relacionadas (si es necesario)
            consultation.Alergias = consultum.ConsultaAlergiasIntNavigation != null
                ? new List<ConsultaAlergiaDTO>
                {
     new ConsultaAlergiaDTO
     {
         CatalogoalergiaId = consultum.ConsultaAlergiasIntNavigation.CatalogoalergiaId,
         ObservacionAlergias = consultum.ConsultaAlergiasIntNavigation.ObservacionAlergias,
         EstadoAlergias = consultum.ConsultaAlergiasIntNavigation.EstadoAlergias
     }
                }
                : new List<ConsultaAlergiaDTO>();

            consultation.Cirugias = consultum.ConsultaCirugias != null
                ? new List<ConsultaCirugiaDTO>
                {
     new ConsultaCirugiaDTO
     {
         CatalogocirugiaId = consultum.ConsultaCirugias.CatalogocirugiaId,
         ObservacionCirugia = consultum.ConsultaCirugias.ObservacionCirugia,
         EstadoCirugias = consultum.ConsultaCirugias.EstadoCirugias
     }
                }
                : new List<ConsultaCirugiaDTO>();

            // Realiza el mapeo similar para los demás campos relacionados si es necesario
            consultation.Medicamentos = consultum.ConsultaMedicamentos != null
                ? new List<ConsultaMedicamentoDTO>
                {
     new ConsultaMedicamentoDTO
     {
         MedicamentoId = consultum.ConsultaMedicamentos.MedicamentoId,
         DosisMedicamento = consultum.ConsultaMedicamentos.DosisMedicamento,
         ObservacionMedicamento = consultum.ConsultaMedicamentos.ObservacionMedicamento,
         SecuencialMedicamento = consultum.ConsultaMedicamentos.SecuencialMedicamento,
         EstadoMedicamento = consultum.ConsultaMedicamentos.EstadoMedicamento
     }
                }
                : new List<ConsultaMedicamentoDTO>();

            consultation.Laboratorios = consultum.ConsultaLaboratorio != null
                ? new List<ConsultaLaboratorioDTO>
                {
     new ConsultaLaboratorioDTO
     {
         CatalogoLaboratorioId = consultum.ConsultaLaboratorio.CatalogoLaboratorioId,
         CantidadLaboratorio = consultum.ConsultaLaboratorio.CantidadLaboratorio,
         Observacion = consultum.ConsultaLaboratorio.Observacion,
         SecuencialLaboratorio = consultum.ConsultaLaboratorio.SecuencialLaboratorio,
         EstadoLaboratorio = consultum.ConsultaLaboratorio.EstadoLaboratorio
     }
                }
                : new List<ConsultaLaboratorioDTO>();

            consultation.Imagenes = consultum.ConsultaImagen != null
                ? new List<ConsultaImagenDTO>
                {
     new ConsultaImagenDTO
     {
         ImagenId = consultum.ConsultaImagen.ImagenId,
         ObservacionImagen = consultum.ConsultaImagen.ObservacionImagen,
         CantidadImagen = consultum.ConsultaImagen.CantidadImagen,
         SecuencialImagen = consultum.ConsultaImagen.SecuencialImagen,
         EstadoImagen = consultum.ConsultaImagen.EstadoImagen
     }
                }
                : new List<ConsultaImagenDTO>();

            consultation.Diagnosticos = consultum.ConsultaDiagnostico != null
                ? new List<ConsultaDiagnosticoDTO>
                {
     new ConsultaDiagnosticoDTO
     {
         DiagnosticoId = consultum.ConsultaDiagnostico.DiagnosticoId,
         ObservacionDiagnostico = consultum.ConsultaDiagnostico.ObservacionDiagnostico,
         PresuntivoDiagnosticos = consultum.ConsultaDiagnostico.PresuntivoDiagnosticos,
         DefinitivoDiagnosticos = consultum.ConsultaDiagnostico.DefinitivoDiagnosticos,
         SecuencialDiagnostico = consultum.ConsultaDiagnostico.SecuencialDiagnostico,
         EstadoDiagnostico = consultum.ConsultaDiagnostico.EstadoDiagnostico
     }
                }
                : new List<ConsultaDiagnosticoDTO>();

            return consultation;
        }

        /// <summary>
        /// Mapeo consultation consultum
        /// </summary>
        /// <param name="consultation"></param>
        /// <returns></returns>

        



        /// <summary>
        /// Busqueda de pacientes
        /// </summary>
        /// <param name="cedula"></param>
        /// <param name="primerNombre"></param>
        /// <param name="primerApellido"></param>
        /// <returns></returns>

        public async Task<IEnumerable<Paciente>> BuscarPacientesAsync(int? cedula, string primerNombre, string primerApellido)
        {
            var query = _context.Pacientes.AsQueryable();

            if (cedula.HasValue)
            {
                query = query.Where(p => p.CiPacientes == cedula.Value);
            }

            if (!string.IsNullOrEmpty(primerNombre))
            {
                query = query.Where(p => p.PrimernombrePacientes.Contains(primerNombre));
            }

            if (!string.IsNullOrEmpty(primerApellido))
            {
                query = query.Where(p => p.PrimerapellidoPacientes.Contains(primerApellido));
            }

            // Aquí combinamos las condiciones usando lógica OR
            query = _context.Pacientes.Where(p =>
                (cedula.HasValue && p.CiPacientes == cedula.Value) ||
                (!string.IsNullOrEmpty(primerNombre) && p.PrimernombrePacientes.Contains(primerNombre)) ||
                (!string.IsNullOrEmpty(primerApellido) && p.PrimerapellidoPacientes.Contains(primerApellido))
            );

            return await query.ToListAsync();
        }

        /// <summary>
        /// Crear una nueva consulta
        /// </summary>
        /// <param name="consultation"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>


        public async Task CrearConsultaAsync(
         string usuariocreacionConsulta,
         string historialConsulta,
         int pacienteConsultaP,
         string motivoConsulta,
         string enfermedadConsulta,
         string nombreparienteConsulta,
         string signosalarmaConsulta,
         string reconofarmacologicas,
         int tipoparienteConsulta,
         string telefonoParienteConsulta,
         string temperaturaConsulta,
         string frecuenciarespiratoriaConsulta,
         string presionarterialsistolicaConsulta,
         string presionarterialdiastolicaConsulta,
         string pulsoConsulta,
         string pesoConsulta,
         string tallaConsulta,
         string plantratamientoConsulta,
         string observacionConsulta,
         string antecedentespersonalesConsulta,
         int diasincapacidadConsulta,
         int medicoConsultaD,
         int especialidadId,
         int tipoConsultaC,
         string notasevolucionConsulta,
         string consultaprincipalConsulta,
         int estadoConsultaC,
         // Parámetros para órganos y sistemas
         bool? orgSentidos,
         string obserOrgSentidos,
         bool? respiratorio,
         string obserRespiratorio,
         bool? cardioVascular,
         string obserCardioVascular,
         bool? digestivo,
         string obserDigestivo,
         bool? genital,
         string obserGenital,
         bool? urinario,
         string obserUrinario,
         bool? mEsqueletico,
         string obserMEsqueletico,
         bool? endocrino,
         string obserEndocrino,
         bool? linfatico,
         string obserLinfatico,
         bool? nervioso,
         string obserNervioso,
         // Parámetros para examen físico
         bool? cabeza,
         string obserCabeza,
         bool? cuello,
         string obserCuello,
         bool? torax,
         string obserTorax,
         bool? abdomen,
         string obserAbdomen,
         bool? pelvis,
         string obserPelvis,
         bool? extremidades,
         string obserExtremidades,
         // Parámetros para antecedentes familiares
         bool? cardiopatia,
         string obserCardiopatia,
         int? parentescocatalogoCardiopatia,
         bool? diabetes,
         string obserDiabetes,
         int? parentescocatalogoDiabetes,
         bool? enfCardiovascular,
         string obserEnfCardiovascular,
         int? parentescocatalogoEnfCardiovascular,
         bool? hipertension,
         string obserHipertension,
         int? parentescocatalogoHipertension,
         bool? cancer,
         string obserCancer,
         int? parentescocatalogoCancer,
         bool? tuberculosis,
         string obserTuberculosis,
         int? parentescocatalogoTuberculosis,
         bool? enfMental,
         string obserEnfMental,
         int? parentescocatalogoEnfMental,
         bool? enfInfecciosa,
         string obserEnfInfecciosa,
         int? parentescocatalogoEnfInfecciosa,
         bool? malFormacion,
         string obserMalFormacion,
         int? parentescocatalogoMalFormacion,
         bool? otro,
         string obserOtro,
         int? parentescocatalogoOtro,
         // Tablas relacionadas
         List<ConsultaAlergiaDTO> alergias,
         List<ConsultaCirugiaDTO> cirugias,
         List<ConsultaMedicamentoDTO> medicamentos,
         List<ConsultaLaboratorioDTO> laboratorios,
         List<ConsultaImagenDTO> imagenes,
         List<ConsultaDiagnosticoDTO> diagnosticos)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("sp_CreateConsultation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetros de consulta
                    AddSqlParameter(command, "@usuariocreacion_consulta", usuariocreacionConsulta);
                    AddSqlParameter(command, "@historial_consulta", historialConsulta);
                    AddSqlParameter(command, "@paciente_consulta_p", pacienteConsultaP);
                    AddSqlParameter(command, "@motivo_consulta", motivoConsulta);
                    AddSqlParameter(command, "@enfermedad_consulta", enfermedadConsulta);
                    AddSqlParameter(command, "@nombrepariente_consulta", nombreparienteConsulta);
                    AddSqlParameter(command, "@signosalarma_consulta", signosalarmaConsulta);
                    AddSqlParameter(command, "@reconofarmacologicas", reconofarmacologicas);
                    AddSqlParameter(command, "@tipopariente_consulta", tipoparienteConsulta);
                    AddSqlParameter(command, "@telefono_pariente_consulta", telefonoParienteConsulta);
                    AddSqlParameter(command, "@temperatura_consulta", temperaturaConsulta);
                    AddSqlParameter(command, "@frecuenciarespiratoria_consulta", frecuenciarespiratoriaConsulta);
                    AddSqlParameter(command, "@presionarterialsistolica_consulta", presionarterialsistolicaConsulta);
                    AddSqlParameter(command, "@presionarterialdiastolica_consulta", presionarterialdiastolicaConsulta);
                    AddSqlParameter(command, "@pulso_consulta", pulsoConsulta);
                    AddSqlParameter(command, "@peso_consulta", pesoConsulta);
                    AddSqlParameter(command, "@talla_consulta", tallaConsulta);
                    AddSqlParameter(command, "@plantratamiento_consulta", plantratamientoConsulta);
                    AddSqlParameter(command, "@observacion_consulta", observacionConsulta);
                    AddSqlParameter(command, "@antecedentespersonales_consulta", antecedentespersonalesConsulta);
                    AddSqlParameter(command, "@diasincapacidad_consulta", diasincapacidadConsulta);
                    AddSqlParameter(command, "@medico_consulta_d", medicoConsultaD);
                    AddSqlParameter(command, "@especialidad_id", especialidadId);
                    AddSqlParameter(command, "@tipo_consulta_c", tipoConsultaC);
                    AddSqlParameter(command, "@notasevolucion_consulta", notasevolucionConsulta);
                    AddSqlParameter(command, "@consultaprincipal_consulta", consultaprincipalConsulta);
                    AddSqlParameter(command, "@estado_consulta_c", estadoConsultaC);

                    // Parámetros de órganos y sistemas
                    AddSqlParameter(command, "@org_sentidos", orgSentidos);
                    AddSqlParameter(command, "@obser_org_sentidos", obserOrgSentidos);
                    AddSqlParameter(command, "@respiratorio", respiratorio);
                    AddSqlParameter(command, "@obser_respiratorio", obserRespiratorio);
                    AddSqlParameter(command, "@cardio_vascular", cardioVascular);
                    AddSqlParameter(command, "@obser_cardio_vascular", obserCardioVascular);
                    AddSqlParameter(command, "@digestivo", digestivo);
                    AddSqlParameter(command, "@obser_digestivo", obserDigestivo);
                    AddSqlParameter(command, "@genital", genital);
                    AddSqlParameter(command, "@obser_genital", obserGenital);
                    AddSqlParameter(command, "@urinario", urinario);
                    AddSqlParameter(command, "@obser_urinario", obserUrinario);
                    AddSqlParameter(command, "@m_esqueletico", mEsqueletico);
                    AddSqlParameter(command, "@obser_m_esqueletico", obserMEsqueletico);
                    AddSqlParameter(command, "@endocrino", endocrino);
                    AddSqlParameter(command, "@obser_endocrino", obserEndocrino);
                    AddSqlParameter(command, "@linfatico", linfatico);
                    AddSqlParameter(command, "@obser_linfatico", obserLinfatico);
                    AddSqlParameter(command, "@nervioso", nervioso);
                    AddSqlParameter(command, "@obser_nervioso", obserNervioso);

                    // Parámetros de examen físico
                    AddSqlParameter(command, "@cabeza", cabeza);
                    AddSqlParameter(command, "@obser_cabeza", obserCabeza);
                    AddSqlParameter(command, "@cuello", cuello);
                    AddSqlParameter(command, "@obser_cuello", obserCuello);
                    AddSqlParameter(command, "@torax", torax);
                    AddSqlParameter(command, "@obser_torax", obserTorax);
                    AddSqlParameter(command, "@abdomen", abdomen);
                    AddSqlParameter(command, "@obser_abdomen", obserAbdomen);
                    AddSqlParameter(command, "@pelvis", pelvis);
                    AddSqlParameter(command, "@obser_pelvis", obserPelvis);
                    AddSqlParameter(command, "@extremidades", extremidades);
                    AddSqlParameter(command, "@obser_extremidades", obserExtremidades);

                    // Parámetros de antecedentes familiares
                    AddSqlParameter(command, "@cardiopatia", cardiopatia);
                    AddSqlParameter(command, "@obser_cardiopatia", obserCardiopatia);
                    AddSqlParameter(command, "@parentescocatalogo_cardiopatia", parentescocatalogoCardiopatia);
                    AddSqlParameter(command, "@diabetes", diabetes);
                    AddSqlParameter(command, "@obser_diabetes", obserDiabetes);
                    AddSqlParameter(command, "@parentescocatalogo_diabetes", parentescocatalogoDiabetes);
                    AddSqlParameter(command, "@enf_cardiovascular", enfCardiovascular);
                    AddSqlParameter(command, "@obser_enf_cardiovascular", obserEnfCardiovascular);
                    AddSqlParameter(command, "@parentescocatalogo_enfcardiovascular", parentescocatalogoEnfCardiovascular);
                    AddSqlParameter(command, "@hipertension", hipertension);
                    AddSqlParameter(command, "@obser_hipertension", obserHipertension);
                    AddSqlParameter(command, "@parentescocatalogo_hipertension", parentescocatalogoHipertension);
                    AddSqlParameter(command, "@cancer", cancer);
                    AddSqlParameter(command, "@obser_cancer", obserCancer);
                    AddSqlParameter(command, "@parentescocatalogo_cancer", parentescocatalogoCancer);
                    AddSqlParameter(command, "@tuberculosis", tuberculosis);
                    AddSqlParameter(command, "@obser_tuberculosis", obserTuberculosis);
                    AddSqlParameter(command, "@parentescocatalogo_tuberculosis", parentescocatalogoTuberculosis);
                    AddSqlParameter(command, "@enf_mental", enfMental);
                    AddSqlParameter(command, "@obser_enf_mental", obserEnfMental);
                    AddSqlParameter(command, "@parentescocatalogo_enfmental", parentescocatalogoEnfMental);
                    AddSqlParameter(command, "@enf_infecciosa", enfInfecciosa);
                    AddSqlParameter(command, "@obser_enf_infecciosa", obserEnfInfecciosa);
                    AddSqlParameter(command, "@parentescocatalogo_enfinfecciosa", parentescocatalogoEnfInfecciosa);
                    AddSqlParameter(command, "@mal_formacion", malFormacion);
                    AddSqlParameter(command, "@obser_mal_formacion", obserMalFormacion);
                    AddSqlParameter(command, "@parentescocatalogo_malformacion", parentescocatalogoMalFormacion);
                    AddSqlParameter(command, "@otro", otro);
                    AddSqlParameter(command, "@obser_otro", obserOtro);
                    AddSqlParameter(command, "@parentescocatalogo_otro", parentescocatalogoOtro);

                    // Tablas relacionadas (se inicializan con CreateDataTable)
                    AddSqlParameter(command, "@Alergias", CreateDataTable(alergias));
                    AddSqlParameter(command, "@Cirugias", CreateDataTable(cirugias));
                    AddSqlParameter(command, "@Medicamentos", CreateDataTable(medicamentos));
                    AddSqlParameter(command, "@Laboratorio", CreateDataTable(laboratorios));
                    AddSqlParameter(command, "@Imagenes", CreateDataTable(imagenes));
                    AddSqlParameter(command, "@Diagnosticos", CreateDataTable(diagnosticos));

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        private void AddSqlParameter(SqlCommand command, string paramName, object value)
        {
            if (value == null)
            {
                command.Parameters.AddWithValue(paramName, DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue(paramName, value);
            }
        }





        public async Task ActualizarConsultaAsync(
    int idConsulta,
    string usuariocreacionConsulta,
    string historialConsulta,
    int pacienteConsultaP,
    string motivoConsulta,
    string enfermedadConsulta,
    string nombreparienteConsulta,
    string signosalarmaConsulta,
    string reconofarmacologicas,
    int tipoparienteConsulta,
    string telefonoParienteConsulta,
    string temperaturaConsulta,
    string frecuenciarespiratoriaConsulta,
    string presionarterialsistolicaConsulta,
    string presionarterialdiastolicaConsulta,
    string pulsoConsulta,
    string pesoConsulta,
    string tallaConsulta,
    string plantratamientoConsulta,
    string observacionConsulta,
    string antecedentespersonalesConsulta,
    int diasincapacidadConsulta,
    int medicoConsultaD,
    int especialidadId,
    int tipoConsultaC,
    string notasevolucionConsulta,
    string consultaprincipalConsulta,
    int estadoConsultaC,
    // Parámetros para órganos y sistemas
    bool orgSentidos,
    string obserOrgSentidos,
    bool respiratorio,
    string obserRespiratorio,
    bool cardioVascular,
    string obserCardioVascular,
    bool digestivo,
    string obserDigestivo,
    bool genital,
    string obserGenital,
    bool urinario,
    string obserUrinario,
    bool mEsqueletico,
    string obserMEsqueletico,
    bool endocrino,
    string obserEndocrino,
    bool linfatico,
    string obserLinfatico,
    bool nervioso,
    string obserNervioso,
    // Parámetros para examen físico
    bool cabeza,
    string obserCabeza,
    bool cuello,
    string obserCuello,
    bool torax,
    string obserTorax,
    bool abdomen,
    string obserAbdomen,
    bool pelvis,
    string obserPelvis,
    bool extremidades,
    string obserExtremidades,
    // Parámetros para antecedentes familiares
    bool cardiopatia,
    string obserCardiopatia,
    int parentescocatalogoCardiopatia,
    bool diabetes,
    string obserDiabetes,
    int parentescocatalogoDiabetes,
    bool enfCardiovascular,
    string obserEnfCardiovascular,
    int parentescocatalogoEnfCardiovascular,
    bool hipertension,
    string obserHipertension,
    int parentescocatalogoHipertension,
    bool cancer,
    string obserCancer,
    int parentescocatalogoCancer,
    bool tuberculosis,
    string obserTuberculosis,
    int parentescocatalogoTuberculosis,
    bool enfMental,
    string obserEnfMental,
    int parentescocatalogoEnfMental,
    bool enfInfecciosa,
    string obserEnfInfecciosa,
    int parentescocatalogoEnfInfecciosa,
    bool malFormacion,
    string obserMalFormacion,
    int parentescocatalogoMalFormacion,
    bool otro,
    string obserOtro,
    int parentescocatalogoOtro,
    // Parámetros para tablas relacionadas
    List<ConsultaAlergiaDTO> alergias,
    List<ConsultaCirugiaDTO> cirugias,
    List<ConsultaMedicamentoDTO> medicamentos,
    List<ConsultaLaboratorioDTO> laboratorios,
    List<ConsultaImagenDTO> imagenes,
    List<ConsultaDiagnosticoDTO> diagnosticos)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("sp_UpdateConsultation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Parámetro de ID de consulta
                    command.Parameters.AddWithValue("@idConsulta", idConsulta);

                    // Parámetros de consulta
                    command.Parameters.AddWithValue("@usuariocreacion_consulta", usuariocreacionConsulta);
                    command.Parameters.AddWithValue("@historial_consulta", historialConsulta);
                    command.Parameters.AddWithValue("@paciente_consulta_p", pacienteConsultaP);
                    command.Parameters.AddWithValue("@motivo_consulta", motivoConsulta);
                    command.Parameters.AddWithValue("@enfermedad_consulta", enfermedadConsulta);
                    command.Parameters.AddWithValue("@nombrepariente_consulta", nombreparienteConsulta);
                    command.Parameters.AddWithValue("@signosalarma_consulta", signosalarmaConsulta);
                    command.Parameters.AddWithValue("@reconofarmacologicas", reconofarmacologicas);
                    command.Parameters.AddWithValue("@tipopariente_consulta", tipoparienteConsulta);
                    command.Parameters.AddWithValue("@telefono_pariente_consulta", telefonoParienteConsulta);
                    command.Parameters.AddWithValue("@temperatura_consulta", temperaturaConsulta);
                    command.Parameters.AddWithValue("@frecuenciarespiratoria_consulta", frecuenciarespiratoriaConsulta);
                    command.Parameters.AddWithValue("@presionarterialsistolica_consulta", presionarterialsistolicaConsulta);
                    command.Parameters.AddWithValue("@presionarterialdiastolica_consulta", presionarterialdiastolicaConsulta);
                    command.Parameters.AddWithValue("@pulso_consulta", pulsoConsulta);
                    command.Parameters.AddWithValue("@peso_consulta", pesoConsulta);
                    command.Parameters.AddWithValue("@talla_consulta", tallaConsulta);
                    command.Parameters.AddWithValue("@plantratamiento_consulta", plantratamientoConsulta);
                    command.Parameters.AddWithValue("@observacion_consulta", observacionConsulta);
                    command.Parameters.AddWithValue("@antecedentespersonales_consulta", antecedentespersonalesConsulta);
                    command.Parameters.AddWithValue("@diasincapacidad_consulta", diasincapacidadConsulta);
                    command.Parameters.AddWithValue("@medico_consulta_d", medicoConsultaD);
                    command.Parameters.AddWithValue("@especialidad_id", especialidadId);
                    command.Parameters.AddWithValue("@tipo_consulta_c", tipoConsultaC);
                    command.Parameters.AddWithValue("@notasevolucion_consulta", notasevolucionConsulta);
                    command.Parameters.AddWithValue("@consultaprincipal_consulta", consultaprincipalConsulta);
                    command.Parameters.AddWithValue("@estado_consulta_c", estadoConsultaC);

                    // Parámetros de órganos y sistemas
                    command.Parameters.AddWithValue("@org_sentidos", orgSentidos);
                    command.Parameters.AddWithValue("@obser_org_sentidos", obserOrgSentidos);
                    command.Parameters.AddWithValue("@respiratorio", respiratorio);
                    command.Parameters.AddWithValue("@obser_respiratorio", obserRespiratorio);
                    command.Parameters.AddWithValue("@cardio_vascular", cardioVascular);
                    command.Parameters.AddWithValue("@obser_cardio_vascular", obserCardioVascular);
                    command.Parameters.AddWithValue("@digestivo", digestivo);
                    command.Parameters.AddWithValue("@obser_digestivo", obserDigestivo);
                    command.Parameters.AddWithValue("@genital", genital);
                    command.Parameters.AddWithValue("@obser_genital", obserGenital);
                    command.Parameters.AddWithValue("@urinario", urinario);
                    command.Parameters.AddWithValue("@obser_urinario", obserUrinario);
                    command.Parameters.AddWithValue("@m_esqueletico", mEsqueletico);
                    command.Parameters.AddWithValue("@obser_m_esqueletico", obserMEsqueletico);
                    command.Parameters.AddWithValue("@endocrino", endocrino);
                    command.Parameters.AddWithValue("@obser_endocrino", obserEndocrino);
                    command.Parameters.AddWithValue("@linfatico", linfatico);
                    command.Parameters.AddWithValue("@obser_linfatico", obserLinfatico);
                    command.Parameters.AddWithValue("@nervioso", nervioso);
                    command.Parameters.AddWithValue("@obser_nervioso", obserNervioso);

                    // Parámetros de examen físico
                    command.Parameters.AddWithValue("@cabeza", cabeza);
                    command.Parameters.AddWithValue("@obser_cabeza", obserCabeza);
                    command.Parameters.AddWithValue("@cuello", cuello);
                    command.Parameters.AddWithValue("@obser_cuello", obserCuello);
                    command.Parameters.AddWithValue("@torax", torax);
                    command.Parameters.AddWithValue("@obser_torax", obserTorax);
                    command.Parameters.AddWithValue("@abdomen", abdomen);
                    command.Parameters.AddWithValue("@obser_abdomen", obserAbdomen);
                    command.Parameters.AddWithValue("@pelvis", pelvis);
                    command.Parameters.AddWithValue("@obser_pelvis", obserPelvis);
                    command.Parameters.AddWithValue("@extremidades", extremidades);
                    command.Parameters.AddWithValue("@obser_extremidades", obserExtremidades);

                    // Parámetros de antecedentes familiares
                    command.Parameters.AddWithValue("@cardiopatia", cardiopatia);
                    command.Parameters.AddWithValue("@obser_cardiopatia", obserCardiopatia);
                    command.Parameters.AddWithValue("@parentescocatalogo_cardiopatia", parentescocatalogoCardiopatia);
                    command.Parameters.AddWithValue("@diabetes", diabetes);
                    command.Parameters.AddWithValue("@obser_diabetes", obserDiabetes);
                    command.Parameters.AddWithValue("@parentescocatalogo_diabetes", parentescocatalogoDiabetes);
                    command.Parameters.AddWithValue("@enf_cardiovascular", enfCardiovascular);
                    command.Parameters.AddWithValue("@obser_enf_cardiovascular", obserEnfCardiovascular);
                    command.Parameters.AddWithValue("@parentescocatalogo_enfcardiovascular", parentescocatalogoEnfCardiovascular);
                    command.Parameters.AddWithValue("@hipertension", hipertension);
                    command.Parameters.AddWithValue("@obser_hipertension", obserHipertension);
                    command.Parameters.AddWithValue("@parentescocatalogo_hipertension", parentescocatalogoHipertension);
                    command.Parameters.AddWithValue("@cancer", cancer);
                    command.Parameters.AddWithValue("@obser_cancer", obserCancer);
                    command.Parameters.AddWithValue("@parentescocatalogo_cancer", parentescocatalogoCancer);
                    command.Parameters.AddWithValue("@tuberculosis", tuberculosis);
                    command.Parameters.AddWithValue("@obser_tuberculosis", obserTuberculosis);
                    command.Parameters.AddWithValue("@parentescocatalogo_tuberculosis", parentescocatalogoTuberculosis);
                    command.Parameters.AddWithValue("@enf_mental", enfMental);
                    command.Parameters.AddWithValue("@obser_enf_mental", obserEnfMental);
                    command.Parameters.AddWithValue("@parentescocatalogo_enfmental", parentescocatalogoEnfMental);
                    command.Parameters.AddWithValue("@enf_infecciosa", enfInfecciosa);
                    command.Parameters.AddWithValue("@obser_enf_infecciosa", obserEnfInfecciosa);
                    command.Parameters.AddWithValue("@parentescocatalogo_enfinfecciosa", parentescocatalogoEnfInfecciosa);
                    command.Parameters.AddWithValue("@mal_formacion", malFormacion);
                    command.Parameters.AddWithValue("@obser_mal_formacion", obserMalFormacion);
                    command.Parameters.AddWithValue("@parentescocatalogo_malformacion", parentescocatalogoMalFormacion);
                    command.Parameters.AddWithValue("@otro", otro);
                    command.Parameters.AddWithValue("@obser_otro", obserOtro);
                    command.Parameters.AddWithValue("@parentescocatalogo_otro", parentescocatalogoOtro);

                    // Tablas relacionadas (TVP)
                    command.Parameters.AddWithValue("@Alergias", CreateDataTable(alergias));
                    command.Parameters.AddWithValue("@Cirugias", CreateDataTable(cirugias));
                    command.Parameters.AddWithValue("@Medicamentos", CreateDataTable(medicamentos));
                    command.Parameters.AddWithValue("@Laboratorio", CreateDataTable(laboratorios));
                    command.Parameters.AddWithValue("@Imagenes", CreateDataTable(imagenes));
                    command.Parameters.AddWithValue("@Diagnosticos", CreateDataTable(diagnosticos));

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        // Método para crear un DataTable desde una lista de objetos
        private DataTable CreateDataTable<T>(List<T> list)
        {
            var table = new DataTable();
            var properties = typeof(T).GetProperties();

            // Crear columnas en el DataTable basadas en las propiedades de la clase
            foreach (var prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            // Rellenar las filas del DataTable con los valores de los objetos
            foreach (var item in list)
            {
                var row = table.NewRow();
                foreach (var prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }

            return table;
        }



    }
}
