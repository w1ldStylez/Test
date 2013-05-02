<%@ Control Language="VB" Inherits="System.Web.Mvc.ViewUserControl" %>
<%-- Die folgende Zeile vermeidet eine ASP.NET Compiler-Warnung --%>
<%: ""%>
<%
    If Request.IsAuthenticated Then
    %>
        Willkommen <b><%: Page.User.Identity.Name %></b>!
        [ <%: Html.ActionLink("Abmelden", "LogOff", "Account")%> ]
    <%
    Else
    %>
        [ <%: Html.ActionLink("Anmelden", "LogOn", "Account")%> ]
    <%        
    End If
%>