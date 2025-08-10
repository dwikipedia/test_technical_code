$(document).ready(() => {
    $("#bookingForm").on("submit", function (e) {
        e.preventDefault(); // Stop form from submitting right away

        Swal.fire({
            title: 'Confirm Booking?',
            text: "Do you want to proceed with this meeting booking?",
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, book it!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                document.getElementById("bookingForm").submit();
            }
        });
    });

    $(".time-suggestion").on("click", function () {
        const selectedTime = $(this).data("time").split('to')[1];
        let timeOnly = selectedTime.trim().substring(0, 5);

        Swal.fire({
            title: 'Use this time?',
            text: `Set time to ${selectedTime}?`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Yes, set it!',
            cancelButtonText: 'Cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                $("#Time").val(timeOnly);

                Swal.fire({
                    title: 'Time Set!',
                    text: `The time has been set to ${selectedTime}.`,
                    icon: 'success',
                    timer: 1000,
                    showConfirmButton: false
                });
            }
            $('#alert-error').hide();
        });
    });

    var $table = $("#suggestionsTable");

    // delegate hover to thead th
    $table.on("mouseenter", "thead th, tbody td.time-suggestion", function () {
        var idx = $(this).index() + 1; // nth-child is 1-based
        $table.find("thead th:nth-child(" + idx + "), tbody tr td:nth-child(" + idx + ")")
            .addClass("hovered");
    });

    $table.on("mouseleave", "thead th, tbody td.time-suggestion", function () {
        var idx = $(this).index() + 1;
        $table.find("thead th:nth-child(" + idx + "), tbody tr td:nth-child(" + idx + ")")
            .removeClass("hovered");
    });

});
