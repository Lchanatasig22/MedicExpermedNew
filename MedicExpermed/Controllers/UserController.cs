using MedicExpermed.Models;
using MedicExpermed.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MedicExpermed.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly medicossystembdIContext _context;

        public UserController(UserService userService, medicossystembdIContext context)
        {
            _userService = userService;
            _context = context;
        }

        //Listar Usuarios

        [HttpGet("Listar-Usuario")]
        public async Task<IActionResult> ListarUsuarios()
        {
            var usuarios = await _userService.GetAllUsersAsync();
            return View(usuarios);
        }

        // Método para crear un usuario vista
        [HttpGet("Nuevo-Usuario")]
        public IActionResult CrearUsuario()
        {
            try
            {
                ViewBag.Perfiles = _context.Perfils.ToList();
                ViewBag.Establecimientos = _context.Establecimientos.ToList();
                ViewBag.Especialidades = _context.Especialidads.ToList(); // Asegúrate de que estás obteniendo las especialidades correctamente
                return View();
            }
            catch (Exception ex)
            {
                // Manejo de errores, muestra un mensaje de error al usuario o redirige a una página de error
                TempData["ErrorMessage"] = "Ocurrió un problema al cargar los datos. Por favor, inténtalo de nuevo.";
                return RedirectToAction("Error", "Home"); // Redirige a una vista de error o maneja según tu lógica
            }
        }


        [HttpPost("Nuevo-Usuario")]
        public async Task<IActionResult> CrearUsuario(Usuario usuario, IFormFile? firmadigitalFile, IFormFile? codigoqrFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Datos inválidos. Por favor, revisa los campos e intenta de nuevo.";
                return View(usuario);
            }

            // Convertir IFormFile a byte[] si se ha subido un archivo
            if (firmadigitalFile != null && firmadigitalFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await firmadigitalFile.CopyToAsync(memoryStream);
                    usuario.FirmadigitalUsuario = memoryStream.ToArray();
                }
            }

            if (codigoqrFile != null && codigoqrFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await codigoqrFile.CopyToAsync(memoryStream);
                    usuario.CodigoqrUsuario = memoryStream.ToArray();
                }
            }

            try
            {
                await _userService.CreateUserAsync(usuario);
                TempData["SuccessMessage"] = "Usuario creado exitosamente.";
                return RedirectToAction("ListarUsuarios", "User");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("El usuario con este CI ya existe."))
                {
                    TempData["ErrorMessage"] = "El usuario con este CI ya existe.";
                }
                else if (ex.Message.Contains("El nombre de usuario ya existe."))
                {
                    TempData["ErrorMessage"] = "El nombre de usuario ya existe.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error inesperado: " + ex.Message;
                }

                await CargarListasDesplegables();
                return View(usuario);
            }
        }


        /// <summary>
        /// Método GET para cargar la vista de edición
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Actualizar-Usuario/{id}")]
        public async Task<IActionResult> ActualizarUsuario(int id)
        {
            var usuario = await _userService.GetUsuarioByIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            // Remover la validación de la contraseña ya que no es necesaria en la carga inicial de la vista
            ModelState.Remove("ClaveUsuario");

            ViewBag.Perfiles = _context.Perfils.ToList();
            ViewBag.Establecimientos = _context.Establecimientos.ToList();
            ViewBag.Especialidades = _context.Especialidads.ToList();

            return View(usuario);
        }

        /// <summary>
        /// Método POST para procesar la actualización
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="firmadigitalFile"></param>
        /// <param name="codigoqrFile"></param>
        /// <returns></returns>
        [HttpPost("Actualizar-Usuario/{id}")]
        public async Task<IActionResult> ActualizarUsuario(Usuario usuario, IFormFile? firmadigitalFile, IFormFile? codigoqrFile)
        {
            if (usuario.IdUsuario > 0) // Actualización de usuario
            {
                // Remover la validación de ClaveUsuario si no se está proporcionando una nueva contraseña
                ModelState.Remove("ClaveUsuario");
            }

            if (!ModelState.IsValid)
            {
                await CargarListasDesplegables();
                TempData["ErrorMessage"] = "Datos inválidos. Por favor, revisa los campos e intenta de nuevo.";
                return View(usuario);
            }

            // Convertir IFormFile a byte[] si se ha subido un archivo
            if (firmadigitalFile != null && firmadigitalFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await firmadigitalFile.CopyToAsync(memoryStream);
                    usuario.FirmadigitalUsuario = memoryStream.ToArray();
                }
            }

            if (codigoqrFile != null && codigoqrFile.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await codigoqrFile.CopyToAsync(memoryStream);
                    usuario.CodigoqrUsuario = memoryStream.ToArray();
                }
            }

            // Mantener la contraseña existente si no se proporciona una nueva
            if (usuario.IdUsuario > 0 && string.IsNullOrEmpty(usuario.ClaveUsuario))
            {
                var usuarioExistente = await _context.Usuarios.FindAsync(usuario.IdUsuario);
                if (usuarioExistente != null)
                {
                    usuario.ClaveUsuario = usuarioExistente.ClaveUsuario;
                }
            }

            try
            {
                await _userService.UpdateUserAsync(usuario);
                TempData["SuccessMessage"] = "Usuario actualizado exitosamente.";
                return RedirectToAction("ListarUsuarios", "User");
            }
            catch (Exception ex)
            {
                await CargarListasDesplegables();
                TempData["ErrorMessage"] = "Error inesperado: " + ex.Message;
                return View(usuario);
            }
        }


        /// Método POST para actualizar el estado del usuario
        [HttpPost("AlternarEstadoUsuario/{id}")]
        public async Task<IActionResult> AlternarEstadoUsuario(int id)
        {
            try
            {
                await _userService.ToggleUserStateAsync(id);
                TempData["SuccessMessage"] = "El estado del usuario ha sido actualizado.";
                return RedirectToAction("ListarUsuarios");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al actualizar el estado del usuario: " + ex.Message;
                return RedirectToAction("ListarUsuarios");
            }
        }

        // Carga de listas
        private async Task CargarListasDesplegables()
        {
            ViewBag.Perfiles = await _context.Perfils.ToListAsync();
            ViewBag.Establecimientos = await _context.Establecimientos.ToListAsync();
            ViewBag.Especialidades = await _context.Especialidads.ToListAsync(); // Asegúrate de que estás obteniendo las especialidades
        }


   
    }

}
