using MedicExpermed.Models;
using MedicExpermed.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicExpermed.Controllers
{
    public class PatientController : Controller
    {

        private readonly PatientService _patientService;
        private readonly CatalogService _catalogService;

        public PatientController(PatientService patientService, CatalogService catalogService)
        {
            _patientService = patientService;
            _catalogService = catalogService;
        }

        /// <summary>
        /// CargarDatosCatalogo
        /// </summary>
        /// <returns></returns>
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
        }

        /// <summary>
        /// Metodo Listr Pacientes
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public async Task<IActionResult> ListarPacientes()
        {
            var pacientes = await _patientService.GetAllPacientesAsync();
            return View(pacientes);
        }

        /// <summary>
        /// Metodo vista crear pacientes
        /// </summary>
        /// <returns></returns>
        [HttpGet ("Creacion-Paciente")]
        public async Task<IActionResult> CrearPaciente()
        {
            ViewBag.UsuarioNombre = HttpContext.Session.GetString("UsuarioNombre");
            int Ecuador = await _catalogService.ObtenerIdNacionalidadEcuatorianoAsync();

            await CargarListasDesplegables();
            var model = new Paciente
            {
                NacionalidadPacientesPa = Ecuador // Nacionalidad por defecto Dpende el valor que tenga en la base
            };
            return View(model);
        }


        /// <summary>
        /// Metodo guardar pacientes
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        [HttpPost("Creacion-Paciente")]
        public async Task<IActionResult> CrearPaciente(Paciente patient)
        {
            try
            {
                int patientId = await _patientService.CreatePatientAsync(patient);
                TempData["SuccessMessage"] = $"Paciente creado exitosamente";
                return RedirectToAction("ListarPacientes");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al crear el paciente: {ex.Message}";
                await CargarListasDesplegables();
                ViewBag.UsuarioNombre = HttpContext.Session.GetString("UsuarioNombre");

                return View(patient);
            }
        }


        /// <summary>
        /// Actualizar Paciente Vista
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet ("Actualizar-Paciente")]
        public async Task<IActionResult> EditarPaciente(int id)
        {
            ViewBag.UsuarioNombre = HttpContext.Session.GetString("UsuarioNombre");

            var paciente = await _patientService.GetPacienteByIdAsync(id);
            if (paciente == null)
            {
                return NotFound();
            }

            await CargarListasDesplegables();
            return View(paciente);
        }


        /// <summary>
        /// Actualizacion Paciente
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        [HttpPost("Actualizar-Paciente")]
        public async Task<IActionResult> EditarPaciente(Paciente patient)
        {
            try
            {
                int patientId = await _patientService.UpdatePatientAsync(patient);
                TempData["SuccessMessage"] = $"Paciente actualizado exitosamente con ID: {patientId}";
                return RedirectToAction("ListarPacientes");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al actualizar el paciente: {ex.Message}";
                ViewBag.UsuarioNombre = HttpContext.Session.GetString("UsuarioNombre");

                await CargarListasDesplegables();
                return View(patient);
            }
        }

        //Eliminar paciente
        // Acción para eliminar un paciente
        [HttpPost]
        public async Task<IActionResult> DeletePatient(int id)
        {
            try
            {
                await _patientService.DeletePatientAsync(id);
                TempData["SuccessMessage"] = "Paciente eliminado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error al eliminar el paciente: {ex.Message}";
            }

            return RedirectToAction("ListarPacientes");
        }


    }
}
