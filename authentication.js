function register(event) {
	event.preventDefault();
	
	let login = document.getElementById("loginInp").value;
	let email = document.getElementById("emailInp").value;
	let password = document.getElementById("passInp").value;
	let passwordConfirmation = document.getElementById("passConfInp").value;
	
	if (email.includes("@") == false) {
		alert("Некоректний email")
	}
	if (passwordConfirmation != password) {
		alert("Введіть однакові паролі")
	}
	
	if(login && email && password && passwordConfirmation && password === passwordConfirmation) { // add some validation
		const requestOptions = {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify( { login: login, password: password, passwordConfirmation: passwordConfirmation, email: email } )
		};

		fetch(window.env.apiUrl + '/api/accounts/register', requestOptions)
			.then(handleResponse)
			.then(data => alert(JSON.stringify(data)));
	}
}

function login(event) {
	event.preventDefault();

	let loginOrEmail = document.getElementById("emailInp").value;
	let password = document.getElementById("passInp").value;

	if (loginOrEmail && password) {
		const requestOptions = {
			method: 'POST',
			headers: { 'Content-Type': 'application/json' },
			body: JSON.stringify( { loginOrEmail: loginOrEmail, password: password } )
		};

		fetch(window.env.apiUrl + '/api/accounts/login', requestOptions)
			.then(handleResponse)
			.then(data => alert(JSON.stringify(data)));
	}
}

function handleResponse(response) {
    return response.text().then(text => {
        const data = text && JSON.parse(text);

        if (!response.ok) {
            const error = (data && data.message) || response.statusText;

            return Promise.reject(error);
        }

        return data;
    });
}