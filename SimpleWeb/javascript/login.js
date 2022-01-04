function addInput() {
var loginButton = document.getElementById("loginButton");
loginButton.onclick = inputHandler;

}

function inputHandler() {
	var userName = document.getElementById("userNameBox").value;
	var password = document.getElementById("passwordBox").value;

	var data = "userName=" + userName + "&password=" + password;
	//alert(data);


	var newRequest = new XMLHttpRequest();
	newRequest.open("GET","gameGrid.json",false);
	//newRequest.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
	newRequest.send(null);


		var response = 	JSON.parse(newRequest.responseText).gameGrid;
		console.log(response);





}
