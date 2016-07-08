function getUriParameter(theParameter) { 
  var params = window.location.search.substr(1).split('&');
 
  for (var i = 0; i < params.length; i++) {
    var p=params[i].split('=');
	if (p[0] == theParameter) {
	  return decodeURIComponent(p[1]);
	}
  }
  return false;
}

$(document).ready(function () {
    //$('#mensajeOk').hide();
    //$('#mensajeError').hide(); 
});

function mensaje(mensaje) {
    $('#mensajeOk').show();
    $('#mensaje').html(mensaje);
}

function error(mensaje) {
    $('#mensajeError').show();
    $('#error').html(mensaje);
}