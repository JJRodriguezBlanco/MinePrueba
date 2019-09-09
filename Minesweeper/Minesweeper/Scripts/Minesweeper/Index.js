var Mine = Mine || {};
Mine.Index = Mine.Index || (function () {
    var settings;
    var selectedElementBanda;
    var selectedBolsa;
    var selectedSubBolsa;
    var isBotonEdit = false;
    var isCargaMasiva = false;
    //var excelControl = $("#adjuntar");
    var uid = "uid";
    var isChangeUtilizacion = false;
    var datosContratosBolsa;
    var datosEditarValidar = {
        Origen: "",
        Cluster: "",
        TipoFichero: ""
    }

    function initSettings() {
        settings = {
    
            fields: {
            },
            forms: {
            
            },
            campos: {
    
            },
            buttons: {
              
            },
            urls: {
            
            }
        };
    }

    function input() {

        var text = $("#TextInput").val();
        var textArea = $("#areaIntput").val();
        var textoInsertar = "";

        if (!isNaN(text) && textArea != "" && text != "00") {
            textoInsertar = textArea + "\n" + text;
        } else {
            textoInsertar = textArea + text;
        }

        
        $("#areaIntput").text(textoInsertar + "\n");
        $("#areaIntput").val(textoInsertar + "\n");
        $("#TextInput").val("");
    }

    function output() {

        var urlCalculo = $("#urlCalculo").val();
        var textArea = $("#areaIntput").val();
        var textSerializado = JSON.stringify({ textArea : textArea });
        //var textSerializado = textArea.serialize();


        $.ajax({
            type: "POST",
            url: urlCalculo,
            contentType: "application/json",
            data: textSerializado,
            success: function (data) {
                //debugger;
                $("#areaOutput").text(data);
                $("#areaOutput").val(data);
            },
            error: function () {
                alert("Error");
            }
        });

    }

    function isNumberKeyEnteros(evt) {
        //Entero con 3 decimales
        var charCode = (evt.which) ? evt.which : event.keyCode;

        if (charCode != 46 && charCode != 42 && charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }

    //-- Común --//
    function initEvents() { }
  

    function init() {
        initSettings();
        initEvents();
    }

    return {
        Init: init,
        Input: input,
        Output: output,
        IsNumberKeyEnteros: isNumberKeyEnteros
    };
})();

$(function () {
    Mine.Index.Init();
    
});



