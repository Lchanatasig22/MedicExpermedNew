using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class OrganosSistema
    {
        public OrganosSistema()
        {
            Consulta = new HashSet<Consultum>();
        }

        public int IdOrganosSistemas { get; set; }
        public bool? OrgSentidos { get; set; }
        public string? ObserOrgSentidos { get; set; }
        public bool? Respiratorio { get; set; }
        public string? ObserRespiratorio { get; set; }
        public bool? CardioVascular { get; set; }
        public string? ObserCardioVascular { get; set; }
        public bool? Digestivo { get; set; }
        public string? ObserDigestivo { get; set; }
        public bool? Genital { get; set; }
        public string? ObserGenital { get; set; }
        public bool? Urinario { get; set; }
        public string? ObserUrinario { get; set; }
        public bool? MEsqueletico { get; set; }
        public string? ObserMEsqueletico { get; set; }
        public bool? Endocrino { get; set; }
        public string? ObserEndocrino { get; set; }
        public bool? Linfatico { get; set; }
        public string? ObserLinfatico { get; set; }
        public bool? Nervioso { get; set; }
        public string? ObserNervioso { get; set; }

        public virtual ICollection<Consultum> Consulta { get; set; }
    }
}
