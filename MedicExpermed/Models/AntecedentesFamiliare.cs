using System;
using System.Collections.Generic;

namespace MedicExpermed.Models
{
    public partial class AntecedentesFamiliare
    {
        public AntecedentesFamiliare()
        {
            Consulta = new HashSet<Consultum>();
        }

        public int IdAntecedente { get; set; }
        public bool? Cardiopatia { get; set; }
        public string? ObserCardiopatia { get; set; }
        public int? ParentescocatalogoCardiopatia { get; set; }
        public bool? Diabetes { get; set; }
        public string? ObserDiabetes { get; set; }
        public int? ParentescocatalogoDiabetes { get; set; }
        public bool? EnfCardiovascular { get; set; }
        public string? ObserEnfCardiovascular { get; set; }
        public int? ParentescocatalogoEnfcardiovascular { get; set; }
        public bool? Hipertension { get; set; }
        public string? ObserHipertension { get; set; }
        public int? ParentescocatalogoHipertension { get; set; }
        public bool? Cancer { get; set; }
        public string? ObserCancer { get; set; }
        public int? ParentescocatalogoCancer { get; set; }
        public bool? Tuberculosis { get; set; }
        public string? ObserTuberculosis { get; set; }
        public int? ParentescocatalogoTuberculosis { get; set; }
        public bool? EnfMental { get; set; }
        public string? ObserEnfMental { get; set; }
        public int? ParentescocatalogoEnfmental { get; set; }
        public bool? EnfInfecciosa { get; set; }
        public string? ObserEnfInfecciosa { get; set; }
        public int? ParentescocatalogoEnfinfecciosa { get; set; }
        public bool? MalFormacion { get; set; }
        public string? ObserMalFormacion { get; set; }
        public int? ParentescocatalogoMalformacion { get; set; }
        public bool? Otro { get; set; }
        public string? ObserOtro { get; set; }
        public int? ParentescocatalogoOtro { get; set; }

        public virtual Catalogo? ParentescocatalogoCancerNavigation { get; set; }
        public virtual Catalogo? ParentescocatalogoCardiopatiaNavigation { get; set; }
        public virtual Catalogo? ParentescocatalogoDiabetesNavigation { get; set; }
        public virtual Catalogo? ParentescocatalogoEnfcardiovascularNavigation { get; set; }
        public virtual Catalogo? ParentescocatalogoEnfinfecciosaNavigation { get; set; }
        public virtual Catalogo? ParentescocatalogoEnfmentalNavigation { get; set; }
        public virtual Catalogo? ParentescocatalogoHipertensionNavigation { get; set; }
        public virtual Catalogo? ParentescocatalogoMalformacionNavigation { get; set; }
        public virtual Catalogo? ParentescocatalogoOtroNavigation { get; set; }
        public virtual Catalogo? ParentescocatalogoTuberculosisNavigation { get; set; }
        public virtual ICollection<Consultum> Consulta { get; set; }
    }
}
