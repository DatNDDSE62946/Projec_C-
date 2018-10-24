 function myValidate() {
     var username = document.getElementById("username").value;
     var firstName = document.getElementById("first_name").value;
     var lastName = document.getElementById("last_name").value;
     var password = document.getElementById("password").value;
     var confirm = document.getElementById("confirm").value;
     var email = document.getElementById("email").value;
     if (username == "" || username.length <= 8) {
         document.getElementById("usernameValidate").innerHTML = "Username must have more than 8 characters!"
         
     }
     if (firstName == "") {
         document.getElementById("fNameValidate").innerHTML = "First name cannot be blank!"
     }  
     if (lastName == "") {
         document.getElementById("lastNameValidate").innerHTML = "Last name cannot be blank!"
     }
     if (password == "") {
         document.getElementById("passwordValidate").innerHTML = "Password must have more than 8 characters!"
         
     }
     if (password.match("[A-Z][A-Za-z0-9_]+") == null) {
         document.getElementById("passwordValidate").innerHTML = "Password must have this format A[A-z0-9]"
     }
     if (confirm != password || confirm == "") {
         document.getElementById("confirmValidate").innerHTML = "Confirm password is not correct!"
     }
     if (email.trim().match(/^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{1,5}|[0-9]{1,3})(\]?)$/) == null) {
         document.getElementById("emailValidate").innerHTML = "Email is not correct!"
     }
    return false;
}