// DataTables
$.extend(true, $.fn.dataTable.defaults, {
    "language": {
        "sProcessing": "Procesando...",
        "sLengthMenu": "Mostrar _MENU_",
        "sZeroRecords": "No se encontraron resultados",
        "sEmptyTable": "Ningún dato disponible en esta tabla",
        "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
        "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
        "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
        "sInfoPostFix": "",
        "sSearch": "Buscar:",
        "sUrl": "",
        "sInfoThousands": ",",
        "sLoadingRecords": "Cargando...",
        "oPaginate": {
            "sFirst": "Primero",
            "sLast": "Último",
            "sNext": "Siguiente",
            "sPrevious": "Anterior"
        },
        "oAria": {
            "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
            "sSortDescending": ": Activar para ordenar la columna de manera descendente"
        },
        "buttons": {
            "copy": "Copiar",
            "colvis": "Visibilidad"
        }
    },
    "dom":
        "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'f>>" +
        "<'row'<'col-sm-12'<'table-responsive't>r>>" +
        "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
    "info": false,
    "lengthMenu": [5, 10, 25, 50, 75, 100],
});

// Select2
$.fn.select2.defaults.set("theme", "bootstrap4");
$.fn.select2.defaults.set("width", "100%");

function nuevo_tab(id, titulo) {
    var id_tab = 'tab_' + id;
    var boton_cerrar = '<button class="close" title="Cerrar" aria-label="Cerrar" onclick="cerrar_tab(' + id + ')"><span aria-hidden="true">&times;</span></button>';

    // Agrega un tab
    $('ul#tabs').append(
        '<li class="nav-item" role="presentation">' +
        '<a class="nav-link" id="link_' + id + '" data-toggle="tab" href="#' + id_tab + '" role="tab" aria-controls="' + id_tab + '" aria-selected="false">' +
        '<span class="mr-2">' + titulo + '</span>' +
        boton_cerrar +
        '</a>' +
        '</li>'
    );

    // Agrega un contenedor de ese tab
    $('#tab-content1').append(
        '<div class="tab-pane" id="' + id_tab + '" role="tabpanel" aria-labelledby="link_' + id + '"></div>'
    );
}

function cerrar_tab(id) {
    var link = $('a#link_' + id)
    var li = link.closest('li');

    // Si el tab está activo, se redirigirá a la anterior
    if (link.hasClass('active'))
        li.prev().find("a[data-toggle='tab']").trigger('click');

    // Elimina el tab
    li.remove();

    // Elimina el contenedor del tab
    $('#tab_' + id).remove();

    return;
}

$(function () {
    var tabla_asesorias = $('#tabla_asesorias').DataTable({
        "order": [[1, 'desc']],
    });

    $('#tabla_asesorias [data-toggle="popover"]').popover();

    function recargar_tabla_asesorias() {
        var $div = $('#tabListado');
        $.ajax({
            type: "POST",
            url: baseUrl + "Asesoria/_ListaAsesorias",
            beforeSend: function () {
                $div.html('<strong class="text-center">Un momento por favor...</strong>');
            },
            success: function (response) {
                $div.html(response);
                tabla_asesorias = $('#tabla_asesorias').DataTable({
                    "order": [[1, 'desc']],
                });

                $('#tabla_asesorias [data-toggle="popover"]').popover();
            },
            error: function () {
                $div.html('<strong class="text-center">Error al procesar su petición</strong>');
            }
        });
    }

    $('body').on('click', '#tabla_asesorias tbody tr[data-id]', function () {
        var id_asesoria = $(this).data('id');
        var nom_cli = $(this).data('nom');

        var param = {
            "id_asesoria": id_asesoria,
        };
        var div_tab = $('#tab_' + id_asesoria);

        if (div_tab.length == 1) {
            $('#link_' + id_asesoria).trigger('click');
            return;
        } else {
            nuevo_tab(id_asesoria, nom_cli);
            $('#link_' + id_asesoria).trigger('click');
        }

        $.ajax({
            type: "POST",
            url: baseUrl + "Asesoria/_Asesoria",
            data: param,
            beforeSend: function () {
                //$('#link_' + index).prepend('<div class="loader-tab"></div>');
                $('#tab_' + id_asesoria).append('<h4 class="text-center font-weight-bold">Un momento por favor...</h4>');
            },
            success: function (response) {
                //$('#link_' + index).find('.loader-tab').remove();
                $('#tab_' + id_asesoria).html(response);
                $('#tab_' + id_asesoria).attr('data-id', id_asesoria);
                $('#tab_' + id_asesoria).find('select.s2').select2();
            },
            error: function () {
                //$('#link_' + index).find('.loader-tab').remove();
                $('#tab_' + id_asesoria).html('<h4 class="text-center font-weight-bold">Error al procesar su petición</h4>');
            }
        });
    });

    $('body').on('click', '.btnCambiarAsesor', function () {
        var $this = $(this);
        var $tab = $this.closest('.tab-pane');
        var $asesor = $tab.find('.asesoria_asesor');
        var $btnConfirmarCambioAsesor = $tab.find('.btnConfirmarCambioAsesor');
        var $btnCancelarCambioAsesor = $tab.find('.btnCancelarCambioAsesor');

        $asesor.prop('disabled', false);
        $asesor.focus();

        $this.hide();
        $btnConfirmarCambioAsesor.show();
        $btnCancelarCambioAsesor.show();
    });

    $('body').on('click', '.btnConfirmarCambioAsesor', function () {
        var $this = $(this);
        var $tab = $this.closest('.tab-pane');
        var $asesor = $tab.find('.asesoria_asesor');
        var $btnCambiarAsesor = $tab.find('.btnCambiarAsesor');
        var $btnCancelarCambioAsesor = $tab.find('.btnCancelarCambioAsesor');
        var id_asesoria = $tab.data('id');

        var param = {
            id_asesoria: id_asesoria,
            n_rut_asesor: $asesor.val(),
        };

        $this.prop('disabled', true);
        $.post(baseUrl + 'Asesoria/CambiarAsesor', param)
            .done(function (res) {
                $this.prop('disabled', false);

                alert(res.mensaje);

                if (!res.estado) return;

                $asesor.data('value', $asesor.val());
                $asesor.prop('disabled', true);

                $btnCambiarAsesor.show();
                $this.hide();
                $btnCancelarCambioAsesor.hide();

                recargar_tabla_asesorias();
            })
            .fail(function () {
                $this.prop('disabled', false);

                alert('Error interno al procesar la solicitud');
            });
    });

    $('body').on('click', '.btnCancelarCambioAsesor', function () {
        var $this = $(this);
        var $tab = $this.closest('.tab-pane');
        var $asesor = $tab.find('.asesoria_asesor');
        var $btnCambiarAsesor = $tab.find('.btnCambiarAsesor');
        var $btnConfirmarCambioAsesor = $tab.find('.btnConfirmarCambioAsesor');

        $asesor.val($asesor.data('value'));
        $asesor.prop('disabled', true);
        $asesor.trigger('change');

        $btnCambiarAsesor.show();
        $this.hide();
        $btnConfirmarCambioAsesor.hide();
    });

    $('body').on('click', '.btnEnviar', function () {
        var $this = $(this);
        var $tab = $this.closest('.tab-pane');
        var id_asesoria = $tab.data('id');
        var comentario = $tab.find('.g_comentario').val();

        var param = {
            "id_asesoria": id_asesoria,
            "comentario": comentario,
        };

        $.ajax({
            type: "POST",
            url: baseUrl + "Asesoria/_EnviaAsesoria",
            data: param,
            beforeSend: function () {
                $this.prop('disabled', true);
            },
            success: function (res) {
                $this.prop('disabled', false);
                if (res.estado == "1") {
                    $('#m_asesoria').modal('hide');
                    recargar_tabla_asesorias();
                }
                alert(res.mensaje);
            },
            error: function () {
                $this.prop('disabled', false);
                alert('Hubo un error al procesar su solicitud :-(\nInténtelo más tarde.');
            }
        });
    });
});
