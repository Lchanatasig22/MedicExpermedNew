--Creacion de sp

--sp_ValidateCredentials

CREATE PROCEDURE dbo.sp_ValidateCredentials
    @login_usuario NVARCHAR(255),
    @clave_usuario NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        u.id_usuario,
        u.ci_usuario,
        u.nombres_usuario,
        u.apellidos_usuario,
        u.telefono_usuario,
        u.email_usuario,
        u.fechacreacion_usuario,
        u.fechamodificacion_usuario,
        u.direccion_usuario,
        u.firmadigital_usuario,
        u.codigoqr_usuario,
        u.codigo_senecyt,
        u.login_usuario,
        u.codigo_usuario,
        u.estado_usuario,
        u.perfil_id,
        p.descripcion_perfil AS descripcion_perfil,
        u.establecimiento_id,
        es.direccion_establecimiento AS direccion_establecimiento,
        u.especialidad_id,
        e.nombre_especialidad AS nombre_especialidad
    FROM 
        usuario u
    INNER JOIN 
        perfil p ON u.perfil_id = p.id_perfil
    INNER JOIN 
        especialidad e ON u.especialidad_id = e.id_especialidad
    INNER JOIN 
        establecimiento es ON u.establecimiento_id = es.id_establecimiento
    WHERE 
        u.login_usuario = @login_usuario
        AND u.clave_usuario = @clave_usuario
        AND u.estado_usuario = 1;  -- Asegura que el usuario esté activo
END;

EXEC [dbo].[sp_ValidateCredentials] @login_usuario = 'admin', @clave_usuario = 'admin';


--PROCEDIMIENTOS ALMACENADOS PARA LA TABLA USUARIO--

--Insert Usuario--
ALTER PROCEDURE sp_CreateUser
    @ci_usuario INT,
    @nombres_usuario NVARCHAR(255),
    @apellidos_usuario NVARCHAR(255),
    @telefono_usuario NVARCHAR(20),
    @email_usuario NVARCHAR(255),
    @direccion_usuario NVARCHAR(1000),
    @firmadigital_usuario VARBINARY(MAX),
    @codigoqr_usuario VARBINARY(MAX),
    @codigo_senecyt NVARCHAR(255),
    @login_usuario NVARCHAR(255),
    @clave_usuario NVARCHAR(255),
    @estado_usuario INT,
    @perfil_id INT = NULL,
    @establecimiento_id INT = NULL,
    @especialidad_id INT = NULL
AS
BEGIN
    DECLARE @codigo_usuario NVARCHAR(20);
    DECLARE @next_sequence INT;

    -- Obtener el siguiente número en la secuencia
    SELECT @next_sequence = ISNULL(MAX(CAST(SUBSTRING(codigo_usuario, 4, 3) AS INT)), 0) + 1
    FROM usuario;

    -- Generar el código de usuario con el prefijo 'COD' y el número secuencial con ceros a la izquierda
    SET @codigo_usuario = 'COD' + RIGHT('000' + CAST(@next_sequence AS NVARCHAR(3)), 3);

    -- Insertar un nuevo usuario en la tabla
    INSERT INTO usuario (
        ci_usuario, 
        nombres_usuario, 
        apellidos_usuario, 
        telefono_usuario, 
        email_usuario, 
        fechacreacion_usuario, 
        fechamodificacion_usuario, 
        direccion_usuario, 
        firmadigital_usuario, 
        codigoqr_usuario, 
        codigo_senecyt, 
        login_usuario, 
        clave_usuario, 
        codigo_usuario, 
        estado_usuario, 
        perfil_id, 
        establecimiento_id, 
        especialidad_id
    )
    VALUES (
        @ci_usuario, 
        @nombres_usuario, 
        @apellidos_usuario, 
        @telefono_usuario, 
        @email_usuario, 
        GETDATE(), -- fechacreacion_usuario
        GETDATE(), -- fechamodificacion_usuario
        @direccion_usuario, 
        @firmadigital_usuario, 
        @codigoqr_usuario, 
        @codigo_senecyt, 
        @login_usuario, 
        @clave_usuario, 
        @codigo_usuario, -- Código generado con formato 'COD###'
        @estado_usuario, 
        @perfil_id, 
        @establecimiento_id, 
        @especialidad_id
    );
END;
--UPDATE USER--
CREATE PROCEDURE sp_UpdateUser
    @id_usuario INT,
    @ci_usuario INT,
    @nombres_usuario NVARCHAR(255),
    @apellidos_usuario NVARCHAR(255),
    @telefono_usuario NVARCHAR(20),
    @email_usuario NVARCHAR(255),
    @direccion_usuario NVARCHAR(1000),
    @firmadigital_usuario VARBINARY(MAX) = NULL, -- Puede ser NULL si no se proporciona
    @codigoqr_usuario VARBINARY(MAX) = NULL,     -- Puede ser NULL si no se proporciona
    @codigo_senecyt NVARCHAR(255),
    @login_usuario NVARCHAR(255),
    @clave_usuario NVARCHAR(255),
    @estado_usuario INT,
    @perfil_id INT = NULL,          -- Puede ser NULL si no se proporciona
    @establecimiento_id INT = NULL, -- Puede ser NULL si no se proporciona
    @especialidad_id INT = NULL     -- Puede ser NULL si no se proporciona
AS
BEGIN
    -- Actualizar los datos del usuario
    UPDATE usuario
    SET 
        ci_usuario = @ci_usuario,
        nombres_usuario = @nombres_usuario,
        apellidos_usuario = @apellidos_usuario,
        telefono_usuario = @telefono_usuario,
        email_usuario = @email_usuario,
        direccion_usuario = @direccion_usuario,
        firmadigital_usuario = ISNULL(@firmadigital_usuario, firmadigital_usuario), -- Mantener el valor anterior si es NULL
        codigoqr_usuario = ISNULL(@codigoqr_usuario, codigoqr_usuario),             -- Mantener el valor anterior si es NULL
        codigo_senecyt = @codigo_senecyt,
        login_usuario = @login_usuario,
        clave_usuario = @clave_usuario,
        estado_usuario = @estado_usuario,
        perfil_id = @perfil_id,
        establecimiento_id = @establecimiento_id,
        especialidad_id = @especialidad_id,
        fechamodificacion_usuario = GETDATE() -- Actualizar la fecha de modificación
    WHERE 
        id_usuario = @id_usuario;
END;
--CAMBIAR ESTADO USUARIO--
CREATE PROCEDURE sp_UpdateState
    @id_usuario INT
AS
BEGIN
    -- Verificar el estado actual del usuario
    DECLARE @estado_actual INT;

    SELECT @estado_actual = estado_usuario 
    FROM usuario 
    WHERE id_usuario = @id_usuario;

    -- Alternar el estado del usuario: si está activo (1), cambiar a inactivo (0) y viceversa
    IF @estado_actual = 1
    BEGIN
        SET @estado_actual = 0;
    END
    ELSE IF @estado_actual = 0
    BEGIN
        SET @estado_actual = 1;
    END

    -- Actualizar el estado del usuario y la fecha de modificación
    UPDATE usuario
    SET 
        estado_usuario = @estado_actual,
        fechamodificacion_usuario = GETDATE()
    WHERE 
        id_usuario = @id_usuario;
END;

--PROCEDIMIENTOS ALMACENADOS PARA PACIENTES--
--Insert Pacientes--
CREATE PROCEDURE sp_CreatePatient
    @usuariocreacion_pacientes NVARCHAR(255),
    @tipodocumento_pacientes_ca INT,
    @ci_pacientes INT,
    @primernombre_pacientes NVARCHAR(255),
    @segundonombre_pacientes NVARCHAR(255) = NULL,
    @primerapellido_pacientes NVARCHAR(255),
    @segundoapellido_pacientes NVARCHAR(255) = NULL,
    @sexo_pacientes_ca INT,
    @fechanacimiento_pacientes DATE,
    @edad_pacientes INT,
    @tiposangre_pacientes_ca INT,
    @donante_pacientes NVARCHAR(50),
    @estadocivil_pacientes_ca INT,
    @formacionprofesional_pacientes_ca INT,
    @telefonofijo_pacientes NVARCHAR(20) = NULL,
    @telefonocelular_pacientes NVARCHAR(20) = NULL,
    @email_pacientes NVARCHAR(255) = NULL,
    @nacionalidad_pacientes_pa INT,
    @provincia_pacientes_l INT,
    @direccion_pacientes NVARCHAR(MAX),
    @ocupacion_pacientes NVARCHAR(255) = NULL,
    @empresa_pacientes NVARCHAR(255) = NULL,
    @segurosalud_pacientes_ca INT,
    @estado_pacientes INT
AS
BEGIN
    -- Insertar un nuevo paciente en la tabla `pacientes`
    INSERT INTO pacientes (
        fechacreacion_pacientes,
        usuariocreacion_pacientes,
        fechamodificacion_pacientes,
        usuariomodificacion_pacientes,
        tipodocumento_pacientes_ca,
        ci_pacientes,
        primernombre_pacientes,
        segundonombre_pacientes,
        primerapellido_pacientes,
        segundoapellido_pacientes,
        sexo_pacientes_ca,
        fechanacimiento_pacientes,
        edad_pacientes,
        tiposangre_pacientes_ca,
        donante_pacientes,
        estadocivil_pacientes_ca,
        formacionprofesional_pacientes_ca,
        telefonofijo_pacientes,
        telefonocelular_pacientes,
        email_pacientes,
        nacionalidad_pacientes_pa,
        provincia_pacientes_l,
        direccion_pacientes,
        ocupacion_pacientes,
        empresa_pacientes,
        segurosalud_pacientes_ca,
        estado_pacientes
    )
    VALUES (
        GETDATE(), -- fechacreacion_pacientes
        @usuariocreacion_pacientes,
        GETDATE(), -- fechamodificacion_pacientes
        @usuariocreacion_pacientes, -- Al crear, el usuario de creación y modificación es el mismo
        @tipodocumento_pacientes_ca,
        @ci_pacientes,
        @primernombre_pacientes,
        @segundonombre_pacientes,
        @primerapellido_pacientes,
        @segundoapellido_pacientes,
        @sexo_pacientes_ca,
        @fechanacimiento_pacientes,
        @edad_pacientes,
        @tiposangre_pacientes_ca,
        @donante_pacientes,
        @estadocivil_pacientes_ca,
        @formacionprofesional_pacientes_ca,
        @telefonofijo_pacientes,
        @telefonocelular_pacientes,
        @email_pacientes,
        @nacionalidad_pacientes_pa,
        @provincia_pacientes_l,
        @direccion_pacientes,
        @ocupacion_pacientes,
        @empresa_pacientes,
        @segurosalud_pacientes_ca,
        @estado_pacientes
    );

    -- Opcional: Devolver el ID del nuevo paciente insertado
    SELECT SCOPE_IDENTITY() AS id_pacientes;
END;

--UPDATE PACIENTE--
CREATE PROCEDURE sp_UpdatePatient
    @id_pacientes INT,  -- Identificador único del paciente
    @usuariomodificacion_pacientes NVARCHAR(255),
    @tipodocumento_pacientes_ca INT,
    @ci_pacientes INT,
    @primernombre_pacientes NVARCHAR(255),
    @segundonombre_pacientes NVARCHAR(255) = NULL,
    @primerapellido_pacientes NVARCHAR(255),
    @segundoapellido_pacientes NVARCHAR(255) = NULL,
    @sexo_pacientes_ca INT,
    @fechanacimiento_pacientes DATE,
    @edad_pacientes INT,
    @tiposangre_pacientes_ca INT,
    @donante_pacientes NVARCHAR(50),
    @estadocivil_pacientes_ca INT,
    @formacionprofesional_pacientes_ca INT,
    @telefonofijo_pacientes NVARCHAR(20) = NULL,
    @telefonocelular_pacientes NVARCHAR(20) = NULL,
    @email_pacientes NVARCHAR(255) = NULL,
    @nacionalidad_pacientes_pa INT,
    @provincia_pacientes_l INT,
    @direccion_pacientes NVARCHAR(MAX),
    @ocupacion_pacientes NVARCHAR(255) = NULL,
    @empresa_pacientes NVARCHAR(255) = NULL,
    @segurosalud_pacientes_ca INT,
    @estado_pacientes INT
AS
BEGIN
    -- Actualizar el paciente en la tabla `pacientes`
    UPDATE pacientes
    SET
        usuariomodificacion_pacientes = @usuariomodificacion_pacientes,
        fechamodificacion_pacientes = GETDATE(),
        tipodocumento_pacientes_ca = @tipodocumento_pacientes_ca,
        ci_pacientes = @ci_pacientes,
        primernombre_pacientes = @primernombre_pacientes,
        segundonombre_pacientes = @segundonombre_pacientes,
        primerapellido_pacientes = @primerapellido_pacientes,
        segundoapellido_pacientes = @segundoapellido_pacientes,
        sexo_pacientes_ca = @sexo_pacientes_ca,
        fechanacimiento_pacientes = @fechanacimiento_pacientes,
        edad_pacientes = @edad_pacientes,
        tiposangre_pacientes_ca = @tiposangre_pacientes_ca,
        donante_pacientes = @donante_pacientes,
        estadocivil_pacientes_ca = @estadocivil_pacientes_ca,
        formacionprofesional_pacientes_ca = @formacionprofesional_pacientes_ca,
        telefonofijo_pacientes = @telefonofijo_pacientes,
        telefonocelular_pacientes = @telefonocelular_pacientes,
        email_pacientes = @email_pacientes,
        nacionalidad_pacientes_pa = @nacionalidad_pacientes_pa,
        provincia_pacientes_l = @provincia_pacientes_l,
        direccion_pacientes = @direccion_pacientes,
        ocupacion_pacientes = @ocupacion_pacientes,
        empresa_pacientes = @empresa_pacientes,
        segurosalud_pacientes_ca = @segurosalud_pacientes_ca,
        estado_pacientes = @estado_pacientes
    WHERE
        id_pacientes = @id_pacientes;

    -- Opcional: Devolver un indicador de éxito o el ID del paciente actualizado
    SELECT @id_pacientes AS id_pacientes;
END;


--DELETE PATIENT--
CREATE PROCEDURE sp_DeletePatient
    @id_pacientes INT
AS
BEGIN
    -- Iniciar una transacción para asegurar la atomicidad de las operaciones
    BEGIN TRANSACTION;

    BEGIN TRY
        -- Eliminar citas asociadas al paciente
        DELETE FROM cita
        WHERE paciente_id = @id_pacientes;

        -- Eliminar consultas asociadas al paciente
        DELETE FROM consulta
        WHERE paciente_consulta_p = @id_pacientes;

        -- Eliminar el paciente
        DELETE FROM pacientes
        WHERE id_pacientes = @id_pacientes;

        -- Confirmar la transacción
        COMMIT TRANSACTION;

        -- Devolver un indicador de éxito
        SELECT 1 AS Success;
    END TRY
    BEGIN CATCH
        -- Si ocurre un error, deshacer la transacción
        ROLLBACK TRANSACTION;

        -- Devolver el error
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;

--FORMULARIOS DE LAS CITAS--

--OBTENER HORAS--
ALTER PROCEDURE sp_GetAvailableHours
    @usuario_id INT,
    @fecha DATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar la fecha
    IF @fecha < '1753-01-01'
    BEGIN
        RAISERROR('Fecha fuera del rango permitido.', 16, 1);
        RETURN;
    END

    -- Definir el rango de horas laborales
    DECLARE @hora_inicio TIME = '08:00';
    DECLARE @hora_fin TIME = '18:00';
    DECLARE @intervalo_minutos INT = 30;

    -- Crear una tabla temporal para almacenar los intervalos de tiempo disponibles
    DECLARE @horas_disponibles TABLE (hora TIME);

    -- Llenar la tabla temporal con los intervalos de tiempo
    DECLARE @hora_actual TIME = @hora_inicio;
    WHILE @hora_actual < @hora_fin
    BEGIN
        INSERT INTO @horas_disponibles (hora)
        VALUES (@hora_actual);

        -- Incrementar la hora actual por el intervalo definido
        SET @hora_actual = DATEADD(MINUTE, @intervalo_minutos, @hora_actual);
    END

    -- Eliminar las horas ya ocupadas por citas
    DELETE FROM @horas_disponibles
    WHERE hora IN (
        SELECT horadelacita_cita
        FROM cita
        WHERE usuario_id = @usuario_id AND fechadelacita_cita = @fecha
    );

    -- Retornar las horas disponibles
    SELECT hora AS horadelacitaCita
    FROM @horas_disponibles
    ORDER BY hora;
END;


--CREAR CITAS--
ALTER PROCEDURE sp_CreateAppointment
    @usuariocreacion_cita NVARCHAR(255),
    @fechadelacita_cita DATE,
    @horadelacita_cita TIME(7),
    @usuario_id INT,
    @paciente_id INT,
    @consulta_id INT,
    @motivo NVARCHAR(MAX),
	@estado_cita INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Insertar una nueva cita en la tabla `cita`
    INSERT INTO cita (
        fechacreacion_cita,
        usuariocreacion_cita,
        fechadelacita_cita,
        horadelacita_cita,
        usuario_id,
        paciente_id,
        consulta_id,
        motivo,
		estado_cita
    )
    VALUES (
        GETDATE(),                -- fechacreacion_cita
        @usuariocreacion_cita,    -- usuariocreacion_cita
        @fechadelacita_cita,      -- fechadelacita_cita
        @horadelacita_cita,       -- horadelacita_cita
        @usuario_id,              -- usuario_id
        @paciente_id,             -- paciente_id
        @consulta_id,             -- consulta_id
        @motivo,                   -- motivo
		@estado_cita
    );

    -- Opcional: Devolver el ID de la nueva cita insertada
    SELECT SCOPE_IDENTITY() AS id_cita;
END;

--Actualizar Cita--
ALTER PROCEDURE sp_UpdateAppointment
    @id_cita INT,
    @fechacreacion_cita DATETIME = NULL,
    @usuariocreacion_cita NVARCHAR(255) = NULL,
    @fechadelacita_cita DATE = NULL,
    @horadelacita_cita TIME(7) = NULL,
    @usuario_id INT = NULL,
    @paciente_id INT = NULL,
    @consulta_id INT = NULL,
    @motivo NVARCHAR(MAX) = NULL,
	@estado_cita INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Actualizar la cita en la tabla `cita`
        UPDATE cita
        SET
            fechacreacion_cita = ISNULL(@fechacreacion_cita, fechacreacion_cita),
            usuariocreacion_cita = ISNULL(@usuariocreacion_cita, usuariocreacion_cita),
            fechadelacita_cita = ISNULL(@fechadelacita_cita, fechadelacita_cita),
            horadelacita_cita = ISNULL(@horadelacita_cita, horadelacita_cita),
            usuario_id = ISNULL(@usuario_id, usuario_id),
            paciente_id = ISNULL(@paciente_id, paciente_id),
            consulta_id = @consulta_id,  -- Asignar el valor directamente, ya que puede ser NULL
            motivo = ISNULL(@motivo, motivo),
			estado_cita = ISNULL(@estado_cita,estado_cita)
        WHERE
            id_cita = @id_cita;

        -- Verificar si se actualizó alguna fila
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR('No se encontró la cita con el ID especificado.', 16, 1);
        END
    END TRY
    BEGIN CATCH
        -- Registrar el error en una tabla de errores, si existe, o manejarlo de manera adecuada
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        -- Aquí podrías insertar el error en una tabla de errores, si es necesario
        -- INSERT INTO LogErrores (ErrorMessage, ErrorSeverity, ErrorState, ErrorDate)
        -- VALUES (@ErrorMessage, @ErrorSeverity, @ErrorState, GETDATE());

        -- Rethrow the error to the calling process
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;


--Borrar Cita--
CREATE PROCEDURE sp_DeleteAppointment
    @id_cita INT,
    @estado INT  -- Valor predeterminado para el estado de la cita
AS
BEGIN
    SET NOCOUNT ON;

    -- Actualizar el estado de la cita en la tabla `cita`
    UPDATE cita
    SET estado_cita = @estado
    WHERE id_cita = @id_cita;

    -- Verificar si se actualizó alguna fila
    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR('No se encontró la cita con el ID especificado.', 16, 1);
    END
END;


--SP PARA CONSULTAS--
--Busqueda del paciente--
CREATE PROCEDURE sp_SearchPatient
    @Cedula INT = NULL,
    @PrimerNombre NVARCHAR(255) = NULL,
    @PrimerApellido NVARCHAR(255) = NULL
AS
BEGIN
    -- Asegura que solo retorne los resultados si al menos un parámetro es proporcionado
    IF @Cedula IS NULL AND @PrimerNombre IS NULL AND @PrimerApellido IS NULL
    BEGIN
        PRINT 'Debe proporcionar al menos un criterio de búsqueda.'
        RETURN;
    END

    -- Consulta para buscar pacientes según los criterios proporcionados
    SELECT
        id_pacientes,
        fechacreacion_pacientes,
        usuariocreacion_pacientes,
        fechamodificacion_pacientes,
        usuariomodificacion_pacientes,
        tipodocumento_pacientes_ca,
        ci_pacientes,
        primernombre_pacientes,
        segundonombre_pacientes,
        primerapellido_pacientes,
        segundoapellido_pacientes,
        sexo_pacientes_ca,
        fechanacimiento_pacientes,
        edad_pacientes,
        tiposangre_pacientes_ca,
        donante_pacientes,
        estadocivil_pacientes_ca,
        formacionprofesional_pacientes_ca,
        telefonofijo_pacientes,
        telefonocelular_pacientes,
        email_pacientes,
        nacionalidad_pacientes_pa,
        provincia_pacientes_l,
        direccion_pacientes,
        ocupacion_pacientes,
        empresa_pacientes,
        segurosalud_pacientes_ca,
        estado_pacientes
    FROM
        Pacientes
    WHERE
        (@Cedula IS NULL OR ci_pacientes = @Cedula) AND
        (@PrimerNombre IS NULL OR primernombre_pacientes LIKE '%' + @PrimerNombre + '%') AND
        (@PrimerApellido IS NULL OR primerapellido_pacientes LIKE '%' + @PrimerApellido + '%')
    ORDER BY
        primernombre_pacientes, primerapellido_pacientes;
END
GO
--Crear una nueva consulta--
ALTER PROCEDURE sp_CreateConsultation
    -- Parámetros para la tabla 'consulta'
    @FechaCreacion DATETIME,
    @UsuarioCreacion NVARCHAR(255),
    @Historial NVARCHAR(MAX),
    @PacienteID INT,
    @Motivo NVARCHAR(MAX),
    @Enfermedad NVARCHAR(MAX),
    @NombrePariente NVARCHAR(255),
    @SignosAlarma NVARCHAR(MAX),
    @ReconocFarmacologicas NVARCHAR(MAX),
    @TipoParienteID INT,
    @Telefono NVARCHAR(20),
    @Temperatura NVARCHAR(4),
    @FrecuenciaRespiratoria NVARCHAR(3),
    @PresionArterialSistolica NVARCHAR(3),
    @PresionArterialDiastolica NVARCHAR(3),
    @Pulso NVARCHAR(3),
    @Peso NVARCHAR(3),
    @Talla NVARCHAR(3),
    @PlanTratamiento NVARCHAR(MAX),
    @Observacion NVARCHAR(MAX),
    @AntecedentesPersonales NVARCHAR(MAX),
    @DiasIncapacidad INT,
    @MedicoID INT,
    @EspecialidadID INT,
    @AlergiasID INT,
    @ObserAlergias NVARCHAR(1000),
    @CirugiasID INT,
    @ObserCirugias NVARCHAR(1000),
    @EstadoConsulta INT,
    @TipoConsulta INT,
    @NotasEvolucion NVARCHAR(MAX),
    @ConsultaPrincipal NVARCHAR(MAX),
    @ActivoConsulta INT,
    
    -- Parámetros para tablas secundarias (relaciones de uno a muchos)
    @Medicamentos NVARCHAR(MAX),
    @Laboratorios NVARCHAR(MAX),
    @Imagenes NVARCHAR(MAX),
    @Diagnosticos NVARCHAR(MAX),
    @Cirugias NVARCHAR(MAX),
    @Alergias NVARCHAR(MAX),

    -- Parámetros para tablas de uno a uno
    @AntecedentesFamiliares NVARCHAR(MAX),
    @OrganosSistemas NVARCHAR(MAX),
    @ExamenFisico NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @IdConsulta INT;
    DECLARE @Secuencial NVARCHAR(50);
    DECLARE @NextNumber INT;

    -- Obtener el siguiente número para el secuencial
    SELECT @NextNumber = ISNULL(MAX(CAST(SUBSTRING(secuencial_consulta, 5, LEN(secuencial_consulta)) AS INT)), 0) + 1
    FROM consulta;

    -- Formatear el secuencial como CON-001, CON-002, etc.
    SET @Secuencial = 'CON-' + RIGHT('000' + CAST(@NextNumber AS NVARCHAR(10)), 3);

    -- Insertar en la tabla 'consulta'
    INSERT INTO consulta (
        fechacreacion_consulta, usuariocreacion_consulta, historial_consulta, secuencial_consulta, 
        paciente_consulta_p, motivo_consulta, enfermedad_consulta, nombrepariente_consulta, 
        signosalarma_consulta, reconofarmacologicas, tipopariente_consulta, telefono_consulta, 
        temperatura_consulta, frecuenciarespiratoria_consulta, presionarterialsistolica_consulta, 
        presionarterialdiastolica_consulta, pulso_consulta, peso_consulta, talla_consulta, 
        plantratamiento_consulta, observacion_consulta, antecedentespersonales_consulta, 
        diasincapacidad_consulta, medico_consulta_d, especialidad_id, alergiasconsulta_id, 
        obseralergiasconsulta, cirugiasconsulta_id, obsercirugias_id, estado_consulta_c, 
        tipo_consulta_c, notasevolucion_consulta, consultaprincipal_consulta, activo_consulta
    )
    VALUES (
        @FechaCreacion, @UsuarioCreacion, @Historial, @Secuencial, 
        @PacienteID, @Motivo, @Enfermedad, @NombrePariente, 
        @SignosAlarma, @ReconocFarmacologicas, @TipoParienteID, @Telefono, 
        @Temperatura, @FrecuenciaRespiratoria, @PresionArterialSistolica, 
        @PresionArterialDiastolica, @Pulso, @Peso, @Talla, 
        @PlanTratamiento, @Observacion, @AntecedentesPersonales, 
        @DiasIncapacidad, @MedicoID, @EspecialidadID, @AlergiasID, 
        @ObserAlergias, @CirugiasID, @ObserCirugias, @EstadoConsulta, 
        @TipoConsulta, @NotasEvolucion, @ConsultaPrincipal, @ActivoConsulta
    );

    -- Obtener el ID de la consulta recién creada
    SET @IdConsulta = SCOPE_IDENTITY();

    -- Calcular secuenciales para relaciones de uno a muchos
    DECLARE @SecuencialMedicamento INT = (SELECT ISNULL(MAX(secuencial_medicamentos), 0) + 1 FROM consulta_medicamentos WHERE consulta_medicamentos_id = @IdConsulta);
    DECLARE @SecuencialLaboratorio INT = (SELECT ISNULL(MAX(secuencial_laboratorio), 0) + 1 FROM consulta_laboratorio WHERE consulta_laboratorio_id = @IdConsulta);
    DECLARE @SecuencialImagen INT = (SELECT ISNULL(MAX(secuencial_imagen), 0) + 1 FROM consulta_imagen WHERE consulta_imagen_id = @IdConsulta);
    DECLARE @SecuencialDiagnostico INT = (SELECT ISNULL(MAX(secuencial_diagnostico), 0) + 1 FROM consulta_diagnostico WHERE consulta_diagnostico_id = @IdConsulta);

    -- Inserciones en tablas de uno a uno

    -- Antecedentes Familiares
    INSERT INTO antecedentes_familiares(cardiopatia, obser_cardiopatia, diabetes, obser_diabetes, 
                                                  enf_cardiovascular, obser_enf_cardiovascular, hipertension, 
                                                  obser_hipertension, cancer, obser_cancer, tuberculosis, 
                                                  obser_tuberculosis, enf_mental, obser_enf_mental, 
                                                  enf_infecciosa, obser_enf_infecciosa, mal_formacion, 
                                                  obser_mal_formacion, otro, obser_otro, parentescocatalogo_cardiopatia, 
                                                  parentescocatalogo_diabetes, parentescocatalogo_enfcardiovascular, 
                                                  parentescocatalogo_hipertension, parentescocatalogo_cancer, 
                                                  parentescocatalogo_tuberculosis, parentescocatalogo_enfmental, 
                                                  parentescocatalogo_enfinfecciosa, parentescocatalogo_malformacion, 
                                                  parentescocatalogo_otro)
    SELECT
        JSON_VALUE(@AntecedentesFamiliares, '$.Cardiopatia'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ObserCardiopatia'),
        JSON_VALUE(@AntecedentesFamiliares, '$.Diabetes'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ObserDiabetes'),
        JSON_VALUE(@AntecedentesFamiliares, '$.EnfCardiovascular'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ObserEnfCardiovascular'),
        JSON_VALUE(@AntecedentesFamiliares, '$.Hipertension'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ObserHipertension'),
        JSON_VALUE(@AntecedentesFamiliares, '$.Cancer'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ObserCancer'),
        JSON_VALUE(@AntecedentesFamiliares, '$.Tuberculosis'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ObserTuberculosis'),
        JSON_VALUE(@AntecedentesFamiliares, '$.EnfMental'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ObserEnfMental'),
        JSON_VALUE(@AntecedentesFamiliares, '$.EnfInfecciosa'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ObserEnfInfecciosa'),
        JSON_VALUE(@AntecedentesFamiliares, '$.MalFormacion'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ObserMalFormacion'),
        JSON_VALUE(@AntecedentesFamiliares, '$.Otro'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ObserOtro'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ParentescoCatalogoCardiopatia'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ParentescoCatalogoDiabetes'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ParentescoCatalogoEnfCardiovascular'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ParentescoCatalogoHipertension'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ParentescoCatalogoCancer'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ParentescoCatalogoTuberculosis'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ParentescoCatalogoEnfMental'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ParentescoCatalogoEnfInfecciosa'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ParentescoCatalogoMalFormacion'),
        JSON_VALUE(@AntecedentesFamiliares, '$.ParentescoCatalogoOtro');

    -- Órganos y Sistemas
    INSERT INTO organos_sistemas(org_sentidos, obser_org_sentidos, respiratorio, obser_respiratorio, 
                                           cardio_vascular, obser_cardio_vascular, digestivo, obser_digestivo, 
                                           genital, obser_genital, urinario, obser_urinario, m_esqueletico, 
                                           obser_m_esqueletico, endocrino, obser_endocrino, linfatico, 
                                           obser_linfatico, nervioso, obser_nervioso)
    SELECT
        JSON_VALUE(@OrganosSistemas, '$.OrgSentidos'),
        JSON_VALUE(@OrganosSistemas, '$.ObserOrgSentidos'),
        JSON_VALUE(@OrganosSistemas, '$.Respiratorio'),
        JSON_VALUE(@OrganosSistemas, '$.ObserRespiratorio'),
        JSON_VALUE(@OrganosSistemas, '$.CardioVascular'),
        JSON_VALUE(@OrganosSistemas, '$.ObserCardioVascular'),
        JSON_VALUE(@OrganosSistemas, '$.Digestivo'),
        JSON_VALUE(@OrganosSistemas, '$.ObserDigestivo'),
        JSON_VALUE(@OrganosSistemas, '$.Genital'),
        JSON_VALUE(@OrganosSistemas, '$.ObserGenital'),
        JSON_VALUE(@OrganosSistemas, '$.Urinario'),
        JSON_VALUE(@OrganosSistemas, '$.ObserUrinario'),
        JSON_VALUE(@OrganosSistemas, '$.MEsqueletico'),
        JSON_VALUE(@OrganosSistemas, '$.ObserMEsqueletico'),
        JSON_VALUE(@OrganosSistemas, '$.Endocrino'),
        JSON_VALUE(@OrganosSistemas, '$.ObserEndocrino'),
        JSON_VALUE(@OrganosSistemas, '$.Linfatico'),
        JSON_VALUE(@OrganosSistemas, '$.ObserLinfatico'),
        JSON_VALUE(@OrganosSistemas, '$.Nervioso'),
        JSON_VALUE(@OrganosSistemas, '$.ObserNervioso');

    -- Examen Físico
    INSERT INTO examen_fisico(cabeza, obser_cabeza, cuello, obser_cuello, torax, 
                                        obser_torax, abdomen, obser_abdomen, pelvis, 
                                        obser_pelvis, extremidades, obser_extremidades)
    SELECT
        JSON_VALUE(@ExamenFisico, '$.Cabeza'),
        JSON_VALUE(@ExamenFisico, '$.ObserCabeza'),
        JSON_VALUE(@ExamenFisico, '$.Cuello'),
        JSON_VALUE(@ExamenFisico, '$.ObserCuello'),
        JSON_VALUE(@ExamenFisico, '$.Torax'),
        JSON_VALUE(@ExamenFisico, '$.ObserTorax'),
        JSON_VALUE(@ExamenFisico, '$.Abdomen'),
        JSON_VALUE(@ExamenFisico, '$.ObserAbdomen'),
        JSON_VALUE(@ExamenFisico, '$.Pelvis'),
        JSON_VALUE(@ExamenFisico, '$.ObserPelvis'),
        JSON_VALUE(@ExamenFisico, '$.Extremidades'),
        JSON_VALUE(@ExamenFisico, '$.ObserExtremidades');

    -- Inserciones en tablas secundarias (relaciones de uno a muchos)

    -- Medicamentos
    INSERT INTO consulta_medicamentos (fechacreacion_medicamento, medicamento_id, consulta_medicamentos_id, dosis_medicamento, observacion_medicamento, estado_medicamento, secuencial_medicamentos)
    SELECT
        JSON_VALUE(value, '$.FechaCreacion'),
        JSON_VALUE(value, '$.MedicamentoID'),
        @IdConsulta,
        JSON_VALUE(value, '$.Dosis'),
        JSON_VALUE(value, '$.Observacion'),
        JSON_VALUE(value, '$.Estado'),
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) + (@SecuencialMedicamento - 1)
    FROM OPENJSON(@Medicamentos);

    -- Laboratorios
    INSERT INTO consulta_laboratorio (cantidad_laboratorio, consulta_laboratorio_id, observacion, catalogo_laboratorio_id, estado_laboratorio, secuencial_laboratorio)
    SELECT
        JSON_VALUE(value, '$.Cantidad'),
        @IdConsulta,
        JSON_VALUE(value, '$.Observacion'),
        JSON_VALUE(value, '$.CatalogoLaboratorioID'),
        JSON_VALUE(value, '$.Estado'),
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) + (@SecuencialLaboratorio - 1)
    FROM OPENJSON(@Laboratorios);

    -- Imágenes
    INSERT INTO consulta_imagen (imagen_id, consulta_imagen_id, observacion_imagen, cantidad_imagen, estado_imagen, secuencial_imagen)
    SELECT
        JSON_VALUE(value, '$.ImagenID'),
        @IdConsulta,
        JSON_VALUE(value, '$.Observacion'),
        JSON_VALUE(value, '$.Cantidad'),
        JSON_VALUE(value, '$.Estado'),
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) + (@SecuencialImagen - 1)
    FROM OPENJSON(@Imagenes);

    -- Diagnósticos
    INSERT INTO consulta_diagnostico (diagnostico_id, consulta_diagnostico_id, observacion_diagnostico, presuntivo_diagnosticos, definitivo_diagnosticos, estado_diagnostico, secuencial_diagnostico)
    SELECT
        JSON_VALUE(value, '$.DiagnosticoID'),
        @IdConsulta,
        JSON_VALUE(value, '$.Observacion'),
        JSON_VALUE(value, '$.Presuntivo'),
        JSON_VALUE(value, '$.Definitivo'),
        JSON_VALUE(value, '$.Estado'),
        ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) + (@SecuencialDiagnostico - 1)
    FROM OPENJSON(@Diagnosticos);

    -- Cirugías
    INSERT INTO consulta_cirugias (fechacreacion_cirugia, catalogocirugia_id, consulta_cirugias_id, observacion_cirugia, estado_cirugias)
    SELECT
        JSON_VALUE(value, '$.FechaCreacion'),
        JSON_VALUE(value, '$.CatalogoCirugiaID'),
        @IdConsulta,
        JSON_VALUE(value, '$.Observacion'),
        JSON_VALUE(value, '$.Estado')
    FROM OPENJSON(@Cirugias);

    -- Alergias
    INSERT INTO consulta_alergias (fechacreacion_alergia, catalogoalergia_id, consulta_alergias_int, observacion_alergias, estado_alergias)
    SELECT
        JSON_VALUE(value, '$.FechaCreacion'),
        JSON_VALUE(value, '$.CatalogoAlergiaID'),
        @IdConsulta,
        JSON_VALUE(value, '$.Observacion'),
        JSON_VALUE(value, '$.Estado')
    FROM OPENJSON(@Alergias);

    -- Retornar el ID de la consulta creada
    SELECT @IdConsulta AS IdConsulta, @Secuencial AS Secuencial;
END;
GO




select * from catalogo where categoria_catalogo = 'ALERGIAS'

update cita set estado_cita = 1 where id_cita=4
EXEC sp_SearchPatient @PrimerNombre = 'Kimberly';
EXEC sp_UpdateAppointment 
    @id_cita = 4, 
    @fechadelacita_cita = '2024-09-01', 
    @horadelacita_cita = '10:00:00', 
    @usuario_id = 1, 
    @paciente_id = 2, 
    @consulta_id = NULL, 
    @motivo = 'Consulta general';
