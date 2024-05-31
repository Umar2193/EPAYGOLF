$(function () {
	//Initialize Select2 Elements
	$('.select2').select2();

	//Initialize Select2 Elements
	$('.select2bs4').select2({
		theme: 'bootstrap4'
	});
});
$(document).ready(function () {
    // Get the current URL path
    debugger
    var currentPath = window.location.pathname;

    // Select all menu items
    $('.nav-link').each(function () {
        // Get the href attribute of the menu item
        var menuItemPath = $(this).attr('href');

        // Check if the href matches the current path
        if (menuItemPath === currentPath) {
            // Add the active class to the matching menu item
            $('.nav-link').removeClass('active');
            $(this).addClass('active');
        }
    });
});
