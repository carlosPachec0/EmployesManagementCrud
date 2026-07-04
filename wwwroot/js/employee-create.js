document.getElementById('create-employee-form').addEventListener('submit', async (event) => {
    event.preventDefault();

    const errorsEl = document.getElementById('form-errors');
    errorsEl.replaceChildren();

    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    const payload = {
        firstName: document.getElementById('firstName').value,
        lastName: document.getElementById('lastName').value,
        type: document.getElementById('type').value
    };

    try {
        const response = await fetch('/api/v1/employees', {
            method: 'POST',
            credentials: 'same-origin',
            headers: {
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': token
            },
            body: JSON.stringify(payload)
        });

        if (response.status === 401) {
            window.location.href = '/Account/Login';
            return;
        }

        if (response.status === 400) {
            const problem = await response.json();
            renderErrors(problem.errors);
            return;
        }

        if (response.ok) {
            window.location.href = '/';
            return;
        }

        errorsEl.textContent = 'An unexpected error occurred. Please try again.';
    } catch (err) {
        errorsEl.textContent = 'An unexpected error occurred. Please try again.';
    }
});

function renderErrors(errors) {
    const errorsEl = document.getElementById('form-errors');
    errorsEl.replaceChildren();

    const list = document.createElement('ul');
    for (const field in errors) {
        for (const message of errors[field]) {
            const item = document.createElement('li');
            item.textContent = message;
            list.appendChild(item);
        }
    }
    errorsEl.appendChild(list);
}
