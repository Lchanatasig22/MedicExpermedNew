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

    window.confirmarEliminarCita = function (citaId) {
        Swal.fire({
            title: '¿Estás seguro de que deseas eliminar esta cita?',
            text: "Esta acción no se puede deshacer.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, eliminar',
            cancelButtonText: 'Cancelar'
        }).then((result) => {
            if (result.isConfirmed) {
                document.getElementById('eliminarCitaForm-' + citaId).submit();
            }
        });
    };

    // Inicializar la búsqueda para Citas
    setupSearch('opcionCitas', 'buscarFieldsCitas', 'buscarCriterioCitas', 'noResultsCitas', 'citas');
});
