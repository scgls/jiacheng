$(".shuaxin").click(function () {
    location.reload();
})
//*****************************************************添加盘点单用******************************************************************
//获取盘点货位
function getAreas() {
    $("#detailTable").html("");
    var Svalue = $("#Svalue").val();
    var areano = $("#areano").val();
    var houseno = $("#houseno").val();
    var warehouseno = $("#warehouseno").val();
    if (warehouseno == "") {
        alert("仓库编号不能为空！");
        return;
    }
    loading();
    $.ajax({
        type: "get",
        url: "GetDetail?Svalue=" + Svalue + "&areano=" + areano + "&houseno=" + houseno + "&warehouseno=" + warehouseno,
        dataType: "json",
        async: false,
        success: function (data) {
            loading(false);
            if (data.length == 0) {
                var html = "<tr><td colspan='5' align='center'><b>无数据</b></td></tr>";
                alert("查询无数据！");
                $("#detailTable").append(html);
                return;
            }
            for (var i = 0; i < data.length; i++) {
                var html = "<tr>";
                html += "<td><input type='checkbox' class='minimal' /></td>"
                html += "<td style='display: none;'>" + data[i].ID + "</td>"
                html += "<td>" + (i + 1) + "</td>"
                html += "<td>" + data[i].AREANO + "</td>"
                html += "<td>" + data[i].AREANAME + "</td>"
                html += "<td>" + data[i].houseno + "</td>"
                html += "<td>" + data[i].warehouseno + "</td>"
                html += "</tr>";
                $("#detailTable").append(html);
            }
        }
    });
    return false;
}
//正选反选
function checkall() {
    var flag = true;
    if (document.getElementById('button').value == "全选") {
        flag = true;
        document.getElementById('button').value = "取消";
    } else {
        flag = false;
        document.getElementById('button').value = "全选";
    }
    $("input[type=checkbox]").each(function () { //循环checkbox选择或取消
        $(this).prop("checked", flag);
    })

}


//提交数据
function saveExcel() {
    var strAll = "";
    $('#detailTable').find('tr').each(function () {
        if ($(this).find('td:eq(0)').children().is(':checked')) {
            strAll = strAll + ($(this).find('td:eq(1)').text()) + "," + ($(this).find('td:eq(3)').text()) + ";"
        }
    })
    fun1(strAll);
}
function fun1(strAll) {
    var CHECKNO = $("#CHECKNO").val();
    var CHECKDESC = $("#CHECKDESC").val();
    var REMARKS = $("#REMARKS").val();
    $.ajax({
        url: "/Check/Add",
        async: true,
        type: "POST",
        data: { "CHECKNO": CHECKNO, "CHECKDESC": CHECKDESC, "REMARKS": REMARKS, "strAll": strAll },
        dataType: "json",
        success: function (data) {
            alert(data);
            if (data == "保存成功！") {
                window.location.href = "/Check/GetModelList"
            }
        }
    })
}
//*****************************************************添加盘点单用******************************************************************

function shanchu() {
    if (confirm("确定删除操作?")) {
        $('#tbody').find('tr').each(function () {
            if ($(this).find('td:eq(0)').children('.icheckbox_minimal').is('.checked')) {
                if ($(this).find('td:eq(4)').text() != "新建") {
                    alert("只有新建单据才能删除");
                    return;
                } else {
                    var CHECKNO = $(this).find('td:eq(1)').text();
                    $.ajax({
                        url: "/Check/shanchu",
                        async: true,
                        type: "POST",
                        data: { "CHECKNO": CHECKNO },
                        dataType: "json",
                        success: function (data) {
                            alert(data);
                            if (data == "删除成功！") {
                                window.location.href = "/Check/GetModelList"
                            }
                        }
                    })
                }

            }
        })
    }

}


function zhongzhi() {
    if (confirm("确定终止操作?")) {
        $('#tbody').find('tr').each(function () {
            if ($(this).find('td:eq(0)').children('.icheckbox_minimal').is('.checked')) {
                if ($(this).find('td:eq(4)').text() != "开始") {
                    alert("只有开始单据才能终止");
                    return;
                } else {
                    var CHECKNO = $(this).find('td:eq(1)').text();
                    $.ajax({
                        url: "/Check/zhongzhi",
                        async: true,
                        type: "POST",
                        data: { "CHECKNO": CHECKNO },
                        dataType: "json",
                        success: function (data) {
                            alert(data);
                            if (data == "终止成功！") {
                                window.location.href = "/Check/GetModelList"
                            }
                        }
                    })
                }

            }
        })
    }
}

function fenxi() {
    if (confirm("确定盈亏分析操作?")) {
        $('#tbody').find('tr').each(function () {
            if ($(this).find('td:eq(0)').children('.icheckbox_minimal').is('.checked')) {
                if ($(this).find('td:eq(4)').text() != "开始" && $(this).find('td:eq(4)').text() != "完成" ) {
                    alert("只有开始单据才能分析");
                    return;
                } else {
                    var CHECKNO = $(this).find('td:eq(1)').text();
                    window.location.href = "/Check/CheckAnalyze?CHECKNO=" + CHECKNO + "&remark=全部";
                }

            }
        })
    }

}

//*****************************************************盈亏分析******************************************************************
function tiaozheng() {
    if (confirm("确认调整库存，如果确认，库存数据将以盘点数据为准进行调整？")) {
        $.ajax({
            url: "/Check/tiaozheng",
            async: true,
            type: "POST",
            data: { "CHECKNO": $("#CHECKNO").val() },
            dataType: "json",
            success: function (data) {
                alert(data);
                if (data == "调整成功！") {
                    $(".tiaozheng").hide();
                }
            }
        })
    }
}

function tiaozheng_ms() {
    if (confirm("确认调整库存，如果确认，库存数据将以盘点数据为准进行调整？")) {
        $.ajax({
            url: "/Check/tiaozheng_ms",
            async: true,
            type: "POST",
            data: { "CHECKNO": $("#CHECKNO").val() },
            dataType: "json",
            success: function (data) {
                alert(data);
                if (data == "调整成功！") {
                    $(".tiaozheng_ms").hide();
                }
            }
        })
    }
}


