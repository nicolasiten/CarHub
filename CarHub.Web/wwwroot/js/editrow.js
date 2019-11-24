$(document).ready(function () {
    $("#add-item").click(function () {
        $.ajax({
            url: this.href,
            cache: false,
            success: function (html) { $("#edit-rows").append(html); }
        });
        return false;
    });

    $("#edit-rows").on("click", "a.remove-row", function (event) {
        event.preventDefault();
        if ($(event.target).parents("tr.edit-row:first").find("input.id-field").val() == 0) {
            $(event.target).parents("tr.edit-row:first").remove();
            return;
        }
        
        Swal.fire({
            backdrop: false,
            title: 'Remove Repair?',
            text: "Do you want to delete the Repair?",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then(function (result) {
            if (result.value) {
                var id = $(event.target).parents("tr.edit-row:first").find("input.id-field").val();
                $.ajax({
                    url: RemoveRepairUrl,
                    type: "post",
                    data: {
                        id: id,
                        date: result.value.Date
                    },
                    success: function (response) {
                        Swal.fire({
                            position: 'top-end',
                            type: 'success',
                            title: response,
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

                return false;
            }
        });
    });
});