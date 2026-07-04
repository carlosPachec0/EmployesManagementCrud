document.addEventListener('DOMContentLoaded', () => loadEmployees(1));

const loadingEl = document.getElementById('employees-loading');
const emptyStateEl = document.getElementById('empty-state');
const employeesSectionEl = document.getElementById('employees-section');
const tbodyEl = document.getElementById('employees-tbody');
const paginationEl = document.getElementById('pagination-container');

async function loadEmployees(page) {
    showOnly(loadingEl);

    try {
        const response = await fetch(`/api/v1/employees?page=${page}&pageSize=5`, {
            credentials: 'same-origin'
        });

        if (response.status === 401) {
            window.location.href = '/Account/Login';
            return;
        }

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();

        if (data.totalCount === 0) {
            showOnly(emptyStateEl);
            return;
        }

        renderRows(data.items);
        renderPagination(data);
        showOnly(employeesSectionEl);
    } catch (err) {
        console.error('Error loading employees:', err);
        loadingEl.textContent = 'An error occurred loading employees.';
        showOnly(loadingEl);
    }
}

function showOnly(sectionToShow) {
    [loadingEl, emptyStateEl, employeesSectionEl].forEach(el => {
        if (el) el.classList.toggle('d-none', el !== sectionToShow);
    });
}

function renderRows(items) {
    tbodyEl.replaceChildren();

    for (const employee of items) {
        const row = document.createElement('tr');

        row.appendChild(createCell(employee.firstName));
        row.appendChild(createCell(employee.lastName));
        row.appendChild(createCell(employee.type));
        row.appendChild(createCell(new Date(employee.createdAt).toLocaleDateString()));

        tbodyEl.appendChild(row);
    }
}

function createCell(text) {
    const cell = document.createElement('td');
    cell.textContent = text;
    return cell;
}

function renderPagination(data) {
    paginationEl.replaceChildren();

    if (data.totalPages <= 1) return;

    for (let i = 1; i <= data.totalPages; i++) {
        const button = document.createElement('button');
        button.textContent = i;
        button.className = 'page-link';
        button.disabled = i === data.page;
        button.addEventListener('click', () => loadEmployees(i));

        const li = document.createElement('li');
        li.className = 'page-item';
        if (i === data.page) li.className += ' active';
        li.appendChild(button);

        paginationEl.appendChild(li);
    }
}
