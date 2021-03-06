var lista;
var usuario;
var vista;

var ViewLista = Backbone.View.extend({
    el: '#listaUsuarios',
    template: Handlebars.compile($("#templateListaUsuarios").html()),
    render: function () {
        this.$el.html(this.template(this.model.toJSON()));
    },
    events: {
        'click [id^=btnVer]': 'ver',
        'click [id^=btnEliminar]': 'proponeEliminar'
    },
    ver: function (event) {
        var id = $(event.target).data('id-ver');
        location.href = "/Admin/Usuario?id=" + id;
    },
    proponeEliminar: function (event) {
        var id = $(event.target).data('id-eliminar');
        this.idEliminar = id;
    },
    eliminar: function () {
        var modDel = lista.get( this.idEliminar );
        modDel.destroy({
            contentType: 'application/json',
            dataType: 'text',
            success: function () {
                mensaje("Registro eliminado correctamente.");
            },
            error: function () {
                error('Error al eliminar el usuario.');
            }
        });
        vista.render();
    }
});

var Usuario = Backbone.Model.extend({
    urlRoot: '/AdminApi/Usuario',
    initialize: function () {
    }
});

var UsuariosCollection = Backbone.Collection.extend({
    model: Usuario,
    url: '/AdminApi/Usuarios'
});

$(document).ready(function () {
    
    $('#btnConfEliminar').click( function() {
       vista.eliminar(); 
    });

    document.getElementById("btnNuevo").onclick = function () {
        location.href = "/Admin/Usuario";
    };

    lista = new UsuariosCollection();
    lista.fetch({
        success: function () {            
            vista = new ViewLista({
                model: lista
            });
            vista.render();
        },
        error: function () {
            error('Error al obtener la lista de usuarios');
        }
    });
});