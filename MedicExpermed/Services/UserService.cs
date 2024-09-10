using MedicExpermed.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MedicExpermed.Services
{
    public class UserService
    {

        private readonly medicossystembdIContext _context;

        public UserService(medicossystembdIContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Método para listar usuarios
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<List<Usuario>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Usuarios
    .AsNoTracking()
  /*  .Where(u => u.EstadoUsuario == 1) */// Filtra solo los usuarios con estado 1
    .Include(u => u.Perfil)           // Incluye la relación con Perfil
    .Include(u => u.Especialidad)     // Incluye la relación con Especialidad
    .Include(u => u.Establecimiento)  // Incluye la relación con Establecimiento
    .ToListAsync();

            }
            catch (Exception ex)
            {
                // Manejo del error, por ejemplo, loguear el error y lanzar una excepción personalizada
                // _logger.LogError(ex, "Error obteniendo usuarios");
                throw new ApplicationException("Error al obtener la lista de usuarios.", ex);
            }
        }


        /// <summary>
        /// Usuario by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Usuario> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        /// <summary>
        /// Servicio crear usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task CreateUserAsync(Usuario usuario)
        {
            using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (var command = new SqlCommand("sp_CreateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros esperados por el procedimiento almacenado
                    command.Parameters.AddWithValue("@ci_usuario", usuario.CiUsuario);
                    command.Parameters.AddWithValue("@nombres_usuario", usuario.NombresUsuario);
                    command.Parameters.AddWithValue("@apellidos_usuario", usuario.ApellidosUsuario);
                    command.Parameters.AddWithValue("@telefono_usuario", usuario.TelefonoUsuario);
                    command.Parameters.AddWithValue("@email_usuario", usuario.EmailUsuario);
                    command.Parameters.AddWithValue("@direccion_usuario", usuario.DireccionUsuario);
                    command.Parameters.Add(new SqlParameter("@firmadigital_usuario", SqlDbType.VarBinary)
                    {
                        Value = usuario.FirmadigitalUsuario ?? (object)DBNull.Value
                    });
                    command.Parameters.Add(new SqlParameter("@codigoqr_usuario", SqlDbType.VarBinary)
                    {
                        Value = usuario.CodigoqrUsuario ?? (object)DBNull.Value
                    });
                    command.Parameters.AddWithValue("@codigo_senecyt", usuario.CodigoSenecyt ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@login_usuario", usuario.LoginUsuario);
                    command.Parameters.AddWithValue("@clave_usuario", usuario.ClaveUsuario);
                    command.Parameters.AddWithValue("@estado_usuario", usuario.EstadoUsuario);
                    command.Parameters.AddWithValue("@perfil_id", usuario.PerfilId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@establecimiento_id", usuario.EstablecimientoId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@especialidad_id", usuario.EspecialidadId ?? (object)DBNull.Value);

                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 50000) // CI duplicado
                        {
                            throw new Exception("El usuario con este CI ya existe.");
                        }
                        else if (ex.Number == 50001) // Nombre de usuario duplicado
                        {
                            throw new Exception("El nombre de usuario ya existe.");
                        }
                        else
                        {
                            throw;
                        }
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            await connection.CloseAsync();
                        }
                    }
                }
            }
        }


        //Servicio de edicion usuario
        public async Task UpdateUserAsync(Usuario usuario)
        {
            using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (var command = new SqlCommand("sp_UpdateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros esperados por el procedimiento almacenado
                    command.Parameters.AddWithValue("@id_usuario", usuario.IdUsuario);
                    command.Parameters.AddWithValue("@ci_usuario", usuario.CiUsuario);
                    command.Parameters.AddWithValue("@nombres_usuario", usuario.NombresUsuario);
                    command.Parameters.AddWithValue("@apellidos_usuario", usuario.ApellidosUsuario);
                    command.Parameters.AddWithValue("@telefono_usuario", usuario.TelefonoUsuario);
                    command.Parameters.AddWithValue("@email_usuario", usuario.EmailUsuario);
                    command.Parameters.AddWithValue("@direccion_usuario", usuario.DireccionUsuario);

                    // Manejar posibles null en las imágenes
                    command.Parameters.Add(new SqlParameter("@firmadigital_usuario", SqlDbType.VarBinary)
                    {
                        Value = usuario.FirmadigitalUsuario ?? (object)DBNull.Value
                    });
                    command.Parameters.Add(new SqlParameter("@codigoqr_usuario", SqlDbType.VarBinary)
                    {
                        Value = usuario.CodigoqrUsuario ?? (object)DBNull.Value
                    });

                    command.Parameters.AddWithValue("@codigo_senecyt", usuario.CodigoSenecyt ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@login_usuario", usuario.LoginUsuario);
                    command.Parameters.AddWithValue("@clave_usuario", usuario.ClaveUsuario);
                    command.Parameters.AddWithValue("@estado_usuario", usuario.EstadoUsuario);
                    command.Parameters.AddWithValue("@perfil_id", usuario.PerfilId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@establecimiento_id", usuario.EstablecimientoId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@especialidad_id", usuario.EspecialidadId ?? (object)DBNull.Value);

                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 50000) // CI duplicado o cualquier error específico manejado en el SP
                        {
                            throw new Exception("El usuario con este CI ya existe.");
                        }
                        else if (ex.Number == 50001) // Nombre de usuario duplicado
                        {
                            throw new Exception("El nombre de usuario ya existe.");
                        }
                        else
                        {
                            throw; // Rethrow the exception if it's something else
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Metodo Actualizar Estado Usuario
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task ToggleUserStateAsync(int idUsuario)
        {
            using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (var command = new SqlCommand("sp_UpdateState", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@id_usuario", idUsuario);

                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Error al alternar el estado del usuario.", ex);
                    }
                }
            }
        }



    }
}
