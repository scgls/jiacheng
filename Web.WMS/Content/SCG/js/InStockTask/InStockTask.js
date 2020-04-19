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
                html += "<td>" + data[i].AreaNo + "</td>"
                html += "<td>" + data[i].MaterialNo + "</td>"
                html += "<td>" + data[i].MaterialDesc + "</td>"
                html += "<td>" + data[i].QualityQty + "</td>"
                html += "<td>" + data[i].TaskQty + "</td>"
                html += "<td>" + data[i].RemainQty + "</td>"
                html += "<td>" + data[i].ShelveQty + "</td>"
                html += "<td>" + data[i].StrLineStatus + "</td>"
                html += "<td>" + data[i].OperatorUserName + "</td>"
                //html += "<td>" + data[i].BatchNo + "</td>"
                html += "</tr>";
                $("#detailTable").append(html);
            }
        }
    });
}

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
            type: "get",
            url: "CloseTask",
            date: "id = " + ID,
            method: "POST",
            dataType: "json",
            async: false,
            success: function (data) {
                alert(data.obj);
                if (data.Status) {
                    window.location.reload();
                } 
            }
        });
    }
})
