<%@ Page Language="VB" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage(Of tesr.RegisterModel)" %>

<asp:Content ID="registerTitle" ContentPlaceHolderID="TitleContent" runat="server"> Registrieren </asp:Content>

<asp:Content ID="registerContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Erstellen eines neuen Kontos</h2>
    <p>
        Verwenden Sie zum Erstellen eines neuen Kontos das Formular unten. 
    </p>
    <p>
        Kennwörter müssen mindestens <%: ViewData("PasswordLength") %> Zeichen lang sein.
    </p>

    <% Using Html.BeginForm() %>
        <%: Html.ValidationSummary(True, "Die Kontoerstellung war nicht erfolgreich. Korrigieren Sie die Fehler, und versuchen Sie es erneut.")%>
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
                    <%: Html.LabelFor(Function(m) m.Email) %>
                </div>
                <div class="editor-field">
                    <%: Html.TextBoxFor(Function(m) m.Email) %>
                    <%: Html.ValidationMessageFor(Function(m) m.Email) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(Function(m) m.Password) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(Function(m) m.Password) %>
                    <%: Html.ValidationMessageFor(Function(m) m.Password) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(Function(m) m.ConfirmPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(Function(m) m.ConfirmPassword) %>
                    <%: Html.ValidationMessageFor(Function(m) m.ConfirmPassword) %>
                </div>
                
                <p>
                    <input type="submit" value="Registrieren" />
                </p>
            </fieldset>
        </div>
    <% End Using %>
</asp:Content>

