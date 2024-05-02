
//-------------------------------------------------------------
function ErrorToast(title, mgs, time = 3000) {
    Swal.fire({
        position: 'top-end',
        icon: 'error',
        title: title ?? 'Error',
        showConfirmButton: false,
        timer: time,
        text: mgs ?? 'Error While Processing Your Request',
        toast: true,
        showCloseButton: true
    })
}

//-------------------------------------------------------------
function ErrorDialog(title, msg) {
    Swal.fire({
        icon: 'error',
        title: title,
        text: msg,
        showConfirmButton: true,
        showCloseButton: true
    })
}

//-------------------------------------------------------------
function SuccessToast(title, time = 3000) {
    Swal.fire({
        position: 'top-end',
        icon: 'success',
        timer: time,
        title: title,
        showConfirmButton: false,
        toast: true,
        showCloseButton: true
    })
}

//-------------------------------------------------------------
function SuccessDialog(title, msg) {
    Swal.fire({
        icon: 'success',
        title: title,
        text: msg,
        showConfirmButton: true,
        showCloseButton: true
    })
}

//-------------------------------------------------------------
function showImgPreview(inputEvent, imgElementId) {
    if (inputEvent.files.length > 0) {
        var src = URL.createObjectURL(inputEvent.files[0]);
        var preview = document.getElementById(imgElementId);
        preview.src = src;
        preview.style.display = 'block';
    }
};


//----------------------------------------------------------------------
function saveAttachments(referenceId, elementId, mediaType, referenceType) {
    var fileInput = document.getElementById(elementId);
    var formdata = new FormData();
    if (fileInput.files.length < 1) return;
    for (i = 0; i < fileInput.files.length; i++) {
        formdata.append("files", fileInput.files[i]);
    }
    formdata.append("mediaType", mediaType);
    formdata.append("referenceType", referenceType);
    formdata.append("referenceId", referenceId);
    var header = { __RequestVerificationToken: $('#form-Attachments input[name="__RequestVerificationToken"]').val() };
    $.ajax({
        headers: header,
        url: '/Attachments/UploadFiles',
        type: 'post',
        data: formdata,
        processData: false,
        contentType: false,
        success: function (res) {
        },
        error: function (e) {
            console.error(e);
        }
    });
}

//----------------------------------------------------------------------
function onDeleteAttachment(id) {
    Swal.fire({
        title: 'هل أنت متأكد من حذف الصورة ؟',
        text: "لا يمكن التراجع بعد الضغط على زر الحذف",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'حذف',
        cancelButtonText: 'تراجع',
    }).then((result) => {
        if (result.isConfirmed) {
            var headers = { __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() };
            $.ajax({
                headers: headers,
                url: '/Attachments/DeleteFile?id=' + id,
                type: 'POST',
                success: function (res) {
                    if (res.responseType === 1) {
                        $('.td-' + id).hide();
                    }
                }
            });
        }
    });
}


//---------------------------------------
function dateDiff(date1, date2, lang) {
    var Difference_In_Time = date2.getTime() - date1.getTime();
    var Difference_In_Days = Difference_In_Time / 1000;
    if (lang === 'ar') {
        switch (true) {
            case (Difference_In_Days < 300):
                return 'منذ قليل';
            case ((Difference_In_Days / 60) < 60):
                return `منذ ${Number(Difference_In_Days / 60).toFixed(0)} دقيقة`;
            case (((Difference_In_Days / 60 / 60) >= 1) && ((Difference_In_Days / 60 / 60) < 24)):
                return `منذ ${Number(Difference_In_Days / 60 / 60).toFixed(0)} ساعة`;
            case (((Difference_In_Days / 60 / 60 / 24) >= 1) && ((Difference_In_Days / 60 / 60 / 24) < 30)):
                return `منذ ${Number(Difference_In_Days / 60 / 60 / 24).toFixed(0)} يوم`
            case (((Difference_In_Days / 60 / 60 / 24 / 30) >= 1) && ((Difference_In_Days / 60 / 60 / 24 / 30) < 12)):
                return `منذ ${Number(Difference_In_Days / 60 / 60 / 24 / 30).toFixed(0)} شهر`
            case (Difference_In_Days > 300):
                return Difference_In_Days;
        }
    } else {
        switch (true) {
            case (Difference_In_Days < 300):
                return 'Just Now';
            case ((Difference_In_Days / 60) < 60):
                return `${Number(Difference_In_Days / 60).toFixed(0)} minutes ago`;
            case (((Difference_In_Days / 60 / 60) >= 1) && ((Difference_In_Days / 60 / 60) < 24)):
                return `${Number(Difference_In_Days / 60 / 60).toFixed(0)} hours ago`;
            case (((Difference_In_Days / 60 / 60 / 24) >= 1) && ((Difference_In_Days / 60 / 60 / 24) < 30)):
                return `${Number(Difference_In_Days / 60 / 60 / 24).toFixed(0)} days ago`
            case (((Difference_In_Days / 60 / 60 / 24 / 30) >= 1) && ((Difference_In_Days / 60 / 60 / 24 / 30) < 12)):
                return `${Number(Difference_In_Days / 60 / 60 / 24 / 30).toFixed(0)} months ago`
            case (Difference_In_Days > 300):
                return Difference_In_Days;
        }
    }
}

//---------------------------------------
function showLoading(id = 'myBody') {
    $('#' + id).block({
        centerY: 0,
        css: {
            top: '425px', left: '', right: '10px',
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .7,
            color: '#fff'
        }
    });
}

//---------------------------------------
function hideLoading(id = 'myBody') {
    $('#' + id).unblock();
}

//-------------------------------------------------------------------
function ConfirmAlert(msg = 'Are You Sure?',func = null) {
    Swal.fire({
        title: msg,
        showDenyButton: true,
        showCancelButton: false,
        confirmButtonText: 'Yes',
        denyButtonText: `No`,
    }).then((result) => {
        if (result.isConfirmed) {
            func();
        } else if (result.isDenied) {
            Swal.fire('Changes are not saved', '', 'info')
        }
    })
}