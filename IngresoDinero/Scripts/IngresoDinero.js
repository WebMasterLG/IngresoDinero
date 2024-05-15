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
        "<'row'<'col-sm-12 col-md-6'Bi><'col-sm-12 col-md-6 text-right'lf>>" +
        "<'row'<'col-sm-12'<'table-responsive scrollbar't>r>>" +
        "<'row'<'col-sm-12 col-md-5'><'col-sm-12 col-md-7'p>>",
    "buttons": [
        {
            extend: 'colvis',
            text: 'Columnas'
        },
        {
            extend: 'excelHtml5',
            autoFilter: true,
            text: 'Excel',
            footer: true,
            exportOptions: {
                columns: ':visible'
            },
            filename: 'IngresoDinero'
        },
        {
            extend: 'pdfHtml5',
            orientation: 'landscape',
            pageSize: 'LEGAL',
            text: 'PDF',
            footer: true,
            exportOptions: {
                columns: ':visible'
            },
            filename: 'IngresoDinero'
        }
    ],
    "iDisplayLength": 25,
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
    var tbl_dt = {
        "order": [[1, 'desc']],
        "columnDefs": [
            { "orderable": false, "targets": 0 }
        ],
        "footerCallback": function (row, data, start, end, display) {

            var api = this.api();

            // Remove the formatting to get integer data for summation
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.trim().replace(/[\$.]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            // Total over all pages
            var total_monto = api.column(10).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);

            // Total over this page
            var subtotal_monto = api.column(10, { page: 'current' }).data().reduce(function (a, b) { return intVal(a) + intVal(b); }, 0);

            var numberFormat = new Intl.NumberFormat('es-CL', { style: 'currency', currency: 'CLP' });
            $("#txt_total_monto").html(numberFormat.format(total_monto));
            $("#txt_subtotal_monto").html(numberFormat.format(subtotal_monto));

        },
    };
    var tabla_ingreso_dinero = $('#tabla_ingreso_dinero').DataTable(tbl_dt);

    function cargarTooltips() {
        $('#tabla_ingreso_dinero [data-toggle="popover"]').popover();
        $('#tabla_ingreso_dinero [data-toggle="tooltip"]').tooltip();
        tabla_ingreso_dinero.on('draw', function () {
            $('#tabla_ingreso_dinero [data-toggle="popover"]').popover();
            $('#tabla_ingreso_dinero [data-toggle="tooltip"]').tooltip();
        });
    }

    cargarTooltips();

    function recargar_tabla_ingreso_dinero() {
        var n_rut_cliente = $("#cliente").val();
        var id_estado = $("#estado").val();
        var id_forma_pago = $("#forma_pago").val();
        var tipo = $("#tipo").val();
        var fecha_desde = $("#fecha_desde").val();
        var fecha_hasta = $("#fecha_hasta").val();
        var empresa = $("#empresa").val();
        var n_rut_cliente_facturar = $("#cliente_facturar").val();
        var ids_cotizaciones = $('#ids_cot').val().replace(',', '|')

        n_rut_cliente = n_rut_cliente && n_rut_cliente.length > 0 ? n_rut_cliente.join('|') : '0';
        id_estado = id_estado && id_estado.length > 0 ? id_estado.join('|') : '0';
        id_forma_pago = id_forma_pago && id_forma_pago.length > 0 ? id_forma_pago.join('|') : '0';
        tipo = tipo && tipo.length > 0 ? tipo.join('|') : '0';
        empresa = empresa && empresa.length > 0 ? empresa.join('|') : '0';
        n_rut_cliente_facturar = n_rut_cliente_facturar && n_rut_cliente_facturar.length > 0 ? n_rut_cliente_facturar.join('|') : '0';
        ids_cotizaciones = ids_cotizaciones ? ids_cotizaciones : '0'

        var proc = 0
        if ($("#chk_facturados").is(':checked')) {
            proc = 1;
        } else {
            proc = 0;
        }

        var regularizados = 0
        if ($("#chk_regularizados").is(':checked')) {
            regularizados = 1;
        } else {
            regularizados = 0;
        }

        var param = {
            "n_rut_cliente": n_rut_cliente,
            "id_estado": id_estado,
            "id_forma_pago": id_forma_pago,
            "id_tipo":tipo,
            "fecha_desde": fecha_desde,
            "fecha_hasta": fecha_hasta,
            "empresa": empresa,
            "n_rut_cliente_facturar": n_rut_cliente_facturar,
            "ids_cotizaciones": ids_cotizaciones,
            "proc": proc,
            "regularizados": regularizados
        };
        
        var $div = $('#cuadroComisiones');
        $.ajax({
            type: "POST",
            url: baseUrl + "IngresoDinero/_ListaIngresoDinero",
            data: param,
            beforeSend: function () {
                $div.html('<strong class="text-center">Un momento por favor...</strong>');
            },
            success: function (response) {
                $div.html(response);
                tabla_ingreso_dinero = $('#tabla_ingreso_dinero').DataTable(tbl_dt);
                cargarTooltips();
            },
            error: function () {
                $div.html('<strong class="text-center">Error al procesar su petición</strong>');
            }
        });
    }

    $("#chk_facturados").on('change', function () {
        recargar_tabla_ingreso_dinero();
    });

    $('body').on('click', '#btnBuscar', function () {
        recargar_tabla_ingreso_dinero();
    });
    $('body').on('click', '#cerrar_resultado_estado', function () {
        recargar_tabla_ingreso_dinero();
    });

    $('body').on('click', '#chk_todos', function () {
        var checked = this.checked;
        tabla_ingreso_dinero.column(0).nodes().to$().each(function (index) {
            $(this).find('.chkIngreso').prop('checked', checked);
        });
        tabla_ingreso_dinero.draw();
    });
    $('body').on('click', '.subir_archivo_cobro', function () {
        var id_promesa = $(this).attr("id_promesa");
        var id_archivo = "#archivo_" + id_promesa;
        //alert(id_archivo);
        $(id_archivo).click();
    });
    $('body').on('click', '#btnReciboMultiple', function () {
        var $chkIngresoChecked = tabla_ingreso_dinero.column(0, { search: 'applied' }).nodes().to$().find('.chkIngreso:checked');
        var ids = [];
        if ($chkIngresoChecked.length == 0) {
            alert('Debe seleccionar al menos un ingreso.');
            return;
        }
        $chkIngresoChecked.each(function () {
            ids.push($(this).val());
        });
        var id = ids.join('|');
        var url = `https://sistemas.lecarosgroup.com/ContratosV3/Home/GeneraContrato?id=${id}&con=7&sp=15`;
        window.location.href = url;
    });
    recargar_tabla_ingreso_dinero();


    //$(".modificar_prom").click(function () {
    $('body').on('click', '.modificar_prom', function () {
        var IdPropiedad = $(this).attr("IdPropiedad");
        var IdCotizacion = $(this).attr("IdCotizacion");
        $("#ModalPromAutom").modal('show');

        var param = { "IdPropiedad": IdPropiedad, "IdCotizacion": IdCotizacion };

        $.ajax({
            type: "GET",
            url: "PromAutom/Index",
            data: param,
            async: true,
            beforeSend: function () {
                $("#Contenido_ModalPromAutom").html("<center><img src='../Images/loading.gif' /></center>");
            },
            success: function (data) {

                $("#Contenido_ModalPromAutom").html(data);

            },
            error: function (request, status, error) { /* alert(error); */ }
        });
    });
    $("#btn_cerrar_modal_promesas").click(function () {
        $("#ModalPromAutom").removeClass('show');
    });
});
