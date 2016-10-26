
//Books
$("#CreateBook").click(function (eve) {
    $("#modal-content").load("/Books/Create");
}
);
//$(".CreateBook").click(function (eve) {
//    $("#modal-content").load("/Books/Create");
//}
//);
$(".btnEditor").click(function (eve) {

    $("#modal-content").load("/Books/Edit/" + $(this).data("id"));
});
$(".btnDetails").click(function (eve) {

    $("#modal-content").load("/Books/Details/" + $(this).data("id"));
});
$(".btnDelet").click(function (eve) {

    $("#modal-content").load("/Books/Delete/" + $(this).data("id"));
});


//Country
$("#btnCreateCountry").click(function (eve) {
    $("#modal-content").load("/Countries/Create");
});
$(".btnCountryEditor").click(function (eve) {

    $("#modal-content").load("/Countries/Edit/" + $(this).data("id"));
});

$(".btnCountryDelete").click(function (eve) {

    $("#modal-content").load("/Countries/Delete/" + $(this).data("id"));
});
$(".btnCountryDetails").click(function (eve) {

    $("#modal-content").load("/Countries/Details/" + $(this).data("id"));
});



//Authors
$("#btnCreateAuthor").click(function (eve) {
    $("#modal-content").load("/Authors/Create");
});

$(".btnAuthorEditor").click(function (eve) {

    $("#modal-content").load("/Authors/Edit/" + $(this).data("id"));
});

$(".btnAuthorDelete").click(function (eve) {

    $("#modal-content").load("/Authors/Delete/" + $(this).data("id"));
});
$(".btnAuthorDetails").click(function (eve) {

    $("#modal-content").load("/Authors/Details/" + $(this).data("id"));
});

$(".CreateAuthor").click(function (eve) {
    $("#modal-content").load("/Authors/Create");
});

