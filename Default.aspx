<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DataHSMforWeb._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="navbar navbar-inverse navbar-fixed-top">
        <div class="container-fluid">
                <div class="navbar-collapse collapse" >
                    <ul class="nav navbar-nav" id="TopUL" style="padding-top:6px;">
                        <li class="li-marg-right">
                            <button type="button" class="btn" onclick="TopMenuClick(this)" id="TopBtn1" style="background-color:wheat; height:38px">
                                <img src="/Resourses/measures.jpg" class="rounded float-start" width="20"/>
                                Измерения
                            </button>
                        </li>
                        <li class="li-marg-right">
                            <button type="button" class="btn" onclick="TopMenuClick(this)" id="TopBtn2" style="height:38px">
                                <img src="/Resourses/Flatness.jpg" class="rounded float-start" width="20" />
                                Статистика
                            </button>
                        </li>
                        <li class="li-marg-right">
                            <button type="button" class="btn" onclick="TopMenuClick(this)" id="TopBtn3" style="height:38px">
                                <img src="/Resourses/179.jpg" class="rounded float-start" width="20"/>
                                Износ валков
                            </button>
                        </li>
                        <li class="li-marg-right">
                            <button type="button" class="btn" onclick="TopMenuClick(this)" id="TopBtn4" style="height:38px"> 
                                <img src="/Resourses/Analyze.png" class="rounded float-start" width="20"/>
                                Факторы влияния
                            </button>
                        </li>
                    </ul>
                </div>
            </div>
    </div>

    <div class="MainCont" id="Cont1">
        <div class="row">
            <div id="Filters1" class="col-lg-3" style="max-width:400px;">
                <fieldset>
                    <legend>Фильтры</legend>
                    <div class="col-md-6 form-group">
                        <asp:Label ID="Label1" runat="server" Text="Начало"></asp:Label>
                        <input id="DateField1_1" type="date" onchange="ChangeFilter_1()"/>
                    </div>
                    <div class="col-md-6 form-group" style="margin-bottom:0px !important;">
                        <asp:Label ID="Label2" runat="server" Text="Конец"></asp:Label>
                        <input id="DateField1_2" type="date" onchange="ChangeFilter_1()"/>
                        <input id="Text3" type="text" style="margin-top:6px; width:100px;"/>
                    </div>
                    <br />
                    <div class="col-md-6 form-group">
                        <asp:Label ID="Label3" runat="server" Text="Марка стали"></asp:Label>
                        <select id="Select1" name="D1" style="width:150px;" onchange="SteelGrAndThicknessFilter()">
                            <option></option>
                        </select>
                    </div>
                    <div class="col-md-6 form-group">
                        <asp:Label ID="Label4" runat="server" Text="Толщина"></asp:Label>
                        <br />
                        <select id="Select2" name="D2" style="width:100px;" onchange="SteelGrAndThicknessFilter()">
                            <option></option>
                        </select>
                        <select id="Select3" name="D3" style="width:150px; margin-top:6px;">
                            <option></option>
                        </select>
                    </div>
                </fieldset>
            </div>

            <div id="RB_Group1" class="col-lg-9" onchange="SwichRBOn_1()">
                <fieldset>
                    <legend>Измерения</legend>
                    <div class="row" id="RB1">
                        <%--Усилие--%>
                        <div class="col-sm-1 rb-group-bg" style="width:130px;">
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_1" checked="checked">
                                <label class="form-check-label" for="RB1_1">
                                    Усилие F1
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_2">
                                <label class="form-check-label" for="RB1_2">
                                    Усилие F2
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_3">
                                <label class="form-check-label" for="RB1_3">
                                    Усилие F3
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_4">
                                <label class="form-check-label" for="RB1_4">
                                    Усилие F4
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_5">
                                <label class="form-check-label" for="RB1_5">
                                    Усилие F5
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_6">
                                <label class="form-check-label" for="RB1_6">
                                    Усилие F6
                                </label>
                            </div>
                        </div>
                        <%--Изгибы--%>
                        <div class="col-sm-1 rb-group-bg" style="width:130px;">
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_7">
                                <label class="form-check-label" for="RB1_7">
                                    Изгиб F1
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_8">
                                <label class="form-check-label" for="RB1_8">
                                    Изгиб F2
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_9">
                                <label class="form-check-label" for="RB1_9">
                                    Изгиб F3
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_10">
                                <label class="form-check-label" for="RB1_10">
                                    Изгиб F4
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_11">
                                <label class="form-check-label" for="RB1_11">
                                    Изгиб F5
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_12">
                                <label class="form-check-label" for="RB1_12">
                                    Изгиб F6
                                </label>
                            </div>
                        </div>
                        <%--Скорость--%>
                        <div class="col-sm-2 rb-group-bg" style="width:150px;">
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_13">
                                <label class="form-check-label" for="RB1_13">
                                    Скорость F1
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_14">
                                <label class="form-check-label" for="RB1_14">
                                    Скорость F2
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_15">
                                <label class="form-check-label" for="RB1_15">
                                    Скорость F3
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_16">
                                <label class="form-check-label" for="RB1_16">
                                    Скорость F4
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_17">
                                <label class="form-check-label" for="RB1_17">
                                    Скорость F5
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_18">
                                <label class="form-check-label" for="RB1_18">
                                    Скорость F6
                                </label>
                            </div>
                        </div>

                        <div class="col-sm-3 rb-group-bg" style="width:300px;">
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_19">
                                <label class="form-check-label" for="RB1_19">
                                    Температура входа в чистовую
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_20">
                                <label class="form-check-label" for="RB1_20">
                                    Температура конца прокатки
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_21">
                                <label class="form-check-label" for="RB1_21">
                                    Температура смотки
                                </label>
                            </div>                            
                        </div>

                        <div class="col-sm-3 rb-group-bg" style="width:250px;">
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_22">
                                <label class="form-check-label" for="RB1_22">
                                    Ширина после черновой
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_23">
                                <label class="form-check-label" for="RB1_23">
                                    Ширина после чистовой
                                </label>
                            </div>                           
                        </div>

                        <div class="col-sm-2 rb-group-bg">
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_24">
                                <label class="form-check-label" for="RB1_24">
                                    Толщина
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB1" id="RB1_25">
                                <label class="form-check-label" for="RB1_25">
                                    Откл. от линии проката
                                </label>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>
        </div>
        <hr style="margin:0 0 6px 0;"/>
        <%--Гриды--%>
        <div class="row">
            <div class="col-md-4" style="padding:3px;">
                <div id="Tb_1_1" class="shadow" style="width:100%; height:340px; margin-bottom:6px; background-color:seashell;"></div>
                <div id="Tb_1_2" class="shadow" style="width:100%; height:340px; margin:6px 0 0 0; background-color:seashell;"></div>
            </div>
            <div class="col-md-8" style="padding:3px;">
                <div id="Chart_1_1" class="shadow" style="width:100%; height:340px; margin-bottom:6px; background-color:seashell; text-align: center;"></div>
                <div id="Chart_1_2" class="shadow" style="width:100%; height:340px; margin:0; background-color:seashell; text-align: center;"></div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-10">
                <div id="Tb_1_3" class="shadow" style="width: 100%; height: 340px; margin: 6px 0 0 0; background-color: seashell;"></div>
            </div>
            <div class="col-md-2">
                <button type="button" class="btn" id="BtnDataCorr" style="width: 200px; height: 50px; margin: 20px;" onclick="GetDataOnCorrelation()">
                    Данные по корреляциям
                </button>
            </div>
        </div>
    </div>

    <div class="MainCont" id="Cont2" style="display: none;">
        <div class="row">
            <div  id="Filters2" style="padding:3px; width:530px;" >
                <div class="col-md-2 form-group" style="padding:3px; width:160px; margin-left:20px; margin-bottom:0;">
                    <input id="DataField_2_1" type="date" style="margin-top:6px; width:150px;"/>
                    <input id="DataField_2_2" type="date" style="margin-top:6px; width:150px;"/>                    
                    <select id="Select4" name="D1" style="margin-top:6px; width:150px;">
                        <option></option>
                    </select>                    
                    <Button type="button" class="btn" id="BtnFilter2" style="margin:6px 0 0 20px;" onclick="GetDataAnalyzeBlobs()">
                        Обработать
                    </Button>
                </div>                
                <div class="col-md-3" style="padding:3px; width:300px;">
                    <fieldset>
                        <legend style="font-size: 12px;">Участок полосы</legend>
                        <div class="rb-group-bg" id="RB8">
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB8" id="RB8_1">
                                <label class="form-check-label" for="RB8_1">
                                    Диапазон
                                </label>
                                <input id="Num_2_1" type="number" style="margin-left: 6px; width: 40px;" min="0"  value="15"/>
                                -
                                <input id="Num_2_2" type="number" style="width: 40px;" min="0" value="90"/>
                                Метров
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB8" id="RB8_2" checked="checked">
                                <label class="form-check-label" for="RB8_2">
                                    Вся длина
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB8" id="RB8_3">
                                <label class="form-check-label" for="RB8_3">
                                    Хвостовая часть
                                </label>
                                <input id="Num_2_3" type="number" min="0" style="margin-left: 6px; width: 40px;" value="90"/>
                                Метров
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
            <div class="col-md-7" style="padding:3px; width:300px;">
                <button type="button" class="btn" id="BtnFilter3" style="margin: 40px 0 0 20px; height: 50px; width: 200px;" onclick="GetExcelAnalyzeBlobs()">
                    Экспорт в Excel
                </button>
            </div>   
        </div>
        
        <div class="row" style="padding:3px;">
            <div class="col-lg-12" style="padding:3px;">
                <div id="Tb_2_1" class="table" style="width:100%; height:755px; background-color:seashell; margin:0px"></div>
            </div>
        </div>
    </div>

    <div class="MainCont" id="Cont3" style="display: none;">
        <div class="row">
            <div class="float-left" style="width:350px; padding:30px 0 0 10px;">
                <div class="row">                    
                    <div class="col-md-6 form-group" style="width:330px; text-align:center;">
                        <asp:Label ID="Label5" runat="server" Text="Выбор даты"></asp:Label>
                        <input id="DateField_3_1" onchange="GetRollID()" type="date" style="margin-left:5px;"/>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6 form-group" style="width:330px; text-align:center;">
                        <asp:Label ID="Label6" runat="server" Text="Номер валка"></asp:Label>
                        <select id="Select5" name="D3" style="width:150px; margin-top:6px;">
                            <option></option>
                        </select>
                    </div>
                </div>
                <div class="row" style="text-align:center;">
                    <button type="button" class="btn" id="BtnFilter_2_1" onclick="BuildGraphRoll()" style="width:200px; height:50px;">
                        Построить график
                    </button>
                </div>
            </div>
            <div class="float-left" style="padding: 3px; width: 130px;">
                <fieldset>
                    <legend style="font-size: 12px;">Выбор измерения</legend>
                    <div class="rb-group-bg" id="RB9" style="padding-left:20px;" onchange="GetRollID()">
                        <div class="form-check">
                            <input class="form-check-input" type="radio" name="RB9" id="RB9_1" value ="F1">
                            <label class="form-check-label" for="RB9_1">
                                F1
                            </label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="radio" name="RB9" id="RB9_2" value ="F2">
                            <label class="form-check-label" for="RB9_2">
                                F2
                            </label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="radio" name="RB9" id="RB9_3" value ="F3">
                            <label class="form-check-label" for="RB9_3">
                                F3
                            </label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="radio" name="RB9" id="RB9_4" value ="F4">
                            <label class="form-check-label" for="RB9_4">
                                F4
                            </label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="radio" name="RB9" id="RB9_5" value ="F5">
                            <label class="form-check-label" for="RB9_5">
                                F5
                            </label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="radio" name="RB9" id="RB9_6" value ="F6">
                            <label class="form-check-label" for="RB9_6">
                                F6
                            </label>
                        </div>
                    </div>
                </fieldset>
            </div>
            <div class="col-md-8" style="padding:3px;">
                <fieldset>
                    <legend style="font-size: 12px;">Характеристики</legend>
                    <div style="padding-left:30px;">                        
                        <div class="row" style="height:35px;">                            
                            <div class="span-width-250">Прокатанная длина(км)</div>
                            <input id="Number_3_2" type="number" readonly style="width: 100px;"/>
                        </div>
                        <div class="row" style="height:35px;">
                            <div class="span-width-250">Прокатный вес(тн)</div>
                            <input id="Number_3_3" type="number" readonly style="width: 100px;"/>
                        </div>
                        <div class="row" style="height:35px;">
                            <div class="span-width-250">Диаметр валка(мм)</div>                            
                            <input id="Number_3_4" type="number" readonly style="width: 100px;"/>
                        </div>
                        <div class="row" style="height:35px;">
                            <div class="span-width-250">Минимальное значения(микроны)</div>                            
                            <input id="Number_3_5" type="number" readonly style="width: 100px;"/>
                        </div>
                    </div>
                </fieldset>
            </div>
        </div>        
        <hr style="margin:0 0 6px 0;"/>
        <div id="Chart_3_1" class="shadow" style="margin-left:15%; width:70%; height:550px;"></div>
    </div>

    <div class="MainCont" id="Cont4" style="display: none;">
        <div class="row">
            <div id="Filters4" class="col-lg-3" style="max-width: 350px;">
                <fieldset>
                    <legend>Фильтры</legend>
                    <div class="row">
                        <div class="col-md-6 form-group">
                            <asp:Label ID="Label7" runat="server" Text="Начало"></asp:Label>
                            <input id="DateField_4_1" type="date" onchange="GetDataSteel_4()"/>
                        </div>
                        <div class="col-md-6 form-group" style="margin-bottom: 0px !important;">
                            <asp:Label ID="Label8" runat="server" Text="Конец"></asp:Label>
                            <input id="DateField_4_2" type="date" onchange="GetDataSteel_4()"/>
                        </div>
                    </div>
                    <div class="row" style="padding-left:20px; margin-bottom:6px;">
                        <asp:Label ID="Label10" runat="server" Text="Количество не менее"></asp:Label>
                        <input id="Num_4_1" type="number" style="margin-top:6px; width:50px;" min="1" value ="1"/>
                    </div>
                </fieldset>
            </div>

            <div id="Filters5" class="col-lg-9">
                <fieldset>
                    <legend>Критерии</legend>
                    <div class="col-lg-6" style="max-width:480px;" id="RB10">
                        <div class="rb-group-bg" style="padding: 30px;">
                            <div class="form-check" style="margin-bottom: 30px;">
                                <input class="form-check-input" type="radio" name="RB10" id="RB10_1" checked="checked">
                                <label class="form-check-label" for="RB10_1">
                                    Процент попадания в температуру смотки
                                </label>
                            </div>
                            <div class="form-check">
                                <input class="form-check-input" type="radio" name="RB10" id="RB10_2">
                                <label class="form-check-label" for="RB10_2">
                                    Процент попадания в температуру конца прокатки
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-6 rb-group-bg" style="padding:20px; margin-top:30px; width:350px;">
                        <div class="row" style="padding-left:20px;">
                            Bad <
                            <input type="number" id="Num_4_3" value="80" min="0" style="width:50px;"/>
                            <= Normal <=
                            <input type="number" id="Num_4_4" value="90" min="0" style="width:50px;"/>
                            < Good
                        </div>
                    </div>
                    <div class="col-lg-6" style="padding:20px; margin-top:15px; width:350px;">
                        <div class="row" style="padding-left: 20px;">
                            <button type="button" class="btn" id="BtnFilter_4_1" style="width: 200px; height: 50px;" onclick="GetDataTable_4()">
                                Выполнить
                            </button>
                        </div>
                    </div>
                </fieldset>
            </div>
        </div>        
        <div class="row">
            <div class="col-lg-3" style="max-width:350px; margin-top:-40px;">                
                <fieldset style="margin-top:2px;">
                    <legend>Марки стали</legend>
                    <div id="Tb_4_2" class="shadow" style="width: 100%; height: 345px; background-color: seashell; 
                        margin: 0px;">
                    </div>
                </fieldset>

                <fieldset>
                    <legend>Группировка</legend>
                    <div id="Tb_4_1" class="shadow" style="width: 100%; height: 345px; background-color: seashell; 
                        margin: 0px;">
                    </div>
                </fieldset>
            </div>
            <div class="col-lg-9">                
                <div id="Tb_4_3" class="shadow" style="width:100%; height:720px; background-color:seashell;
                    margin:6px;">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
