<%@ Page Language="VB" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage(Of tesr.LogOnModel)" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server"> Anmelden </asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Anmelden</h2>
    <p>
        Geben Sie den Benutzernamen und das Kennwort ein. <%: Html.ActionLink("Registrieren Sie sich", "Register") %>, falls Sie noch kein Konto besitzen.
    </p>

    <% Using Html.BeginForm() %>
        <%: Html.ValidationSummary(True, "Die Anmeldung war nicht erfolgreich. Korrigieren Sie die Fehler, und versuchen Sie es erneut.")%>
        <div>
            <fieldset>
                <legend>Kontoinformationen</legend>
                
                <div class="editor-label">
                    <%: Html.LabelFor(Function(m) m.UserName) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(Function(m) m.UserName) %>
                    <%: Html.ValidationMessageFor(Function(m) m.UserName) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(Function(m) m.Password) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(Function(m) m.Password) %>
                    <%: Html.ValidationMessageFor(Function(m) m.Password) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.CheckBoxFor(Function(m) m.RememberMe) %>
                    <%: Html.LabelFor(Function(m) m.RememberMe) %>
                </div>
                <p>
                    <input type="submit" value="Anmelden" />
                </p>
            </fieldset>
        </div>
    <% End Using %>
</asp:Content>

