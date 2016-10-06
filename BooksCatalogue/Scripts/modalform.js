$(function () {

    $.ajaxSetup({ cache: false });

    $("a[data-modal]").on("click", function (e) {
        $("#myModalContent").load(this.href, function () {


            $("#myModal").modal({
                keyboard: true
            }, "show");

            bindForm(this);
        });

        return false;
    });
});

function bindForm(dialog) {

    $("#dataForm", dialog).submit(function () {
        var myform = document.getElementById("dataForm");
        var dataForm = new FormData(myform);
        $.ajax({
            url: this.action,
            data: dataForm,
            cache: false,
            processData: false,
            contentType: false,
            type: "POST",
            success: function (result) {
                if (result.success) {
                    $("#myModal").modal("hide");
                    location.reload();
                } else {
                    $("#myModalContent").html(result);
                    bindForm();
                }
            }
        });
        return false;
    });
}
$(function () {

    $.ajaxSetup({ cache: false });

    $("a[del-modal]").on("click", function (e) {

        $("#myModalContent").load(this.href, function () {


            $("#myModal").modal({
                keyboard: true
            }, "show");

            bindDelForm(this);
        });

        return false;
    });


});

function bindDelForm(dialog) {

    $("form", dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $("#myModal").modal("hide");
                    location.reload();
                } else {
                    $("#myModalContent").html(result);
                    bindDelForm();
                }
            }
        });
        return false;
    });
}