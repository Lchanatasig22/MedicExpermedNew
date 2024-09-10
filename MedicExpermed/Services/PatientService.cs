using MedicExpermed.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MedicExpermed.Services
{

    public class PatientService
    {
        private readonly medicossystembdIContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PatientService> _logger;


        /// <summary>
        /// Siempre que se cree un servicio se tiene que instanciar el DBContext
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="logger"></param>
        public PatientService(medicossystembdIContext context, IHttpContextAccessor httpContextAccessor, ILogger<PatientService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;

        }

        /// <summary>
        /// Metodo para traer todos los pacientes
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<List<Paciente>> GetAllPacientesAsync()
        {
            // Obtener el nombre de usuario de la sesión fuera del contexto asincrónico
            var NombreUsuario = _httpContextAccessor.HttpContext?.Session?.GetString("UsuarioNombre");

            // Validar que el nombre de usuario esté disponible
            if (string.IsNullOrEmpty(NombreUsuario))
            {
                throw new InvalidOperationException("El nombre de usuario no está disponible en la sesión.");
            }

            // Filtrar los pacientes por el usuario de creación y estado activo
            try
            {
                var pacientes = await _context.Pacientes
                    .Where(p => p.UsuariocreacionPacientes == NombreUsuario && p.EstadoPacientes == 1)
                    .Include(p => p.NacionalidadPacientesPaNavigation)
                    .ToListAsync();

                return pacientes;
            }
            catch (Exception ex)
            {
                // Manejo de errores específico y con un mensaje claro
                throw new Exception("Ocurrió un error al obtener la lista de pacientes.", ex);
            }
        }

        /// <summary>
        /// Metodo para la creacion de nuevos pacientes
        /// </summary>
        /// <param name="patient"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> CreatePatientAsync(Paciente patient)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("sp_CreatePatient", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros esperados por el procedimiento almacenado
                    command.Parameters.AddWithValue("@usuariocreacion_pacientes", patient.UsuariocreacionPacientes);
                    command.Parameters.AddWithValue("@tipodocumento_pacientes_ca", patient.TipodocumentoPacientesCa);
                    command.Parameters.AddWithValue("@ci_pacientes", patient.CiPacientes);
                    command.Parameters.AddWithValue("@primernombre_pacientes", patient.PrimernombrePacientes);
                    command.Parameters.AddWithValue("@segundonombre_pacientes", (object)patient.SegundonombrePacientes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@primerapellido_pacientes", patient.PrimerapellidoPacientes);
                    command.Parameters.AddWithValue("@segundoapellido_pacientes", (object)patient.SegundoapellidoPacientes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@sexo_pacientes_ca", (object)patient.SexoPacientesCa ?? DBNull.Value);
                    command.Parameters.AddWithValue("@fechanacimiento_pacientes", patient.FechanacimientoPacientes);
                    command.Parameters.AddWithValue("@edad_pacientes", patient.EdadPacientes);
                    command.Parameters.AddWithValue("@tiposangre_pacientes_ca", (object)patient.TiposangrePacientesCa ?? DBNull.Value);
                    command.Parameters.AddWithValue("@donante_pacientes", patient.DonantePacientes);
                    command.Parameters.AddWithValue("@estadocivil_pacientes_ca", patient.EstadocivilPacientesCa);
                    command.Parameters.AddWithValue("@formacionprofesional_pacientes_ca", patient.FormacionprofesionalPacientesCa);
                    command.Parameters.AddWithValue("@telefonofijo_pacientes", (object)patient.TelefonofijoPacientes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@telefonocelular_pacientes", (object)patient.TelefonocelularPacientes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@email_pacientes", (object)patient.EmailPacientes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@nacionalidad_pacientes_pa", patient.NacionalidadPacientesPa);
                    command.Parameters.AddWithValue("@provincia_pacientes_l", patient.ProvinciaPacientesL);
                    command.Parameters.AddWithValue("@direccion_pacientes", patient.DireccionPacientes);
                    command.Parameters.AddWithValue("@ocupacion_pacientes", (object)patient.OcupacionPacientes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@empresa_pacientes", (object)patient.EmpresaPacientes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@segurosalud_pacientes_ca", (object)patient.SegurosaludPacientesCa ?? DBNull.Value);
                    command.Parameters.AddWithValue("@estado_pacientes", patient.EstadoPacientes);

                    try
                    {
                        await connection.OpenAsync();
                        var result = await command.ExecuteScalarAsync(); // Devuelve el ID del paciente insertado

                        return Convert.ToInt32(result); // Convierte el resultado a int
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores, puedes registrar el error o lanzar la excepción
                        throw new Exception("Error al crear el paciente.", ex);
                    }
                }
            }
        }


        //Metodo Actualizar Paciente
        public async Task<int> UpdatePatientAsync(Paciente patient)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("sp_UpdatePatient", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar los parámetros esperados por el procedimiento almacenado
                    command.Parameters.AddWithValue("@id_pacientes", patient.IdPacientes);
                    command.Parameters.AddWithValue("@usuariomodificacion_pacientes", patient.UsuariomodificacionPacientes);
                    command.Parameters.AddWithValue("@tipodocumento_pacientes_ca", patient.TipodocumentoPacientesCa);
                    command.Parameters.AddWithValue("@ci_pacientes", patient.CiPacientes);
                    command.Parameters.AddWithValue("@primernombre_pacientes", patient.PrimernombrePacientes);
                    command.Parameters.AddWithValue("@segundonombre_pacientes", (object)patient.SegundonombrePacientes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@primerapellido_pacientes", patient.PrimerapellidoPacientes);
                    command.Parameters.AddWithValue("@segundoapellido_pacientes", (object)patient.SegundoapellidoPacientes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@sexo_pacientes_ca", (object)patient.SexoPacientesCa ?? DBNull.Value);
                    command.Parameters.AddWithValue("@fechanacimiento_pacientes", patient.FechanacimientoPacientes);
                    command.Parameters.AddWithValue("@edad_pacientes", patient.EdadPacientes);
                    command.Parameters.AddWithValue("@tiposangre_pacientes_ca", (object)patient.TiposangrePacientesCa ?? DBNull.Value);
                    command.Parameters.AddWithValue("@donante_pacientes", patient.DonantePacientes);
                    command.Parameters.AddWithValue("@estadocivil_pacientes_ca", (object)patient.EstadocivilPacientesCa ?? DBNull.Value);
                    command.Parameters.AddWithValue("@formacionprofesional_pacientes_ca", (object)patient.FormacionprofesionalPacientesCa ?? DBNull.Value);
                    command.Parameters.AddWithValue("@telefonofijo_pacientes", (object)patient.TelefonofijoPacientes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@telefonocelular_pacientes", (object)patient.TelefonocelularPacientes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@email_pacientes", (object)patient.EmailPacientes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@nacionalidad_pacientes_pa",(object)patient.NacionalidadPacientesPa ?? DBNull.Value);
                    command.Parameters.AddWithValue("@provincia_pacientes_l", (object)patient.ProvinciaPacientesL ?? DBNull.Value);
                    command.Parameters.AddWithValue("@direccion_pacientes", (object)patient.DireccionPacientes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ocupacion_pacientes", (object)patient.OcupacionPacientes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@empresa_pacientes", (object)patient.EmpresaPacientes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@segurosalud_pacientes_ca", (object)patient.SegurosaludPacientesCa ?? DBNull.Value);
                    command.Parameters.AddWithValue("@estado_pacientes", patient.EstadoPacientes);

                    try
                    {
                        await connection.OpenAsync();
                        var result = await command.ExecuteScalarAsync(); // Devuelve el ID del paciente actualizado

                        return Convert.ToInt32(result); // Convierte el resultado a int
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores, puedes registrar el error o lanzar la excepción
                        throw new Exception("Error al actualizar el paciente.", ex);
                    }
                }
            }
        }

        //Metodo obtener por id el paciente
        public async Task<Paciente> GetPacienteByIdAsync(int id)
        {
            return await _context.Pacientes.FindAsync(id);
        }


        //Metodo borrar paciente
        public async Task<bool> DeletePatientAsync(int patientId)
        {
            using (var connection = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (var command = new SqlCommand("sp_DeletePatient", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Agregar el parámetro de identificación del paciente
                    command.Parameters.AddWithValue("@id_pacientes", patientId);

                    try
                    {
                        await connection.OpenAsync();
                        int rowsAffected = await command.ExecuteNonQueryAsync(); // Ejecuta el procedimiento almacenado

                        return rowsAffected > 0; // Devuelve true si se eliminó al menos una fila
                    }
                    catch (Exception ex)
                    {
                        // Manejo de errores, puedes registrar el error o lanzar la excepción
                        throw new Exception("Error al eliminar el paciente.", ex);
                    }
                }
            }
        }

    }
}






