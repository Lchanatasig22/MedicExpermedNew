const hamBurger = document.querySelector(".toggle-btn");

hamBurger.addEventListener("click", function () {
    document.querySelector("#sidebar").classList.toggle("expand");
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