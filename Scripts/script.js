/* 
	author: istockphp.com
*/
jQuery(function ($) {

    $("a.toFirma").click(function () {
        loading(); // loading
        setTimeout(function () { // then show popup, deley in .5 second
            loadPopup(); // function show popup 
        }, 500); // .5 second
        return false;
    });
    //

    $("a.toUrun").click(function () {
        loading(); // loading
        setTimeout(function () { // then show popup, deley in .5 second
            loadPopup2(); // function show popup 
        }, 500); // .5 second
        return false;
    });

    /* event for close the popup */
    $("div.close").hover(
					function () {
					    $('span.ecs_tooltip').show();
					},
					function () {
					    $('span.ecs_tooltip').hide();
					}
				);

    $("div.close").click(function () {
        disablePopup();  // function close pop up
        $('#dataFirma').hide();
    });

    $("div.close").click(function () {
        disablePopup2();  // function close pop up
        $('#dataUrun').hide();
    });

    $(this).keyup(function (event) {
        if (event.which == 27) { // 27 is 'Ecs' in the keyboard
            disablePopup();  // function close pop up
        }
    });
    $(this).keyup(function (event) {
        if (event.which == 27) { // 27 is 'Ecs' in the keyboard
            disablePopup2();  // function close pop up
        }
    });

    $('a.livebox').click(function () {
        alert('Hello World!');
        return false;
    });


    /************** start: functions. **************/
    function loading() {

        $("div.loader").show();

    }
    function closeloading() {
        $("div.loader").fadeOut('normal');
    }

    var popupStatus = 0; // set value
    var popupStatusUrun = 0; // set value

    function loadPopup() {
        if (popupStatus == 0) { // if value is 0, show popup
            closeloading(); // fadeout loading
            $("#toFirma").fadeIn(0500); // fadein popup div
            $("#backgroundPopup").css("opacity", "0.7"); // css opacity, supports IE7, IE8
            $("#backgroundPopup").fadeIn(0001);
            popupStatus = 1; // and set value to 1
        }
    }

    function loadPopup2() {
        if (popupStatusUrun == 0) { // if value is 0, show popup
            closeloading(); // fadeout loading
            $("#toUrun").fadeIn(0500); // fadein popup div
            $("#backgroundPopup").css("opacity", "0.7"); // css opacity, supports IE7, IE8
            $("#backgroundPopup").fadeIn(0001);
            popupStatusUrun = 1; // and set value to 1
        }
    }

    function disablePopup() {
        if (popupStatus == 1) { // if value is 1, close popup
            $("#toFirma").fadeOut("normal");
            $("#backgroundPopup").fadeOut("normal");
            popupStatus = 0;  // and set value to 0
        }
    }

    function disablePopup2() {
        if (popupStatusUrun == 1) { // if value is 1, close popup
            $("#toUrun").fadeOut("normal");
            $("#backgroundPopup").fadeOut("normal");
            popupStatusUrun = 0;  // and set value to 0
        }
    }
    /************** end: functions. **************/
});       // jQuery End