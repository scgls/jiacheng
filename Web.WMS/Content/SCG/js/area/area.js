﻿$(".dayin").click(function () {
    var IDs = "";
    if (confirm("确定打印这" + $('input[type=checkbox]:checked').length + "条数据?")) {
        $.each($('input:checkbox:checked'), function () {
            IDs = (IDs + $(this).val() + ",");
        });

        var newA = document.createElement("a");
        newA.id = 'gg'
        newA.target = '_blank';
        var aaa = "/Report/Print/PrintWarehouse.aspx?area=" + IDs;
        newA.href = aaa;
        document.body.appendChild(newA);
        newA.click();
        document.body.removeChild(newA);
    }
})




