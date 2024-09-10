using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using MedicExpermed.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MedicExpermed.Services
{
    public class AutenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AutenticationService> _logger;
        private readonly medicossystembdIContext _context; // Asume que tienes un DbContext configurado

        public AutenticationService(IHttpContextAccessor httpContextAccessor, ILogger<AutenticationService> logger,  medicossystembdIContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _context = context;
        }

        public async Task<Usuario> ValidateUser(string loginUsuario, string claveUsuario)
        {
            if (string.IsNullOrEmpty(loginUsuario) || string.IsNullOrEmpty(claveUsuario))
            {
                throw new ArgumentException("El login y la clave no pueden estar vacíos.");
            }

            var parameterLoginUsuario = new SqlParameter("@login_usuario", loginUsuario);
            var parameterClaveUsuario = new SqlParameter("@clave_usuario", claveUsuario);

            Usuario user = null;
            try
            {
                using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                {
                    await connection.OpenAsync().ConfigureAwait(false);

                    using (var command = new SqlCommand("[dbo].[sp_ValidateCredentials]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add(parameterLoginUsuario);
                        command.Parameters.Add(parameterClaveUsuario);

                        using (var reader = await command.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            if (await reader.ReadAsync().ConfigureAwait(false))
                            {
                                user = new Usuario
                                {
                                    IdUsuario = GetValueOrDefault<int>(reader, "id_usuario"),
                                    PerfilId = GetValueOrDefault<int>(reader, "perfil_id"),
                                    NombresUsuario = GetValueOrDefault<string>(reader, "nombres_usuario"),
                                    ApellidosUsuario = GetValueOrDefault<string>(reader, "apellidos_usuario"),
                                    LoginUsuario = GetValueOrDefault<string>(reader, "login_usuario"),
                                    Especialidad = new Especialidad
                                    {
                                        IdEspecialidad = GetValueOrDefault<int>(reader, "especialidad_id"),
                                        NombreEspecialidad = GetValueOrDefault<string>(reader, "nombre_especialidad")
                                    },
                                    Perfil = new Perfil
                                    {
                                        DescripcionPerfil = GetValueOrDefault<string>(reader, "descripcion_perfil")
                                    },
                                    Establecimiento = new Establecimiento
                                    {
                                        DireccionEstablecimiento = SafeGetString(reader, "direccion_establecimiento")
                                    }
                                };

                                // Guardar los detalles del usuario en la sesión
                                var session = _httpContextAccessor.HttpContext.Session;
                                session.SetString("UsuarioNombre", user.NombresUsuario);
                                session.SetInt32("UsuarioId", user.IdUsuario);
                                session.SetString("UsuarioApellido", user.ApellidosUsuario);
                                session.SetString("UsuarioDescripcion", string.IsNullOrEmpty(user.Perfil.DescripcionPerfil) ? "Default Description" : user.Perfil.DescripcionPerfil);
                                session.SetString("UsuarioEspecialidad", user.Especialidad.NombreEspecialidad);
                                session.SetString("UsuarioDireccion", user.Establecimiento.DireccionEstablecimiento);

                                if (user.PerfilId != null)
                                {
                                    session.SetInt32("PerfilId", user.PerfilId.Value);
                                }

                                if (user.Especialidad != null && user.Especialidad.IdEspecialidad > 0)
                                {
                                    session.SetInt32("UsuarioIdEspecialidad", user.Especialidad.IdEspecialidad);
                                }
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Ocurrió un error al validar el usuario.");
                throw new Exception("Ocurrió un error al validar el usuario.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error inesperado al validar el usuario.");
                throw new Exception("Ocurrió un error inesperado al validar el usuario.", ex);
            }

            return user;
        }

        private T GetValueOrDefault<T>(SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            if (reader.IsDBNull(ordinal))
                return default(T);
            return (T)reader.GetValue(ordinal);
        }

        private string SafeGetString(SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
        }
    }

//    // Clases de apoyo (asegúrate de tenerlas definidas en tu proyecto)
//    public class Usuario
//    {
//        public int IdUsuario { get; set; }
//        public int? PerfilId { get; set; }
//        public string NombresUsuario { get; set; }
//        public string ApellidosUsuario { get; set; }
//        public string LoginUsuario { get; set; }
//        public Especialidad Especialidad { get; set; }
//        public Perfil Perfil { get; set; }
//        public Establecimiento Establecimiento { get; set; }
//    }

//    public class Especialidad
//    {
//        public int IdEspecialidad { get; set; }
//        public string NombreEspecialidad { get; set; }
//    }

//    public class Perfil
//    {
//        public string DescripcionPerfil { get; set; }
//    }

//    public class Establecimiento
//    {
//        public string DireccionEstablecimiento { get; set; }
//    }
}
