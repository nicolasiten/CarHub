function confirmDelete(me) {
    event.preventDefault();

    Swal.fire({
        title: 'Do you want to delete the image?',
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then(function (result) {
        if (result.value) {
            $.ajax({
                url: me.getAttribute('href'),
                success: function (e) {
                    $(me).closest(".car-image").hide();
                }
            });
        }
    });

    return false;
}

function confirmThumbnailUpdate(me) {
    event.preventDefault();

    Swal.fire({
        title: 'Do you want to set the Image as Thumbnail?',
        type: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Save'
    }).then(function (result) {
        if (result.value) {
            $.ajax({
                url: me.getAttribute('href'),
                success: function (e) {
                    console.log($(me).closest(".car-image").find(".img").attr("src"));
                    $('.car-thumbnail').prop("src", $(me).closest(".car-image").find(".img").attr("src"));

                    Swal.fire({
                        position: 'top-end',
                        type: 'success',
                        title: 'New Thumbnail has been set.',
                        showConfirmButton: false,
                        timer: 1000
                    });
                }
            });
        }
    });

    return false;
}