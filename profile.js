var url = new URL(window.location.href);
var usrid = url.searchParams.get("user_id");

if (usrid == null) {
	
	if (window.localStorage.getItem('id'))
		usrid = window.localStorage.getItem('id');
	else
		window.location.href = "startups.html?error=" + encodeURI("Такого користувача не існує!");//encodeURI(text)
}

fetch(env.apiUrl + '/api/Accounts/profile?userId=' + usrid)
	.then(handleResponse)
	.then(function(data) {

		showProfile(data);
		
		if (data.userId == window.localStorage.getItem('id'))
			showOwnProfile(data);
		
		if (window.localStorage.getItem('isAdmin'))
			showForAdmin(data);
		
	});
	
function showProfile(data) {

	document.getElementById('username').href = usrid;
	document.getElementById('username').innerHTML = data.login;
	document.getElementById('registerDate').innerHTML = data.registerDate.substr(0, 10);
		
	if (data.ban)
		document.getElementById('membership_status').innerHTML = '<small class = "text-danger"> Забанений користувач </small>';
	else if (data.membership == 1)
		document.getElementById('membership_status').innerHTML = '<small class = "text-success"> Член клубу </small>';
	else
		document.getElementById('membership_status').innerHTML = '<small class = "text-muted"> Звичайний користувач </small>';
	
	
	if (data.secondName)
		document.getElementById('secondName').innerHTML = data.secondName;
		
	if (data.firstName)
		document.getElementById('firstName').innerHTML = data.firstName;
		
	if (data.gender == 'Male')
		document.getElementById('gender').innerHTML = 'Чоловічий';
	else if (data.gender == 'Female')
		document.getElementById('gender').innerHTML = 'Жіночий';
	
	if (data.description)
		document.getElementById('description').innerHTML = data.description;
		
	if (data.photo)
		document.getElementById('photo').src = data.photo;
		
	if (data.birthday)
		document.getElementById('birthday').innerHTML = data.birthday;
	
	if (data.phone)
		document.getElementById('phone').innerHTML = data.phone;
		
	if (data.email)
		document.getElementById('email').innerHTML = data.email;

}

function showOwnProfile(data) {

	if (data.membership == 0)
		document.getElementById('membership').innerHTML = '<a href="request_membership">Стати членом <i class="fas fa-arrow-alt-circle-up"></i></a> ';
	else if (data.membership == 2) 
		document.getElementById('membership').innerHTML = '<span class="text-muted">Запит на членство надіслано</span> ';
		
	document.getElementById('edit').innerHTML= '<a href="edit_profile.html"><i class="fas fa-pencil-alt"></i></a>';
	
	showProfile(data);

}

function showForAdmin(data) {

	document.getElementById('membership').innerHTML = '<a class="text-success" href="add_member.html">Надати членство <i class="fas fa-arrow-alt-circle-up"></i></a>';

	document.getElementById('ban').innerHTML = '<a class="text-danger" href="ban">Забанити <i class="fas fa-ban"></i></a>';
	
	showProfile(data);

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