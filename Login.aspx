<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DataHSMforWeb.Autentification" %>

<asp:Content ID="AuthContent_" ContentPlaceHolderID="AuthContent" runat="server">
    <div style="width:100%; height:100%;">
        <div class="AuthForm shadow" style="background-color:#7777772e !important; position: relative; margin:auto; text-align:center; border-radius:20px; background-color: antiquewhite;
            padding-top:20px;">            
<%--            <span style="color:black; font-size:40px; text-shadow: 0px 5px 20px black;">Форма входа</span>  
            
            <div class="row" style="height:35px; margin-top:20px;">                           
                <span style="color:#fff5ed; font-size:24px; text-shadow: 0px 3px 6px black; width:70px; margin-left:70px; float:left;">Логин:</span> 
                <input id="Number_3_2" type="text" style="width: 300px; font-size: 24px; border-radius:6px; padding-left:10px;"/>
            </div>
            <div class="row" style="height:35px; margin-top:20px;">
                <span style="color:#fff5ed; font-size:24px; text-shadow: 0px 3px 6px black; width:70px; margin-left:70px; float:left;">Пароль:</span> 

                <input id="Number_3_3" type="password" style="width: 300px; font-size: 24px; border-radius:6px; padding-left:10px;"/>
            </div>            

            <button type="button" class="btn" onclick="Enter()" id="TopBtn2" style="height:50px; width:200px; margin-top: 20px; border:none;">                
                <span style="color:#3d3d3d; font-size:24px; ">Войти</span>
            </button>  --%>    
            <h1>Login</h1>
            <p>
                Username:
                <asp:TextBox ID="UserName" runat="server"></asp:TextBox></p>
            <p>
                Password:
                <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox></p>
            <p>
                <asp:CheckBox ID="RememberMe" runat="server" Text="Remember Me" /> </p>
            <p>
                <asp:Button ID="LoginButton" runat="server" Text="Login" OnClick="TryLogin" /> </p>
        </div>
    </div>
</asp:Content>
