
window.onload = function () {
    //Money Euro
    $("[data-mask]").inputmask();
    //iCheck for checkbox and radio inputs
    $('input[type="checkbox"].minimal, input[type="radio"].minimal').iCheck({
        checkboxClass: 'icheckbox_minimal',
        radioClass: 'iradio_minimal'
    });
}


$(".shanchu").click(function () {
    var IDs = "";
    if (confirm("确定删除数据这" + $('input[type=checkbox]:checked').length + "条数据?")) {
        $.each($('input:checkbox:checked'), function () {
            IDs = (IDs + $(this).val() + ",");
        });
        $.ajax({
            type: "get",
            url: "Delect?ID=" + IDs,
            method: "POST",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data == "") {
                    if (data == "") {
                        window.location.reload()
                    } else {
                        alert(data);
                    }
                }
            }
        });
    }
})

$(".xinzeng").click(function () {
    location.href = 'getmodel'; 
})

$(".shuaxin").click(function () {
    window.location.href = window.location.href;
})









