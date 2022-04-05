window.onload = function () {
    SetCurrentDate("DateField1_1");
    SetCurrentDate("DateField1_2");

    SetCurrentDate("DataField_2_1");
    SetCurrentDate("DataField_2_2");

    SetCurrentDate("DateField_3_1");

    SetCurrentDate("DateField_4_1");
    SetCurrentDate("DateField_4_2");

    ChangeFilter_1();

    GetColNames();

    $(document).keydown(function (e) {        
        ArrowMoveSelectionOnGrid(e);
    });
}
var PieceId_1_1;
var PieceId_1_2;

var ColNames_4 = [];
var SteelNames_4 = [];
var tableColomnsNames;
var tableSteelGr;
var CurrentGrid;
var table1;
var table2;
//-------------------------- 1

function ChangeFilter_1() {
    StartSpiner("Tb_1_1");
    StartSpiner("Tb_1_2");
    var dateFrom = document.getElementById("DateField1_1").value;
    var dateTo = document.getElementById("DateField1_2").value;
    var q = JSON.stringify({ DateFrom: dateFrom, DateTo: dateTo });
    if (dateFrom != "" && dateTo != "") {
        $.ajax({
            type: "POST",
            url: "/WebService1.asmx/GetFirstTabData",
            data: q,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (jObj) {
                StopSpiner("Tb_1_1");
                StopSpiner("Tb_1_2");
                var a = jQuery.parseJSON(jObj.d);
                CheckError(a);
                GridsUpdate_1(a);
            },
            failure: function (response) {
                StopSpiner("Tb_1_1");
                StopSpiner("Tb_1_2");
                alert(response.d);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                StopSpiner("Tb_1_1");
                StopSpiner("Tb_1_2");
                //alert(textStatus);
                //alert(errorThrown);
            }
        });
    }
}

function GridsUpdate_1(json) {
    AddSelectOp("Select1", json.steel_gr);
    AddSelectOp("Select2", json.thick);
    table1 = FillGrid("#Tb_1_1", json.GridsData, GetDataForChart_1, { layout: "fitDataStretch" });
    table2 = FillGrid("#Tb_1_2", json.GridsData, GetDataForChart_1, { layout: "fitDataStretch" });
}

function GridsUpdate_2(json) {
    FillGrid("#Tb_1_3", json.GridsData, function () { }, { layout: "fitDataStretch" });
}

function SwichRBOn_1() {
    if (PieceId_1_1 != undefined) {
        GetDataForChart_1("#Tb_1_1", { PIECE_ID: PieceId_1_1 });
    }
    if (PieceId_1_2 != undefined) {
        GetDataForChart_1("#Tb_1_2", { PIECE_ID: PieceId_1_2 });
    }
}

function GetDataForChart_1(nameTable, row) {
    var piece_id = row.PIECE_ID;
    if (piece_id != undefined) {
        var tn = nameTable.substr(6);
        if (tn == "1") {
            PieceId_1_1 = piece_id;
        }
        else if (tn == "2") {
            PieceId_1_2 = piece_id;
        }
    }
    var chekedRB = CheckRBGroupAnyNesting("RB1").id.substr(4);
    var q = JSON.stringify({ Device: chekedRB, Piece_id: piece_id });
    $.ajax({
        async: false,
        type: "POST",
        url: "/WebService1.asmx/GetDataForChart",
        data: q,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (jObj) {
            var a = JSON.parse(jObj.d);
            //CheckError(a);
            ChartFill_1(nameTable, a);
        },
        failure: function (response) {
            alert(response.d);
        }
    });
}

function ChartFill_1(nameTable, json) {
    ChartSetEmpty(nameTable);
    if (json.comboBox3 != undefined) {
        AddSelectOp("Select3", json.comboBox3);
    }
    if (json.offset == undefined) {
        NoDataChart(nameTable);
        return;
    }
    var limit = json.offset.length;
    var data = [];
    var dataSeries = { type: "line" };
    var dataPoints = [];
    if (json.kudr_coeff != undefined) {
        for (var i = 0; i < limit; i += 1) {
            dataPoints.push({
                x: json.offset[i],
                y: json.measure[i] * json.kudr_coeff
            });
        }
    } else {
        for (var i = 0; i < limit; i += 1) {
            dataPoints.push({
                x: json.offset[i],
                y: json.measure[i]
            });
        }
    }

    dataSeries.dataPoints = dataPoints;
    data.push(dataSeries);
    var options = {
        zoomEnabled: false,
        animationEnabled: false,
        axisY: {
            valueFormatString: "#######",
        },
        data: data,
        lineThickness: 3
    };
    if (json.negtol != undefined && json.postol != undefined && json.target != undefined) {
        options.axisY.minimum = json.target - json.postol * 2;
        options.axisY.maximum = json.target + json.negtol * 2;
        options.axisY.stripLines = [{
            value: json.target - json.negtol,
            showOnTop: false,
            color: "#FF0800",
            thickness: 2
        },
        {
            value: json.target + json.postol,
            showOnTop: false,
            color: "#FF0800",
            thickness: 2
        },
        {
            value: json.target,
            showOnTop: false,
            color: "#00ff21",
            thickness: 2
        }]
    }

    var numRBChecked = parseFloat(CheckRBGroupAnyNesting("RB1").id.substr(4));
    if (numRBChecked > 12 && numRBChecked < 19 || 24) {
        options.axisY.valueFormatString = "##.##";
    }
    var nameChart = "Chart" + nameTable.substr(3);

    var chart = new CanvasJS.Chart(nameChart, options);
    chart.render();
}

function GetDataOnCorrelation() {
    StartSpiner("Tb_1_3");
    var dateFrom = document.getElementById("DateField1_1").value;
    var dateTo = document.getElementById("DateField1_2").value;
    var q = JSON.stringify({ DateFrom: dateFrom, DateTo: dateTo });
    if (dateFrom != "" && dateTo != "") {
        $.ajax({
            type: "POST",
            url: "/WebService1.asmx/GetDataOnCorrelation",
            data: q,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (jObj) {
                StopSpiner("Tb_1_3");
                var a = jQuery.parseJSON(jObj.d);
                CheckError(a);
                GridsUpdate_2(a);
            },
            failure: function (response) {
                StopSpiner("Tb_1_3");
                alert(response.d);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                StopSpiner("Tb_1_3");
                //alert(textStatus);
                //alert(errorThrown);
            },
            //async: false
        });
    }
}

function SteelGrAndThicknessFilter() {
    var Steel = GetValueSelector("Select1");
    var Thickness = parseFloat(GetValueSelector("Select2").replace(/,/, '.'));
    //TARGET_THICK
    //STEEL_GRADE
    var filt = [];
    if (Steel != undefined && Steel != "") {
        filt.push({ field: "STEEL_GRADE", type: "=", value: Steel });
    }
    if (!isNaN(Thickness)) {
        filt.push({ field: "TARGET_THICK", type: "=", value: Thickness });
    }
    table1.setFilter(filt);
    table2.setFilter(filt);
}

//-------------------------- 2

function GetDataAnalyzeBlobs() {
    StartSpiner("Tb_2_1");

    var chekedRB = CheckRBGroupAnyNesting("RB8").id.substr(4);

    var minRange = document.getElementById("Num_2_1").value;
    var maxRange = document.getElementById("Num_2_2").value;
    var tail     = document.getElementById("Num_2_3").value;

    var dateFrom = document.getElementById("DataField_2_1").value;
    var dateTo = document.getElementById("DataField_2_2").value;
    var q = JSON.stringify({ DateFrom: dateFrom, DateTo: dateTo, MinRange: minRange, MaxRange: maxRange, Tail: tail, ChekedRB: chekedRB});
    if (dateFrom != "" && dateTo != "") {
        $.ajax({
            type: "POST",
            url: "/WebService1.asmx/GetDataAnalyzeBlobs",
            data: q,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (jObj) {
                StopSpiner("Tb_2_1");
                var a = jQuery.parseJSON(jObj.d);
                GridsUpdate_3(a);
            },
            failure: function (response) {
                StopSpiner("Tb_2_1");
                alert(response.d);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                StopSpiner("Tb_2_1");
                //alert(textStatus);
                //alert(errorThrown);
            },
        });
    }
}

function GetExcelAnalyzeBlobs() {
    StartSpiner("Cont2")
    var chekedRB = CheckRBGroupAnyNesting("RB8").id.substr(4);

    var minRange = document.getElementById("Num_2_1").value;
    var maxRange = document.getElementById("Num_2_2").value;
    var tail = document.getElementById("Num_2_3").value;

    var dateFrom = document.getElementById("DataField_2_1").value;
    var dateTo = document.getElementById("DataField_2_2").value;
    var q = JSON.stringify({ DateFrom: dateFrom, DateTo: dateTo, MinRange: minRange, MaxRange: maxRange, Tail: tail, ChekedRB: chekedRB });
    if (dateFrom != "" && dateTo != "") {
        $.ajax({
            type: "POST",
            //url: "/WebService1.asmx/GetExcelAnalyzeBlobs",
            url: "/WebService1.asmx/GetCSVAnalyzeBlobs",
            data: q,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (jObj) {
                StopSpiner("Cont2");
                var a = jQuery.parseJSON(jObj.d);
                CheckError(a);
                LoadFile(a);
            },
            failure: function (response) {
                StopSpiner("Cont2");
                alert(response.d);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                StopSpiner("Cont2");
                //alert(textStatus);
                //alert(errorThrown);
            },
        });
    }
}

function GridsUpdate_3(json) {
    FillGrid("#Tb_2_1", json.GridsData, function () { }, { layout: "fitColumns" });
}

//-------------------------- 3

function BuildGraphRoll() {
    var date = document.getElementById("DateField_3_1").value;
    var loc = CheckRBGroupAnyNesting("RB9").value;
    var rollId = GetValueSelector("Select5");
    var q = JSON.stringify({ DateObj: date, Location: loc, RollId: rollId });
    $.ajax({
        type: "POST",
        url: "/WebService1.asmx/GetRollGraph",
        data: q,
        contentType: "application/json; charset=utf-8",        
        dataType: "json",
        success: function (jObj) {
            Cart_3_render(jObj);
        },
        failure: function (response) {
            alert(response.d);
        }
    });
}

function Cart_3_render(j) {
    var json = JSON.parse(j.d);
    if (json.offset == undefined || json.wear == undefined) {
        alert("Данных для построения графика нет. Попробуйте выбрать другую клеть или другой номер валка.");
    } else {
        document.getElementById("Number_3_2").value = parseFloat(json.TextBoxValues.ArrTextValues[0].replace(/,/, '.'));
        document.getElementById("Number_3_3").value = parseFloat(json.TextBoxValues.ArrTextValues[1].replace(/,/, '.'));
        document.getElementById("Number_3_4").value = parseFloat(json.TextBoxValues.ArrTextValues[2].replace(/,/, '.'));
        document.getElementById("Number_3_5").value = json.a;
        var limit = json.offset.length;
        var data = [];
        var dataSeries = { type: "line" };
        var dataPoints = [];
        for (var i = 0; i < limit; i += 1) {
            dataPoints.push({
                x: json.offset[i],
                y: json.wear[i]
            });
        }
        dataSeries.dataPoints = dataPoints;
        data.push(dataSeries);
        var options = {
            zoomEnabled: false,
            animationEnabled: false,
            title: {
                text: "Wear on Diameter(μm)"
            },
            axisY: {
                lineThickness: 2
            },
            data: data
        };
        var chart = new CanvasJS.Chart("Chart_3_1", options);
        chart.render();
    }
}

function GetRollID() {
    var loc = CheckRBGroupAnyNesting("RB9").value;
    var date = document.getElementById("DateField_3_1").value;
    var q = JSON.stringify({ DateObj: date, Location: loc });
    if (date != "" && loc != "") {
        $.ajax({
            type: "POST",
            url: "/WebService1.asmx/GetRollID",
            data: q,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (jObj) {
                var a = JSON.parse(jObj.d);
                CheckError(a);
                AddSelectOp("Select5", a.Arr);
            },
            failure: function (response) {
                alert(response.d);
            }
        });
    }
}

//-------------------------- 4

function GetColNames() {
    $.ajax({
        type: "POST",
        url: "/WebService1.asmx/GetColNames",
        data: {},
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (jObj) {
            var a = jQuery.parseJSON(jObj.d);
            tableColomnsNames = FillGrid("#Tb_4_1", a.GridsData, function () { }, {
                layout: "fitColumns",
                selectable: true,
                rowClick: SwichSelectedRow  
            });
        },
        failure: function (response) {
            alert(response.d);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            //alert(textStatus);
            //alert(errorThrown);
        },
    });    
}

function SwichSelectedRow() {           
    var selectedRows = tableColomnsNames.getSelectedRows();
    ColNames_4 = [];
    for (var i = 0; i < selectedRows.length; i++) {
        ColNames_4[i] = selectedRows[i]._row.data.COLUMN_NAME;
    }        
}

function SwichSelectedRow2() {
    var selectedRows = tableSteelGr.getSelectedRows();
    SteelNames_4 = [];
    for (var i = 0; i < selectedRows.length; i++) {
        SteelNames_4[i] = selectedRows[i]._row.data.STEEL_GRADE;
    }
}

function GetDataSteel_4() {
    SteelNames_4 = [];
    var dateFrom = document.getElementById("DateField_4_1").value;
    var dateTo = document.getElementById("DateField_4_2").value;
    var q = JSON.stringify({ DateFrom: dateFrom, DateTo: dateTo });
    if (dateFrom != "" && dateTo != "") {
        $.ajax({
            type: "POST",
            url: "/WebService1.asmx/GetData_4",
            data: q,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (jObj) {
                var a = jQuery.parseJSON(jObj.d);
                //AddSelectOp("Select_4_1", a.steel_gr);
                tableSteelGr = FillGrid("#Tb_4_2", a.GridsData, function () { }, {
                    layout: "fitColumns",
                    selectable: true,
                    rowClick: SwichSelectedRow2
                });
            },
            failure: function (response) {
                alert(response.d);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //alert(textStatus);
                //alert(errorThrown);
            },
        });
    }
}

function GetDataTable_4() {
    StartSpiner("Tb_4_3");
    var numRBChecked = parseFloat(CheckRBGroupAnyNesting("RB10").id.substr(5));
    var count = document.getElementById("Num_4_1").value;
    var min = document.getElementById("Num_4_3").value;
    var max = document.getElementById("Num_4_4").value;

    var dateFrom = document.getElementById("DateField_4_1").value;
    var dateTo = document.getElementById("DateField_4_2").value;
    var q = JSON.stringify({ DateFrom: dateFrom, DateTo: dateTo, min: min, max: max, st_gradeArr: SteelNames_4, count: count, arr: ColNames_4, checkedRB: numRBChecked});
    if (dateFrom != "" && dateTo != "") {
        $.ajax({
            type: "POST",
            url: "/WebService1.asmx/GetDataTable_4",
            data: q,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (jObj) {
                StopSpiner("Tb_4_3");
                var a = jQuery.parseJSON(jObj.d);
                CheckError(a);
                FillGrid("#Tb_4_3", a.GridsData, function () { }, { layout: "fitColumns" });
            },
            failure: function (response) {
                StopSpiner("Tb_4_3");
                alert(response.d);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                //alert(textStatus);
                //alert(errorThrown);
            },
        });
    }
}

//--------------------------

//----------------Вспомогательные функции--------------------//-----------------------------------//----------------------------------------//--------------------------------------//

function CheckRBGroupAnyNesting(NameGroup) {
    var inputs = $('#' + NameGroup).find("INPUT");
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].checked) {
            return inputs[i];
        }
    }
}

function AddSelectOp(nameSelect, arr) {
    $('#' + nameSelect + ' option').remove();
    for (var i = 0; i < arr.length; i++) {
        $('#' + nameSelect).append('<option value="">' + arr[i] + '</option>');
    }
}

function GetValueSelector(nameSelect) {
    return $('#' + nameSelect + ' option:selected').text();
}

function CheckError(json) {
    if (json.error != "" && json.error != undefined) {
        alert(json.error);
    }
}

function FillGrid(nameGrd, data, fn, params) {
    if (data.length == 0) {
        return false;
    }
    var options = {
        data: data,
        autoColumns: true,
        selectable: 1,
        keybindings: false,
        rowSelected: function (row) {
            fn(nameGrd, row._row.data);
            SwichCurrentGrid(row);
        }
    }
    if (params != undefined) {
        options = Object.assign(options, params);
    }
    var table = new Tabulator(nameGrd, options);
    return table;    
}

function SetCurrentDate(nameDatePicker) {
    document.getElementById(nameDatePicker).valueAsDate = new Date();
}

function TopMenuClick(a) {
    var parentUL = document.getElementById("TopUL");
    for (var i = 0; i < parentUL.children.length; i++) {
        var li = parentUL.children[i];
        for (var j = 0; j < li.children.length; j++) {
            if (li.children[j].tagName == "BUTTON") {
                li.children[j].style.backgroundColor = "#fcffe2";
            }
        }
    }
    document.getElementById(a.id).style.backgroundColor = "wheat";
    var lastN = a.id.toString().slice(-1);
    var El = document.getElementsByClassName("MainCont");
    for (var i = 0; i < El.length; i++) {
        if (El[i].id == "Cont" + lastN) {
            El[i].style.display = "block";
            for (var j = 0; j < El[i].children.length; j++) {
                var child = El[i].children[j];
                if (child.tagName == 'DIV') {
                    child.style.display = "block !important";
                }
            }
        } else {
            El[i].style.display = "none";
        }
    }
}

function LoadFile(r) {
    //var FileName = json.FileName + ".xlsx";
    var bytes = Base64ToBytes(r.File);

    //Convert Byte Array to BLOB.
    var blob = new Blob([bytes], { type: "application/octetstream" });

    //Check the Browser type and download the File.
    var isIE = false || !!document.documentMode;
    if (isIE) {
        window.navigator.msSaveBlob(blob, r.FileName);
    } else {
        var url = window.URL || window.webkitURL;
        link = url.createObjectURL(blob);
        var a = $("<a />");
        a.attr("download", r.FileName);
        a.attr("href", link);
        $("body").append(a);
        a[0].click();
        $("body").remove(a);
    }
}

function Base64ToBytes(base64) {
    var s = window.atob(base64);
    var bytes = new Uint8Array(s.length);
    for (var i = 0; i < s.length; i++) {
        bytes[i] = s.charCodeAt(i);
    }
    return bytes;
}

function StartSpiner(Parent) {
    StopSpiner(Parent);
    var spiner = $("#" + Parent).append('<div class="loader"></div>')
    return spiner;
}

function StopSpiner(Parent) {
    var DIVS = $('#' + Parent).find("DIV");
    for (var i = 0; i < DIVS.length; i++) {
        if (DIVS[i].className =="loader") {
            DIVS[i].remove();
        }
    }
}

function SwichCurrentGrid(row) {
    CurrentGrid = row._row.parent.table;
}

function ArrowMoveSelectionOnGrid(e) {
    var kk = e.keyCode;
    //87 или 38 вверх
    //83 или 40 вниз
    if (CurrentGrid != undefined) {
        if (kk == 87 || kk == 38) {
            var nextRow = CurrentGrid.getSelectedRows()[0].getPrevRow();
        }
        else if (kk == 83 || kk == 40)
        {
            var nextRow = CurrentGrid.getSelectedRows()[0].getNextRow();
        }
        if (nextRow != false && nextRow != undefined) {
            CurrentGrid.deselectRow(CurrentGrid.getSelectedRows()[0])
            CurrentGrid.selectRow(nextRow);
            CurrentGrid.scrollToRow(CurrentGrid.getSelectedRows()[0], "center", true);
        }
    }
}

function NoDataChart(Tb) {
    var Parent;
    if (Tb == "#Tb_1_1") {
        Parent = "Chart_1_1";
    } else {
        Parent = "Chart_1_2";
    }
    var chart = $("#" + Parent).append('<span class="NoDataChart">Нет данных для графика</span>')
    return chart;
}

function ChartSetEmpty(Tb) {
    var Parent;
    if (Tb == "#Tb_1_1") {
        Parent = "Chart_1_1";
    } else {
        Parent = "Chart_1_2";
    }
    $('#' + Parent).empty();
}
