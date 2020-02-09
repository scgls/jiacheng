$(".dprint").click(function () {
    var IDs = "";
    if (confirm("确定打印这" + $('input[type=checkbox]:checked').length + "条数据?")) {
        $.each($('input:checkbox:checked'), function () {
            IDs = (IDs + $(this).val() + ",");
        });

        window.open($("#printIP").val() + "Main.aspx?flag=LogisticsBig&parameter1=" + IDs + "&parameter2=2");

        //var newA = document.createElement("a");
        //newA.id = 'gg'
        //newA.target = '_blank';
        //var aaa = $("#printIP").val() + "/Main.aspx?flag=LogisticsBig&parameter1=" + IDs + "&parameter2=2";
        //newA.href = aaa;
        //document.body.appendChild(newA);
        //newA.click();
        //document.body.removeChild(newA);
    }
})

$(".zprint").click(function () {
    var IDs = "";
    if (confirm("确定打印整批数据?")) {
        $.each($('input:checkbox:checked'), function () {
            IDs = (IDs + $(this).val() + ",");
        });

        window.open($("#printIP").val() + "Main.aspx?flag=LogisticsBig&parameter1=" + IDs + "&parameter2=1");

        //var newA = document.createElement("a");
        //newA.id = 'gg'
        //newA.target = '_blank';
        //var aaa = $("#printIP").val() + "/Main.aspx?flag=LogisticsBig&parameter1=" + IDs +"&parameter2=1";
        //newA.href = aaa;
        //document.body.appendChild(newA);
        //newA.click();
        //document.body.removeChild(newA);

    }
})




