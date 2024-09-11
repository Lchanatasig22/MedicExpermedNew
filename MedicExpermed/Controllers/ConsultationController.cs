using MedicExpermed.Models;
using MedicExpermed.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;


namespace MedicExpermed.Controllers
{
    public class ConsultationController : Controller
    {
        private readonly AppointmentService _citaService;
        private readonly PatientService _patientService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ConsultationService _consultationService;
        private readonly CatalogService _catalogService;
        private readonly ILogger<ConsultationController> _logger;
        private readonly medicossystembdIContext _medical_SystemContext;

        public ConsultationController(AppointmentService citaService, PatientService patientService, IHttpContextAccessor httpContextAccessor, ConsultationService consultationService, CatalogService catalogService, ILogger<ConsultationController> logger, medicossystembdIContext medical_SystemContext)
        {
            _citaService = citaService;
            _patientService = patientService;
            _httpContextAccessor = httpContextAccessor;
            _consultationService = consultationService;
            _catalogService = catalogService;
            _logger = logger;
            _medical_SystemContext = medical_SystemContext;
        }



        private async Task CargarListasDesplegables()
        {
            ViewBag.TiposDocumentos = await _catalogService.ObtenerTiposDocumentoAsync();
            ViewBag.TiposSangre = await _catalogService.ObtenerTiposSangreAsync();
            ViewBag.TiposGenero = await _catalogService.ObtenerTiposGeneroAsync();
            ViewBag.TiposEstadoCivil = await _catalogService.ObtenerTiposEstadoCivilAsync();
            ViewBag.TiposFormacion = await _catalogService.ObtenerTiposFormacionAsync();
            ViewBag.TiposNacionalidad = await _catalogService.ObtenerTiposDeNacionalidadPAsync();
            ViewBag.TiposProvincia = await _catalogService.ObtenerTiposDeProvinciaPAsync();
            ViewBag.TiposSeguro = await _catalogService.ObtenerTiposSeguroSaludAsync();
            ViewBag.TiposPariente = await _catalogService.ObtenerTiposParentescoAsync();
            ViewBag.TiposAlergias = await _catalogService.ObtenerAlergiasAsync();
            ViewBag.TiposCirugias = await _catalogService.ObtenerCirugiasAsync();
            ViewBag.TiposParienteAntece = await _catalogService.ObtenerAntecedentesFAsync();
            ViewBag.TiposMedicamentos = await _catalogService.ObtenerMedicamentosActivasAsync();
            ViewBag.TiposLaboratorios = await _catalogService.ObtenerLaboratorioActivasAsync();
            ViewBag.TiposImagen = await _catalogService.ObtenerImagenActivasAsync();
            ViewBag.TiposDiagnostico = await _catalogService.ObtenerDiagnosticosActivasAsync();
        }

        [HttpGet("Listar-Consultas")]
        public async Task<IActionResult> ListarConsultas()
        {
            try
            {
                var consultas = await _consultationService.GetAllConsultasAsync();
                ViewBag.UsuarioEspecialidad = HttpContext.Session.GetString("UsuarioEspecialidad");

                return View(consultas);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(new List<Consultum>());
            }
        }

        [HttpGet("Crear-Consulta")]
        public async Task<IActionResult> CrearConsulta()
        {
            // Obtener IDs de "NINGUNA"
            int ningunTipoParienteId = await _catalogService.ObtenerIdParentescoNingunoAsync();
            int ningunAlergiasId = await _catalogService.ObtenerIdAlergiasNingunoAsync();

            // Crear una nueva instancia de Consultation
            var consulta = new Consultation
            {
                TipoPariente = ningunTipoParienteId,
                FechaCreacion = DateTime.Now
            };

            // Añadir el ID "NINGUNA" a la lista de alergias
            consulta.Alergias.Add(new ConsultaAlergiaDTO
            {
                CatalogoalergiaId = ningunAlergiasId,
                ObservacionAlergias = "Ninguna alergia registrada",
                EstadoAlergias = 1
            });

            // Asignar datos a ViewBag desde la sesión
            ViewBag.UsuarioNombre = HttpContext.Session.GetString("UsuarioNombre");
            ViewBag.UsuarioId = HttpContext.Session.GetInt32("UsuarioId");
            ViewBag.UsuarioIdEspecialidad = HttpContext.Session.GetInt32("UsuarioIdEspecialidad");

            // Cargar listas desplegables
            await CargarListasDesplegables();

            // Devolver el modelo a la vista
            return View(consulta);
        }


        [HttpPost("Crear-Consulta")]
        public async Task<IActionResult> CrearConsulta([FromBody] Consultation consultaDto)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) })
                    .ToList();

                // Aquí puedes revisar cuáles campos tienen errores y cuál es el mensaje de error
                return Json(new
                {
                    success = false,
                    message = "Datos inválidos",
                    errors = errores
                });
            }


            try
            {
                // Llama al servicio para crear la consulta
                await _consultationService.CrearConsultaAsync(
                    consultaDto.UsuarioCreacion,
                    consultaDto.HistorialConsulta,
                    consultaDto.PacienteId,
                    consultaDto.MotivoConsulta,
                    consultaDto.EnfermedadConsulta,
                    consultaDto.NombrePariente,
                    consultaDto.SignosAlarma,
                    consultaDto.ReconocimientoFarmacologico,
                    consultaDto.TipoPariente,
                    consultaDto.TelefonoPariente,
                    consultaDto.Temperatura,
                    consultaDto.FrecuenciaRespiratoria,
                    consultaDto.PresionArterialSistolica,
                    consultaDto.PresionArterialDiastolica,
                    consultaDto.Pulso,
                    consultaDto.Peso,
                    consultaDto.Talla,
                    consultaDto.PlanTratamiento,
                    consultaDto.Observacion,
                    consultaDto.AntecedentesPersonales,
                    consultaDto.DiasIncapacidad,
                    consultaDto.MedicoId,
                    consultaDto.EspecialidadId,
                    consultaDto.TipoConsultaId ?? 0,
                    consultaDto.NotasEvolucion,
                    consultaDto.ConsultaPrincipal,
                    consultaDto.EstadoConsulta,
                    // Parámetros de órganos y sistemas
                    consultaDto.OrganosSistemas.OrgSentidos,
                    consultaDto.OrganosSistemas.ObserOrgSentidos,
                    consultaDto.OrganosSistemas.Respiratorio,
                    consultaDto.OrganosSistemas.ObserRespiratorio,
                    consultaDto.OrganosSistemas.CardioVascular,
                    consultaDto.OrganosSistemas.ObserCardioVascular,
                    consultaDto.OrganosSistemas.Digestivo,
                    consultaDto.OrganosSistemas.ObserDigestivo,
                    consultaDto.OrganosSistemas.Genital,
                    consultaDto.OrganosSistemas.ObserGenital,
                    consultaDto.OrganosSistemas.Urinario,
                    consultaDto.OrganosSistemas.ObserUrinario,
                    consultaDto.OrganosSistemas.MEsqueletico,
                    consultaDto.OrganosSistemas.ObserMEsqueletico,
                    consultaDto.OrganosSistemas.Endocrino,
                    consultaDto.OrganosSistemas.ObserEndocrino,
                    consultaDto.OrganosSistemas.Linfatico,
                    consultaDto.OrganosSistemas.ObserLinfatico,
                    consultaDto.OrganosSistemas.Nervioso,
                    consultaDto.OrganosSistemas.ObserNervioso,
                    // Parámetros de examen físico
                    consultaDto.ExamenFisico.Cabeza,
                    consultaDto.ExamenFisico.ObserCabeza,
                    consultaDto.ExamenFisico.Cuello,
                    consultaDto.ExamenFisico.ObserCuello,
                    consultaDto.ExamenFisico.Torax,
                    consultaDto.ExamenFisico.ObserTorax,
                    consultaDto.ExamenFisico.Abdomen,
                    consultaDto.ExamenFisico.ObserAbdomen,
                    consultaDto.ExamenFisico.Pelvis,
                    consultaDto.ExamenFisico.ObserPelvis,
                    consultaDto.ExamenFisico.Extremidades,
                    consultaDto.ExamenFisico.ObserExtremidades,
                    // Parámetros de antecedentes familiares
                    consultaDto.AntecedentesFamiliares.Cardiopatia,
                    consultaDto.AntecedentesFamiliares.ObserCardiopatia,
                    consultaDto.AntecedentesFamiliares.ParentescocatalogoCardiopatia,
                    consultaDto.AntecedentesFamiliares.Diabetes,
                    consultaDto.AntecedentesFamiliares.ObserDiabetes,
                    consultaDto.AntecedentesFamiliares.ParentescocatalogoDiabetes,
                    consultaDto.AntecedentesFamiliares.EnfCardiovascular,
                    consultaDto.AntecedentesFamiliares.ObserEnfCardiovascular,
                    consultaDto.AntecedentesFamiliares.ParentescocatalogoEnfcardiovascular,
                    consultaDto.AntecedentesFamiliares.Hipertension,
                    consultaDto.AntecedentesFamiliares.ObserHipertension,
                    consultaDto.AntecedentesFamiliares.ParentescocatalogoHipertension,
                    consultaDto.AntecedentesFamiliares.Cancer,
                    consultaDto.AntecedentesFamiliares.ObserCancer,
                    consultaDto.AntecedentesFamiliares.ParentescocatalogoCancer,
                    consultaDto.AntecedentesFamiliares.Tuberculosis,
                    consultaDto.AntecedentesFamiliares.ObserTuberculosis,
                    consultaDto.AntecedentesFamiliares.ParentescocatalogoTuberculosis,
                    consultaDto.AntecedentesFamiliares.EnfMental,
                    consultaDto.AntecedentesFamiliares.ObserEnfMental,
                    consultaDto.AntecedentesFamiliares.ParentescocatalogoEnfmental,
                    consultaDto.AntecedentesFamiliares.EnfInfecciosa,
                    consultaDto.AntecedentesFamiliares.ObserEnfInfecciosa,
                    consultaDto.AntecedentesFamiliares.ParentescocatalogoEnfinfecciosa,
                    consultaDto.AntecedentesFamiliares.MalFormacion,
                    consultaDto.AntecedentesFamiliares.ObserMalFormacion,
                    consultaDto.AntecedentesFamiliares.ParentescocatalogoMalformacion,
                    consultaDto.AntecedentesFamiliares.Otro,
                    consultaDto.AntecedentesFamiliares.ObserOtro,
                    consultaDto.AntecedentesFamiliares.ParentescocatalogoOtro,
                    // Tablas relacionadas
                    consultaDto.Alergias,
                    consultaDto.Cirugias,
                    consultaDto.Medicamentos,
                    consultaDto.Laboratorios,
                    consultaDto.Imagenes,
                    consultaDto.Diagnosticos
                );

                // Devuelve un JSON con éxito si es una solicitud AJAX
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    TempData["SuccessMessage"] = $"La consulta creado exitosamente";

                    return Json(new { success = true, message = "Consulta creada exitosamente" });

                }
                else
                {
                    // Redirigir en caso de una solicitud normal (no AJAX)
                    TempData["SuccessMessage"] = "Consulta Registrado.";
                    return Json(new { success = true, redirectUrl = Url.Action("ListarConsultas") });
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear el paciente: {ex.Message}";

                _logger.LogError(ex, "Error al crear la consulta");

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Ocurrió un error en el servidor.", details = ex.Message });
                }
                else
                {
                    return View("Error", new ErrorViewModel { RequestId = ex.Message });
                }
            }
        }


        // VER CONSULTA

        [HttpGet("Ver-Consulta/{id}")]
        public async Task<IActionResult> VerConsulta(int id)
        {
            // Obtener la consulta existente por su ID desde el servicio que ya mapea los datos
            var consulta = await _consultationService.ObtenerConsultaPorIdAsync(id);

            if (consulta == null)
            {
                return NotFound(); // Maneja el caso en que no se encuentra la consulta
            }

            // Cargar datos adicionales para la vista
            ViewBag.UsuarioNombre = HttpContext.Session.GetString("UsuarioNombre");
            ViewBag.UsuarioId = HttpContext.Session.GetInt32("UsuarioId");
            ViewBag.UsuarioIdEspecialidad = HttpContext.Session.GetInt32("UsuarioIdEspecialidad");

            await CargarListasDesplegables(); // Carga los dropdowns si es necesario

            // Devuelve directamente el modelo mapeado a la vista
            return View(consulta);
        }




        //EDITAR CONSULTA
        [HttpGet("Editar-Consulta")]
        public async Task<IActionResult> EditarConsulta()
        {
            int ningunTipoParienteId = await _catalogService.ObtenerIdParentescoNingunoAsync();

            ViewBag.UsuarioNombre = HttpContext.Session.GetString("UsuarioNombre");
            ViewBag.UsuarioId = HttpContext.Session.GetInt32("UsuarioId");
            ViewBag.UsuarioIdEspecialidad = HttpContext.Session.GetInt32("UsuarioIdEspecialidad");

            await CargarListasDesplegables();

            var model = new Consultation
            {
                TipoPariente = ningunTipoParienteId,
                FechaCreacion = DateTime.Now,
            };



            return View(model);
        }



        // Endpoint para actualizar una consulta existente
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateConsultation(
            int id,
            [FromBody] Consultation request)
        {
            if (id != request.Id)
            {
                return BadRequest("El ID de la consulta no coincide con el ID en el cuerpo de la solicitud.");
            }

            try
            {
                await _consultationService.ActualizarConsultaAsync(
                    request.Id,
                    request.UsuarioCreacion,
                    request.HistorialConsulta,
                    request.PacienteId,
                    request.MotivoConsulta,
                    request.EnfermedadConsulta,
                    request.NombrePariente,
                    request.SignosAlarma,
                    request.ReconocimientoFarmacologico,
                    request.TipoPariente,
                    request.TelefonoPariente,
                    request.Temperatura,
                    request.FrecuenciaRespiratoria,
                    request.PresionArterialSistolica,
                    request.PresionArterialDiastolica,
                    request.Pulso,
                    request.Peso,
                    request.Talla,
                    request.PlanTratamiento,
                    request.Observacion,
                    request.AntecedentesPersonales,
                    request.DiasIncapacidad,
                    request.MedicoId,
                    request.EspecialidadId,
                    request.TipoConsultaId ?? 0,
                    request.NotasEvolucion,
                    request.ConsultaPrincipal,
                    request.EstadoConsulta,
// Parámetros para órganos y sistemas con manejo de booleanos nullable
request.OrganosSistemas.OrgSentidos ?? false,        // Manejo de bool?
request.OrganosSistemas.ObserOrgSentidos,
request.OrganosSistemas.Respiratorio ?? false,       // Manejo de bool?
request.OrganosSistemas.ObserRespiratorio,
request.OrganosSistemas.CardioVascular ?? false,     // Manejo de bool?
request.OrganosSistemas.ObserCardioVascular,
request.OrganosSistemas.Digestivo ?? false,          // Manejo de bool?
request.OrganosSistemas.ObserDigestivo,
request.OrganosSistemas.Genital ?? false,            // Manejo de bool?
request.OrganosSistemas.ObserGenital,
request.OrganosSistemas.Urinario ?? false,           // Manejo de bool?
request.OrganosSistemas.ObserUrinario,
request.OrganosSistemas.MEsqueletico ?? false,       // Manejo de bool?
request.OrganosSistemas.ObserMEsqueletico,
request.OrganosSistemas.Endocrino ?? false,          // Manejo de bool?
request.OrganosSistemas.ObserEndocrino,
request.OrganosSistemas.Linfatico ?? false,          // Manejo de bool?
request.OrganosSistemas.ObserLinfatico,
request.OrganosSistemas.Nervioso ?? false,           // Manejo de bool?
request.OrganosSistemas.ObserNervioso,


// Parámetros para examen físico con manejo de booleanos nullable
request.ExamenFisico.Cabeza ?? false, // Manejo de bool?
request.ExamenFisico.ObserCabeza,
request.ExamenFisico.Cuello ?? false, // Manejo de bool?
request.ExamenFisico.ObserCuello,
request.ExamenFisico.Torax ?? false, // Manejo de bool?
request.ExamenFisico.ObserTorax,
request.ExamenFisico.Abdomen ?? false, // Manejo de bool?
request.ExamenFisico.ObserAbdomen,
request.ExamenFisico.Pelvis ?? false, // Manejo de bool?
request.ExamenFisico.ObserPelvis,
request.ExamenFisico.Extremidades ?? false, // Manejo de bool?
request.ExamenFisico.ObserExtremidades
,
// Parámetros para antecedentes familiares, manejo de nulos con valor predeterminado
request.AntecedentesFamiliares.Cardiopatia ?? false,
request.AntecedentesFamiliares.ObserCardiopatia,
request.AntecedentesFamiliares.ParentescocatalogoCardiopatia ?? default(int),
request.AntecedentesFamiliares.Diabetes ?? false,
request.AntecedentesFamiliares.ObserDiabetes,
request.AntecedentesFamiliares.ParentescocatalogoDiabetes ?? default(int),
request.AntecedentesFamiliares.EnfCardiovascular ?? false,
request.AntecedentesFamiliares.ObserEnfCardiovascular,
request.AntecedentesFamiliares.ParentescocatalogoEnfcardiovascular ?? default(int),
request.AntecedentesFamiliares.Hipertension ?? false,
request.AntecedentesFamiliares.ObserHipertension,
request.AntecedentesFamiliares.ParentescocatalogoHipertension ?? default(int),
request.AntecedentesFamiliares.Cancer ?? false,
request.AntecedentesFamiliares.ObserCancer,
request.AntecedentesFamiliares.ParentescocatalogoCancer ?? default(int),
request.AntecedentesFamiliares.Tuberculosis ?? false,
request.AntecedentesFamiliares.ObserTuberculosis,
request.AntecedentesFamiliares.ParentescocatalogoTuberculosis ?? default(int),
request.AntecedentesFamiliares.EnfMental ?? false,
request.AntecedentesFamiliares.ObserEnfMental,
request.AntecedentesFamiliares.ParentescocatalogoEnfmental ?? default(int),
request.AntecedentesFamiliares.EnfInfecciosa ?? false,
request.AntecedentesFamiliares.ObserEnfInfecciosa,
request.AntecedentesFamiliares.ParentescocatalogoEnfinfecciosa ?? default(int),
request.AntecedentesFamiliares.MalFormacion ?? false,
request.AntecedentesFamiliares.ObserMalFormacion,
request.AntecedentesFamiliares.ParentescocatalogoMalformacion ?? default(int),
request.AntecedentesFamiliares.Otro ?? false,
request.AntecedentesFamiliares.ObserOtro,
request.AntecedentesFamiliares.ParentescocatalogoOtro ?? default(int),
                    // Tablas relacionadas
                    request.Alergias,
                    request.Cirugias,
                    request.Medicamentos,
                    request.Laboratorios,
                    request.Imagenes,
                    request.Diagnosticos
                );

                return Ok(new { Message = "Consulta actualizada exitosamente." });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones (puedes hacer logging aquí si es necesario)
                return StatusCode(500, new { Message = "Ocurrió un error al actualizar la consulta.", Details = ex.Message });
            }
        }



        [HttpPost]
        public async Task<IActionResult> GeneratePdf(int id, string tipoDocumento)
        {
            // Espera a que se complete la tarea asincrónica para obtener la consulta
            var consulta = await _consultationService.ObtenerConsultaPorIdAsync(id);

            if (consulta == null)
            {
                return NotFound();
            }

            // Generar el PDF según el tipo de documento
            var document = GenerateDocumentByType(tipoDocumento, consulta);

            using var memoryStream = new MemoryStream();
            document.GeneratePdf(memoryStream);

            memoryStream.Position = 0;
            return File(memoryStream.ToArray(), "application/pdf", $"Consulta_{id}_{tipoDocumento}.pdf");
        }


        private IDocument GenerateDocumentByType(string tipoDocumento, Consultation consulta)
        {
            // Configuración para cada tipo de documento
            switch (tipoDocumento)
            {
                case "receta":
                    return CreateRecetaDocument(consulta);
                case "justificacion":
                    return CreateJustificacionDocument(consulta);
                case "consulta":
                    return CreateConsultaDocument(consulta);
                case "laboratorio":
                    return CreateLaboratorioDocument(consulta);
                case "imagen":
                    return CreateImagenDocument(consulta);
                default:
                    throw new InvalidOperationException("Tipo de documento no soportado.");
            }
        }


        private IDocument CreateRecetaDocument(Consultation consulta)
        {
            // Tamaño de página A5 con márgenes grandes
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Size(1194, 847); // Tamaño A3 horizontal

                    page.DefaultTextStyle(x => x.FontFamily("Arial")); // Cambiar la familia de fuentes a Arial

                    page.Header().Row(row =>
                    {
                        row.ConstantItem(550).Column(col =>
                        {
                            col.Item().Text("Dr. Kim Morales").Bold().FontSize(14); // Encabezado más grande
                            col.Item().Text("Especialista en Medicina general").FontSize(12);
                            col.Item().Text("e-mail: helpdesk@asersoat.com").FontSize(11);
                            col.Item().Text("Teléfonos:").FontSize(11);
                            col.Item().PaddingTop(2).Text("").FontSize(11);
                            col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                        });

                        row.AutoItem().PaddingHorizontal(10).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                        row.RelativeColumn().Column(col =>
                        {
                            col.Item().Text("Dr. Kim Morales").Bold().FontSize(14); // Encabezado más grande
                            col.Item().Text("Especialista en Medicina general").FontSize(12);
                            col.Item().Text("e-mail: helpdesk@asersoat.com").FontSize(11);
                            col.Item().Text("Teléfonos:").FontSize(11);
                            col.Item().PaddingTop(2).Text("").FontSize(11);
                            col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                        });
                    });


                    page.Content().Column(content =>
                    {
                        // Primera fila
                        content.Item().Row(row =>
                        {
                            row.ConstantItem(275).Column(col =>
                            {
                                col.Item().PaddingTop(10).Text("Fecha:").FontSize(11).Bold();
                                col.Item().Text("Apellidos:").FontSize(11).Bold();
                                col.Item().Text("Nombres:").FontSize(11).Bold();
                                col.Item().Text("Edad:").FontSize(11).Bold();
                                col.Item().Text("Alergias:").FontSize(11).Bold();
                                col.Item().Text("Diagnostico:").FontSize(11).Bold();

                            });

                            row.ConstantItem(275).Column(col =>
                            {
                                col.Item().PaddingTop(10).Text("Receta: ").Bold().FontSize(14); // Encabezado más grande
                                col.Item().Text("CC: ").FontSize(12);

                            });

                            row.AutoItem().PaddingHorizontal(10).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                            row.ConstantItem(275).Column(col =>
                            {
                                col.Item().PaddingTop(10).Text("Fecha:").FontSize(11).Bold();
                                col.Item().Text("Apellidos:").FontSize(11).Bold();
                                col.Item().Text("Nombres:").FontSize(11).Bold();
                                col.Item().Text("Edad:").FontSize(11).Bold();
                                col.Item().Text("Alergias:").FontSize(11).Bold();
                                col.Item().Text("Diagnostico:").FontSize(11).Bold();
                            });

                            row.ConstantItem(275).Column(col =>
                            {
                                col.Item().PaddingTop(10).Text("Receta: ").Bold().FontSize(14); // Encabezado más grande
                                col.Item().Text("CC: ").FontSize(12);
                            });
                        });

                        // Segunda fila (Nueva fila)
                        content.Item().Row(row =>
                        {
                            row.ConstantItem(530).PaddingTop(50).Column(col =>
                            {
                                col.Item().Text("Prescipcion").Bold().FontSize(14); // Nuevo contenido
                                col.Item().Text("Texto adicional").FontSize(12);
                            });

                            row.ConstantItem(20).PaddingTop(50).Column(col =>
                            {
                                col.Item().Text("").FontSize(12);
                                col.Item().Text("x4").FontSize(12);
                            });

                            row.AutoItem().PaddingHorizontal(10).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                            row.ConstantItem(550).PaddingTop(50).Column(col =>
                            {
                                col.Item().Text("Indicaciones").Bold().FontSize(14); // Nuevo contenido
                                col.Item().Text("Texto adicional").FontSize(12);
                            });

                        });


                        // Tercera fila (Nueva fila)

                        content.Item().Row(row =>
                        {
                            row.ConstantItem(550).PaddingTop(20).Column(col =>
                            {

                            });

                            row.AutoItem().PaddingHorizontal(10).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                            row.ConstantItem(550).PaddingTop(200).Column(col =>
                            {
                                col.Item().Text("Recomendaciones no Farmacologicas").Bold().FontSize(14); // Nuevo contenido
                                col.Item().Text("Texto adicional").FontSize(12);
                                col.Item().Text("Texto adicional").FontSize(12);
                                col.Item().Text("Texto adicional").FontSize(12);
                                col.Item().Text("Texto adicional").FontSize(12);
                                col.Item().Text("Texto adicional").FontSize(12);
                                col.Item().Text("Texto adicional").FontSize(12);
                                col.Item().PaddingTop(100).Text("").FontSize(12);




                            });


                        });



                    });

                    page.Footer().Column(footer =>
                    {// Nueva fila del footer
                        footer.Item().Row(row =>
                        {
                            row.ConstantItem(300).Column(col =>
                            {
                            });
                            row.ConstantItem(230).Border(1).Column(col =>
                            {
                                col.Item().Text("Dispensada").FontSize(11).AlignStart();
                                col.Item().Text("").FontSize(11).AlignStart();
                                col.Item().Text("Completamente:......... Parcialmente:.........").FontSize(11).AlignStart();
                                col.Item().Text("").FontSize(11).AlignStart();

                                col.Item().Text("Proxima Cita").FontSize(11).AlignStart();
                                col.Item().Text("_____________").FontSize(11).AlignCenter();


                            });

                            row.AutoItem().PaddingHorizontal(30).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                            row.ConstantItem(300).Column(col =>
                            {
                            });
                            row.ConstantItem(230).Border(1).Column(col =>
                            {
                                col.Item().Text("Dispensada").FontSize(11).AlignStart();
                                col.Item().Text("").FontSize(11).AlignStart();
                                col.Item().Text("Completamente:......... Parcialmente:.........").FontSize(11).AlignStart();
                                col.Item().Text("").FontSize(11).AlignStart();

                                col.Item().Text("Proxima Cita").FontSize(11).AlignStart();
                                col.Item().Text("_____________").FontSize(11).AlignCenter();


                            });
                        });

                        // Primera fila del footer
                        footer.Item().Row(row =>
                        {
                            row.ConstantItem(530).Column(col =>
                            {
                                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                                col.Item().Text("Direccion: Av de la Prensa N49-180 y Juan Holguín Edifico Amafkar sexto piso, Chordeleg - Ecuador").FontSize(11).AlignCenter();
                            });

                            row.AutoItem().PaddingHorizontal(30).LineVertical(1).LineColor(Colors.Grey.Lighten1);

                            row.RelativeColumn().Column(col =>
                            {
                                col.Item().PaddingVertical(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                                col.Item().Text("Direccion: Av de la Prensa N49-180 y Juan Holguín Edifico Amafkar sexto piso, Chordeleg - Ecuador").FontSize(11).AlignCenter();
                            });
                        });


                    });

                });

            });
        }

        private IDocument CreateJustificacionDocument(Consultation consulta)
        {
            // Tamaño carta con orientación horizontal
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    
                    page.Margin(30);
                    page.Header().Text("Certificado Médico").FontSize(20).Bold().AlignCenter();
                    page.Content().Column(column =>
                    {
                        column.Item().Text($"Certifico que el paciente {consulta.PacienteConsultaPNavigation.PrimernombrePacientes}...");
                        column.Item().Text($"Días de reposo: {consulta.DiasIncapacidad}");
                        column.Item().Text($"Fecha: {System.DateTime.Now.ToShortDateString()}");
                    });
                    page.Footer().AlignRight().Text("Doctor Firma");
                });
            });
        }

        private IDocument CreateConsultaDocument(Consultation consulta)
        {
            // Tamaño de página A4 estándar
            return Document.Create(container =>
            {

                container.Page(page =>
                {
                    page.Margin(20);
                    page.Size(PageSizes.A4);

                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(10));

                    // Header con una tabla de 6 columnas
                    page.Header().Border(2).BorderColor("#808080").Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3); // Establecimiento
                            columns.RelativeColumn(2); // Nombre
                            columns.RelativeColumn(2); // Apellido
                            columns.RelativeColumn(1); // Sexo
                            columns.RelativeColumn(1); // Edad
                            columns.RelativeColumn(2); // Nº Historia Clínica
                        });

                        // Fila de encabezados
                        table.Cell().Border(1).BorderColor("#808080").Element(CellStyle => CellStyle.Background("#ccffcc"))
                            .MinHeight(14).AlignCenter().PaddingTop(3).Text("ESTABLECIMIENTO").FontSize(6);
                        table.Cell().Border(1).BorderColor("#808080").Element(CellStyle => CellStyle.Background("#ccffcc"))
                            .MinHeight(14).AlignCenter().PaddingTop(3).Text("NOMBRE").FontSize(6);
                        table.Cell().Border(1).BorderColor("#808080").Element(CellStyle => CellStyle.Background("#ccffcc"))
                            .MinHeight(14).AlignCenter().PaddingTop(3).Text("APELLIDO").FontSize(6);
                        table.Cell().Border(1).BorderColor("#808080").Element(CellStyle => CellStyle.Background("#ccffcc"))
                            .MinHeight(14).AlignCenter().PaddingTop(3).Text("SEXO").FontSize(6);
                        table.Cell().Border(1).BorderColor("#808080").Element(CellStyle => CellStyle.Background("#ccffcc"))
                            .MinHeight(14).AlignCenter().PaddingTop(3).Text("EDAD").FontSize(6);
                        table.Cell().Border(1).BorderColor("#808080").Element(CellStyle => CellStyle.Background("#ccffcc"))
                            .MinHeight(14).AlignCenter().PaddingTop(3).Text("Nº HISTORIA CLÍNICA").FontSize(6);

                        // Fila de contenido
                        table.Cell().Border(1).BorderColor("#808080").MinHeight(7).AlignCenter().PaddingTop(3)
                            .Element(CellStyle => CellStyle.Background("#FFFFFF")).Text("AMARE INSTITUTO").FontSize(7);
                        table.Cell().Border(1).BorderColor("#808080").MinHeight(7).AlignCenter().PaddingTop(3)
                            .Element(CellStyle => CellStyle.Background("#FFFFFF")).Text("Julia Fernanda").FontSize(7);
                        table.Cell().Border(1).BorderColor("#808080").MinHeight(7).AlignCenter().PaddingTop(3)
                            .Element(CellStyle => CellStyle.Background("#FFFFFF")).Text("Lema Flores").FontSize(7);
                        table.Cell().Border(1).BorderColor("#808080").MinHeight(7).AlignCenter().PaddingTop(3)
                            .Element(CellStyle => CellStyle.Background("#FFFFFF")).Text("F").FontSize(7);
                        table.Cell().Border(1).BorderColor("#808080").MinHeight(7).AlignCenter().PaddingTop(3)
                            .Element(CellStyle => CellStyle.Background("#FFFFFF")).Text("43").FontSize(7);
                        table.Cell().Border(1).BorderColor("#808080").MinHeight(7).AlignCenter().PaddingTop(3)
                            .Element(CellStyle => CellStyle.Background("#FFFFFF")).Text("1713456780").FontSize(7);
                    });

                    // Contenido principal con múltiples tablas
                    page.Content().PaddingTop(6).Column(contentColumn =>
                    {
                        // Primera tabla
                        contentColumn.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(555);
                            });

                            // Fila de encabezado
                            table.Cell().MinHeight(14).Border(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).PaddingLeft(3).Text("1. MOTIVO DE CONSULTA").FontSize(10).Bold();

                            // Fila de datos
                            table.Cell().MinHeight(14).Border(2).BorderColor(Colors.Grey.Medium).Text("Dolor abdominal").FontSize(10);
                        });

                        // Segunda tabla
                        contentColumn.Item().PaddingTop(7).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(555);
                            });

                            // Fila de encabezado
                            table.Cell().MinHeight(14).Border(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).PaddingLeft(3).Text("2. ANTECEDENTES PERSONALES").FontSize(10).Bold();

                            // Fila de datos
                            table.Cell().MinHeight(14).BorderLeft(2).BorderBottom(1).BorderRight(2).BorderColor("#808080").Text("Antecedente de hipertensión").FontSize(10);
                            table.Cell().MinHeight(14).BorderLeft(2).BorderBottom(1).BorderRight(2).BorderColor("#808080").Text("").FontSize(10);
                            table.Cell().MinHeight(14).BorderLeft(2).BorderBottom(1).BorderRight(2).BorderColor("#808080").Text("").FontSize(10);
                            table.Cell().MinHeight(14).BorderLeft(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Text("").FontSize(10);
                        });

                        // Tercera tabla

                        contentColumn.Item().PaddingTop(7).Table(table =>
                        {
                            // Definir las columnas de la tabla para el encabezado
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(555); // Encabezado general ocupa toda la fila
                            });

                            // Fila de encabezado general "3 ANTECEDENTES FAMILIARES"
                            table.Cell().MinHeight(14).BorderLeft(2).BorderRight(2).BorderTop(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).Text("3 ANTECEDENTES FAMILIARES").FontSize(10).Bold();

                            table.Cell().Element(CellStyle =>
                            {
                                // Crear una tabla interna con varias columnas dentro de la celda "padre"
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Añadir columnas dentro de la tabla anidada
                                    nestedTable.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(27); // Primera columna
                                        columns.ConstantColumn(28); // Segunda columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(28); // Tercera columna
                                        columns.ConstantColumn(29); // Tercera columna
                                        columns.ConstantColumn(20); // Tercera columna


                                    });

                                    // Fila dentro de la tabla anidada 
                                    nestedTable.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("1     CARDIOPATIA").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("2.     DIABETES").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("3. ENF.         CARDIOVASCULAR").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("4.  HIPERTENSION").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("5.              CANCER").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("6. TUBERCULOSIS").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("7.ENF MENTAL").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("8. ENF INFECCIOSA").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("9. MAL FORMACION").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("10 OTRO").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();

                                });
                            });
                            table.Cell().MinHeight(14).BorderLeft(2).BorderRight(2).BorderBottom(1).BorderColor("#808080").Text(" Antecedente de hipertensión").FontSize(10);
                            table.Cell().MinHeight(16).BorderLeft(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Text(" Antecedente de hipertensión").FontSize(10);


                        });
                        //CUARTA TABLA
                        contentColumn.Item().PaddingTop(7).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(555);
                            });

                            // Fila de encabezado
                            table.Cell().MinHeight(14).Border(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).PaddingLeft(3).Text("4 ENFERMEDAD O PROBLEMA ACTUAL").FontSize(10).Bold();

                            // Fila de datos
                            table.Cell().BorderLeft(2).MinHeight(14).BorderBottom(1).BorderRight(2).BorderColor("#808080").Text("Antecedente de hipertensión").FontSize(10);
                            table.Cell().BorderLeft(2).MinHeight(14).BorderBottom(1).BorderRight(2).BorderColor("#808080").Text("").FontSize(10);
                            table.Cell().BorderLeft(2).MinHeight(14).BorderBottom(1).BorderRight(2).BorderColor("#808080").Text("").FontSize(10);
                            table.Cell().BorderLeft(2).MinHeight(14).BorderBottom(1).BorderRight(2).BorderColor("#808080").Text("").FontSize(10);
                            table.Cell().BorderLeft(2).MinHeight(14).BorderBottom(1).BorderRight(2).BorderColor("#808080").Text("").FontSize(10);
                            table.Cell().BorderLeft(2).MinHeight(14).BorderBottom(2).BorderRight(2).BorderColor("#808080").Text("").FontSize(10);

                        });
                        //QUINTA TABLA
                        contentColumn.Item().PaddingTop(7).Table(table =>
                        {
                            // Definir las columnas de la tabla para el encabezado
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(555); // Encabezado general ocupa toda la fila
                            });

                            // Fila de encabezado general "3 ANTECEDENTES FAMILIARES"
                            table.Cell().MinHeight(14).BorderLeft(2).BorderRight(2).BorderTop(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).Text("5 REVISIÓN ACTUAL DE ÓRGANOS Y SISTEMAS").FontSize(10).Bold();
                            table.Cell().Element(CellStyle =>
                            {
                                // Crear una tabla interna con varias columnas dentro de la celda "padre"
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Añadir columnas dentro de la tabla anidada
                                    nestedTable.ColumnsDefinition(columns =>
                                    {

                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna


                                    });

                                    // Fila dentro de la tabla anidada 
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();















                                });
                            });
                            table.Cell().Element(CellStyle =>
                            {
                                // Crear una tabla interna con varias columnas dentro de la celda "padre"
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Añadir columnas dentro de la tabla anidada
                                    nestedTable.ColumnsDefinition(columns =>
                                    {

                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(79); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna
                                        columns.ConstantColumn(16); // Tercera columna


                                    });

                                    // Fila dentro de la tabla anidada 
                                    nestedTable.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("1 ÓRGANO DE LOS\r\nSENTIDOS").FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("3 CARDIO\r\nVASCULAR").FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("5.  GENITAL").FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("7. MÚSCULO\r\nESQUELÉTICO").FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("9. HEMO LINFÁTICO").FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("2. RESPIRATORIO").FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("4. DIGESTIVO").FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("6. URINARIO").FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("8. ENDOCRINO").FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("10. NERVIOSO").FontSize(5).Bold().AlignEnd();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                });
                            });
                            table.Cell().MinHeight(13).BorderLeft(2).BorderRight(2).BorderBottom(1).BorderColor("#808080").Text(" Antecedente de hipertensión").FontSize(10);
                            table.Cell().MinHeight(13).BorderLeft(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Text(" Antecedente de hipertensión").FontSize(10);


                        });
                        //SEXTA TABLA
                        contentColumn.Item().PaddingTop(7).Table(table =>
                        {
                            // Definir las columnas de la tabla para el encabezado
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(10); // Encabezado general ocupa toda la fila
                            });

                            // Fila de encabezado general "3 ANTECEDENTES FAMILIARES"
                            table.Cell().MinHeight(14).BorderLeft(2).BorderRight(2).BorderTop(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).Text("6 SIGNOS VITALES Y ANTROPOMETRIA").FontSize(10).Bold();
                            table.Cell().Element(CellStyle =>
                            {
                                // Crear una tabla interna con varias columnas dentro de la celda "padre"
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Añadir columnas dentro de la tabla anidada
                                    nestedTable.ColumnsDefinition(columns =>
                                    {

                                        columns.ConstantColumn(92); // Tercera columna
                                        columns.ConstantColumn(92); // Tercera columna
                                        columns.ConstantColumn(92); // Tercera columna
                                        columns.ConstantColumn(92); // Tercera columna
                                        columns.ConstantColumn(92); // Tercera columna
                                        columns.ConstantColumn(93); // Tercera columna


                                    });

                                    // Fila dentro de la tabla anidada 
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("FECHA DE MEDICIÓN").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("4").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("5").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("6").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("6").Bold().FontSize(5).AlignCenter();
                                    // Fila dentro de la tabla anidada 
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("TEMPERATURA °C").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("3").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("4").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("5").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("6").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("6").Bold().FontSize(5).AlignCenter();


                                });
                            });
                            table.Cell().Element(CellStyle =>
                            {
                                // Crear una tabla interna con varias columnas dentro de la celda "padre"
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Añadir columnas dentro de la tabla anidada
                                    nestedTable.ColumnsDefinition(columns =>
                                    {

                                        columns.ConstantColumn(92); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(47); // Tercera columna


                                    });

                                    // Fila dentro de la tabla anidada 
                                    nestedTable.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("PRESIÓN ARTERIAL").FontSize(5).Bold().AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();

                                });
                            });

                            table.Cell().BorderBottom(2).BorderColor("808080").Element(CellStyle =>
                            {
                                // Crear una tabla interna con varias columnas dentro de la celda "padre"
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Añadir columnas dentro de la tabla anidada
                                    nestedTable.ColumnsDefinition(columns =>
                                    {

                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(46); // Tercera columna
                                        columns.ConstantColumn(47); // Tercera columna


                                    });

                                    // Fila dentro de la tabla anidada 
                                    nestedTable.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("PULSO / min").FontSize(5).Bold().AlignCenter();
                                    nestedTable.Cell().BorderRight(1).BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("FRECUENCIA\r\nRESPIRATORIA").FontSize(5).Bold().AlignCenter();

                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();

                                    nestedTable.Cell().BorderRight(1).BorderBottom(2).BorderColor("#808080").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("PESO / Kg").FontSize(5).Bold().AlignCenter();
                                    nestedTable.Cell().BorderRight(1).BorderBottom(2).BorderColor("#808080").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("TALLA / cm").FontSize(5).Bold().AlignCenter();

                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(10).MinWidth(3).Text("").FontSize(4).AlignCenter();

                                });
                            });



                        });

                        //SEPTIMA TABLA
                        contentColumn.Item().PaddingTop(7).Table(table =>
                        {
                            // Definir las columnas de la tabla principal para el encabezado
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Primera columna
                                columns.RelativeColumn(3); // Segunda columna
                                columns.RelativeColumn(4); // Tercera columna
                            });

                            // Fila de encabezado "7 EXAMEN FÍSICO REGIONAL"
                            table.Cell().MinHeight(14).BorderLeft(2).BorderTop(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).PaddingLeft(20).Text("7 EXAMEN FÍSICO REGIONAL ").FontSize(10).Bold();

                            table.Cell().MinHeight(14).BorderTop(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).PaddingLeft(70).Text("CP = CON EVIDENCIA DE PATOLOGÍA: MARCAR \"X\" Y DESCRIBIR ").FontSize(6).Bold().AlignCenter();

                            table.Cell().MinHeight(14).BorderRight(2).BorderTop(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).PaddingLeft(65).Text("SP = SIN EVIDENCIA DE PATOLOGÍA:\r\n MARCAR \"X\" Y NO DESCRIBIR\r\n").FontSize(6).Bold().AlignCenter();

                            // Aquí creamos una nueva celda para la tabla interna con 18 columnas
                            table.Cell().ColumnSpan(3).Element(CellStyle =>
                            {
                                // Crear una tabla interna con 18 columnas
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Definir 18 columnas dentro de la tabla anidada
                                    nestedTable.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(70);  // Columna 1
                                        columns.ConstantColumn(16);  // Columna 2
                                        columns.ConstantColumn(10);  // Columna 3
                                        columns.ConstantColumn(70);  // Columna 4
                                        columns.ConstantColumn(10);  // Columna 5
                                        columns.ConstantColumn(10);  // Columna 6
                                        columns.ConstantColumn(70);  // Columna 7
                                        columns.ConstantColumn(10);  // Columna 8
                                        columns.ConstantColumn(10);  // Columna 9
                                        columns.ConstantColumn(70);  // Columna 10
                                        columns.ConstantColumn(10);  // Columna 11
                                        columns.ConstantColumn(10);  // Columna 12
                                        columns.ConstantColumn(70);  // Columna 13
                                        columns.ConstantColumn(10);  // Columna 14
                                        columns.ConstantColumn(10);  // Columna 15
                                        columns.ConstantColumn(79);  // Columna 16
                                        columns.ConstantColumn(10);  // Columna 17
                                        columns.ConstantColumn(10);  // Columna 18
                                    });

                                    // Fila dentro de la tabla anidada con 18 celdas creadas manualmente
                                    // Aquí agregas todas las celdas para la tabla de 18 columnas
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderLeft(2).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderLeft(2).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderLeft(2).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderLeft(2).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderLeft(2).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("CP").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderLeft(2).BorderBottom(1).BorderColor("#C6C2C2").Background("#99ccff").MinHeight(10).MinWidth(3).Text("SP").Bold().FontSize(5).AlignCenter();
                                    // Continúa agregando las celdas necesarias
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(12).MinWidth(3).Text("1. CABEZA").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();

                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(12).MinWidth(3).Text("2. CUELLO").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();

                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(10).MinWidth(3).Text("3. TÓRAX").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();

                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(12).MinWidth(3).Text("4. ABDOMEN").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();

                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(12).MinWidth(3).Text("5. PELVIS").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();

                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(12).MinWidth(3).Text("6 . EXTREMIDADES").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();
                                    nestedTable.Cell().BorderTop(1).BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3).Text("").Bold().FontSize(5).AlignCenter();


                                });
                            });

                            // Aquí agregamos una segunda tabla anidada que abarque todo el ancho
                            table.Cell().ColumnSpan(3).Element(CellStyle =>
                            {
                                // Crear una tabla interna con una sola columna
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderBottom(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Definir una sola columna
                                    nestedTable.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(1); // Una columna que abarca todo el ancho
                                    });

                                    // Contenido de la tabla de una sola columna
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(12).MinWidth(3)
                                        .Text("Contenido de la tabla anidada con una sola columna").FontSize(9).AlignStart();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(12).MinWidth(3)
                                       .Text("Contenido de la tabla anidada con una sola columna").FontSize(9).AlignStart();
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(12).MinWidth(3)
                                       .Text("Contenido de la tabla anidada con una sola columna").FontSize(9).AlignStart();
                                });
                            });
                        });

                        //OCTAVA TABLA

                        contentColumn.Item().PaddingTop(7).Table(table =>
                        {
                            // Definir las columnas de la tabla principal con tamaños específicos
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(170);  // Columna 1 (Diagnóstico)
                                columns.ConstantColumn(95);  // Columna 3 (CIE)
                                columns.ConstantColumn(19);  // Columna 4 (PRE)
                                columns.ConstantColumn(20);  // Columna 5 (DEF)
                                columns.ConstantColumn(12);  // Columna 6 (Espacio en blanco)
                                columns.ConstantColumn(193);  // Columna 7 (CIE)
                                columns.ConstantColumn(13);  // Columna 8 (PRE)
                                columns.ConstantColumn(16);  // Columna 9 (DEF)
                                columns.ConstantColumn(16);  // Columna 9 (DEF)
                            });

                            // Fila de encabezado "8 DIAGNOSTICO"
                            table.Cell().MinHeight(14).BorderLeft(2).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).Text("8 DIAGNOSTICO").FontSize(10).Bold();


                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).Text("PRE = PRESUNTIVO\r\nDEF = DEFINITIVO").FontSize(7).Bold();

                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignCenter().PaddingTop(3).MinWidth(2).Text("CIE").FontSize(6).Bold();
                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                                CellStyle.Background("#ccccff")).AlignCenter().PaddingTop(3).MinWidth(2).Text("PRE").FontSize(6).Bold();
                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                                CellStyle.Background("#ccccff")).AlignCenter().PaddingTop(3).MinWidth(2).Text("DEF").FontSize(6).Bold();
                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                               CellStyle.Background("#ccccff")).AlignCenter().PaddingTop(3).MinWidth(2).Text("").FontSize(6).Bold();
                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                               CellStyle.Background("#ccccff")).AlignCenter().PaddingTop(3).MinWidth(2).Text("CIE").FontSize(6).Bold();
                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                               CellStyle.Background("#ccccff")).AlignCenter().PaddingTop(3).MinWidth(2).Text("PRE").FontSize(6).Bold();

                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Element(CellStyle =>
                                               CellStyle.Background("#ccccff")).AlignCenter().PaddingTop(3).MinWidth(2).Text("DEF").FontSize(6).Bold();

                            // Las demás columnas siguen el mismo patrón...

                            // Subtabla con 10 columnas
                            table.Cell().ColumnSpan(9).Element(CellStyle =>
                            {
                                // Crear una subtabla con 10 columnas
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderBottom(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Definir 10 columnas dentro de la subtabla
                                    nestedTable.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(14);  // Columna 1
                                        columns.ConstantColumn(250);   // Columna 2 (Proporción más amplia)
                                        columns.ConstantColumn(20);  // Columna 3
                                        columns.ConstantColumn(18);  // Columna 4
                                        columns.ConstantColumn(16);  // Columna 5
                                        columns.ConstantColumn(14);  // Columna 6
                                        columns.RelativeColumn(2);   // Columna 7 (Proporción más amplia)
                                        columns.ConstantColumn(18);  // Columna 8
                                        columns.ConstantColumn(18);  // Columna 9
                                        columns.ConstantColumn(18);  // Columna 10
                                    });

                                    // Fila dentro de la subtabla con 10 celdas
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(12).MinWidth(3)
                                        .Text("1").FontSize(9).AlignCenter();
                                    nestedTable.Cell().BorderBottom(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(12).MinWidth(3)
                                        .Text("").FontSize(9).AlignCenter();
                                    // Fila dentro de la subtabla con 10 celdas
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(12).MinWidth(3)
                                        .Text("1").FontSize(9).AlignCenter();
                                    nestedTable.Cell().BorderBottom(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3)
                                        .Text("").FontSize(9).AlignCenter();
                                    nestedTable.Cell().BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3)
                                        .Text("").FontSize(9).AlignCenter();
                                    // Fila dentro de la subtabla con 10 celdas
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ccffcc").MinHeight(12).MinWidth(3)
                                        .Text("2").FontSize(9).AlignCenter();
                                    nestedTable.Cell().BorderBottom(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(12).MinWidth(3)
                                        .Text("").FontSize(9).AlignCenter();
                                    // Fila dentro de la subtabla con 10 celdas
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#FFFFFF").MinHeight(12).MinWidth(3)
                                        .Text("1").FontSize(9).AlignCenter();
                                    nestedTable.Cell().BorderBottom(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3)
                                        .Text("").FontSize(9).AlignCenter();
                                    nestedTable.Cell().BorderBottom(1).BorderLeft(1).BorderColor("#C6C2C2").Background("#ffff99").MinHeight(12).MinWidth(3)
                                        .Text("").FontSize(9).AlignCenter();
                                    // Continúa con las celdas...
                                });
                            });
                        });


                        //Novena TABLA

                        contentColumn.Item().PaddingTop(7).Table(table =>
                        {
                            // Definir las columnas de la tabla principal con dos columnas
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);  // Columna 1
                                columns.RelativeColumn(1);  // Columna 2
                            });

                            // Fila de encabezado "9 PLANES DE TRATAMIENTO"
                            table.Cell().MinHeight(14).BorderLeft(2).BorderTop(2).BorderBottom(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignLeft().PaddingTop(3).Text("9 PLANES DE TRATAMIENTO ").FontSize(10).Bold();

                            // Segunda columna con la descripción
                            table.Cell().MinHeight(14).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Element(CellStyle =>
                                CellStyle.Background("#ccccff")).AlignRight().PaddingTop(3).Text("REGISTRAR LOS PLANES: DIAGNOSTICO, TERAPÉUTICO Y\r\nEDUCACIONAL").FontSize(7);

                            // Subtabla debajo del encabezado que abarca todo el ancho
                            table.Cell().ColumnSpan(2).Element(CellStyle =>
                            {
                                // Crear una subtabla con una columna y cuatro filas de manera estática
                                CellStyle.Background("#ffffff").BorderLeft(2).BorderRight(2).BorderBottom(2).BorderColor("#808080").Table(nestedTable =>
                                {
                                    // Definir una sola columna en la subtabla
                                    nestedTable.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(1);  // Una columna que abarca todo el ancho
                                    });

                                    // Primera fila
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffffff").MinHeight(20).MinWidth(3)
                                        .Text("Farmacologica\r\n").FontSize(9).AlignLeft();

                                    // Segunda fila
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffffff").MinHeight(20).MinWidth(3)
                                        .Text(" ").FontSize(9).AlignLeft();

                                    // Tercera fila
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffffff").MinHeight(20).MinWidth(3)
                                        .Text("").FontSize(9).AlignLeft();

                                    // Cuarta fila
                                    nestedTable.Cell().Border(1).BorderColor("#C6C2C2").Background("#ffffff").MinHeight(20).MinWidth(3)
                                        .Text("").FontSize(9).AlignLeft();
                                });
                            });
                        });

                        contentColumn.Item().PaddingTop(50).Table(table =>
                        {
                            // Definir las columnas de la tabla principal con medidas específicas en puntos (pt)
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(54);  // Columna 1 (FECHA)
                                columns.ConstantColumn(57);  // Columna 2 (Valor de FECHA)
                                columns.ConstantColumn(30);  // Columna 3 (HORA)
                                columns.ConstantColumn(54);  // Columna 4 (Valor de HORA)
                                columns.ConstantColumn(57);  // Columna 5 (NOMBRE DEL PROFESIONAL)
                                columns.ConstantColumn(100); // Columna 6 (Valor del NOMBRE)
                                columns.ConstantColumn(57);  // Columna 7 (Número del Profesional)
                                columns.ConstantColumn(50);  // Columna 8 (FIRMA)
                                columns.ConstantColumn(40);  // Columna 9 (Campo vacío para FIRMA)
                                columns.ConstantColumn(30);  // Columna 10 (HOJA)
                                columns.ConstantColumn(22);  // Columna 11 (Valor de HOJA)
                            });

                            // Fila con las celdas del content que abarcan el ancho completo de la página
                            table.Cell().Element(CellStyle => CellStyle.Background("#ccffcc").Border(1)).AlignCenter().Text("FECHA").FontSize(8);
                            table.Cell().Element(CellStyle => CellStyle.Background("#FFFFFF").Border(1)).AlignCenter().Text("2024-06-11").FontSize(8);
                            table.Cell().Element(CellStyle => CellStyle.Background("#ccffcc").Border(1)).AlignCenter().Text("HORA").FontSize(8);
                            table.Cell().Element(CellStyle => CellStyle.Background("#ffffff").Border(1)).AlignCenter().Text("10:07").FontSize(8);
                            table.Cell().Element(CellStyle => CellStyle.Background("#ccffcc").Border(1)).AlignCenter().Text("NOMBRE DEL\r\nPROFESIONAL").FontSize(7);
                            table.Cell().Element(CellStyle => CellStyle.Background("#ffffff").Border(1)).AlignCenter().Text("ANGELICA MARIELA ACOSTA SIL").FontSize(6);
                            table.Cell().Element(CellStyle => CellStyle.Background("#ffffff").Border(1)).AlignCenter().Text("0503257362").FontSize(8);
                            table.Cell().Element(CellStyle => CellStyle.Background("#ccffcc").Border(1)).AlignCenter().Text("FIRMA").FontSize(8);
                            table.Cell().Element(CellStyle => CellStyle.Background("#ffffff").Border(1)).AlignCenter().Text("").FontSize(8);
                            table.Cell().Element(CellStyle => CellStyle.Background("#ccffcc").Border(1)).AlignCenter().Text("HOJA").FontSize(8);
                            table.Cell().Element(CellStyle => CellStyle.Background("#ffffff").Border(1)).AlignCenter().Text("1").FontSize(8);
                        });



                    });

                    // Footer de la página
                    // Footer de la página
                    page.Footer().Height(20).PaddingHorizontal(2).Row(row =>
                    {
                        // Texto a la izquierda
                        row.RelativeItem().AlignLeft().Text(text =>
                        {
                            text.Span("SNS-MSP / HCU-form.002 / 2008")
                                .FontSize(7)
                                .Bold();
                        });

                        // Texto a la derecha
                        row.RelativeItem().AlignRight().Text(text =>
                        {
                            text.Span("CONSULTA EXTERNA - ANAMNESIS Y EXAMEN FÍSICO")
                                .FontSize(9)
                                .Bold();
                        });
                    });






                });
                // Segunda página

                container.Page(page =>
                {
                    page.Margin(20);
                    page.Size(PageSizes.A4);

                    page.DefaultTextStyle(x => x.FontFamily("Arial").FontSize(10));

                    // Contenido de la segunda página con una sola tabla de 5 columnas
                    page.Content().Column(column =>
                    {
                        // Primera tabla de cinco columnas
                        column.Item().Table(table =>
                        {
                            // Definir las columnas de la tabla principal con cinco columnas
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(135);  // Columna 1
                                columns.ConstantColumn(135);  // Columna 2
                                columns.ConstantColumn(10);   // Columna 3 (espaciador)
                                columns.ConstantColumn(135);  // Columna 4
                                columns.ConstantColumn(135);  // Columna 5
                            });

                            // Fila de datos
                            table.Cell().MinHeight(20).BorderLeft(2).BorderTop(2).BorderBottom(2).BorderColor("#808080").Background("#ccccff")
                                .Text("10 EVOLUCIÓN").FontSize(10).AlignLeft().Bold();

                            table.Cell().MinHeight(20).BorderTop(2).BorderBottom(2).BorderRight(2).BorderColor("#808080").Background("#ccccff")
                                .Text("FIRMAR AL PIE DE CADA NOTA").FontSize(7).AlignEnd();

                            table.Cell().MinHeight(20).BorderColor("#808080").Background("#ffffff")
                                .Text("").FontSize(9).AlignLeft().Bold();

                            table.Cell().MinHeight(20).BorderTop(2).BorderBottom(2).BorderLeft(2).BorderColor("#808080").Background("#ccccff")
                                .Text("11 PRESCRIPCIONES").FontSize(9).AlignLeft();

                            table.Cell().MinHeight(20).BorderRight(2).BorderBottom(2).BorderTop(2).BorderColor("#808080").Background("#ccccff")
                                .Text("FIRMAR AL PIE DE CADA PRESCRIPCIÓN").FontSize(5).AlignRight();
                        });

                        // Espacio entre tablas

                        // NUEVA TABLA de seis columnas
                        column.Item().Table(table =>
                        {
                            // Definir las columnas de la nueva tabla con seis columnas
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(100);  // Columna 1
                                columns.ConstantColumn(120);  // Columna 2
                                columns.ConstantColumn(50);   // Columna 3
                                columns.ConstantColumn(150);  // Columna 4
                                columns.ConstantColumn(130);  // Columna 5
                                columns.ConstantColumn(80);   // Columna 6
                            });

                            // Fila de encabezados
                            table.Cell().Padding(5).Border(1).Background("#ccccff").Text("Columna 1").FontSize(10).Bold().AlignCenter();
                            table.Cell().Padding(5).Border(1).Background("#ccccff").Text("Columna 2").FontSize(10).Bold().AlignCenter();
                            table.Cell().Padding(5).Border(1).Background("#ccccff").Text("Columna 3").FontSize(10).Bold().AlignCenter();
                            table.Cell().Padding(5).Border(1).Background("#ccccff").Text("Columna 4").FontSize(10).Bold().AlignCenter();
                            table.Cell().Padding(5).Border(1).Background("#ccccff").Text("Columna 5").FontSize(10).Bold().AlignCenter();
                            table.Cell().Padding(5).Border(1).Background("#ccccff").Text("Columna 6").FontSize(10).Bold().AlignCenter();

                            // Fila de datos de ejemplo
                            table.Cell().Padding(5).Border(1).Background("#ffffff").Text("Dato 1").FontSize(9).AlignCenter();
                            table.Cell().Padding(5).Border(1).Background("#ffffff").Text("Dato 2").FontSize(9).AlignCenter();
                            table.Cell().Padding(5).Border(1).Background("#ffffff").Text("Dato 3").FontSize(9).AlignCenter();
                            table.Cell().Padding(5).Border(1).Background("#ffffff").Text("Dato 4").FontSize(9).AlignCenter();
                            table.Cell().Padding(5).Border(1).Background("#ffffff").Text("Dato 5").FontSize(9).AlignCenter();
                            table.Cell().Padding(5).Border(1).Background("#ffffff").Text("Dato 6").FontSize(9).AlignCenter();
                        });
                    });




                    // Footer de la segunda página
                    page.Footer().Height(20).PaddingHorizontal(2).Row(row =>
                    {
                        row.RelativeItem().AlignLeft().Text(text =>
                        {
                            text.Span("SNS-MSP / HCU-form.002 / 2008").FontSize(7).Bold();
                        });

                        row.RelativeItem().AlignRight().Text(text =>
                        {
                            text.Span("CONSULTA EXTERNA - ANAMNESIS Y EXAMEN FÍSICO").FontSize(9).Bold();
                        });
                    });
                });


            });
        }

        private IDocument CreateLaboratorioDocument(Consultation consulta)
        {
            // Formato especializado de laboratorio, tamaño A4 con márgenes pequeños
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(10);
                    page.Header().Text("Resultados de Laboratorio").FontSize(16).Bold().AlignCenter();
                    page.Content().Column(column =>
                    {
                        column.Item().Text($"Examen: {consulta.ConsultaPrincipal}");
                        column.Item().Text($"Resultado: {consulta.ReconocimientoFarmacologico}");
                        column.Item().Text("Detalles adicionales...");
                    });
                    page.Footer().AlignCenter().Text("Laboratorio Central");
                });
            });
        }

        private IDocument CreateImagenDocument(Consultation consulta)
        {
            // Informe de imagenología, tamaño A3 con orientación horizontal
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A3);
                  
                    page.Margin(50);
                    page.Header().Text("Informe de Imagenología").FontSize(18).Bold().AlignCenter();
                    page.Content().Column(column =>
                    {
                        column.Item().Text($"Paciente: {consulta.PacienteConsultaPNavigation.PrimernombrePacientes}");
                        column.Item().Text($"Examen: {consulta.EnfermedadConsulta}");
                        column.Item().Text($"Observaciones: {consulta.Laboratorios}");
                    });
                    page.Footer().AlignRight().Text("Firma del radiólogo");
                });
            });
        }

        [HttpGet("Buscar")]
        public async Task<IActionResult> BuscarPacientes(int? cedula, string primerNombre, string primerApellido)
        {
            if (cedula == null && string.IsNullOrEmpty(primerNombre) && string.IsNullOrEmpty(primerApellido))
            {
                return BadRequest("Debe proporcionar al menos un criterio de búsqueda: cédula, primer nombre o primer apellido.");
            }

            var pacientes = await _consultationService.BuscarPacientesAsync(cedula, primerNombre, primerApellido);

            if (pacientes == null || !pacientes.Any())
            {
                return NotFound("No se encontraron pacientes con los criterios proporcionados.");
            }

            return Ok(pacientes);
        }

   

    }

    // DTO (Data Transfer Object) que se espera recibir en la solicitud HTTP
    public class ConsultationDto
    {
        public string UsuarioCreacion { get; set; }
        public string Historial { get; set; }
        public int PacienteId { get; set; }
        public string Motivo { get; set; }
        public string Enfermedad { get; set; }
        public string NombrePariente { get; set; }
        public string SignosAlarma { get; set; }
        public string ReconoFarmacologicas { get; set; }
        public int TipoPariente { get; set; }
        public string TelefonoPariente { get; set; }
        public string Temperatura { get; set; }
        public string FrecuenciaRespiratoria { get; set; }
        public string PresionArterialSistolica { get; set; }
        public string PresionArterialDiastolica { get; set; }
        public string Pulso { get; set; }
        public string Peso { get; set; }
        public string Talla { get; set; }
        public string PlanTratamiento { get; set; }
        public string Observacion { get; set; }
        public string AntecedentesPersonales { get; set; }
        public int DiasIncapacidad { get; set; }
        public int MedicoId { get; set; }
        public int EspecialidadId { get; set; }
        public int TipoConsulta { get; set; }
        public string NotasEvolucion { get; set; }
        public string ConsultaPrincipal { get; set; }
        public int EstadoConsulta { get; set; }

        // Otros parámetros adicionales (organos_sistemas, examen_fisico, etc.)
        public bool OrgSentidos { get; set; }
        public string ObserOrgSentidos { get; set; }
        public bool Respiratorio { get; set; }
        public string ObserRespiratorio { get; set; }
        public bool CardioVascular { get; set; }
        public string ObserCardioVascular { get; set; }
        public bool Digestivo { get; set; }
        public string ObserDigestivo { get; set; }
        public bool Genital { get; set; }
        public string ObserGenital { get; set; }
        public bool Urinario { get; set; }
        public string ObserUrinario { get; set; }
        public bool MEsqueletico { get; set; }
        public string ObserMEsqueletico { get; set; }
        public bool Endocrino { get; set; }
        public string ObserEndocrino { get; set; }
        public bool Linfatico { get; set; }
        public string ObserLinfatico { get; set; }
        public bool Nervioso { get; set; }
        public string ObserNervioso { get; set; }

        // Parámetros para examen físico
        public bool Cabeza { get; set; }
        public string ObserCabeza { get; set; }
        public bool Cuello { get; set; }
        public string ObserCuello { get; set; }
        public bool Torax { get; set; }
        public string ObserTorax { get; set; }
        public bool Abdomen { get; set; }
        public string ObserAbdomen { get; set; }
        public bool Pelvis { get; set; }
        public string ObserPelvis { get; set; }
        public bool Extremidades { get; set; }
        public string ObserExtremidades { get; set; }

        // Parámetros tipo tabla (listas de objetos)
        public List<ConsultaAlergia> Alergias { get; set; }
        public List<ConsultaCirugia> Cirugias { get; set; }
        public List<ConsultaMedicamento> Medicamentos { get; set; }
        public List<ConsultaLaboratorio> Laboratorio { get; set; }
        public List<ConsultaImagen> Imagenes { get; set; }
        public List<ConsultaDiagnostico> Diagnosticos { get; set; }
    }
}
