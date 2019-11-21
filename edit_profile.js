var url = new URL(window.location.href);
var usrid = url.searchParams.get("user_id");

if (usrid == null) {
	
	if (window.localStorage.getItem('id'))
		usrid = window.localStorage.getItem('id');
	else
		window.location.href = "startups.html?error=" + encodeURI("Такого користувача не існує!");
}
else if (usrid != window.localStorage.getItem('id'))
	window.location.href = "startups.html?error=" + encodeURI("Ви не маєте права редагувати цей профіль!");

fetch(env.apiUrl + '/api/Accounts?userId=' + usrid)
	.then(handleResponse)
	.then(function(data) {
		
		showProfile(data);
		
	});
	
function showProfile(data) {

	document.getElementById('username').href = usrid;
	document.getElementById('username').innerHTML = data.login;	
		
	if (data.firstName)
		document.getElementById('firstName').value = data.firstName;
		
	if (data.secondName)
		document.getElementById('secondName').value = data.secondName;
		
	if (data.gender == 'Male')
		document.getElementById('gender').value = 1;
	else if (data.gender == 'Female')
		document.getElementById('gender').value = 2;
	
	if (data.description)
		document.getElementById('description').value = data.description;
		
	if (data.birthday)
		document.getElementById('birthday').value = data.birthday.substr(0, 10);
	
	if (data.phone)
		document.getElementById('phone').value = data.phone;
		
	if (data.email)
		document.getElementById('email').value = data.email;

}

function handleResponse(response) {
	return response.text().then(text => {
		const data = text && JSON.parse(text);

		if (!response.ok) {
			const error = (data && data.message) || response.statusText;

			//window.location.href = "startups.html?error="+encodeURI("Сталася помилка!");
			
			return Promise.reject(error);
		}

		return data;
	});
}

function readURL(input) {
	if (input.files && input.files[0]) {

		var reader = new FileReader();

		reader.onload = function (e) {
			$('.file-upload-image').attr('src', e.target.result);
			$('.image-upload-wrap').show();
			$('.image-title').html(input.files[0].name);
		};

		reader.readAsDataURL(input.files[0]);

	}  else {
		$('.image-upload-wrap').hide();
	}
}

function removeUpload(){
	$('.file-upload-input').replaceWith($('.file-upload-input').clone());
	$('.image-upload-wrap').hide();
}

function saveProfile(event) {
	event.preventDefault();

	if (document.getElementById('firstName').value.length > 32) {
	
		message('Перевищено обсяг тексту для імені! Максимальний обсяг - 32 символів.', 0);
		return;
	
	}
	
	if (document.getElementById('secondName').value.length > 32) {
	
		message('Перевищено обсяг тексту для прізвища! Максимальний обсяг - 32 символів.', 0);
		return;
	
	}
	
	if (document.getElementById('description').value.length > 500) {
	
		message('Перевищено обсяг тексту для біографії! Максимальний обсяг - 500 символів.', 0);
		return;
	
	}
	
	if (document.getElementById('phone').value.length > 13) {
	
		message('Перевищено обсяг тексту для номеру телефону! Максимальний обсяг - 13 символів.', 0);
		return;
	
	}
	
	if (document.getElementById('email').value.length > 32) {
	
		message('Перевищено обсяг тексту для електронної пошти! Максимальний обсяг - 32 символів.', 0);
		return;
	
	}
	
	if (document.getElementById('gender').value < 0 && document.getElementById('gender').value > 2) {
	
		message('Такий гендер є недопустимим! Оберіть один з запропонованих.', 0);
		return;
	
	}

	const requestOptions = {
		method: 'PUT',
		headers: { 'Content-Type': 'application/json' },
		body: JSON.stringify( { login: document.getElementById('username').innerHTML, 
								firstName: document.getElementById('firstName').value, 
								secondName: document.getElementById('secondName').value, 
								gender: document.getElementById('gender').value,
								photo: "null",
								description: document.getElementById('description').value,
								phone: document.getElementById('phone').value,
								showPhone: "true",
								birthday: document.getElementById('birthday').value,
								showBirthday: "true",
								email: document.getElementById('email').value,
								showEmail: "true"
							})
	};

	fetch(env.apiUrl + '/api/Accounts', requestOptions)
		.then(handleResponse)
		.then(data => alert(JSON.stringify(data)));
}

function message(text, type) {

	if (!type)
		document.getElementById('alert').innerHTML='<div class="alert alert-danger text-center" role="alert" id="alert-text"></div>';
	else
		document.getElementById('alert').innerHTML='<div class="alert alert-success text-center" role="alert" id="alert-text"></div>';
	
	document.getElementById('alert-text').innerHTML=text;

}

if (window.localStorage.getItem('token') && window.localStorage.getItem('id')) {
	fetch('http://eliasb13-001-site1.itempurl.com/api/Accounts/profile?userId=' + window.localStorage.getItem('id'))
		.then(handleMenuResponse)
		.then(function(data) {
			document.getElementById('account_login').innerHTML = data.login;
		});
}
		
function handleMenuResponse(response) {
	return response.text().then(text => {
		const data = text && JSON.parse(text);

		if (!response.ok) {
			const error = (data && data.message) || response.statusText;

			return Promise.reject(error);
		}

		return data;
	});
}