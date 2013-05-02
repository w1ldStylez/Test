<%@ Page Language="VB" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage(Of tesr.ChangePasswordModel)" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server"> Ändern des Kennworts </asp:Content>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Ändern des Kennworts</h2>
    <p>
        Verwenden Sie zum Ändern des Kennworts das Formular unten. 
    </p>
    <p>
        Neue Kennwörter müssen mindestens <%: ViewData("PasswordLength") %> Zeichen lang sein.
    </p>

    <% Using Html.BeginForm() %>
        <%: Html.ValidationSummary(True, "Die Kennwortänderung war nicht erfolgreich. Korrigieren Sie die Fehler, und versuchen Sie es erneut.")%>
        <div>
            <fieldset>
                <legend>Kontoinformationen</legend>
                
                <div class="editor-label">
                    <%: Html.LabelFor(Function(m) m.OldPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(Function(m) m.OldPassword) %>
                    <%: Html.ValidationMessageFor(Function(m) m.OldPassword) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(Function(m) m.NewPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(Function(m) m.NewPassword) %>
                    <%: Html.ValidationMessageFor(Function(m) m.NewPassword) %>
                </div>
                
                <div class="editor-label">
                    <%: Html.LabelFor(Function(m) m.ConfirmPassword) %>
                </div>
                <div class="editor-field">
                    <%: Html.PasswordFor(Function(m) m.ConfirmPassword) %>
                    <%: Html.ValidationMessageFor(Function(m) m.ConfirmPassword) %>
                </div>
                
                <p>
                    <input type="submit" value="Ändern des Kennworts" />
                </p>
            </fieldset>
        </div>
    <% End Using %>
</asp:Content>

