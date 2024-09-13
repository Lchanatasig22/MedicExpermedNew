using MedicExpermed.Models;
using Microsoft.EntityFrameworkCore;

namespace MedicExpermed.Services
{
    public class CatalogService
    {
        private readonly medicossystembdIContext _context;

        public CatalogService(medicossystembdIContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metodo para taer las listas del catalogo
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        private async Task<List<Catalogo>> ObtenerCatalogoPorCategoriaAsync(string categoria)
        {
            return await _context.Catalogos.Where(c => c.CategoriaCatalogo == categoria).ToListAsync();
        }

        /// <summary>
        /// Uso del metodo 
        /// </summary>
        /// <returns></returns>
        public Task<List<Catalogo>> ObtenerTiposGeneroAsync()
        {
            return ObtenerCatalogoPorCategoriaAsync("GENERO");
        }
        public Task<List<Catalogo>> ObtenerTiposSangreAsync()
        {
            return ObtenerCatalogoPorCategoriaAsync("TIPO DE SANGRE");
        }

        public Task<List<Catalogo>> ObtenerTiposDocumentoAsync()
        {
            return ObtenerCatalogoPorCategoriaAsync("TIPO DOCUMENTO");
        }
        public Task<List<Catalogo>> ObtenerTiposEstadoCivilAsync()
        {
            return ObtenerCatalogoPorCategoriaAsync("ESTADO CIVIL");
        }
        public Task<List<Catalogo>> ObtenerTiposFormacionAsync()
        {
            return ObtenerCatalogoPorCategoriaAsync("FORMACION PROFESIONAL");
        }
        public Task<List<Catalogo>> ObtenerTiposSeguroSaludAsync()
        {
            return ObtenerCatalogoPorCategoriaAsync("SEGUROS DE SALUD");
        }
           public Task<List<Catalogo>> ObtenerTiposParentescoAsync()
        {
            return ObtenerCatalogoPorCategoriaAsync("PARENTESCO");
        }
        //Parentesco 
        public async Task<int> ObtenerIdParentescoNingunoAsync()
        {
            var tiposParentesco = await ObtenerTiposParentescoAsync();

            // Busca el ID donde el nombre es "NINGUNO"
            var ningunTipo = tiposParentesco.FirstOrDefault(tp => tp.DescripcionCatalogo.Equals("NINGUNO", StringComparison.OrdinalIgnoreCase));

            if (ningunTipo == null)
            {
                throw new InvalidOperationException("No se encontró un tipo de parentesco con el nombre 'NINGUNO'.");
            }

            return ningunTipo.IdCatalogo;
        }

        public Task<List<Catalogo>> ObtenerAntecedentesFAsync()
        {
            return ObtenerCatalogoPorCategoriaAsync("ANTECEDENTES FAMILIARES");
        }
        public Task<List<Catalogo>> ObtenerAlergiasAsync()
        {
            return ObtenerCatalogoPorCategoriaAsync("ALERGIAS");
        }
        //ALERGIAS 
        public async Task<int> ObtenerIdAlergiasNingunoAsync()
        {
            var tiposAlergias = await ObtenerAlergiasAsync();

            // Busca el ID donde el nombre es "NINGUNO"
            var ningunTipo = tiposAlergias.FirstOrDefault(tp => tp.DescripcionCatalogo.Equals("NINGUNA", StringComparison.OrdinalIgnoreCase));

            if (ningunTipo == null)
            {
                throw new InvalidOperationException("No se encontró un tipo de Alergias con el nombre 'NINGUNO'.");
            }

            return ningunTipo.IdCatalogo;
        }
        public Task<List<Catalogo>> ObtenerCirugiasAsync()
        {
            return ObtenerCatalogoPorCategoriaAsync("CIRUGIAS");
        }
        //ALERGIAS 
        public async Task<int> ObtenerIdCirugiasNingunoAsync()
        {
            var tiposCirugias = await ObtenerCirugiasAsync();

            // Busca el ID donde el nombre es "NINGUNO"
            var ningunTipo = tiposCirugias.FirstOrDefault(tp => tp.DescripcionCatalogo.Equals("NINGUNA", StringComparison.OrdinalIgnoreCase));

            if (ningunTipo == null)
            {
                throw new InvalidOperationException("No se encontró un tipo de parentesco con el nombre 'NINGUNO'.");
            }

            return ningunTipo.IdCatalogo;
        }

        //Otros


        public async Task<List<Localidad>> ObtenerLocalidadesActivasAsync()
        {
            return await _context.Localidads.Where(l => l.EstadoLocalidad == 1).ToListAsync();
        }
        public async Task<List<ConsultaMedicamento>> ObtenerMedicamentosPorConsultaIdAsync(int consultaId)
        {
            return await _context.ConsultaMedicamentos
                                 .Where(m => m.ConsultaMedicamentosId == consultaId)
                                 .ToListAsync();
        }
        public async Task<List<ConsultaLaboratorio>> ObtenerLaboratoriosPorConsultaIdAsync(int consultaId)
        {
            return await _context.ConsultaLaboratorios
                                 .Where(l => l.ConsultaLaboratorioId == consultaId)
                                 .ToListAsync();
        }
        public async Task<List<ConsultaImagen>> ObtenerImagenesPorConsultaIdAsync(int consultaId)
        {
            return await _context.ConsultaImagens
                                 .Where(i => i.ConsultaImagenId == consultaId)
                                 .ToListAsync();
        }
        public async Task<List<ConsultaDiagnostico>> ObtenerDiagnosticosPorConsultaIdAsync(int consultaId)
        {
            return await _context.ConsultaDiagnosticos
                                 .Where(d => d.ConsultaDiagnosticoId == consultaId)
                                 .ToListAsync();
        }

        /// <summary>
        /// Lista Nacionalidades
        /// </summary>
        /// <returns></returns>
        public async Task<List<Pai>> ObtenerNacionalidadesActivasAsync()
        {
            return await _context.Pais.Where(l => l.EstadoPais == 1).ToListAsync();
        }
       
        public async Task<List<Laboratorio>> ObtenerLaboratorioActivasAsync()
        {
            return await _context.Laboratorios.Where(l => l.EstadoLaboratorios == 1).ToListAsync();
        }
        public async Task<List<Imagen>> ObtenerImagenActivasAsync()
        {
            return await _context.Imagens.Where(l => l.EstadoImagen == 1).ToListAsync();
        }

        public async Task<List<Medicamento>> ObtenerMedicamentosActivasAsync()
        {
            return await _context.Medicamentos.Where(l => l.EstadoMedicamento == 1).ToListAsync();
        }

        public async Task<List<Diagnostico>> ObtenerDiagnosticosActivasAsync()
        {
            return await _context.Diagnosticos.Where(l => l.EstadoDiagnostico == 1).ToListAsync();
        }

        public async Task<List<Pai>> ObtenerTiposDeNacionalidadPAsync()
        {
            return await ObtenerNacionalidadesActivasAsync();
        }
        public async Task<int> ObtenerIdNacionalidadEcuatorianoAsync()
        {
            var nacionalidadEcuatoriano = await ObtenerTiposDeNacionalidadPAsync();

            // Busca el ID donde el nombre es "NINGUNO"
            var ningunTipo = nacionalidadEcuatoriano.FirstOrDefault(tp => tp.NombrePais.Equals("Ecuador", StringComparison.OrdinalIgnoreCase));

            if (ningunTipo == null)
            {
                throw new InvalidOperationException("No se encontró un tipo de parentesco con el nombre 'NINGUNO'.");
            }

            return ningunTipo.IdPais;
        }
        public async Task<List<Localidad>> ObtenerTiposDeProvinciaPAsync()
        {
            return await ObtenerLocalidadesActivasAsync();
        }



    }
}
