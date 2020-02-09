function login_click() {
    var username = document.getElementById("username").value;
    var password = document.getElementById("password").value;
    var wrong = document.getElementById("wrong");
    var lable = document.getElementById("wrong_lab");

    if (username == "") {
        wrong.style.display = 'block';
        lable.innerHTML = "用户名不能为空";
        return false;
    }
    if (password == "") {
        wrong.style.display = 'block';
        lable.innerHTML = "密码不能为空";
        return false;
    }
    if (password != "" && username != "") {
        return true;
    }
}
