// Write your JavaScript code.
$(function () {
    // $('#commentBox').hide();

    $('#showCommentBox').click(function () {
        $('#commentBox').show();
    });

})

function showRatingComment() {
    $('#ratingBox').show();
}

function refreshRestaurantListPage(SortBy, url) {
    window.history.pushState(SortBy, 'SortBy', url);
    location.reload();
}

function loadPage(id, page) {
    $.ajax({
        url: '/Restaurant/' + page + '/' + id,
        cache: false,
        success: function (data) {
            $('#dashboardView').html(data);
        }
    });
}


function loadUpdateAccount(id) {
    $.ajax({
        url: '/Admin/UpdateAccount/' + id,
        cache: false,
        success: function (data) {
            $('#update-modal-body').html(data);
            $("#update-modal").modal('show');
        }
    });
}

function loadUpdateRestaurant(id) {
    $.ajax({
        url: '/Admin/updateRestaurant/' + id,
        cache: false,
        success: function (data) {
            $('#update-modal-body').html(data);
            $("#update-modal").modal('show');
        }
    });
}

function loadCreateAccount() {
    $.ajax({
        url: '/Admin/CreateAccount/',
        cache: false,
        success: function (data) {
            $('#create-modal-body').html(data);
            $("#create-modal").modal('show');
        }
    });
}

function loadCreateRestaurant() {
    $.ajax({
        url: '/Admin/CreateRestaurant/',
        cache: false,
        success: function (data) {
            $('#create-modal-body').html(data);
            $("#create-modal").modal('show');
        }
    });
}

function loadDetailsAccount(id) {
    $.ajax({
        url: '/Admin/DetailsAccount/' + id,
        cache: false,
        success: function (data) {
            $('#details-modal-body').html(data);
            $("#details-modal").modal('show');
        }
    });
}

function loadDetailsRestaurant(id) {
    $.ajax({
        url: '/Admin/DetailsRestaurant/' + id,
        cache: false,
        success: function (data) {
            $('#details-modal-body').html(data);
            $("#details-modal").modal('show');
        }
    });
}