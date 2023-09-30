
// Write your JavaScript code.
$(function () {
    //console.log("Page is loaded, yeah!");

    /* $(document).on("click", ".game-button", function (event) {
        event.preventDefault();

        var buttonNumber = $(this).val();

        console.log("Game button " + buttonNumber + " was clicked!");

        doButtonUpdate(buttonNumber);
    }); */

    $(document).on("mousedown", ".game-button", function (event) {
        event.preventDefault();
        switch (event.which) {
            case 1:
                var buttonNumber = $(this).val();

                doButtonUpdate(buttonNumber, '/Button/ShowOneButton');

                //alert("Left mouse button clicked! Button number is: " + buttonNumber);
                break;
            case 2:
                alert("Middle mouse button clicked!");
                break;
            case 3:
                var buttonNumber = $(this).val();
                doButtonUpdate(buttonNumber, '/Button/RightClickShowOneButton');
                //alert("Right mouse button clicked! Button number is: " + buttonNumber);
                break;
            default:
                alert("Nothing Clicked!");
        }
    });
    $(document).bind("contextmenu", function (e) {
        e.preventDefault();

        console.log("The context menu is disabled");
    })

    function doButtonUpdate(buttonNumber, urlString) {
        $.ajax({
            method: 'POST',
            url: urlString,
            data: {
                "buttonNumber": buttonNumber
            },
            success: function (data) {
                //data should be a json with part1 and part2 objects in it, see button controller method SHOWONEBUTTON
                console.log(data);
                $("#" + buttonNumber).html(data.part1);
                $(".messageArea").html(data.part2);
            }
        });
    }
});