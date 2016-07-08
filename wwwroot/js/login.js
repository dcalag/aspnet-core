$(document).ready(function() {
    $('#btnCloseErr').click(function() {
       $('#mensajeError').slideUp();
       $('#username').focus();
    });
    
    $('#mensajeError').hide();

    $('#username').focus();

    $('#btnLogin').click(login);
});

function cargarDatos() {
    var lDatos = { username:"", password:""};
    lDatos.username = $('#username').val();
    lDatos.password = $('#password').val();

    return lDatos;
}

function login() {    
    $('#btnLogin').prop('disabled', true);    
    $.ajax({
        url: "/MainApi/Login",
        type: "POST",
        data: JSON.stringify(cargarDatos()),
        headers: {
            "Content-Type": "application/json"
        },
        success: function (data, textStatus, jqXHR) {
            if (data.error ===''){
                // login exitoso.
                window.location.href = "/Main/Inicio";
            }
            else
            {
                mostrarError('Error: ' + data.error);
            }
            $('#btnLogin').prop('disabled', false);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            mostrarError('Error: ' + errorThrown);
            $('#btnLogin').prop('disabled', false);
        }
    });
}

function mostrarError(error){
    $('#error').html(error);
    $('#mensajeError').slideDown();
}