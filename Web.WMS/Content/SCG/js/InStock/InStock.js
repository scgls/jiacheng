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
                //html += "<td>" + data[i].FromBatchNo + "</td>"
                html += "<td>" + data[i].InStockQty + "</td>"
                html += "<td>" + data[i].RemainQty + "</td>"
                html += "<td>" + data[i].ReceiveQty + "</td>"
                html += "<td>" + data[i].FromErpWarehouse + "</td>"
                html += "</tr>";
                $("#detailTable").append(html);
            }
        }
    });
}


