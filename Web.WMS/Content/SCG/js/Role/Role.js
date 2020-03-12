$(document).on('click', '.switchOn', function () {
    $(this).toggleClass('switchOff');
    var ID = this.dataset.id;
    var IsEnable = ($(this).hasClass("switchOff") == true ? "1" : "2");
    $.ajax({
        type: "get",
        url: "update?ID=" + ID + "&IsEnable=" + IsEnable,
        dataType: "json",
        async: false,
        success: function (data) {
            if (!data) {
                window.location.reload()
            }
        }
    });
});
