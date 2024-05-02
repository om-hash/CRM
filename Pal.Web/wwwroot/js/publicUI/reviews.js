
function AddComment(referenceId, referenceType, rate, IncompleteInfoTitle, IncompleteInfoSubtitle, onSaveSuccess) {
    if (rate == 0) {
        ErrorDialog(IncompleteInfoTitle, IncompleteInfoSubtitle);
        return;
    }
    var comment = $('#comment').val()
    console.log(comment + " " + referenceId + " " + referenceType + " " + rate)
    var header = { RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() }

    var formdata = new FormData();
    formdata.append("ReferenceId", referenceId)
    formdata.append("ReferenceType", referenceType)
    formdata.append("Content", comment)
    formdata.append("Rate", rate)

    $.ajax({
        type: "post",
        headers: header,
        url: "/RatingAndComments/RatingAndCommentAdd",
        contentType: false,
        processData: false,
        data: formdata,
        success: function (res) {
            if (res.responseType === 1) {
                SuccessDialog(onSaveSuccess);
                $('#comment').val('')
               
            }
        },
        error: function (e) {
            console.error(e);
        }
    });
}