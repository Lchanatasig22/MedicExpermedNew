document.addEventListener('DOMContentLoaded', function () {
    function setupSearch(opcionId, buscarFieldsId, buscarCriterioId, noResultsId, entityType) {
        const opcionSelect = document.getElementById(opcionId);
        const buscarFields = document.getElementById(buscarFieldsId);
        const buscarCriterio = document.getElementById(buscarCriterioId);
        const noResults = document.getElementById(noResultsId);

        if (opcionSelect && buscarFields && buscarCriterio && noResults) {
            opcionSelect.addEventListener('change', function () {
                onSearchChanged(entityType);
            });

            window.onSearch = function () {
                const criterio = buscarCriterio.value.toLowerCase();
                const optionValue = opcionSelect.value;
                const rows = document.querySelectorAll(`tbody tr[data-type="${entityType}"]`);
                const cards = document.querySelectorAll(`.card-body[data-type="${entityType}"]`);
                let found = false;

                rows.forEach(row => {
                    const cellValue = getCellValue(row, optionValue).toLowerCase();
                    if (cellValue.includes(criterio)) {
                        row.style.display = '';
                        found = true;
                    } else {
                        row.style.display = 'none';
                    }
                });

                cards.forEach(card => {
                    const cellValue = getCardValue(card, optionValue).toLowerCase();
                    if (cellValue.includes(criterio)) {
                        card.parentElement.style.display = '';
                        found = true;
                    } else {
                        card.parentElement.style.display = 'none';
                    }
                });

                noResults.classList.toggle('d-none', found);
            };

            function onSearchChanged(entityType) {
                if (opcionSelect.value === '-2') {
                    buscarFields.classList.add('d-none');
                    showAllRowsAndCards(entityType);
                    noResults.classList.add('d-none');
                } else {
                    buscarFields.classList.remove('d-none');
                }
            }
        }
    }

    function getCellValue(row, optionValue) {
        return row.querySelector(`[data-search-type="${optionValue}"]`).innerText || '';
    }

    function getCardValue(card, optionValue) {
        return card.querySelector(`[data-search-type="${optionValue}"]`).innerText || '';
    }

    function showAllRowsAndCards(entityType) {
        document.querySelectorAll(`tbody tr[data-type="${entityType}"], .card-body[data-type="${entityType}"]`).forEach(el => el.style.display = '');
    }

    // Inicializar la búsqueda para Consultas
    setupSearch('opcionConsultas', 'buscarFieldsConsultas', 'buscarCriterioConsultas', 'noResultsConsultas', 'consultas');

   

});
function showModal() {
    $('#pdfModal').modal('show');
}



function buscarPaciente() {
    const searchValue = document.getElementById('search-input').value.trim();
    const searchCriteria = document.getElementById('search-criteria').value;

    let cedula = null;
    let primerNombre = null;
    let primerApellido = null;

    if (searchCriteria === 'cedula') {
        cedula = searchValue;
    } else if (searchCriteria === 'nombre') {
        primerNombre = searchValue;
    } else if (searchCriteria === 'apellido') {
        primerApellido = searchValue;
    }

    const baseUrl = document.getElementById('search-container').getAttribute('data-url');
    const url = `${baseUrl}?cedula=${encodeURIComponent(cedula)}&primerNombre=${encodeURIComponent(primerNombre)}&primerApellido=${encodeURIComponent(primerApellido)}`;
    console.log("URL construida:", url);

    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            console.log("Datos recibidos:", data);
            const pacientes = data["$values"] || [];

            if (pacientes.length > 0) {
                const paciente = pacientes[0];  // Asumimos que solo necesitas llenar con el primer resultado

                // Rellenar los campos del formulario con los datos del paciente
                document.getElementById('idPaciente').value = paciente.idPacientes;
                document.getElementById('primerApellido').value = paciente.primerapellidoPacientes || '';
                document.getElementById('segundoApellido').value = paciente.segundoapellidoPacientes || '';
                document.getElementById('primerNombre').value = paciente.primernombrePacientes || '';
                document.getElementById('segundoNombre').value = paciente.segundonombrePacientes || '';
                document.getElementById('tipoDocumentoSelect').value = paciente.tipodocumentoPacientesCa || '';
                document.getElementById('numeroDocumento').value = paciente.ciPacientes || '';
                document.getElementById('tipoSangre').value = paciente.tiposangrePacientesCa || '';
                document.getElementById('esdonante').value = paciente.donantePacientes ? 'Si' : 'No';
                document.getElementById('fechaNacimiento').value = paciente.fechanacimientoPacientes ? paciente.fechanacimientoPacientes.split('T')[0] : '';
                document.getElementById('edad').value = paciente.edadPacientes || '';
                document.getElementById('sexoSelect').value = paciente.sexoPacientesCa || '';
                document.getElementById('estadoCivilSelect').value = paciente.estadocivilPacientesCa || '';
                document.getElementById('formacionProfesionalSelect').value = paciente.formacionprofesionalPacientesCa || '';
                document.getElementById('nacionalidadSelect').value = paciente.nacionalidadPacientesPa || '';
                document.getElementById('direccion').value = paciente.direccionPacientes || '';
                document.getElementById('telefono').value = paciente.telefonofijoPacientes || '';
                document.getElementById('telefonoCelular').value = paciente.telefonocelularPacientes || '';
                document.getElementById('email').value = paciente.emailPacientes || '';
                document.getElementById('ocupacion').value = paciente.ocupacionPacientes || '';
                document.getElementById('empresa').value = paciente.empresaPacientes || '';
                document.getElementById('seguroSelect').value = paciente.segurosaludPacientesCa || '';
                document.getElementById('historiaClinica').value = paciente.ciPacientes || '';

             

            } else {
                alert('No se encontraron pacientes.');
            }
        })
        .catch(error => {
            console.error('Error al buscar pacientes:', error);
            alert('Error al buscar pacientes.');
        });
}

// Función para convertir texto a "Title Case"
function toTitleCase(str) {
    return str.toLowerCase().replace(/\b\w/g, function (letter) {
        return letter.toUpperCase();
    });
}

// Función para manejar la transición entre steps y aplicar Title Case
function goToNextStep(stepNumber, event) {
    event.preventDefault();

    if (stepNumber === 2) {
        const historiaClinica = document.getElementById('historiaClinica').value.trim();
        if (!historiaClinica) {
            swal({
                title: "Campo obligatorio",
                text: "El campo 'Historia Clínica' es obligatorio.",
                icon: "warning",
                button: "Ok",
            });
            return;
        }

        // Actualiza la tabla con los datos del paciente
        var pacienteNombre = toTitleCase($('#primerNombre').val()) + ' ' + toTitleCase($('#primerApellido').val());
        var numeroDocumento = $('#numeroDocumento').val();
        var sexo = toTitleCase($('#sexoSelect option:selected').text()); // Muestra el texto del sexo
        var tipoSangre = toTitleCase($('#tipoSangre option:selected').text()); // Muestra el texto del tipo de sangre
        var fechaNacimiento = $('#fechaNacimiento').val();
        var edad = $('#edad').val();
        var estadoCivil = toTitleCase($('#estadoCivilSelect option:selected').text()); // Muestra el texto del estado civil
        var formacionProfesional = toTitleCase($('#formacionProfesionalSelect option:selected').text()); // Muestra el texto de la formación profesional
        var nacionalidad = toTitleCase($('#nacionalidadSelect option:selected').text()); // Muestra el texto de la nacionalidad
        var telefono = $('#telefono').val();
        var telefonoCelular = $('#telefonoCelular').val();
        var email = $('#email').val();

        // Actualiza todos los steps con los datos
        $('#pacienteNombre, #pacienteNombreStep2, #pacienteNombreStep3, #pacienteNombreStep4').text(pacienteNombre);
        $('#numeroDocumentoTabla, #numeroDocumentoTablaStep2, #numeroDocumentoTablaStep3,#numeroDocumentoTablaStep4').text(numeroDocumento);
        $('#sexoTabla, #sexoTablaStep2, #sexoTablaStep3,#sexoTablaStep4').text(sexo);
        $('#tipoSangreTabla, #tipoSangreTablaStep2, #tipoSangreTablaStep3,#tipoSangreTablaStep4').text(tipoSangre);
        $('#fechaNacimientoTabla, #fechaNacimientoTablaStep2, #fechaNacimientoTablaStep3,#fechaNacimientoTablaStep4').text(fechaNacimiento);
        $('#edadTabla, #edadTablaStep2, #edadTablaStep3,#edadTablaStep4').text(edad);
        $('#estadoCivilTabla, #estadoCivilTablaStep2, #estadoCivilTablaStep3,#estadoCivilTablaStep4').text(estadoCivil);
        $('#formacionProfesionalTabla, #formacionProfesionalTablaStep2, #formacionProfesionalTablaStep3,#formacionProfesionalTablaStep4').text(formacionProfesional);
        $('#nacionalidadTabla, #nacionalidadTablaStep2, #nacionalidadTablaStep3,#nacionalidadTablaStep4').text(nacionalidad);
        $('#telefonoTabla, #telefonoTablaStep2, #telefonoTablaStep3,#telefonoTablaStep4').text(telefono);
        $('#telefonoCelularTabla, #telefonoCelularTablaStep2, #telefonoCelularTablaStep3,#telefonoCelularTablaStep4').text(telefonoCelular);
        $('#emailTabla, #emailTablaStep2, #emailTablaStep3,#emailTablaStep4').text(email);
    }

    // Ocultar todos los steps
    const steps = document.querySelectorAll('.setup-content');
    steps.forEach(step => {
        step.style.display = 'none';
    });

    // Mostrar el step seleccionado
    const selectedStep = document.getElementById(`step-${stepNumber}`);
    if (selectedStep) {
        selectedStep.style.display = 'block';
    }

    // Cambiar el estado de los botones en el stepwizard
    const stepButtons = document.querySelectorAll('.stepwizard-step .btn-circle');
    stepButtons.forEach(button => {
        if (parseInt(button.getAttribute('data-step')) === stepNumber) {
            button.classList.remove('btn-secondary');
            button.classList.add('btn-primary');
        } else {
            button.classList.remove('btn-primary');
            button.classList.add('btn-secondary');
        }
    });
}

function goToPreviousStep(stepNumber) {
    // Ocultar todos los steps
    const steps = document.querySelectorAll('.setup-content');
    steps.forEach(step => {
        step.style.display = 'none';
    });

    // Mostrar el step seleccionado (anterior)
    const selectedStep = document.getElementById(`step-${stepNumber}`);
    if (selectedStep) {
        selectedStep.style.display = 'block';
    }

    // Cambiar el estado de los botones en el stepwizard
    const stepButtons = document.querySelectorAll('.stepwizard-step .btn-circle');
    stepButtons.forEach(button => {
        if (parseInt(button.getAttribute('data-step')) === stepNumber) {
            button.classList.remove('btn-secondary');
            button.classList.add('btn-primary');
        } else {
            button.classList.remove('btn-primary');
            button.classList.add('btn-secondary');
        }
    });
}


//Speech
var recognition;
var recognizing = false;

function toggleDictation(textareaId, iconId) {
    if (recognizing) {
        stopDictation(iconId);
    } else {
        startDictation(textareaId, iconId);
    }
}

function startDictation(textareaId, iconId) {
    if (window.hasOwnProperty('webkitSpeechRecognition')) {
        recognition = new webkitSpeechRecognition();

        recognition.continuous = true; // Permite que la grabación sea continua
        recognition.interimResults = false;

        recognition.lang = "es-ES"; // Cambia el idioma según sea necesario

        recognition.onstart = function () {
            recognizing = true;
            updateIconState(iconId);
            console.log("Reconocimiento de voz iniciado. Por favor, hable.");
        };

        recognition.onresult = function (event) {
            const newText = event.results[event.results.length - 1][0].transcript;
            document.getElementById(textareaId).value += ' ' + newText; // Concatena al texto existente
        };

        recognition.onerror = function (event) {
            console.error("Error en el reconocimiento de voz: ", event.error);
        };

        recognition.onend = function () {
            recognizing = false;
            updateIconState(iconId);
            console.log("El reconocimiento de voz ha finalizado.");
        };

        recognition.start();
    } else {
        alert("Tu navegador no soporta el reconocimiento de voz.");
    }
}

function stopDictation(iconId) {
    if (recognition && recognizing) {
        recognizing = false;
        recognition.stop();
        updateIconState(iconId);
        console.log("Reconocimiento de voz detenido.");
    }
}

function updateIconState(iconId) {
    var icon = document.getElementById(iconId);

    if (recognizing) {
        icon.classList.remove('fa-microphone');
        icon.classList.add('fa-microphone-slash');
    } else {
        icon.classList.remove('fa-microphone-slash');
        icon.classList.add('fa-microphone');
    }
}

// Asignar eventos de clic a los iconos
document.getElementById('dictationIcon1').addEventListener('click', function () {
    toggleDictation('antecedentespersonalesConsulta', 'dictationIcon1');
});

document.getElementById('dictationIcon2').addEventListener('click', function () {
    toggleDictation('enfermedadProblema', 'dictationIcon2');
});

document.getElementById('dictationIconPlan').addEventListener('click', function () {
    toggleDictation('plantratamiento_consulta', 'dictationIconPlan');
});

document.getElementById('dictationIconReconofarmacologicas').addEventListener('click', function () {
    toggleDictation('reconofarmacologicas', 'dictationIconReconofarmacologicas');
});

document.getElementById('dictationIconSignosAlarma').addEventListener('click', function () {
    toggleDictation('alergias_consulta', 'dictationIconSignosAlarma');
});
// Asignar eventos de clic al icono
document.getElementById('dictationIconObservacion').addEventListener('click', function () {
    toggleDictation('observacion_consulta', 'dictationIconObservacion');
});
// Inicialización de Select2
$('.select2').select2({
    width: '100%'
});
//Tarjetitas para Alergias y Cirugías

$('.select2-tags').select2({
    placeholder: "Buscar",
    width: '100%'
});

function updateTags(selectId, containerId) {
    const selectedOptions = $(selectId).select2('data');
    const container = $(containerId);
    container.empty(); // Limpiar el contenedor antes de agregar los nuevos tags

    selectedOptions.forEach(option => {
        if (option.id) {
            const tag = $('<span class="tag"></span>').text(option.text);
            const removeIcon = $('<span class="remove-tag">&times;</span>').click(function () {
                const optionElement = $(selectId).find(`option[value='${option.id}']`);
                optionElement.prop('selected', false).trigger('change');
            });
            tag.append(removeIcon);
            container.append(tag);
        }
    });
}

$('#tipoAlergiaSelect').on('change', function () {
    updateTags('#tipoAlergiaSelect', '#alergiasTagsContainer');
    updateHiddenInputs('#tipoAlergiaSelect', '#hiddenAlergiaInputsContainer', 'Alergias');
});

$('#tipoCirugiaSelect').on('change', function () {
    updateTags('#tipoCirugiaSelect', '#cirugiasTagsContainer');
    updateHiddenInputs('#tipoCirugiaSelect', '#hiddenCirugiaInputsContainer', 'Cirugias');
});

// Inicializa las tarjetitas en caso de que haya valores preseleccionados
updateTags('#tipoAlergiaSelect', '#alergiasTagsContainer');
updateTags('#tipoCirugiaSelect', '#cirugiasTagsContainer');

function updateHiddenInputs(selectId, containerId, tipo) {
    const selectedOptions = $(selectId).val(); // Obtener valores seleccionados
    const container = $(containerId);
    container.empty(); // Limpiar inputs antes de agregar nuevos

    selectedOptions.forEach(value => {
        const hiddenInput = $('<input>')
            .attr('type', 'hidden')
            .attr('name', tipo + '[]') // Para separar Alergias y Cirugías
            .val(value);
        container.append(hiddenInput);
    });
}

// Función para mostrar u ocultar la observación según el estado del switch
$('.consulta-antecedente-checked').on('change', function () {
    var id = $(this).attr('id').replace('consulta-antecedente-checked-', '');
    var targetObservacion = $('#consulta-observacion-' + id);

    if ($(this).is(':checked')) {
        targetObservacion.show();
        targetObservacion.find('input, select').prop('disabled', false);
    } else {
        targetObservacion.hide();
        targetObservacion.find('input, select').prop('disabled', true);
    }
});

// Inicializa el estado de los campos al cargar la página
$('.consulta-antecedente-checked').each(function () {
    $(this).trigger('change');
});

//Preecion Arterial
document.getElementById('presionArterial').addEventListener('input', function (e) {
    let value = e.target.value.replace(/\D/g, ''); // Elimina cualquier caracter que no sea un dígito

    if (value.length > 3) {
        value = value.slice(0, 3) + '/' + value.slice(3); // Inserta el '/'
    }

    e.target.value = value; // Actualiza el campo de entrada con el nuevo valor

    // Opcional: Si deseas actualizar los campos ocultos para diastólica y sistólica
    if (value.length >= 5) {
        document.getElementById('PresionarterialdiastolicaConsulta').value = value.slice(4, 6);
        document.getElementById('PresionarterialsistolicaConsulta').value = value.slice(0, 3);
    } else {
        document.getElementById('PresionarterialdiastolicaConsulta').value = '';
        document.getElementById('PresionarterialsistolicaConsulta').value = value.slice(0, 3);
    }
});

//Tablas de a;adir DIAGNOSTICO
document.getElementById('anadirFila').addEventListener('click', function () {
    // Obtener el diagnóstico seleccionado y su texto
    var selectDiagnostico = document.getElementById('DiagnosticoId');
    var diagnosticoId = selectDiagnostico.value;
    var diagnosticoTexto = selectDiagnostico.options[selectDiagnostico.selectedIndex].text;

    if (diagnosticoId === "") {
        alert("Seleccione un diagnóstico antes de añadir.");
        return;
    }

    // Crear elementos HTML para la nueva fila
    var tr = document.createElement('tr');

    // Columna de Diagnóstico
    var tdDiagnostico = document.createElement('td');
    tdDiagnostico.textContent = diagnosticoTexto;
    tr.appendChild(tdDiagnostico);

    // Columna de Presuntivo/Definitivo
    var tdPresuntivoDefinitivo = document.createElement('td');
    var inputPresuntivo = document.createElement('input');
    inputPresuntivo.type = "checkbox";
    inputPresuntivo.className = "presuntivo-diagnostico";
    inputPresuntivo.name = "Diagnosticos[" + diagnosticoId + "].PresuntivoDiagnosticos"; // Binding para presuntivo
    var inputDefinitivo = document.createElement('input');
    inputDefinitivo.type = "checkbox";
    inputDefinitivo.className = "definitivo-diagnostico";
    inputDefinitivo.name = "Diagnosticos[" + diagnosticoId + "].DefinitivoDiagnosticos"; // Binding para definitivo
    tdPresuntivoDefinitivo.appendChild(inputPresuntivo);
    tdPresuntivoDefinitivo.appendChild(document.createTextNode(' Presuntivo '));
    tdPresuntivoDefinitivo.appendChild(inputDefinitivo);
    tdPresuntivoDefinitivo.appendChild(document.createTextNode(' Definitivo '));
    tr.appendChild(tdPresuntivoDefinitivo);

    // Columna para el botón de eliminar
    var tdEliminar = document.createElement('td');
    var btnEliminar = document.createElement('button');
    btnEliminar.className = "btn btn-outline-danger";
    btnEliminar.innerHTML = '<i class="fas fa-trash"></i>';
    btnEliminar.addEventListener('click', function () {
        this.closest('tr').remove();
    });
    tdEliminar.appendChild(btnEliminar);
    tr.appendChild(tdEliminar);

    // Campo oculto para enviar el ID del diagnóstico
    var hiddenInputDiagnosticoId = document.createElement('input');
    hiddenInputDiagnosticoId.type = "hidden";
    hiddenInputDiagnosticoId.className = "diagnostico-id"; // Clase para identificar el ID
    hiddenInputDiagnosticoId.value = diagnosticoId; // Valor del diagnóstico seleccionado
    tr.appendChild(hiddenInputDiagnosticoId);



    // Añadir la fila a la tabla
    document.getElementById('diagnosticoTableBody').appendChild(tr);

    // Opcional: limpiar el select después de añadir
    selectDiagnostico.value = "";
});

//Tablas de a;adir medicamento
document.getElementById('anadirFilaMedicamento').addEventListener('click', function () {
    // Obtener el medicamento seleccionado y su texto
    var selectMedicamento = document.getElementById('MedicamentoId');
    var medicamentoId = selectMedicamento.value;
    var medicamentoTexto = selectMedicamento.options[selectMedicamento.selectedIndex].text;

    if (medicamentoId === "") {
        alert("Seleccione un medicamento antes de añadir.");
        return;
    }

    // Crear la fila de la tabla
    var tr = document.createElement('tr');

    // Columna de Medicamento
    var tdMedicamento = document.createElement('td');
    tdMedicamento.textContent = medicamentoTexto;
    tr.appendChild(tdMedicamento);

    // Columna de Dosis
    var tdDosis = document.createElement('td');
    var inputDosis = document.createElement('input');
    inputDosis.type = "number";
    inputDosis.className = "form-control dosis-medicamento";
    inputDosis.name = "Medicamentos[" + medicamentoId + "].DosisMedicamento"; // Nombre para el binding del modelo
    inputDosis.placeholder = "Cantidad";
    tdDosis.appendChild(inputDosis);
    tr.appendChild(tdDosis);

    // Columna de Observación
    var tdObservacion = document.createElement('td');
    var inputObservacion = document.createElement('input');
    inputObservacion.type = "text";
    inputObservacion.className = "form-control observacion-medicamento";
    inputObservacion.name = "Medicamentos[" + medicamentoId + "].ObservacionMedicamento"; // Nombre para el binding del modelo
    inputObservacion.placeholder = "Observación";
    tdObservacion.appendChild(inputObservacion);
    tr.appendChild(tdObservacion);

    // Columna para el botón de eliminar
    var tdEliminar = document.createElement('td');
    var btnEliminar = document.createElement('button');
    btnEliminar.className = "btn btn-outline-danger";
    btnEliminar.innerHTML = '<i class="fas fa-trash"></i>';
    btnEliminar.addEventListener('click', function () {
        this.closest('tr').remove(); // Elimina la fila actual
    });
    tdEliminar.appendChild(btnEliminar);
    tr.appendChild(tdEliminar);

    // Añadir la fila a la tabla
    document.getElementById('medicamentosTableBody').appendChild(tr);

    // Campo oculto para enviar el ID del medicamento
    var hiddenInputMedicamentoId = document.createElement('input');
    hiddenInputMedicamentoId.type = "hidden";
    hiddenInputMedicamentoId.className = "medicamento-id"; // Clase para facilitar la captura
    hiddenInputMedicamentoId.value = medicamentoId;
    tr.appendChild(hiddenInputMedicamentoId);

    // Limpiar el select después de añadir
    selectMedicamento.value = "";
});


//Tabla a;adir imagenes
document.getElementById('anadirFilaImagen').addEventListener('click', function () {
    // Obtener la imagen seleccionada y su texto
    var selectImagen = document.getElementById('ImagenId');
    var imagenId = selectImagen.value;
    var imagenTexto = selectImagen.options[selectImagen.selectedIndex].text;

    if (imagenId === "") {
        alert("Seleccione una imagen antes de añadir.");
        return;
    }

    // Crear elementos HTML para la nueva fila
    var tr = document.createElement('tr');

    // Columna de Imagen
    var tdImagen = document.createElement('td');
    tdImagen.textContent = imagenTexto;
    tr.appendChild(tdImagen);

    // Columna de Cantidad
    var tdCantidad = document.createElement('td');
    var inputCantidad = document.createElement('input');
    inputCantidad.type = "number";
    inputCantidad.className = "form-control cantidad-imagen"; // Clase para facilitar la búsqueda
    inputCantidad.placeholder = "Cantidad";
    inputCantidad.min = "1";
    tr.appendChild(tdCantidad);
    tdCantidad.appendChild(inputCantidad);

    // Columna de Observación
    var tdObservacion = document.createElement('td');
    var inputObservacion = document.createElement('input');
    inputObservacion.type = "text";
    inputObservacion.className = "form-control observacion-imagen"; // Clase para facilitar la búsqueda
    inputObservacion.placeholder = "Observación";
    tr.appendChild(tdObservacion);
    tdObservacion.appendChild(inputObservacion);

    // Columna de Secuencial
    var tdSecuencial = document.createElement('td');
    var inputSecuencial = document.createElement('input');
    inputSecuencial.type = "hidden";
    inputSecuencial.className = "form-control secuencial-imagen"; // Clase para facilitar la búsqueda
    inputSecuencial.placeholder = "Secuencial";
    inputSecuencial.min = "1";
    tr.appendChild(tdSecuencial);
    tdSecuencial.appendChild(inputSecuencial);

    // Columna para el botón de eliminar
    var tdEliminar = document.createElement('td');
    var btnEliminar = document.createElement('button');
    btnEliminar.className = "btn btn-outline-danger";
    btnEliminar.innerHTML = '<i class="fas fa-trash"></i>';
    btnEliminar.addEventListener('click', function () {
        this.closest('tr').remove();
    });
    tr.appendChild(tdEliminar);
    tdEliminar.appendChild(btnEliminar);

    // Campo oculto para enviar el ID de la imagen
    var hiddenInputImagenId = document.createElement('input');
    hiddenInputImagenId.type = "hidden";
    hiddenInputImagenId.className = "imagen-id"; // Clase para facilitar la búsqueda
    hiddenInputImagenId.value = imagenId;
    tr.appendChild(hiddenInputImagenId);

    // Campo oculto para el estado de la imagen (predeterminado en 1)
    var hiddenInputEstadoImagen = document.createElement('input');
    hiddenInputEstadoImagen.type = "hidden";
    hiddenInputEstadoImagen.className = "estado-imagen"; // Clase para facilitar la búsqueda
    hiddenInputEstadoImagen.value = 1; // Estado predeterminado a '1'
    tr.appendChild(hiddenInputEstadoImagen);

    // Añadir la fila a la tabla
    document.getElementById('imagenesTableBody').appendChild(tr);

    // Opcional: limpiar el select después de añadir
    selectImagen.value = "";
});

//tabla a;adir laboratorio
document.getElementById('anadirFilaLaboratorio').addEventListener('click', function () {
    // Obtener el laboratorio seleccionado y su texto
    var selectLaboratorio = document.getElementById('LaboratorioId');
    var laboratorioId = selectLaboratorio.value;
    var laboratorioTexto = selectLaboratorio.options[selectLaboratorio.selectedIndex].text;

    if (laboratorioId === "") {
        alert("Seleccione un laboratorio antes de añadir.");
        return;
    }

    // Crear elementos HTML para la nueva fila
    var tr = document.createElement('tr');

    // Columna de Laboratorio
    var tdLaboratorio = document.createElement('td');
    tdLaboratorio.textContent = laboratorioTexto;
    tr.appendChild(tdLaboratorio);

    // Columna de Cantidad
    var tdCantidad = document.createElement('td');
    var inputCantidad = document.createElement('input');
    inputCantidad.type = "number";
    inputCantidad.className = "form-control cantidad";
    inputCantidad.name = "Laboratorios[" + laboratorioId + "].CantidadLaboratorio"; // Nombre para el binding del modelo
    inputCantidad.min = "1";
    inputCantidad.placeholder = "Cantidad";
    tdCantidad.appendChild(inputCantidad);
    tr.appendChild(tdCantidad);

    // Columna de Observación
    var tdObservacion = document.createElement('td');
    var inputObservacion = document.createElement('input');
    inputObservacion.type = "text";
    inputObservacion.className = "form-control observacion";
    inputObservacion.name = "Laboratorios[" + laboratorioId + "].Observacion"; // Nombre para el binding del modelo
    inputObservacion.placeholder = "Observación";
    tdObservacion.appendChild(inputObservacion);
    tr.appendChild(tdObservacion);

    // Columna para el botón de eliminar
    var tdEliminar = document.createElement('td');
    var btnEliminar = document.createElement('button');
    btnEliminar.className = "btn btn-outline-danger";
    btnEliminar.innerHTML = '<i class="fas fa-trash"></i>';
    btnEliminar.addEventListener('click', function () {
        this.closest('tr').remove();
    });
    tdEliminar.appendChild(btnEliminar);
    tr.appendChild(tdEliminar);

    // Campo oculto para enviar el ID del laboratorio
    var hiddenInputLaboratorioId = document.createElement('input');
    hiddenInputLaboratorioId.type = "hidden";
    hiddenInputLaboratorioId.className = "laboratorio-id"; // Clase para identificar el ID
    hiddenInputLaboratorioId.value = laboratorioId; // Valor del laboratorio seleccionado
    tr.appendChild(hiddenInputLaboratorioId);


    // Añadir la fila a la tabla
    document.getElementById('laboratorioTableBody').appendChild(tr);

    // Opcional: limpiar el select después de añadir
    selectLaboratorio.value = "";
});




document.getElementById('consultationForm').addEventListener('submit', async function (event) {
    event.preventDefault(); // Evitar el comportamiento por defecto del formulario

    // Parámetros de consulta
    const consultaDto = {
        UsuarioCreacion: document.getElementById('usuarioNombre')?.value || null,
        HistorialConsulta: document.getElementById('historiaClinica')?.value || null,
        PacienteId: parseInt(document.getElementById('idPaciente')?.value) || null,
        MotivoConsulta: document.getElementById('motivoConsulta')?.value || null,
        EnfermedadConsulta: document.getElementById('enfermedadProblema')?.value || null,
        NombrePariente: document.getElementById('acompañante')?.value || 'Sin especificar',
        SignosAlarma: document.getElementById('signosAlarma')?.value || 'Sin especificar',
        ReconocimientoFarmacologico: document.getElementById('reconofarmacologicas')?.value || 'Sin especificar',
        TipoPariente: parseInt(document.getElementById('tipoParienteSelect')?.value) || null,
        TelefonoPariente: document.getElementById('telefonoPariente')?.value || 'Sin especificar',
        Temperatura: document.getElementById('temperatura_consulta')?.value || null,
        FrecuenciaRespiratoria: document.getElementById('frecuenciarespiratoria_consulta')?.value || null,
        PresionArterialSistolica: document.getElementById('PresionarterialsistolicaConsulta')?.value || null,
        PresionArterialDiastolica: document.getElementById('PresionarterialdiastolicaConsulta')?.value || null,
        Pulso: document.getElementById('pulso_consulta')?.value || null,
        Peso: document.getElementById('peso_consulta')?.value || null,
        Talla: document.getElementById('talla_consulta')?.value || null,
        PlanTratamiento: document.getElementById('plantratamiento_consulta')?.value || null,
        DiasIncapacidad: parseInt(document.getElementById('diasincapacidad_consulta')?.value) || 0,
        Observacion: document.getElementById('observacion_consulta')?.value || null,
        AntecedentesPersonales: document.getElementById('antecedentespersonales_consulta')?.value || 'Sin especificar',
        MedicoId: parseInt(document.getElementById('medicoId')?.value) || null,
        EspecialidadId: parseInt(document.getElementById('especialidadId')?.value) || null,
        TipoConsultaId: parseInt(document.getElementById('tipoConsultaC')?.value) || null,
        NotasEvolucion: document.getElementById('notasevolucion_consulta')?.value || 'Sin especificar',
        ConsultaPrincipal: document.getElementById('consultaprincipal_consulta')?.value || null,
        EstadoConsulta: parseInt(document.getElementById('estadoConsultaC')?.value) || null,

        // Parámetros de órganos y sistemas
        OrganosSistemas: {
            OrgSentidos: document.getElementById('consulta-antecedente-checked-orgsentidos')?.checked || null,
            ObserOrgSentidos: document.getElementById('consulta-antecedente-observacion-orgsentidos')?.value || null,
            Respiratorio: document.getElementById('consulta-antecedente-checked-respiratorio')?.checked || null,
            ObserRespiratorio: document.getElementById('consulta-antecedente-observacion-respiratorio')?.value || null,
            CardioVascular: document.getElementById('consulta-antecedente-checked-cardiovascular')?.checked || null,
            ObserCardioVascular: document.getElementById('consulta-antecedente-observacion-cardiovascular')?.value || null,
            Digestivo: document.getElementById('consulta-antecedente-checked-digestivo')?.checked || null,
            ObserDigestivo: document.getElementById('consulta-antecedente-observacion-digestivo')?.value || null,
            Genital: document.getElementById('consulta-antecedente-checked-genital')?.checked || null,
            ObserGenital: document.getElementById('consulta-antecedente-observacion-genital')?.value || null,
            Urinario: document.getElementById('consulta-antecedente-checked-urinario')?.checked || null,
            ObserUrinario: document.getElementById('consulta-antecedente-observacion-urinario')?.value || null,
            MEsqueletico: document.getElementById('consulta-antecedente-checked-mesqueletico')?.checked || null,
            ObserMEsqueletico: document.getElementById('consulta-antecedente-observacion-mesqueletico')?.value || null,
            Endocrino: document.getElementById('consulta-antecedente-checked-endocrino')?.checked || null,
            ObserEndocrino: document.getElementById('consulta-antecedente-observacion-endocrino')?.value || null,
            Linfatico: document.getElementById('consulta-antecedente-checked-linfatico')?.checked || null,
            ObserLinfatico: document.getElementById('consulta-antecedente-observacion-linfatico')?.value || null,
            Nervioso: document.getElementById('consulta-antecedente-checked-nervioso')?.checked || null,
            ObserNervioso: document.getElementById('consulta-antecedente-observacion-nervioso')?.value || null
        },

        // Parámetros de examen físico
        ExamenFisico: {
            Cabeza: document.getElementById('consulta-antecedente-checked-cabeza')?.checked || null,
            ObserCabeza: document.getElementById('consulta-antecedente-observacion-cabeza')?.value || null,
            Cuello: document.getElementById('consulta-antecedente-checked-cuello')?.checked || null,
            ObserCuello: document.getElementById('consulta-antecedente-observacion-cuello')?.value || null,
            Torax: document.getElementById('consulta-antecedente-checked-torax')?.checked || null,
            ObserTorax: document.getElementById('consulta-antecedente-observacion-torax')?.value || null,
            Abdomen: document.getElementById('consulta-antecedente-checked-abdomen')?.checked || null,
            ObserAbdomen: document.getElementById('consulta-antecedente-observacion-abdomen')?.value || null,
            Pelvis: document.getElementById('consulta-antecedente-checked-pelvis')?.checked || null,
            ObserPelvis: document.getElementById('consulta-antecedente-observacion-pelvis')?.value || null,
            Extremidades: document.getElementById('consulta-antecedente-checked-extremidades')?.checked || null,
            ObserExtremidades: document.getElementById('consulta-antecedente-observacion-extremidades')?.value || null
        },

        // Parámetros de antecedentes familiares
        AntecedentesFamiliares: {
            Cardiopatia: document.getElementById('consulta-antecedente-checked-cardiopatia')?.checked || null,
            ObserCardiopatia: document.getElementById('consulta-observacion-cardiopatia')?.value || null,
            ParentescoCardiopatia: document.getElementById('tipoDocumentoSelectCardiopatia')?.value ? parseInt(document.getElementById('tipoDocumentoSelectCardiopatia')?.value) : null,

            Diabetes: document.getElementById('consulta-antecedente-checked-diabetes')?.checked || null,
            ObserDiabetes: document.getElementById('consulta-observacion-diabetes')?.value || null,
            ParentescoDiabetes: document.getElementById('tipoDocumentoSelectDiabetes')?.value ? parseInt(document.getElementById('tipoDocumentoSelectDiabetes')?.value) : null,

            EnfCardiovascular: document.getElementById('consulta-antecedente-checked-enfcardiovascular')?.checked || null,
            ObserEnfCardiovascular: document.getElementById('consulta-observacion-enfcardiovascular')?.value || null,
            ParentescoEnfCardiovascular: document.getElementById('tipoDocumentoSelectEnfCardiovascular')?.value ? parseInt(document.getElementById('tipoDocumentoSelectEnfCardiovascular')?.value) : null,

            Hipertension: document.getElementById('consulta-antecedente-checked-hipertension')?.checked || null,
            ObserHipertension: document.getElementById('consulta-observacion-hipertension')?.value || null,
            ParentescoHipertension: document.getElementById('tipoDocumentoSelectHipertension')?.value ? parseInt(document.getElementById('tipoDocumentoSelectHipertension')?.value) : null,

            Cancer: document.getElementById('consulta-antecedente-checked-cancer')?.checked || null,
            ObserCancer: document.getElementById('consulta-observacion-cancer')?.value || null,
            ParentescoCancer: document.getElementById('tipoDocumentoSelectCancer')?.value ? parseInt(document.getElementById('tipoDocumentoSelectCancer')?.value) : null,

            Tuberculosis: document.getElementById('consulta-antecedente-checked-tuberculosis')?.checked || null,
            ObserTuberculosis: document.getElementById('consulta-observacion-tuberculosis')?.value || null,
            ParentescoTuberculosis: document.getElementById('tipoDocumentoSelectTuberculosis')?.value ? parseInt(document.getElementById('tipoDocumentoSelectTuberculosis')?.value) : null,

            EnfMental: document.getElementById('consulta-antecedente-checked-enfmental')?.checked || null,
            ObserEnfMental: document.getElementById('consulta-observacion-enfmental')?.value || null,
            ParentescoEnfMental: document.getElementById('tipoDocumentoSelectEnfMental')?.value ? parseInt(document.getElementById('tipoDocumentoSelectEnfMental')?.value) : null,

            EnfInfecciosa: document.getElementById('consulta-antecedente-checked-enfinfecciosa')?.checked || null,
            ObserEnfInfecciosa: document.getElementById('consulta-observacion-enfinfecciosa')?.value || null,
            ParentescoEnfInfecciosa: document.getElementById('tipoDocumentoSelectEnfInfecciosa')?.value ? parseInt(document.getElementById('tipoDocumentoSelectEnfInfecciosa')?.value) : null,

            MalFormacion: document.getElementById('consulta-antecedente-checked-malformacion')?.checked || null,
            ObserMalFormacion: document.getElementById('consulta-observacion-malformacion')?.value || null,
            ParentescoMalFormacion: document.getElementById('tipoDocumentoSelectMalFormacion')?.value ? parseInt(document.getElementById('tipoDocumentoSelectMalFormacion')?.value) : null,

            Otro: document.getElementById('consulta-antecedente-checked-otro')?.checked || null,
            ObserOtro: document.getElementById('consulta-observacion-otro')?.value || null,
            ParentescoOtro: document.getElementById('tipoDocumentoSelectOtro')?.value ? parseInt(document.getElementById('tipoDocumentoSelectOtro')?.value) : null
        },

        // Tablas relacionadas (Arrays de objetos)
        Alergias: Array.from(document.querySelectorAll('#hiddenAlergiaInputsContainer input')).map(input => ({
            CatalogoalergiaId: parseInt(input.value, 10),
            ObservacionAlergias: "",
            EstadoAlergias: 1
        })),
        Cirugias: Array.from(document.querySelectorAll('#hiddenCirugiaInputsContainer input')).map(input => ({
            CatalogocirugiaId: parseInt(input.value, 10),
            ObservacionCirugia: "",
            EstadoCirugias: 1
        })),
        Medicamentos: Array.from(document.querySelectorAll('#medicamentosTableBody tr')).map(tr => ({
            MedicamentoId: parseInt(tr.querySelector('.medicamento-id')?.value || 0),
            DosisMedicamento: tr.querySelector('.dosis-medicamento')?.value || null,
            ObservacionMedicamento: tr.querySelector('.observacion-medicamento')?.value || null,
            SecuencialMedicamento: null,
            EstadoMedicamento: 1
        })),

        Laboratorios: Array.from(document.querySelectorAll('#laboratorioTableBody tr')).map(tr => ({
            CatalogoLaboratorioId: parseInt(tr.querySelector('.laboratorio-id')?.value || 0),
            CantidadLaboratorio: parseInt(tr.querySelector('.cantidad')?.value || 0),
            Observacion: tr.querySelector('.observacion')?.value || null,
            SecuencialLaboratorio: null,
            EstadoLaboratorio: 1
        })),

        Imagenes: Array.from(document.querySelectorAll('#imagenesTableBody tr')).map(tr => ({
            ImagenId: parseInt(tr.querySelector('.imagen-id')?.value || 0),
            ObservacionImagen: tr.querySelector('.observacion-imagen')?.value || null,
            CantidadImagen: parseInt(tr.querySelector('.cantidad-imagen')?.value || 0),
            SecuencialImagen: null,
            EstadoImagen: 1
        })),

        Diagnosticos: Array.from(document.querySelectorAll('#diagnosticoTableBody tr')).map(tr => ({
            DiagnosticoId: parseInt(tr.querySelector('.diagnostico-id')?.value || 0),
            ObservacionDiagnostico: tr.querySelector('.observacion-diagnostico')?.value || null,
            PresuntivoDiagnosticos: tr.querySelector('.presuntivo-diagnostico')?.checked || null,
            DefinitivoDiagnosticos: tr.querySelector('.definitivo-diagnostico')?.checked || null,
            SecuencialDiagnostico: null,
            EstadoDiagnostico: 1
        }))
    };

    // Muestra el JSON generado en la consola para debug
    console.log("JSON generado para consulta:", JSON.stringify(consultaDto));
    console.log("Parámetros de antecedentes familiares:", consultaDto.AntecedentesFamiliares);

    try {
        const response = await fetch(consultaUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(consultaDto)
        });

        const result = await response.json();

        if (response.ok) {
            console.log('Consulta creada exitosamente:', result);
            window.location.href = redirectur; // Cambia esta URL si es diferente

        } else {
            console.error('Error al crear la consulta:', result);
        }
    } catch (error) {
        console.error('Error de la solicitud:', error);
    }
});

function generatePdf() {
    const consultaId = $('#consultaId').val(); // Asegúrate de tener un campo hidden con el ID de la consulta
    const selectedOption = $('#pdfOptions').val();

    if (!selectedOption) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Por favor, seleccione un tipo de documento.'
        });
        return;
    }

    if (!consultaId) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'ID de consulta no encontrado.'
        });
        return;
    }

    // Crear la URL dinámica de forma manual
    const url = `/Consultation/GeneratePdf?id=${consultaId}&tipoDocumento=${selectedOption}`;

    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Error al generar el PDF');
        }
        return response.blob();
    })
    .then(blob => {
        const url = window.URL.createObjectURL(blob);
        window.open(url, '_blank');

        // Mostrar mensaje con SweetAlert después de generar el PDF
        Swal.fire({
            icon: 'success',
            title: 'Documento generado',
            text: 'El documento PDF ha sido generado exitosamente.'
        });

        // Cerrar el modal después de generar el PDF
        $('#pdfModal').modal('hide');
    })
    .catch(error => {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: error.message
        });
    });
}

