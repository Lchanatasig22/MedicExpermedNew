using MedicExpermed.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MedicExpermed.Services
{
    public class AppointmentService
    {
        private readonly medicossystembdIContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(medicossystembdIContext context, IHttpContextAccessor httpContextAccessor, ILogger<AppointmentService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// Metodo obtener todas las citas
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<List<Citum>> GetAllAppointmentsAsync()
        {
            // Obtener el nombre de usuario de la sesión fuera del contexto asincrónico
            var nombreUsuario = _httpContextAccessor.HttpContext?.Session?.GetString("UsuarioNombre");

            // Validar que el nombre de usuario esté disponible
            if (string.IsNullOrEmpty(nombreUsuario))
            {
                throw new InvalidOperationException("El nombre de usuario no está disponible en la sesión.");
            }

            // Obtener todas las citas filtradas por el usuario de creación
            try
            {
                var citas = await _context.Cita
                    .Where(c => c.UsuariocreacionCita == nombreUsuario)
                    .Include(c => c.Paciente)
                    .Include(c => c.Usuario)
                    .ToListAsync();

                return citas;
            }
            catch (Exception ex)
            {
                // Manejo de errores específico y con un mensaje claro
                throw new Exception("Ocurrió un error al obtener la lista de citas.", ex);
            }
        }

        /// <summary>
        /// Obtener la cita por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<Citum> GetAppointmentByIdAsync(int id)
        {
            try
            {
                var appointment = await _context.Cita
     .Where(c => c.EstadoCita == 1 && c.IdCita == id)
     .Include(c => c.Paciente)
     .Include(c => c.Usuario)
     .FirstOrDefaultAsync();


                if (appointment == null)
                {
                    throw new KeyNotFoundException("Cita no encontrada.");
                }

                return appointment;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al obtener la cita por ID.", ex);
            }
        }

        /// <summary>
        /// Metodo para generar una cita
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns></returns>
        public async Task<int> CreateAppointmentAsync(Citum appointment)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("sp_CreateAppointment", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar parámetros
                    command.Parameters.AddWithValue("@usuariocreacion_cita", appointment.UsuariocreacionCita);
                    command.Parameters.AddWithValue("@fechadelacita_cita", appointment.FechadelacitaCita);
                    command.Parameters.AddWithValue("@horadelacita_cita", appointment.HoradelacitaCita);
                    command.Parameters.AddWithValue("@usuario_id", appointment.UsuarioId);
                    command.Parameters.AddWithValue("@paciente_id", appointment.PacienteId);
                    command.Parameters.AddWithValue("@consulta_id", appointment.ConsultaId.HasValue ? (object)appointment.ConsultaId.Value : DBNull.Value);
                    command.Parameters.AddWithValue("@motivo", appointment.Motivo);
                    command.Parameters.AddWithValue("@estado_cita", appointment.EstadoCita);

                    await connection.OpenAsync();

                    // Ejecutar el comando y obtener el ID de la cita
                    var idCita = await command.ExecuteScalarAsync();
                    return Convert.ToInt32(idCita);
                }
            }
        }


        /// <summary>
        /// Metodo para obtener las horas
        /// </summary>
        /// <param name="usuarioId"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public async Task<List<Citum>> ObtenerHorasDisponiblesAsync(int medicoId, DateTime fechaCita)
        {
            var horasDisponibles = new List<Citum>();

            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("dbo.sp_GetAvailableHours", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros para el procedimiento almacenado
                    command.Parameters.AddWithValue("@usuario_id", medicoId);
                    command.Parameters.AddWithValue("@fecha", fechaCita);

                    await connection.OpenAsync();

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            horasDisponibles.Add(new Citum
                            {
                                HoradelacitaCita = reader.GetTimeSpan(reader.GetOrdinal("horadelacitaCita"))
                            });
                        }
                    }
                }
            }

            return horasDisponibles;
        }




        public class AppointmentUpdateException : Exception
        {
            public AppointmentUpdateException(string message) : base(message) { }

            public AppointmentUpdateException(string message, Exception innerException)
                : base(message, innerException) { }
        }

        /// <summary>
        /// Metodo para actualizar una cita
        /// </summary>
        /// <param name="appointment"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        public async Task<bool> UpdateAppointmentAsync(Citum appointment)
        {
            // Validación previa de los datos
            if (appointment == null)
            {
                throw new ArgumentNullException(nameof(appointment), "La cita no puede ser nula.");
            }

            if (appointment.IdCita <= 0)
            {
                throw new ArgumentException("ID de cita inválido.", nameof(appointment.IdCita));
            }

            try
            {
                using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    using (var command = new SqlCommand("sp_UpdateAppointment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar los parámetros para la actualización
                        command.Parameters.AddWithValue("@id_cita", appointment.IdCita);
                        command.Parameters.AddWithValue("@fechadelacita_cita", appointment.FechadelacitaCita);
                        command.Parameters.AddWithValue("@horadelacita_cita", appointment.HoradelacitaCita);
                        command.Parameters.AddWithValue("@usuario_id", appointment.UsuarioId);
                        command.Parameters.AddWithValue("@paciente_id", appointment.PacienteId);
                        command.Parameters.AddWithValue("@consulta_id", appointment.ConsultaId.HasValue ? (object)appointment.ConsultaId.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@motivo", appointment.Motivo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@estado_cita", appointment.EstadoCita);

                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected < 0)
                        {
                            return true; // Actualización exitosa
                        }
                        else
                        {
                            throw new AppointmentUpdateException("No se actualizó ninguna fila en la base de datos.");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                // Logging de errores específicos de SQL
                _logger.LogError(sqlEx, "Error SQL al intentar actualizar la cita con ID {AppointmentId}", appointment.IdCita);
                throw new AppointmentUpdateException("Error al actualizar la cita en la base de datos.", sqlEx);
            }
            catch (Exception ex)
            {
                // Logging de errores generales
                _logger.LogError(ex, "Error inesperado al intentar actualizar la cita con ID {AppointmentId}", appointment.IdCita);
                throw new AppointmentUpdateException("Ocurrió un error inesperado al intentar actualizar la cita.", ex);
            }
        }

        //  Eliminar Cita


        public async Task<bool> DeleteAppointmentAsync(int appointmentId, int newState)
        {
            if (appointmentId <= 0)
            {
                throw new ArgumentException("ID de cita inválido.", nameof(appointmentId));
            }

            try
            {
                using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
                {
                    using (var command = new SqlCommand("sp_DeleteAppointment", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Agregar los parámetros necesarios
                        command.Parameters.AddWithValue("@id_cita", appointmentId);
                        command.Parameters.AddWithValue("@estado", newState);

                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected < 0)
                        {
                            return true; // Actualización exitosa
                        }
                        else
                        {
                            throw new AppointmentUpdateException("No se encontró la cita con el ID especificado o no se pudo actualizar el estado.");
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                _logger.LogError(sqlEx, "Error SQL al intentar eliminar (cambiar estado) la cita con ID {AppointmentId}", appointmentId);
                throw new AppointmentUpdateException("Error al intentar cambiar el estado de la cita en la base de datos.", sqlEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al intentar eliminar (cambiar estado) la cita con ID {AppointmentId}", appointmentId);
                throw new AppointmentUpdateException("Ocurrió un error inesperado al intentar cambiar el estado de la cita.", ex);
            }
        }


    }
}
