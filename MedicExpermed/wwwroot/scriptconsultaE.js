$(document).ready(function () {
    var tiposDiagnosticoOptions = JSON.parse(tiposDiagnosticoJson);
    var tiposMedicamentosOptions = JSON.parse(tiposMedicamentosJson);
    var tiposLaboratoriosOptions = JSON.parse(tiposLaboratoriosJson);
    var tiposImagenOptions = JSON.parse(tiposImagenJson);

    console.log("Tipos Diagnóstico Options:", tiposDiagnosticoOptions);
    console.log("Tipos Medicamentos Options:", tiposMedicamentosOptions);
    console.log("Tipos Laboratorios Options:", tiposLaboratoriosOptions);
    console.log("Tipos Imagen Options:", tiposImagenOptions);

    var filaIndex = 1;
    var medicamentoIndex = 1;
    var laboratorioIndex = 1;
    var imagenIndex = 1;

    $('#anadirFila').on('click', function () {
        if (!Array.isArray(tiposDiagnosticoOptions) || tiposDiagnosticoOptions.length === 0) {
            console.error('No hay opciones de diagnóstico disponibles.');
            return;
        }

        var nuevaFila = `<tr>
                    <td>
                        <div class="input-group">
                            <select class="form-control" id="DiagnosticoId">
                                <option value="">Seleccione...</option>`;
        tiposDiagnosticoOptions.forEach(function (tiposDiagnostico) {
            nuevaFila += `<option value="${tiposDiagnostico.IdDiagnostico}">${tiposDiagnostico.NombreDiagnostico}</option>`;
        });
        nuevaFila += `</select>
                            <div class="input-group-append">
                                <span class="input-group-text"><i class="fas fa-search"></i></span>
                            </div>
                        </div>
                    </td>
                    <td>
                        <div class="btn-group btn-group-toggle" data-toggle="buttons">
                            <label class="btn btn-outline-secondary">
                                <input type="checkbox" name="options${filaIndex}" id="presuntivo${filaIndex}" autocomplete="off"> Presuntivo
                            </label>
                            <label class="btn btn-outline-secondary">
                                <input type="checkbox" name="options${filaIndex}" id="definitivo${filaIndex}" autocomplete="off"> Definitivo
                            </label>
                        </div>
                    </td>
                    <td>
                        <button type="button" class="btn btn-outline-secondary eliminar-fila"><i class="fas fa-times-circle"></i> Eliminar</button>
                    </td>
                </tr>`;
        $('#diagnosticoTableBody').append(nuevaFila);
        filaIndex++;
    });

    $('#anadirFilaMedicamento').on('click', function () {
        if (!Array.isArray(tiposMedicamentosOptions) || tiposMedicamentosOptions.length === 0) {
            console.error('No hay opciones de medicamentos disponibles.');
            return;
        }

        var nuevaFila = `<tr>
                    <td>
                        <div class="input-group">
                            <select class="form-control" id="MedicamentoId">
                                <option value="">Seleccione...</option>`;
        tiposMedicamentosOptions.forEach(function (tiposMedicamento) {
            nuevaFila += `<option value="${tiposMedicamento.IdMedicamento}">${tiposMedicamento.NombreMedicamento}</option>`;
        });
        nuevaFila += `</select>
                            <div class="input-group-append">
                                <span class="input-group-text"><i class="fas fa-search"></i></span>
                            </div>
                        </div>
                    </td>
                    <td>
                        <input type="number" class="form-control" id="Cantidad" placeholder="Cantidad">
                    </td>
                    <td>
                        <button type="button" class="btn btn-outline-secondary eliminar-fila"><i class="fas fa-times-circle"></i> Eliminar</button>
                    </td>
                </tr>`;
        $('#medicamentosTableBody').append(nuevaFila);
        medicamentoIndex++;
    });

    $('#anadirFilaLaboratorio').on('click', function () {
        if (!Array.isArray(tiposLaboratoriosOptions) || tiposLaboratoriosOptions.length === 0) {
            console.error('No hay opciones de laboratorio disponibles.');
            return;
        }

        var nuevaFila = `<tr>
                    <td>
                        <div class="input-group">
                            <select class="form-control" id="LaboratorioId">
                                <option value="">Seleccione...</option>`;
        tiposLaboratoriosOptions.forEach(function (tiposLaboratorio) {
            nuevaFila += `<option value="${tiposLaboratorio.IdLaboratorio}">${tiposLaboratorio.NombreLaboratorio}</option>`;
        });
        nuevaFila += `</select>
                            <div class="input-group-append">
                                <span class="input-group-text"><i class="fas fa-search"></i></span>
                            </div>
                        </div>
                    </td>
                    <td>
                        <input type="text" class="form-control" id="Resultado" placeholder="Resultado">
                    </td>
                    <td>
                        <button type="button" class="btn btn-outline-secondary eliminar-fila"><i class="fas fa-times-circle"></i> Eliminar</button>
                    </td>
                </tr>`;
        $('#laboratorioTableBody').append(nuevaFila);
        laboratorioIndex++;
    });

    $('#anadirFilaImagen').on('click', function () {
        if (!Array.isArray(tiposImagenOptions) || tiposImagenOptions.length === 0) {
            console.error('No hay opciones de imágenes disponibles.');
            return;
        }

        var nuevaFila = `<tr>
                    <td>
                        <div class="input-group">
                            <select class="form-control" id="ImagenId">
                                <option value="">Seleccione...</option>`;
        tiposImagenOptions.forEach(function (tiposImagen) {
            nuevaFila += `<option value="${tiposImagen.IdImagen}">${tiposImagen.NombreImagen}</option>`;
        });
        nuevaFila += `</select>
                            <div class="input-group-append">
                                <span class="input-group-text"><i class="fas fa-search"></i></span>
                            </div>
                        </div>
                    </td>
                    <td>
                        <input type="text" class="form-control" id="Resultado" placeholder="Resultado">
                    </td>
                    <td>
                        <button type="button" class="btn btn-outline-secondary eliminar-fila"><i class="fas fa-times-circle"></i> Eliminar</button>
                    </td>
                </tr>`;
        $('#imagenesTableBody').append(nuevaFila);
        imagenIndex++;
    });

    $(document).on('click', '.eliminar-fila', function () {
        $(this).closest('tr').remove();
    });
});


var navListItems = $('div.stepwizard-step button'),
    allWells = $('.setup-content');

allWells.hide();
// Manejo de clicks en los pasos del wizard
navListItems.click(function (e) {
    e.preventDefault();
    var $target = $('#step-' + $(this).data('step')),
        $item = $(this);

    if (!$item.hasClass('disabled')) {
        navListItems.removeClass('btn-primary').addClass('btn-secondary');
        $item.addClass('btn-primary');
        allWells.hide();
        $target.show();
        $target.find('input:eq(0)').focus();
    }
});

// Función para manejar el botón "Siguiente"
function goToNextStep() {
    var curStep = $(this).closest(".setup-content"),
        curStepBtn = curStep.attr("id"),
        nextStepWizard = $('div.stepwizard-step button[data-step="' + (parseInt(curStepBtn.split('-')[1]) + 1) + '"]'),
        curInputs = curStep.find("input[type='text'],input[type='url']"),
        isValid = true;

    $(".form-group").removeClass("has-error");
    for (var i = 0; i < curInputs.length; i++) {
        if (!curInputs[i].validity.valid) {
            isValid = false;
            $(curInputs[i]).closest(".form-group").addClass("has-error");
        }
    }

    if (isValid) {
        nextStepWizard.removeAttr('disabled').trigger('click');
    }
}

// Función para manejar el botón "Regresar"
$('div.setup-content button.previousBtn').click(function () {
    var curStep = $(this).closest(".setup-content"),
        curStepBtn = curStep.attr("id"),
        prevStepWizard = $('div.stepwizard-step button[data-step="' + (parseInt(curStepBtn.split('-')[1]) - 1) + '"]');

    navListItems.removeClass('btn-primary').addClass('btn-secondary');
    prevStepWizard.addClass('btn-primary');
    allWells.hide();
    $('#step-' + (parseInt(curStepBtn.split('-')[1]) - 1)).show();
});

// Mostrar u ocultar campos de observación al cambiar los switches
$('.consulta-antecedente-checked').change(function () {
    var isChecked = $(this).prop('checked');
    var $observacionField = $(this).closest('.fields').find('.consulta-antecedente-observacion');

    if (isChecked) {
        $observacionField.show();
        $observacionField.find('input').removeAttr('disabled');
    } else {
        $observacionField.hide();
        $observacionField.find('input').attr('disabled', 'disabled');
    }
});

// Attach the goToNextStep function to the buttons
$('div.setup-content button.nextBtn').on('click', goToNextStep);
