
function getDetail(ID) {
    $("#detailTable").html("");
    $.ajax({
        type: "get",
        url: "GetDetail?ID=" + ID,
        dataType: "json",
        async: false,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                var html = "<tr>";
                html += "<td>" + data[i].RowNo + "</td>"
                html += "<td>" + data[i].RowNoDel + "</td>"
                html += "<td>" + data[i].StrongHoldCode + "</td>"
                html += "<td>" + data[i].MaterialNo + "</td>"
                html += "<td>" + data[i].MaterialDesc + "</td>"
                html += "<td>" + data[i].FromBatchNo + "</td>"
                html += "<td>" + data[i].OutStockQty + "</td>"
                //html += "<td>" + data[i].RemainQty + "</td>"
                html += "<td>" + data[i].FromErpWarehouse + "</td>"
                //html += "<td>" + data[i].FromErpAreaNo + "</td>"
                html += "</tr>";
                $("#detailTable").append(html);
            }
        }
    });
}


$(".shengdan").click(function () {
    var IDs = "";
    if ($('input[type=checkbox]:checked').length == 0) {
        return;
    }
    if (confirm("确定这" + $('input[type=checkbox]:checked').length + "条出库单生成拣货单?")) {
        $.each($('input:checkbox:checked'), function () {
            if ($(this).val() != "on") {
                IDs = (IDs + $(this).val() + ",");
            }
            
        });
        $.ajax({
            type: "POST",
            url: "Shengdan?ID=" + IDs,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.state) {
                    alert("生单成功！");
                    window.location.reload()
                } else {
                    alert(data.obj);
                }
            }
        });
    }
})


$(".shengxiang").click(function () {
    var IDs = "";
    var HeaderName = "奥碧虹";
    HeaderName = $('#HeaderName').val();
    if ($('input[type=checkbox]:checked').length == 0) {
        return;
    }
    if (confirm("确定这" + $('input[type=checkbox]:checked').length + "条出库单生成拣货单?")) {
        $.each($('input:checkbox:checked'), function () {
            if ($(this).val() != "on") {
                IDs = (IDs + $(this).val() + ",");
            }

        });
        $.ajax({
            type: "POST",
            url: "Shengxiang?ID=" + IDs + "&HeaderName=" + HeaderName,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.state) {
                    alert("生箱成功！");
                    window.location.reload()
                } else {
                    alert(data.obj);
                }
            }
        });
    }
})


$(".dayin").click(function () {
    var IDs = "";
    if ($('input[type=checkbox]:checked').length == 0) {
        return;
    }
    if ($('input[type=checkbox]:checked').length > 1) {
        alert("请选中一行打印");
        return;
    }
    if (confirm("确定这出库单生成物流标签?")) {
        $.each($('input:checkbox:checked'), function () {
            IDs = (IDs + $(this).val() + ",");
        });
        $.ajax({
            type: "POST",
            url: "Print?ID=" + IDs,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.state) {
                    window.location.reload()
                } else {
                    alert(data.obj);
                }
            }
        });
    }
})

$(".guanbi").click(function () {
    if ($('input[type=checkbox]:checked').length != 1) {
        alert("必须选中一行任务");
        return;
    }
    var ID = "";
    if (confirm("确定关闭这" + $('input[type=checkbox]:checked').length + "个任务单据?")) {
        $.each($('input:checkbox:checked'), function () {
            ID = $(this).val();
        });
        $.ajax({
            type: "POST",
            url: "CloseOutstock?ID=" + ID,
            date: null,
            dataType: "json",
            async: false,
            success: function (data) {
                alert(data.obj);
                if (data.Status) {
                    //window.location.reload();
                }
            },
            fail: function () {
                alert("提交失败！")
            }
        });
    }
})

