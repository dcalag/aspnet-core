var usuario;
var vista;

var ViewDatosUsuario = Backbone.View.extend({
    el: '#divUsuarioNuevo',
    template: Handlebars.compile($("#templateUsuario").html()),
    render: function () {
        this.$el.html(this.template({usuario: this.model.toJSON()}));
    },
    events: {
        'click #btnGuardar': 'guardarUsuario',
        'click #btnCancelar': 'cancelar'
    },
    guardarUsuario: function () {
        usuario.cargarDatos();
        usuario.save(null, {
            contentType: 'application/json',
            dataType: 'text',
            success: function () {
                mensaje("Usuario guardado correctamente.");
            },
            error: function () {
                error('Error al guardar el usuario.');
            }
        }
        );
    },
    cancelar: function () {
        location.href = "/Admin/Usuarios";
    }
});

var Usuario = Backbone.Model.extend({
    initialize: function () {

    },
    cargarDefaults: function () {
        this.set('username', '');
        this.set('password', '');
        this.set('enabled', true);

        var roles = [];
        roles.push({role: "ROLE_USER"});
        this.set('roles', roles);
    },
    getError: function () {
        return this.get('error');
    },
    cargarDatos: function () {
        this.set('username', $('#username').val());
        this.set('password', $('#password').val());
        this.set('enabled', $("#checkEnabled").is(':checked'));

        // construir roles tal y como los necesita el servicio REST.
        var roles = [];

        if ($("#adminRole").is(':checked')) {
            roles.push({role: "ROLE_ADMIN"});
        }
        if ($("#userRole").is(':checked')) {
            roles.push({role: "ROLE_USER"});
        }

        this.set('roles', roles);
    },
    calculateRoles: function () {
        this.set('userRole', false);
        this.set('userAdmin', false);

        for (i = 0; i < this.get('roles').length; i++) {
            if (this.get('roles')[i].role === 'ROLE_USER') {
                this.set('userRole', true);
            }
            if (this.get('roles')[i].role === 'ROLE_ADMIN') {
                this.set('adminRole', true);
            }
        }
    },
    urlRoot: '/AdminApi/Usuario'
});

$(document).ready(function () {       

    var verId = getUriParameter('id');
    usuario = new Usuario();

    if (verId !== false)
    {
        usuario.set('id', verId);
        usuario.fetch({
            success: function () {
                //function (model,response)
                //var responseObj = $.parseJSON(response);
                //console.debug(responseObj.error);
                usuario.calculateRoles();

                vista = new ViewDatosUsuario({
                    model: usuario
                });
                vista.render();
                $('#username').focus();
            },
            error: function () {
                error('Error al obtener el usuario.');
            }
        });
    }
    else {
        usuario.cargarDefaults();
        usuario.calculateRoles();

        vista = new ViewDatosUsuario({
            model: usuario
        });
        vista.render();
        $('#username').focus();
    }
});