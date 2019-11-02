var url = new URL(window.location.href);
var usrid = url.searchParams.get("user_id");

/*if (usrid == null) {
	
	if (window.localStorage.getItem('user_id'))
		usrid = window.localStorage.getItem('user_id');
	else
		window.location.href = "startups.html?error=" + encodeURI("Такого користувача не існує!");
}
else if (window.localStorage.getItem('user_id') && usrid != window.localStorage.getItem('user_id'))
	window.location.href = "startups.html?error=" + encodeURI("Ви не маєте права редагувати цей профіль!");*/

fetch(env.apiUrl + '/api/Accounts/profile?userId=' + usrid)
	.then(handleResponse)
	.then(function(data) {
		
		showProfile(data);
		
	});
	
function showProfile(data) {

	document.getElementById('username').href = usrid;
	document.getElementById('username').innerHTML = data.login;	
	
	if (data.secondName)
		document.getElementById('secondName').value = data.secondName;
		
	if (data.firstName)
		document.getElementById('firstName').value = data.firstName;
		
	if (data.gender == 'Male')
		document.getElementById('gender').value = 1;
	else if (data.gender == 'Female')
		document.getElementById('gender').value = 2;
	
	if (data.description)
		document.getElementById('description').value = data.description;
		
	if (data.birthday)
		document.getElementById('birthday').value = data.birthday;
	
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

			window.location.href = "startups.html?error="+encodeURI("Такого користувача не існує!");
			
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
