
$('.print').click(function () {
    var Printednumber = $(this).parent().parent().find("td").eq(10);
    var BatchNo = $(this).parent().parent().find("td").eq(3).children().val();
    if (BatchNo == "") {
        alert("第一次打印前先输入批次号！");
    }

    var voucherno = $(this).parent().parent().find("td").eq(0).html();
    var mateno = $(this).parent().parent().find("td").eq(1).html();
    var matename = $(this).parent().parent().find("td").eq(2).html();
    var qty = $(this).parent().parent().find("td").eq(4).html();

    var PrintNownumber = $(this).parent().parent().find("td").eq(6).children().val();
    var Bnumber = $(this).parent().parent().find("td").eq(7).children().val();
    var Barcodenumber = $(this).parent().parent().find("td").eq(8).children().html();
    if (PrintNownumber == "" || Bnumber == "" || Barcodenumber == "") {
        alert("请先填完整打印信息");
        return;
    }

    var Printnumber = parseInt(PrintNownumber) + parseInt(Printednumber.html());

    var PrintNownumbera = $(this).parent().parent().find("td").eq(6).children();
    var Bnumberaa = $(this).parent().parent().find("td").eq(7).children();
    var Barcodenumbera = $(this).parent().parent().find("td").eq(8).children();
    var compontentID = $(this).parent().parent().find('.compontentID').html();

    var serialNo = $(this).parent().parent().find('.serialNo').html();
    var orderType = $('#orderType').val();
    $.ajax({
        type: "Get",
        url: "/BPrint/PrintBiaoStart?PONO=" + voucherno + "&mateno=" + mateno + "&matename=" + encodeURI(encodeURI(matename)) + "&colorbatch=" + BatchNo + "&qty=" + qty + "&Printnumber=" + parseInt(PrintNownumber) + "&compontentID=" + compontentID + "&serialNo=" + serialNo + "&orderType=" + orderType,
        data: null,
        success: function (msg) {
            if (msg == "" || msg == null) {
                Printednumber.html(Printnumber);

                var newA = document.createElement("a");
                newA.id = 'gg'
                newA.target = '_blank';

                var aaa = "/ReportWeb/BPrinter.aspx?vid=" + voucherno + "&mateno=" + mateno + "&Bnumber=" + Bnumber + "&PrintNownumber=" + PrintNownumber + "&SerialNo=" + serialNo;
                newA.href = aaa;
                document.body.appendChild(newA);
                newA.click();
                document.body.removeChild(newA);
                PrintNownumbera.val("");
                Bnumberaa.val("");
                Barcodenumbera.html("");
            }
            else {
                alert(msg);
            }
        },
        fail: function () {
            alert("提交失败！")
        }
    });
})








