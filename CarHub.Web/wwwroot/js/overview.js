function setSaleDate(id) {
    swal.fire({
        title: "Sales Date",
        html: '<input id="datepicker" type="" class="form-control" autofocus>',
        type: "question",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes',
        onOpen: function () {
            $("#datepicker").prop('type', 'date');
            var date = new Date();
            var currentDate = date.toISOString().slice(0, 10);
            document.getElementById('datepicker').value = currentDate;
        },
        preConfirm: function () {
            return new Promise((resolve) => {
                resolve({
                    Date: $('#datepicker').val(),
                });
            });
        }
    }).then(function (result) {
        if (result.value) {
            $.ajax({
                url: SaveSalesDateUrl,
                type: "post",
                data: {
                    id: id,
                    date: result.value.Date
                },
                success: function () {
                    Swal.fire({
                        position: 'top-end',
                        type: 'success',
                        title: "Successfully saved car!",
                        showConfirmButton: false,
                        timer: 1000
                    });
                    window.setTimeout(function () { location.reload() }, 1000)
                },
                error: function (response) {
                    Swal.fire({
                        position: 'top-end',
                        type: 'error',
                        title: response.responseText,
                        showConfirmButton: false,
                        timer: 1000
                    });
                }
            });
        }
    });
}