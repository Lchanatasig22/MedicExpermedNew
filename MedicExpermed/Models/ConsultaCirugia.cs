using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class ConsultaCirugia
    {
        public ConsultaCirugia()
        {
            Consulta = new HashSet<Consultum>();
        }

        public int IdConsultaCirugias { get; set; }
        public DateTime? FechacreacionCirugia { get; set; }
        public int? CatalogocirugiaId { get; set; }
        public int? ConsultaCirugiasId { get; set; }
        public string? ObservacionCirugia { get; set; }
        public int EstadoCirugias { get; set; }
        [JsonIgnore] // Evita que este campo se serialice en la petición JSON

        public virtual Catalogo? Catalogocirugia { get; set; }
        public virtual ICollection<Consultum> Consulta { get; set; }
    }
}
