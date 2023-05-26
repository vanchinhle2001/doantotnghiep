$('#submit_contact').click(function () {
    const name = $('#name').val()
    const email = $('#email').val();
    const phone = $('#phone').val();
    const content = $('#content').val();
    return $.ajax({
        type: "POST",
        url: '/Home/Contact',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ name: name, email: email, phone: phone, content: content }),
        dataType: "json",
        success: function (result) {
            if (result == "success") {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 1500,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'success',
                    title: 'Gửi thành công'
                })
                //deleteModal.modal('hide');
                //$("#item_" + brandID).remove();
                return;
            }
            alert("thanh cong")
        },
        error: function () {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 1500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'error',
                title: 'Gửi không thành công'
            })
        }
    });
});