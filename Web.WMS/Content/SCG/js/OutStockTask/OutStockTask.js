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
                //html += "<td>" + data[i].RowNoDel + "</td>"
                html += "<td>" + data[i].MaterialNo + "</td>"
                html += "<td>" + data[i].MaterialDesc + "</td>"
                html += "<td>" + data[i].TaskQty + "</td>"
                html += "<td>" + data[i].RemainQty + "</td>"
                html += "<td>" + data[i].UnShelveQty + "</td>"
                html += "<td>" + data[i].StrLineStatus + "</td>"
                //html += "<td>" + data[i].StrIsSpcBatch + "</td>"
                html += "<td>" + data[i].FromBatchNo + "</td>"
                html += "<td>" + data[i].FromErpWarehouse + "</td>"
                //html += "<td>" + data[i].FromErpAreaNo + "</td>"
                //html += "<td>" + data[i].ToErpWareHouse + "</td>"
                //html += "<td>" + data[i].ToErpAreaNo + "</td>"
                //html += "<td>" + data[i].ProductNo + "</td>"
                html += "<td>" + data[i].OperatorUserName + "</td>"
                //html += "<td>" + data[i].OperatorDateTime + "</td>"
                //html += "<td>" + data[i].StrModifyer + "</td>"
                //html += "<td>" + data[i].Modifytime + "</td>"
                html += "</tr>";
                $("#detailTable").append(html);
            }
        }
    });
}

