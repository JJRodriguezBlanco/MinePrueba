var Mine = Mine || {};
Mine.Index = Mine.Index || (function () {
    function initSettings() {
        settings = { 
            fields: {
                textInput: $("#TextInput"),
                areaInput: $("#AreaIntput"),
                areaOutput: $("#AreaOutput")
            },
            urls: {
                urlCalculation: $("#urlCalculation").val()
            }
        };
    }

    function input() {

        var text = settings.fields.textInput.val();
        var textArea = settings.fields.areaInput.val();
        var textoInsert = "";

        if (!isNaN(text) && textArea != "" && text != "00") {
            textoInsert = textArea + "\n" + text;
        } else {
            textoInsert = textArea + text;
        }

        
        settings.fields.areaInput.text(textoInsert + "\n");
        settings.fields.areaInput.val(textoInsert + "\n");
        settings.fields.textInput.val("");

    }

    function output() {

        $.ajax({
            type: "POST",
            url: settings.urls.urlCalculation,
            contentType: "application/json",
            data: JSON.stringify({ textArea: settings.fields.areaInput.val() }),
            success: function (data) {
                if (data.IsOk) {
                    settings.fields.areaOutput.text(data.data);
                    settings.fields.areaOutput.val(data.data);
                } else {
                    bootbox.alert("Error: Check the input parameters, if you have any questions you can check the rules of the game.");
                }
            },
            error: function () {
                bootbox.alert("Error: Check the input parameters, if you have any questions you can check the rules of the game.");
            }
        });

    }

    function clearAreas() {     
        settings.fields.areaInput.val("");
        settings.fields.areaOutput.val("");
    }

    function isNumberKeyEnterosAndAsterisk(evt) {

        var charCode = (evt.which) ? evt.which : event.keyCode;

        if (charCode != 46 && charCode != 42 && charCode > 31 && (charCode < 48 || charCode > 57))
            return false;
        return true;
    }
  
    function init() {
        initSettings();
    }

    return {
        Init: init,
        Input: input,
        Output: output,
        ClearAreas: clearAreas,
        IsNumberKeyEnterosAndAsterisk: isNumberKeyEnterosAndAsterisk
    };
})();

$(function () {
    Mine.Index.Init();
});



