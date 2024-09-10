//FUNCION BUSQUESDA PACIENTE
document.addEventListener('DOMContentLoaded', function () {
    const opcionSelect = document.getElementById('opcion');
    const buscarFields = document.getElementById('buscarFields');
    const buscarCriterio = document.getElementById('buscarCriterio');
    const noResults = document.getElementById('noResults');

    window.onSearchChanged = function () {
        if (opcionSelect.value === '-2') {
            buscarFields.classList.add('d-none');
            showAllRowsAndCards();
            noResults.classList.add('d-none');
        } else {
            buscarFields.classList.remove('d-none');
        }
    };

    window.onSearch = function () {
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

        noResults.classList.toggle('d-none', found);
    };

    function getCellValue(row, optionValue) {
        return row.querySelector(`[data-search-type="${optionValue}"]`).innerText || '';
    }

    function getCardValue(card, optionValue) {
        return card.querySelector(`[data-search-type="${optionValue}"]`).innerText || '';
    }

    function showAllRowsAndCards() {
        document.querySelectorAll('tbody tr, .card-body').forEach(el => el.style.display = '');
    }

    
 

   
});

