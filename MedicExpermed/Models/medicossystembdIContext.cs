using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MedicExpermed.Models
{
    public partial class medicossystembdIContext : DbContext
    {
        public medicossystembdIContext()
        {
        }

        public medicossystembdIContext(DbContextOptions<medicossystembdIContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AntecedentesFamiliare> AntecedentesFamiliares { get; set; } = null!;
        public virtual DbSet<Catalogo> Catalogos { get; set; } = null!;
        public virtual DbSet<Citum> Cita { get; set; } = null!;
        public virtual DbSet<ConsultaAlergia> ConsultaAlergias { get; set; } = null!;
        public virtual DbSet<ConsultaCirugia> ConsultaCirugias { get; set; } = null!;
        public virtual DbSet<ConsultaDiagnostico> ConsultaDiagnosticos { get; set; } = null!;
        public virtual DbSet<ConsultaImagen> ConsultaImagens { get; set; } = null!;
        public virtual DbSet<ConsultaLaboratorio> ConsultaLaboratorios { get; set; } = null!;
        public virtual DbSet<ConsultaMedicamento> ConsultaMedicamentos { get; set; } = null!;
        public virtual DbSet<Consultum> Consulta { get; set; } = null!;
        public virtual DbSet<Diagnostico> Diagnosticos { get; set; } = null!;
        public virtual DbSet<Especialidad> Especialidads { get; set; } = null!;
        public virtual DbSet<Establecimiento> Establecimientos { get; set; } = null!;
        public virtual DbSet<ExamenFisico> ExamenFisicos { get; set; } = null!;
        public virtual DbSet<Facturacion> Facturacions { get; set; } = null!;
        public virtual DbSet<Imagen> Imagens { get; set; } = null!;
        public virtual DbSet<Laboratorio> Laboratorios { get; set; } = null!;
        public virtual DbSet<Localidad> Localidads { get; set; } = null!;
        public virtual DbSet<Medicamento> Medicamentos { get; set; } = null!;
        public virtual DbSet<OrganosSistema> OrganosSistemas { get; set; } = null!;
        public virtual DbSet<Paciente> Pacientes { get; set; } = null!;
        public virtual DbSet<Pai> Pais { get; set; } = null!;
        public virtual DbSet<Perfil> Perfils { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=localhost;Database=medicossystembdII;User Id=sa;Password=1717;Trusted_Connection=False;MultipleActiveResultSets=true");
          }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AntecedentesFamiliare>(entity =>
            {
                entity.HasKey(e => e.IdAntecedente)
                    .HasName("PK__antecede__3E014641CAB864D5");

                entity.ToTable("antecedentes_familiares");

                entity.Property(e => e.IdAntecedente).HasColumnName("id_antecedente");

                entity.Property(e => e.Cancer).HasColumnName("cancer");

                entity.Property(e => e.Cardiopatia).HasColumnName("cardiopatia");

                entity.Property(e => e.Diabetes).HasColumnName("diabetes");

                entity.Property(e => e.EnfCardiovascular).HasColumnName("enf_cardiovascular");

                entity.Property(e => e.EnfInfecciosa).HasColumnName("enf_infecciosa");

                entity.Property(e => e.EnfMental).HasColumnName("enf_mental");

                entity.Property(e => e.Hipertension).HasColumnName("hipertension");

                entity.Property(e => e.MalFormacion).HasColumnName("mal_formacion");

                entity.Property(e => e.ObserCancer)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_cancer");

                entity.Property(e => e.ObserCardiopatia)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_cardiopatia");

                entity.Property(e => e.ObserDiabetes)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_diabetes");

                entity.Property(e => e.ObserEnfCardiovascular)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_enf_cardiovascular");

                entity.Property(e => e.ObserEnfInfecciosa)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_enf_infecciosa");

                entity.Property(e => e.ObserEnfMental)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_enf_mental");

                entity.Property(e => e.ObserHipertension)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_hipertension");

                entity.Property(e => e.ObserMalFormacion)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_mal_formacion");

                entity.Property(e => e.ObserOtro)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_otro");

                entity.Property(e => e.ObserTuberculosis)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_tuberculosis");

                entity.Property(e => e.Otro).HasColumnName("otro");

                entity.Property(e => e.ParentescocatalogoCancer).HasColumnName("parentescocatalogo_cancer");

                entity.Property(e => e.ParentescocatalogoCardiopatia).HasColumnName("parentescocatalogo_cardiopatia");

                entity.Property(e => e.ParentescocatalogoDiabetes).HasColumnName("parentescocatalogo_diabetes");

                entity.Property(e => e.ParentescocatalogoEnfcardiovascular).HasColumnName("parentescocatalogo_enfcardiovascular");

                entity.Property(e => e.ParentescocatalogoEnfinfecciosa).HasColumnName("parentescocatalogo_enfinfecciosa");

                entity.Property(e => e.ParentescocatalogoEnfmental).HasColumnName("parentescocatalogo_enfmental");

                entity.Property(e => e.ParentescocatalogoHipertension).HasColumnName("parentescocatalogo_hipertension");

                entity.Property(e => e.ParentescocatalogoMalformacion).HasColumnName("parentescocatalogo_malformacion");

                entity.Property(e => e.ParentescocatalogoOtro).HasColumnName("parentescocatalogo_otro");

                entity.Property(e => e.ParentescocatalogoTuberculosis).HasColumnName("parentescocatalogo_tuberculosis");

                entity.Property(e => e.Tuberculosis).HasColumnName("tuberculosis");

                entity.HasOne(d => d.ParentescocatalogoCancerNavigation)
                    .WithMany(p => p.AntecedentesFamiliareParentescocatalogoCancerNavigations)
                    .HasForeignKey(d => d.ParentescocatalogoCancer)
                    .HasConstraintName("FK_catalogo_parentesco_cancer");

                entity.HasOne(d => d.ParentescocatalogoCardiopatiaNavigation)
                    .WithMany(p => p.AntecedentesFamiliareParentescocatalogoCardiopatiaNavigations)
                    .HasForeignKey(d => d.ParentescocatalogoCardiopatia)
                    .HasConstraintName("FK_catalogo_parentesco_cardiopatia");

                entity.HasOne(d => d.ParentescocatalogoDiabetesNavigation)
                    .WithMany(p => p.AntecedentesFamiliareParentescocatalogoDiabetesNavigations)
                    .HasForeignKey(d => d.ParentescocatalogoDiabetes)
                    .HasConstraintName("FK_catalogo_parentesco_diabetes");

                entity.HasOne(d => d.ParentescocatalogoEnfcardiovascularNavigation)
                    .WithMany(p => p.AntecedentesFamiliareParentescocatalogoEnfcardiovascularNavigations)
                    .HasForeignKey(d => d.ParentescocatalogoEnfcardiovascular)
                    .HasConstraintName("FK_catalogo_parentesco_cardiovascular");

                entity.HasOne(d => d.ParentescocatalogoEnfinfecciosaNavigation)
                    .WithMany(p => p.AntecedentesFamiliareParentescocatalogoEnfinfecciosaNavigations)
                    .HasForeignKey(d => d.ParentescocatalogoEnfinfecciosa)
                    .HasConstraintName("FK_catalogo_parentesco_enfinfecciosa");

                entity.HasOne(d => d.ParentescocatalogoEnfmentalNavigation)
                    .WithMany(p => p.AntecedentesFamiliareParentescocatalogoEnfmentalNavigations)
                    .HasForeignKey(d => d.ParentescocatalogoEnfmental)
                    .HasConstraintName("FK_catalogo_parentesco_enfmental");

                entity.HasOne(d => d.ParentescocatalogoHipertensionNavigation)
                    .WithMany(p => p.AntecedentesFamiliareParentescocatalogoHipertensionNavigations)
                    .HasForeignKey(d => d.ParentescocatalogoHipertension)
                    .HasConstraintName("FK_catalogo_parentesco_hipertension");

                entity.HasOne(d => d.ParentescocatalogoMalformacionNavigation)
                    .WithMany(p => p.AntecedentesFamiliareParentescocatalogoMalformacionNavigations)
                    .HasForeignKey(d => d.ParentescocatalogoMalformacion)
                    .HasConstraintName("FK_catalogo_parentesco_malformacion");

                entity.HasOne(d => d.ParentescocatalogoOtroNavigation)
                    .WithMany(p => p.AntecedentesFamiliareParentescocatalogoOtroNavigations)
                    .HasForeignKey(d => d.ParentescocatalogoOtro)
                    .HasConstraintName("FK_catalogo_parentesco_otro");

                entity.HasOne(d => d.ParentescocatalogoTuberculosisNavigation)
                    .WithMany(p => p.AntecedentesFamiliareParentescocatalogoTuberculosisNavigations)
                    .HasForeignKey(d => d.ParentescocatalogoTuberculosis)
                    .HasConstraintName("FK_catalogo_parentesco_tuberculosis");
            });

            modelBuilder.Entity<Catalogo>(entity =>
            {
                entity.HasKey(e => e.IdCatalogo)
                    .HasName("PK__catalogo__4B673DCA8793859E");

                entity.ToTable("catalogo");

                entity.HasIndex(e => e.UuidCatalogo, "UQ__catalogo__C58C4DEB38D48CE6")
                    .IsUnique();

                entity.Property(e => e.IdCatalogo).HasColumnName("id_catalogo");

                entity.Property(e => e.CategoriaCatalogo)
                    .HasMaxLength(255)
                    .HasColumnName("categoria_catalogo");

                entity.Property(e => e.DescripcionCatalogo)
                    .HasMaxLength(1000)
                    .HasColumnName("descripcion_catalogo");

                entity.Property(e => e.EstadoCatalogo).HasColumnName("estado_catalogo");

                entity.Property(e => e.FechacreacionCatalogo)
                    .HasColumnType("datetime")
                    .HasColumnName("fechacreacion_catalogo")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FechamodificacionCatalogo)
                    .HasColumnType("datetime")
                    .HasColumnName("fechamodificacion_catalogo")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UsuariocreacionCatalogo)
                    .HasMaxLength(255)
                    .HasColumnName("usuariocreacion_catalogo");

                entity.Property(e => e.UsuariomodificacionCatalogo)
                    .HasMaxLength(255)
                    .HasColumnName("usuariomodificacion_catalogo");

                entity.Property(e => e.UuidCatalogo).HasColumnName("uuid_catalogo");
            });

            modelBuilder.Entity<Citum>(entity =>
            {
                entity.HasKey(e => e.IdCita)
                    .HasName("PK__cita__6AEC3C0970B95177");

                entity.ToTable("cita");

                entity.Property(e => e.IdCita).HasColumnName("id_cita");

                entity.Property(e => e.ConsultaId).HasColumnName("consulta_id");

                entity.Property(e => e.EstadoCita).HasColumnName("estado_cita");

                entity.Property(e => e.FechacreacionCita)
                    .HasColumnType("datetime")
                    .HasColumnName("fechacreacion_cita")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FechadelacitaCita)
                    .HasColumnType("date")
                    .HasColumnName("fechadelacita_cita");

                entity.Property(e => e.HoradelacitaCita).HasColumnName("horadelacita_cita");

                entity.Property(e => e.Motivo).HasColumnName("motivo");

                entity.Property(e => e.PacienteId).HasColumnName("paciente_id");

                entity.Property(e => e.UsuarioId).HasColumnName("usuario_id");

                entity.Property(e => e.UsuariocreacionCita)
                    .HasMaxLength(255)
                    .HasColumnName("usuariocreacion_cita");

                entity.HasOne(d => d.Consulta)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.ConsultaId)
                    .HasConstraintName("FK_cita_consulta");

                entity.HasOne(d => d.Paciente)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.PacienteId)
                    .HasConstraintName("FK_cita_paciente");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.Cita)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK_cita_usuario");
            });

            modelBuilder.Entity<ConsultaAlergia>(entity =>
            {
                entity.HasKey(e => e.IdConsultaAlergias)
                    .HasName("PK__consulta__04246C79C9505F2C");

                entity.ToTable("consulta_alergias");

                entity.Property(e => e.IdConsultaAlergias).HasColumnName("id_consulta_alergias");

                entity.Property(e => e.CatalogoalergiaId).HasColumnName("catalogoalergia_id");

                entity.Property(e => e.ConsultaAlergiasInt).HasColumnName("consulta_alergias_int");

                entity.Property(e => e.EstadoAlergias).HasColumnName("estado_alergias");

                entity.Property(e => e.FechacreacionAlergia)
                    .HasColumnType("datetime")
                    .HasColumnName("fechacreacion_alergia")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ObservacionAlergias)
                    .HasMaxLength(1000)
                    .HasColumnName("observacion_alergias");

                entity.HasOne(d => d.Catalogoalergia)
                    .WithMany(p => p.ConsultaAlergia)
                    .HasForeignKey(d => d.CatalogoalergiaId)
                    .HasConstraintName("FK_catalogo_alergias");
            });

            modelBuilder.Entity<ConsultaCirugia>(entity =>
            {
                entity.HasKey(e => e.IdConsultaCirugias)
                    .HasName("PK__consulta__4383EA6F6F94F10B");

                entity.ToTable("consulta_cirugias");

                entity.Property(e => e.IdConsultaCirugias).HasColumnName("id_consulta_cirugias");

                entity.Property(e => e.CatalogocirugiaId).HasColumnName("catalogocirugia_id");

                entity.Property(e => e.ConsultaCirugiasId).HasColumnName("consulta_cirugias_id");

                entity.Property(e => e.EstadoCirugias).HasColumnName("estado_cirugias");

                entity.Property(e => e.FechacreacionCirugia)
                    .HasColumnType("datetime")
                    .HasColumnName("fechacreacion_cirugia")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ObservacionCirugia)
                    .HasMaxLength(1000)
                    .HasColumnName("observacion_cirugia");

                entity.HasOne(d => d.Catalogocirugia)
                    .WithMany(p => p.ConsultaCirugia)
                    .HasForeignKey(d => d.CatalogocirugiaId)
                    .HasConstraintName("FK_catalogo_cirugias");
            });

            modelBuilder.Entity<ConsultaDiagnostico>(entity =>
            {
                entity.HasKey(e => e.IdConsultaDiagnostico)
                    .HasName("PK__consulta__D8B1E6BD7467C47D");

                entity.ToTable("consulta_diagnostico");

                entity.Property(e => e.IdConsultaDiagnostico).HasColumnName("id_consulta_diagnostico");

                entity.Property(e => e.ConsultaDiagnosticoId).HasColumnName("consulta_diagnostico_id");

                entity.Property(e => e.DefinitivoDiagnosticos).HasColumnName("definitivo_diagnosticos");

                entity.Property(e => e.DiagnosticoId).HasColumnName("diagnostico_id");

                entity.Property(e => e.EstadoDiagnostico).HasColumnName("estado_diagnostico");

                entity.Property(e => e.ObservacionDiagnostico)
                    .HasMaxLength(1000)
                    .HasColumnName("observacion_diagnostico");

                entity.Property(e => e.PresuntivoDiagnosticos).HasColumnName("presuntivo_diagnosticos");

                entity.Property(e => e.SecuencialDiagnostico).HasColumnName("secuencial_diagnostico");

                entity.HasOne(d => d.Diagnostico)
                    .WithMany(p => p.ConsultaDiagnosticos)
                    .HasForeignKey(d => d.DiagnosticoId)
                    .HasConstraintName("FK_consulta_diagnostico_diagnostico");
            });

            modelBuilder.Entity<ConsultaImagen>(entity =>
            {
                entity.HasKey(e => e.IdConsultaImagen)
                    .HasName("PK__consulta__43A0875D9EF7535D");

                entity.ToTable("consulta_imagen");

                entity.Property(e => e.IdConsultaImagen).HasColumnName("id_consulta_imagen");

                entity.Property(e => e.CantidadImagen).HasColumnName("cantidad_imagen");

                entity.Property(e => e.ConsultaImagenId).HasColumnName("consulta_imagen_id");

                entity.Property(e => e.EstadoImagen).HasColumnName("estado_imagen");

                entity.Property(e => e.ImagenId).HasColumnName("imagen_id");

                entity.Property(e => e.ObservacionImagen)
                    .HasMaxLength(1000)
                    .HasColumnName("observacion_imagen");

                entity.Property(e => e.SecuencialImagen).HasColumnName("secuencial_imagen");

                entity.HasOne(d => d.Imagen)
                    .WithMany(p => p.ConsultaImagens)
                    .HasForeignKey(d => d.ImagenId)
                    .HasConstraintName("FK_consulta_imagen_imagen");
            });

            modelBuilder.Entity<ConsultaLaboratorio>(entity =>
            {
                entity.HasKey(e => e.IdLaboratorioConsulta)
                    .HasName("PK__consulta__14E40796350310A1");

                entity.ToTable("consulta_laboratorio");

                entity.Property(e => e.IdLaboratorioConsulta).HasColumnName("id_laboratorio_consulta");

                entity.Property(e => e.CantidadLaboratorio).HasColumnName("cantidad_laboratorio");

                entity.Property(e => e.CatalogoLaboratorioId).HasColumnName("catalogo_laboratorio_id");

                entity.Property(e => e.ConsultaLaboratorioId).HasColumnName("consulta_laboratorio_id");

                entity.Property(e => e.EstadoLaboratorio).HasColumnName("estado_laboratorio");

                entity.Property(e => e.Observacion)
                    .HasMaxLength(1000)
                    .HasColumnName("observacion");

                entity.Property(e => e.SecuencialLaboratorio).HasColumnName("secuencial_laboratorio");

                entity.HasOne(d => d.CatalogoLaboratorio)
                    .WithMany(p => p.ConsultaLaboratorios)
                    .HasForeignKey(d => d.CatalogoLaboratorioId)
                    .HasConstraintName("FK_consulta_laboratorio_catalogo");
            });

            modelBuilder.Entity<ConsultaMedicamento>(entity =>
            {
                entity.HasKey(e => e.IdConsultaMedicamento)
                    .HasName("PK__consulta__12CF73425330C9BE");

                entity.ToTable("consulta_medicamentos");

                entity.Property(e => e.IdConsultaMedicamento).HasColumnName("id_consulta_medicamento");

                entity.Property(e => e.ConsultaMedicamentosId).HasColumnName("consulta_medicamentos_id");

                entity.Property(e => e.DosisMedicamento)
                    .HasMaxLength(100)
                    .HasColumnName("dosis_medicamento");

                entity.Property(e => e.EstadoMedicamento).HasColumnName("estado_medicamento");

                entity.Property(e => e.FechacreacionMedicamento)
                    .HasColumnType("datetime")
                    .HasColumnName("fechacreacion_medicamento")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MedicamentoId).HasColumnName("medicamento_id");

                entity.Property(e => e.ObservacionMedicamento)
                    .HasMaxLength(1000)
                    .HasColumnName("observacion_medicamento");

                entity.Property(e => e.SecuencialMedicamento).HasColumnName("secuencial_medicamento");

                entity.HasOne(d => d.Medicamento)
                    .WithMany(p => p.ConsultaMedicamentos)
                    .HasForeignKey(d => d.MedicamentoId)
                    .HasConstraintName("FK_consulta_medicamentos_medicamento");
            });

            modelBuilder.Entity<Consultum>(entity =>
            {
                entity.HasKey(e => e.IdConsulta)
                    .HasName("PK__consulta__6F53588B52D59A63");

                entity.ToTable("consulta");

                entity.Property(e => e.IdConsulta).HasColumnName("id_consulta");

                entity.Property(e => e.AntecedentespersonalesConsulta).HasColumnName("antecedentespersonales_consulta");

                entity.Property(e => e.ConsultaAlergiasInt).HasColumnName("consulta_alergias_int");

                entity.Property(e => e.ConsultaAntecedentesFamiliaresId).HasColumnName("consulta_antecedentes_familiares_id");

                entity.Property(e => e.ConsultaCirugiasId).HasColumnName("consulta_cirugias_id");

                entity.Property(e => e.ConsultaDiagnosticoId).HasColumnName("consulta_diagnostico_id");

                entity.Property(e => e.ConsultaExamenFisicoId).HasColumnName("consulta_examen_fisico_id");

                entity.Property(e => e.ConsultaImagenId).HasColumnName("consulta_imagen_id");

                entity.Property(e => e.ConsultaLaboratorioId).HasColumnName("consulta_laboratorio_id");

                entity.Property(e => e.ConsultaMedicamentosId).HasColumnName("consulta_medicamentos_id");

                entity.Property(e => e.ConsultaOrganosSistemasId).HasColumnName("consulta_organos_sistemas_id");

                entity.Property(e => e.ConsultaprincipalConsulta).HasColumnName("consultaprincipal_consulta");

                entity.Property(e => e.DiasincapacidadConsulta).HasColumnName("diasincapacidad_consulta");

                entity.Property(e => e.EnfermedadConsulta).HasColumnName("enfermedad_consulta");

                entity.Property(e => e.EspecialidadId).HasColumnName("especialidad_id");

                entity.Property(e => e.EstadoConsultaC).HasColumnName("estado_consulta_c");

                entity.Property(e => e.FechacreacionConsulta)
                    .HasColumnType("datetime")
                    .HasColumnName("fechacreacion_consulta")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FrecuenciarespiratoriaConsulta)
                    .HasMaxLength(3)
                    .HasColumnName("frecuenciarespiratoria_consulta");

                entity.Property(e => e.HistorialConsulta).HasColumnName("historial_consulta");

                entity.Property(e => e.MedicoConsultaD).HasColumnName("medico_consulta_d");

                entity.Property(e => e.MotivoConsulta).HasColumnName("motivo_consulta");

                entity.Property(e => e.NombreparienteConsulta)
                    .HasMaxLength(255)
                    .HasColumnName("nombrepariente_consulta");

                entity.Property(e => e.NotasevolucionConsulta).HasColumnName("notasevolucion_consulta");

                entity.Property(e => e.ObservacionConsulta).HasColumnName("observacion_consulta");

                entity.Property(e => e.PacienteConsultaP).HasColumnName("paciente_consulta_p");

                entity.Property(e => e.PesoConsulta)
                    .HasMaxLength(3)
                    .HasColumnName("peso_consulta");

                entity.Property(e => e.PlantratamientoConsulta).HasColumnName("plantratamiento_consulta");

                entity.Property(e => e.PresionarterialdiastolicaConsulta)
                    .HasMaxLength(3)
                    .HasColumnName("presionarterialdiastolica_consulta");

                entity.Property(e => e.PresionarterialsistolicaConsulta)
                    .HasMaxLength(3)
                    .HasColumnName("presionarterialsistolica_consulta");

                entity.Property(e => e.PulsoConsulta)
                    .HasMaxLength(3)
                    .HasColumnName("pulso_consulta");

                entity.Property(e => e.Reconofarmacologicas).HasColumnName("reconofarmacologicas");

                entity.Property(e => e.SecuencialConsulta).HasColumnName("secuencial_consulta");

                entity.Property(e => e.SignosalarmaConsulta).HasColumnName("signosalarma_consulta");

                entity.Property(e => e.TallaConsulta)
                    .HasMaxLength(3)
                    .HasColumnName("talla_consulta");

                entity.Property(e => e.TelefonoParienteConsulta)
                    .HasMaxLength(20)
                    .HasColumnName("telefono_pariente_consulta");

                entity.Property(e => e.TemperaturaConsulta)
                    .HasMaxLength(4)
                    .HasColumnName("temperatura_consulta");

                entity.Property(e => e.TipoConsultaC).HasColumnName("tipo_consulta_c");

                entity.Property(e => e.TipoparienteConsulta).HasColumnName("tipopariente_consulta");

                entity.Property(e => e.UsuariocreacionConsulta)
                    .HasMaxLength(255)
                    .HasColumnName("usuariocreacion_consulta");

                entity.HasOne(d => d.ConsultaAlergiasIntNavigation)
                    .WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.ConsultaAlergiasInt)
                    .HasConstraintName("FK_consulta_alergias");

                entity.HasOne(d => d.ConsultaAntecedentesFamiliares)
                    .WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.ConsultaAntecedentesFamiliaresId)
                    .HasConstraintName("FK_consulta_antecedentes_familiares");

                entity.HasOne(d => d.ConsultaCirugias)
                    .WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.ConsultaCirugiasId)
                    .HasConstraintName("FK_consulta_cirugias");

                entity.HasOne(d => d.ConsultaDiagnostico)
                    .WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.ConsultaDiagnosticoId)
                    .HasConstraintName("FK_consulta_diagnostico");

                entity.HasOne(d => d.ConsultaExamenFisico)
                    .WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.ConsultaExamenFisicoId)
                    .HasConstraintName("FK_consulta_examen_fisico");

                entity.HasOne(d => d.ConsultaImagen)
                    .WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.ConsultaImagenId)
                    .HasConstraintName("FK_consulta_imagen");

                entity.HasOne(d => d.ConsultaLaboratorio)
                    .WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.ConsultaLaboratorioId)
                    .HasConstraintName("FK_consulta_laboratorio");

                entity.HasOne(d => d.ConsultaMedicamentos)
                    .WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.ConsultaMedicamentosId)
                    .HasConstraintName("FK_consulta_medicamento");

                entity.HasOne(d => d.ConsultaOrganosSistemas)
                    .WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.ConsultaOrganosSistemasId)
                    .HasConstraintName("FK_consulta_organos_sistemas");

                entity.HasOne(d => d.Especialidad)
                    .WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.EspecialidadId)
                    .HasConstraintName("FK_consulta_especialidad");

                entity.HasOne(d => d.MedicoConsultaDNavigation)
                    .WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.MedicoConsultaD)
                    .HasConstraintName("FK_consulta_medico");

                entity.HasOne(d => d.PacienteConsultaPNavigation)
                    .WithMany(p => p.Consulta)
                    .HasForeignKey(d => d.PacienteConsultaP)
                    .HasConstraintName("FK_consulta_paciente");
            });

            modelBuilder.Entity<Diagnostico>(entity =>
            {
                entity.HasKey(e => e.IdDiagnostico)
                    .HasName("PK__diagnost__1384B745A276D8F2");

                entity.ToTable("diagnostico");

                entity.HasIndex(e => e.UuidDiagnostico, "UQ__diagnost__A5CF086E7B472C3D")
                    .IsUnique();

                entity.Property(e => e.IdDiagnostico).HasColumnName("id_diagnostico");

                entity.Property(e => e.CategoriaDiagnostico)
                    .HasMaxLength(255)
                    .HasColumnName("categoria_diagnostico");

                entity.Property(e => e.DescripcionDiagnostico)
                    .HasMaxLength(1000)
                    .HasColumnName("descripcion_diagnostico");

                entity.Property(e => e.EstadoDiagnostico).HasColumnName("estado_diagnostico");

                entity.Property(e => e.NombreDiagnostico)
                    .HasMaxLength(255)
                    .HasColumnName("nombre_diagnostico");

                entity.Property(e => e.UuidDiagnostico)
                    .HasMaxLength(20)
                    .HasColumnName("uuid_diagnostico");
            });

            modelBuilder.Entity<Especialidad>(entity =>
            {
                entity.HasKey(e => e.IdEspecialidad)
                    .HasName("PK__especial__C1D137631EFBA2D8");

                entity.ToTable("especialidad");

                entity.HasIndex(e => e.UuidEspecialidad, "UQ__especial__A9596521D59BB3E1")
                    .IsUnique();

                entity.Property(e => e.IdEspecialidad).HasColumnName("id_especialidad");

                entity.Property(e => e.CategoriaEspecialidad)
                    .HasMaxLength(255)
                    .HasColumnName("categoria_especialidad");

                entity.Property(e => e.DescripcionEspecialidad)
                    .HasMaxLength(1000)
                    .HasColumnName("descripcion_especialidad");

                entity.Property(e => e.EstadoEspecialidad).HasColumnName("estado_especialidad");

                entity.Property(e => e.NombreEspecialidad)
                    .HasMaxLength(255)
                    .HasColumnName("nombre_especialidad");

                entity.Property(e => e.UuidEspecialidad).HasColumnName("uuid_especialidad");
            });

            modelBuilder.Entity<Establecimiento>(entity =>
            {
                entity.HasKey(e => e.IdEstablecimiento)
                    .HasName("PK__establec__AFEAEA202B58D93B");

                entity.ToTable("establecimiento");

                entity.Property(e => e.IdEstablecimiento).HasColumnName("id_establecimiento");

                entity.Property(e => e.CiudadEstablecimiento)
                    .HasMaxLength(255)
                    .HasColumnName("ciudad_establecimiento");

                entity.Property(e => e.DescripcionEstablecimiento)
                    .HasMaxLength(1000)
                    .HasColumnName("descripcion_establecimiento");

                entity.Property(e => e.DireccionEstablecimiento)
                    .HasMaxLength(1000)
                    .HasColumnName("direccion_establecimiento");

                entity.Property(e => e.EstadoEstablecimiento).HasColumnName("estado_establecimiento");

                entity.Property(e => e.FechacreacionEstablecimiento)
                    .HasColumnType("datetime")
                    .HasColumnName("fechacreacion_establecimiento")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FechamodificacionEstablecimiento)
                    .HasColumnType("datetime")
                    .HasColumnName("fechamodificacion_establecimiento")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ProvinciaEstablecimiento)
                    .HasMaxLength(255)
                    .HasColumnName("provincia_establecimiento");
            });

            modelBuilder.Entity<ExamenFisico>(entity =>
            {
                entity.HasKey(e => e.IdExamenFisico)
                    .HasName("PK__examen_f__B5B777B94B6BFE88");

                entity.ToTable("examen_fisico");

                entity.Property(e => e.IdExamenFisico).HasColumnName("id_examen_fisico");

                entity.Property(e => e.Abdomen).HasColumnName("abdomen");

                entity.Property(e => e.Cabeza).HasColumnName("cabeza");

                entity.Property(e => e.Cuello).HasColumnName("cuello");

                entity.Property(e => e.Extremidades).HasColumnName("extremidades");

                entity.Property(e => e.ObserAbdomen)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_abdomen");

                entity.Property(e => e.ObserCabeza)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_cabeza");

                entity.Property(e => e.ObserCuello)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_cuello");

                entity.Property(e => e.ObserExtremidades)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_extremidades");

                entity.Property(e => e.ObserPelvis)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_pelvis");

                entity.Property(e => e.ObserTorax)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_torax");

                entity.Property(e => e.Pelvis).HasColumnName("pelvis");

                entity.Property(e => e.Torax).HasColumnName("torax");
            });

            modelBuilder.Entity<Facturacion>(entity =>
            {
                entity.HasKey(e => e.IdFacturacion)
                    .HasName("PK__facturac__AC4FC8941A30C2D0");

                entity.ToTable("facturacion");

                entity.Property(e => e.IdFacturacion).HasColumnName("id_facturacion");

                entity.Property(e => e.CitaId).HasColumnName("cita_id");

                entity.Property(e => e.EstadoFactura)
                    .HasMaxLength(255)
                    .HasColumnName("estado_factura");

                entity.Property(e => e.FechaFacturacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_facturacion")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.MetodoPago)
                    .HasMaxLength(255)
                    .HasColumnName("metodo_pago");

                entity.Property(e => e.TotalFactura)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("total_factura");

                entity.HasOne(d => d.Cita)
                    .WithMany(p => p.Facturacions)
                    .HasForeignKey(d => d.CitaId)
                    .HasConstraintName("FK_facturacion_cita");
            });

            modelBuilder.Entity<Imagen>(entity =>
            {
                entity.HasKey(e => e.IdImagen)
                    .HasName("PK__imagen__27CC2689AF3171A1");

                entity.ToTable("imagen");

                entity.HasIndex(e => e.UuidImagen, "UQ__imagen__448604A0D616F2C3")
                    .IsUnique();

                entity.Property(e => e.IdImagen).HasColumnName("id_imagen");

                entity.Property(e => e.CategoriaImagen)
                    .HasMaxLength(255)
                    .HasColumnName("categoria_imagen");

                entity.Property(e => e.DescripcionImagen)
                    .HasMaxLength(1000)
                    .HasColumnName("descripcion_imagen");

                entity.Property(e => e.EstadoImagen).HasColumnName("estado_imagen");

                entity.Property(e => e.NombreImagen)
                    .HasMaxLength(255)
                    .HasColumnName("nombre_imagen");

                entity.Property(e => e.UuidImagen)
                    .HasMaxLength(50)
                    .HasColumnName("uuid_imagen");
            });

            modelBuilder.Entity<Laboratorio>(entity =>
            {
                entity.HasKey(e => e.IdLaboratorio)
                    .HasName("PK__laborato__781B42E24E2C0DF0");

                entity.ToTable("laboratorio");

                entity.HasIndex(e => e.UuidLaboratorios, "UQ__laborato__4D3AAC176B937164")
                    .IsUnique();

                entity.Property(e => e.IdLaboratorio).HasColumnName("id_laboratorio");

                entity.Property(e => e.CategoriaLaboratorios)
                    .HasMaxLength(255)
                    .HasColumnName("categoria_laboratorios");

                entity.Property(e => e.DescripcionLaboratorio)
                    .HasMaxLength(1000)
                    .HasColumnName("descripcion_laboratorio");

                entity.Property(e => e.EstadoLaboratorios).HasColumnName("estado_laboratorios");

                entity.Property(e => e.NombreLaboratorio)
                    .HasMaxLength(255)
                    .HasColumnName("nombre_laboratorio");

                entity.Property(e => e.UuidLaboratorios)
                    .HasMaxLength(50)
                    .HasColumnName("uuid_laboratorios");
            });

            modelBuilder.Entity<Localidad>(entity =>
            {
                entity.HasKey(e => e.IdLocalidad)
                    .HasName("PK__localida__9A5E82AA32BEAD1D");

                entity.ToTable("localidad");

                entity.Property(e => e.IdLocalidad).HasColumnName("id_localidad");

                entity.Property(e => e.CiaLocalidad)
                    .HasMaxLength(10)
                    .HasColumnName("cia_localidad");

                entity.Property(e => e.CodigoLocalidad)
                    .HasMaxLength(10)
                    .HasColumnName("codigo_localidad");

                entity.Property(e => e.EstadoLocalidad).HasColumnName("estado_localidad");

                entity.Property(e => e.FechacreacionLocalidad)
                    .HasColumnType("datetime")
                    .HasColumnName("fechacreacion_localidad")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FechamodificacionLocalidad)
                    .HasColumnType("datetime")
                    .HasColumnName("fechamodificacion_localidad")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GentilicioLocalidad)
                    .HasMaxLength(255)
                    .HasColumnName("gentilicio_localidad");

                entity.Property(e => e.IsoLocalidad)
                    .HasMaxLength(10)
                    .HasColumnName("iso_localidad");

                entity.Property(e => e.IsoadLocalidad)
                    .HasMaxLength(10)
                    .HasColumnName("isoad_localidad");

                entity.Property(e => e.NombreLocalidad)
                    .HasMaxLength(255)
                    .HasColumnName("nombre_localidad");

                entity.Property(e => e.PaisId).HasColumnName("pais_id");

                entity.Property(e => e.PrefijoLocalidad)
                    .HasMaxLength(10)
                    .HasColumnName("prefijo_localidad");

                entity.Property(e => e.UsuariocreacionLocalidad)
                    .HasMaxLength(255)
                    .HasColumnName("usuariocreacion_localidad");

                entity.Property(e => e.UsuariomodificacionLocalidad)
                    .HasMaxLength(255)
                    .HasColumnName("usuariomodificacion_localidad");

                entity.HasOne(d => d.Pais)
                    .WithMany(p => p.Localidads)
                    .HasForeignKey(d => d.PaisId)
                    .HasConstraintName("FK_localidad_pais");
            });

            modelBuilder.Entity<Medicamento>(entity =>
            {
                entity.HasKey(e => e.IdMedicamento)
                    .HasName("PK__medicame__2588C03291709D0B");

                entity.ToTable("medicamentos");

                entity.HasIndex(e => e.UuidMedicamento, "UQ__medicame__307E4C1D4BED3B83")
                    .IsUnique();

                entity.Property(e => e.IdMedicamento).HasColumnName("id_medicamento");

                entity.Property(e => e.CategoriaMedicamento)
                    .HasMaxLength(255)
                    .HasColumnName("categoria_medicamento");

                entity.Property(e => e.ConcentracionMedicamento)
                    .HasMaxLength(50)
                    .HasColumnName("concentracion_medicamento");

                entity.Property(e => e.DescripcionMedicamento)
                    .HasMaxLength(1000)
                    .HasColumnName("descripcion_medicamento");

                entity.Property(e => e.DistintivoMedicamento)
                    .HasMaxLength(50)
                    .HasColumnName("distintivo_medicamento");

                entity.Property(e => e.EstadoMedicamento).HasColumnName("estado_medicamento");

                entity.Property(e => e.NombreMedicamento)
                    .HasMaxLength(255)
                    .HasColumnName("nombre_medicamento");

                entity.Property(e => e.UuidMedicamento)
                    .HasMaxLength(50)
                    .HasColumnName("uuid_medicamento");
            });

            modelBuilder.Entity<OrganosSistema>(entity =>
            {
                entity.HasKey(e => e.IdOrganosSistemas)
                    .HasName("PK__organos___222F238362559B54");

                entity.ToTable("organos_sistemas");

                entity.Property(e => e.IdOrganosSistemas).HasColumnName("id_organos_sistemas");

                entity.Property(e => e.CardioVascular).HasColumnName("cardio_vascular");

                entity.Property(e => e.Digestivo).HasColumnName("digestivo");

                entity.Property(e => e.Endocrino).HasColumnName("endocrino");

                entity.Property(e => e.Genital).HasColumnName("genital");

                entity.Property(e => e.Linfatico).HasColumnName("linfatico");

                entity.Property(e => e.MEsqueletico).HasColumnName("m_esqueletico");

                entity.Property(e => e.Nervioso).HasColumnName("nervioso");

                entity.Property(e => e.ObserCardioVascular)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_cardio_vascular");

                entity.Property(e => e.ObserDigestivo)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_digestivo");

                entity.Property(e => e.ObserEndocrino)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_endocrino");

                entity.Property(e => e.ObserGenital)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_genital");

                entity.Property(e => e.ObserLinfatico)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_linfatico");

                entity.Property(e => e.ObserMEsqueletico)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_m_esqueletico");

                entity.Property(e => e.ObserNervioso)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_nervioso");

                entity.Property(e => e.ObserOrgSentidos)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_org_sentidos");

                entity.Property(e => e.ObserRespiratorio)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_respiratorio");

                entity.Property(e => e.ObserUrinario)
                    .HasMaxLength(1000)
                    .HasColumnName("obser_urinario");

                entity.Property(e => e.OrgSentidos).HasColumnName("org_sentidos");

                entity.Property(e => e.Respiratorio).HasColumnName("respiratorio");

                entity.Property(e => e.Urinario).HasColumnName("urinario");
            });

            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.HasKey(e => e.IdPacientes)
                    .HasName("PK__paciente__D80336DAD3E20010");

                entity.ToTable("pacientes");

                entity.HasIndex(e => e.CiPacientes, "UQ__paciente__47B248371B61B067")
                    .IsUnique();

                entity.Property(e => e.IdPacientes).HasColumnName("id_pacientes");

                entity.Property(e => e.CiPacientes).HasColumnName("ci_pacientes");

                entity.Property(e => e.DireccionPacientes).HasColumnName("direccion_pacientes");

                entity.Property(e => e.DonantePacientes)
                    .HasMaxLength(50)
                    .HasColumnName("donante_pacientes");

                entity.Property(e => e.EdadPacientes).HasColumnName("edad_pacientes");

                entity.Property(e => e.EmailPacientes)
                    .HasMaxLength(255)
                    .HasColumnName("email_pacientes");

                entity.Property(e => e.EmpresaPacientes)
                    .HasMaxLength(255)
                    .HasColumnName("empresa_pacientes");

                entity.Property(e => e.EstadoPacientes).HasColumnName("estado_pacientes");

                entity.Property(e => e.EstadocivilPacientesCa).HasColumnName("estadocivil_pacientes_ca");

                entity.Property(e => e.FechacreacionPacientes)
                    .HasColumnType("datetime")
                    .HasColumnName("fechacreacion_pacientes")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FechamodificacionPacientes)
                    .HasColumnType("datetime")
                    .HasColumnName("fechamodificacion_pacientes")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FechanacimientoPacientes)
                    .HasColumnType("date")
                    .HasColumnName("fechanacimiento_pacientes");

                entity.Property(e => e.FormacionprofesionalPacientesCa).HasColumnName("formacionprofesional_pacientes_ca");

                entity.Property(e => e.NacionalidadPacientesPa).HasColumnName("nacionalidad_pacientes_pa");

                entity.Property(e => e.OcupacionPacientes)
                    .HasMaxLength(255)
                    .HasColumnName("ocupacion_pacientes");

                entity.Property(e => e.PrimerapellidoPacientes)
                    .HasMaxLength(255)
                    .HasColumnName("primerapellido_pacientes");

                entity.Property(e => e.PrimernombrePacientes)
                    .HasMaxLength(255)
                    .HasColumnName("primernombre_pacientes");

                entity.Property(e => e.ProvinciaPacientesL).HasColumnName("provincia_pacientes_l");

                entity.Property(e => e.SegundoapellidoPacientes)
                    .HasMaxLength(255)
                    .HasColumnName("segundoapellido_pacientes");

                entity.Property(e => e.SegundonombrePacientes)
                    .HasMaxLength(255)
                    .HasColumnName("segundonombre_pacientes");

                entity.Property(e => e.SegurosaludPacientesCa).HasColumnName("segurosalud_pacientes_ca");

                entity.Property(e => e.SexoPacientesCa).HasColumnName("sexo_pacientes_ca");

                entity.Property(e => e.TelefonocelularPacientes)
                    .HasMaxLength(20)
                    .HasColumnName("telefonocelular_pacientes");

                entity.Property(e => e.TelefonofijoPacientes)
                    .HasMaxLength(20)
                    .HasColumnName("telefonofijo_pacientes");

                entity.Property(e => e.TipodocumentoPacientesCa).HasColumnName("tipodocumento_pacientes_ca");

                entity.Property(e => e.TiposangrePacientesCa).HasColumnName("tiposangre_pacientes_ca");

                entity.Property(e => e.UsuariocreacionPacientes)
                    .HasMaxLength(255)
                    .HasColumnName("usuariocreacion_pacientes");

                entity.Property(e => e.UsuariomodificacionPacientes)
                    .HasMaxLength(255)
                    .HasColumnName("usuariomodificacion_pacientes");

                entity.HasOne(d => d.EstadocivilPacientesCaNavigation)
                    .WithMany(p => p.PacienteEstadocivilPacientesCaNavigations)
                    .HasForeignKey(d => d.EstadocivilPacientesCa)
                    .HasConstraintName("FK_pacientes_estadocivil");

                entity.HasOne(d => d.FormacionprofesionalPacientesCaNavigation)
                    .WithMany(p => p.PacienteFormacionprofesionalPacientesCaNavigations)
                    .HasForeignKey(d => d.FormacionprofesionalPacientesCa)
                    .HasConstraintName("FK_pacientes_formacionprofesional");

                entity.HasOne(d => d.NacionalidadPacientesPaNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.NacionalidadPacientesPa)
                    .HasConstraintName("FK_pacientes_nacionalidad");

                entity.HasOne(d => d.ProvinciaPacientesLNavigation)
                    .WithMany(p => p.Pacientes)
                    .HasForeignKey(d => d.ProvinciaPacientesL)
                    .HasConstraintName("FK_pacientes_provincia");

                entity.HasOne(d => d.SegurosaludPacientesCaNavigation)
                    .WithMany(p => p.PacienteSegurosaludPacientesCaNavigations)
                    .HasForeignKey(d => d.SegurosaludPacientesCa)
                    .HasConstraintName("FK_pacientes_segurosalud");

                entity.HasOne(d => d.SexoPacientesCaNavigation)
                    .WithMany(p => p.PacienteSexoPacientesCaNavigations)
                    .HasForeignKey(d => d.SexoPacientesCa)
                    .HasConstraintName("FK_pacientes_sexo");

                entity.HasOne(d => d.TipodocumentoPacientesCaNavigation)
                    .WithMany(p => p.PacienteTipodocumentoPacientesCaNavigations)
                    .HasForeignKey(d => d.TipodocumentoPacientesCa)
                    .HasConstraintName("FK_pacientes_tipodocumento");

                entity.HasOne(d => d.TiposangrePacientesCaNavigation)
                    .WithMany(p => p.PacienteTiposangrePacientesCaNavigations)
                    .HasForeignKey(d => d.TiposangrePacientesCa)
                    .HasConstraintName("FK_pacientes_tiposangre");
            });

            modelBuilder.Entity<Pai>(entity =>
            {
                entity.HasKey(e => e.IdPais)
                    .HasName("PK__pais__0941A3A7783BA50E");

                entity.ToTable("pais");

                entity.HasIndex(e => e.IsoPais, "UQ__pais__5515698EDD74C4F0")
                    .IsUnique();

                entity.Property(e => e.IdPais).HasColumnName("id_pais");

                entity.Property(e => e.CodigoPais)
                    .HasMaxLength(5)
                    .HasColumnName("codigo_pais");

                entity.Property(e => e.EstadoPais).HasColumnName("estado_pais");

                entity.Property(e => e.GentilicioPais)
                    .HasMaxLength(255)
                    .HasColumnName("gentilicio_pais");

                entity.Property(e => e.IsoPais)
                    .HasMaxLength(3)
                    .HasColumnName("iso_pais");

                entity.Property(e => e.NombrePais)
                    .HasMaxLength(255)
                    .HasColumnName("nombre_pais");
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.HasKey(e => e.IdPerfil)
                    .HasName("PK__perfil__1D1C8768E099B3E5");

                entity.ToTable("perfil");

                entity.Property(e => e.IdPerfil).HasColumnName("id_perfil");

                entity.Property(e => e.DescripcionPerfil)
                    .HasMaxLength(1000)
                    .HasColumnName("descripcion_perfil");

                entity.Property(e => e.EstadoPerfil).HasColumnName("estado_perfil");

                entity.Property(e => e.FechacreacionPerfil)
                    .HasColumnType("datetime")
                    .HasColumnName("fechacreacion_perfil")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NombrePerfil)
                    .HasMaxLength(255)
                    .HasColumnName("nombre_perfil");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario)
                    .HasName("PK__usuario__4E3E04AD8AFA49AA");

                entity.ToTable("usuario");

                entity.HasIndex(e => e.CiUsuario, "UQ__usuario__38AEB2A867056BC2")
                    .IsUnique();

                entity.HasIndex(e => e.EmailUsuario, "UQ__usuario__CD3151FFAF94AA30")
                    .IsUnique();

                entity.Property(e => e.IdUsuario).HasColumnName("id_usuario");

                entity.Property(e => e.ApellidosUsuario)
                    .HasMaxLength(255)
                    .HasColumnName("apellidos_usuario");

                entity.Property(e => e.CiUsuario).HasColumnName("ci_usuario");

                entity.Property(e => e.ClaveUsuario)
                    .HasMaxLength(255)
                    .HasColumnName("clave_usuario");

                entity.Property(e => e.CodigoSenecyt)
                    .HasMaxLength(255)
                    .HasColumnName("codigo_senecyt");

                entity.Property(e => e.CodigoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("codigo_usuario");

                entity.Property(e => e.CodigoqrUsuario).HasColumnName("codigoqr_usuario");

                entity.Property(e => e.DireccionUsuario)
                    .HasMaxLength(1000)
                    .HasColumnName("direccion_usuario");

                entity.Property(e => e.EmailUsuario)
                    .HasMaxLength(255)
                    .HasColumnName("email_usuario");

                entity.Property(e => e.EspecialidadId).HasColumnName("especialidad_id");

                entity.Property(e => e.EstablecimientoId).HasColumnName("establecimiento_id");

                entity.Property(e => e.EstadoUsuario).HasColumnName("estado_usuario");

                entity.Property(e => e.FechacreacionUsuario)
                    .HasColumnType("datetime")
                    .HasColumnName("fechacreacion_usuario")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FechamodificacionUsuario)
                    .HasColumnType("datetime")
                    .HasColumnName("fechamodificacion_usuario")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.FirmadigitalUsuario).HasColumnName("firmadigital_usuario");

                entity.Property(e => e.LoginUsuario)
                    .HasMaxLength(255)
                    .HasColumnName("login_usuario");

                entity.Property(e => e.NombresUsuario)
                    .HasMaxLength(255)
                    .HasColumnName("nombres_usuario");

                entity.Property(e => e.PerfilId).HasColumnName("perfil_id");

                entity.Property(e => e.TelefonoUsuario)
                    .HasMaxLength(20)
                    .HasColumnName("telefono_usuario");

                entity.HasOne(d => d.Especialidad)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.EspecialidadId)
                    .HasConstraintName("FK_usuario_especialidad");

                entity.HasOne(d => d.Establecimiento)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.EstablecimientoId)
                    .HasConstraintName("FK_usuario_establecimiento");

                entity.HasOne(d => d.Perfil)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.PerfilId)
                    .HasConstraintName("FK_usuario_perfil");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
