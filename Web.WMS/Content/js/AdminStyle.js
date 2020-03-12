String.prototype.startWith = function (compareStr) {
    return this.indexOf(compareStr) == 0;
}

// 失去焦点添加千位分隔符
function formatDigit(num) {
    if (num == "") {
        return "";
    }
    // 去掉所有逗号
    num = num.replace(/,/g, "");
    // 取得负号
    var h = "";
    if (num.charAt(0) == '-') {
        h = "-";
        num = num.slice(1);
    }
    // 去掉以零开头的数字
    while (!num.startWith("0.") && num.startWith("0") && num.length > 1) {
        num = num.substring(1);
    }
    if (!/^(\+|-)?(\d+)(\.\d+)?$/.test(num)) {
        return "invalid value";
    }
    var a = RegExp.$1, b = RegExp.$2, c = RegExp.$3;
    var re = new RegExp().compile("(\\d)(\\d{3})(,|$)");
    while (re.test(b)) {
        b = b.replace(re, "$1,$2$3");
    }
    return h + a + b + c;
}

// 失去焦点添加千位分隔符(保留2位小数)
function formatDigit2(num) {
    if (num == "") {
        return "";
    }
    // 去掉所有逗号
    num = num.replace(/,/g, "");
    // 取得负号
    var h = "";
    if (num.charAt(0) == '-') {
        h = "-";
        num = num.slice(1);
    }
    // 去掉以零开头的数字
    while (!num.startWith("0.") && num.startWith("0") && num.length > 1) {
        num = num.substring(1);
    }
    if (/[^0-9\.]/.test(num)) return "invalid value";
    num = num.replace(/^(\d*)$/, "$1.");
    num = (num + "00").replace(/(\d*\.\d\d)\d*/, "$1");
    num = num.replace(".", ",");
    var re = /(\d)(\d{3},)/;
    while (re.test(num)) num = num.replace(re, "$1,$2");
    num = num.replace(/,(\d\d)$/, ".$1");
    return h + num.replace(/^\./, "0.")
}

Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month
        "d+": this.getDate(), //day
        "h+": this.getHours(), //hour
        "m+": this.getMinutes(), //minute
        "s+": this.getSeconds(), //second
        "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
        "S": this.getMilliseconds() //millisecond
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
    (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1,
        RegExp.$1.length == 1 ? o[k] :
        ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}