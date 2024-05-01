function registerFormSubmit(event) {
    event.preventDefault();

    const email = document.getElementById("email").value;
    if (email) {

        $.ajax({
            url: "Register/PostEmail",
            dataType: "json",
            type: "POST",
            data: { email: email },
            success: function (data) {
                if (data.success) {
                    window.location.href = `${window.location.origin}`;
                } else {
                    console.error("Error:", data.message || "Unknown error");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error("Error sending AJAX request:", textStatus, errorThrown);
            }
        });
    } else {
        console.error("Please enter a valid email address.");
    }
}