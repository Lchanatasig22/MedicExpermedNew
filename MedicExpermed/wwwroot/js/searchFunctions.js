//FUNCION BUSQUEDA USUARIOS LISTA
document.addEventListener('DOMContentLoaded', function () {
    const opcionSelect = document.getElementById('opcion');
    const buscarFields = document.getElementById('buscarFields');
    const buscarBtn = document.getElementById('buscarBtn');
    const buscarCriterio = document.getElementById('buscarCriterio');
    const noResults = document.getElementById('noResults');

    opcionSelect.addEventListener('change', onSearchChanged);
    buscarBtn.addEventListener('click', debounce(onSearch, 300));

    function onSearchChanged() {
        if (opcionSelect.value === '-2') {
            buscarFields.classList.add('d-none');
            showAllRowsAndCards();
            noResults.classList.add('d-none');
        } else {
            buscarFields.classList.remove('d-none');
        }
    }

    function onSearch() {
        const criterio = buscarCriterio.value.toLowerCase();
        const optionValue = opcionSelect.value;
        const rows = document.querySelectorAll('tbody tr');
        const cards = document.querySelectorAll('.card-body');
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

        toggleNoResults(found);
    }

    function getCellValue(row, optionValue) {
        return row.querySelector(`[data-search-type="${optionValue}"]`).innerText || '';
    }

    function getCardValue(card, optionValue) {
        return card.querySelector(`[data-search-type="${optionValue}"]`).innerText || '';
    }

    function showAllRowsAndCards() {
        document.querySelectorAll('tbody tr, .card-body').forEach(el => el.style.display = '');
    }

    function toggleNoResults(found) {
        if (!found) {
            noResults.classList.remove('d-none');
        } else {
            noResults.classList.add('d-none');
        }
    }

    function debounce(func, wait) {
        let timeout;
        return function () {
            const context = this, args = arguments;
            clearTimeout(timeout);
            timeout = setTimeout(() => func.apply(context, args), wait);
        };
    }
});

