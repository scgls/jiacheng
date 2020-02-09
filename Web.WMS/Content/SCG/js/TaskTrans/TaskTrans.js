$(".dayin").click(function () {
    var IDs = "";
    if (confirm("确定这" + $('input[type=checkbox]:checked').length + "条记录?")) {
        $.each($('input:checkbox:checked'), function () {
            IDs = (IDs + $(this).val() + ",");
        });
        $.ajax({
            type: "get",
            url: "?ID=" + IDs,
            method: "POST",
            dataType: "json",
            async: false,
            success: function (data) {
                if (data == "") {
                    window.location.reload();
                } else {
                    alert(data);
                }
            }
        });
    }
})

$(".chaxun").click(function () {
    $("form").submit();
})

