Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Globalization

#Region "Models"
<PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage:="Das neue Kennwort entspricht nicht dem Bestätigungskennwort.")> _
Public Class ChangePasswordModel
    Private oldPasswordValue As String
    Private newPasswordValue As String
    Private confirmPasswordValue As String

    <Required()> _
    <DataType(DataType.Password)> _
    <DisplayName("Aktuelles Kennwort")> _
    Public Property OldPassword() As String
        Get
            Return oldPasswordValue
        End Get
        Set(ByVal value As String)
            oldPasswordValue = value
        End Set
    End Property

    <Required()> _
    <ValidatePasswordLength()> _
    <DataType(DataType.Password)> _
    <DisplayName("Neues Kennwort")> _
    Public Property NewPassword() As String
        Get
            Return newPasswordValue
        End Get
        Set(ByVal value As String)
            newPasswordValue = value
        End Set
    End Property

    <Required()> _
    <DataType(DataType.Password)> _
    <DisplayName("Neues Kennwort bestätigen")> _
    Public Property ConfirmPassword() As String
        Get
            Return confirmPasswordValue
        End Get
        Set(ByVal value As String)
            confirmPasswordValue = value
        End Set
    End Property
End Class

Public Class LogOnModel
    Private userNameValue As String
    Private passwordValue As String
    Private rememberMeValue As Boolean

    <Required()> _
    <DisplayName("Benutzername")> _
    Public Property UserName() As String
        Get
            Return userNameValue
        End Get
        Set(ByVal value As String)
            userNameValue = value
        End Set
    End Property

    <Required()> _
    <DataType(DataType.Password)> _
    <DisplayName("Kennwort")> _
    Public Property Password() As String
        Get
            Return passwordValue
        End Get
        Set(ByVal value As String)
            passwordValue = value
        End Set
    End Property

    <DisplayName("Speichern?")> _
    Public Property RememberMe() As Boolean
        Get
            Return rememberMeValue
        End Get
        Set(ByVal value As Boolean)
            rememberMeValue = value
        End Set
    End Property
End Class

<PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage:="Das Kennwort entspricht nicht dem Bestätigungskennwort.")> _
Public Class RegisterModel
    Private userNameValue As String
    Private passwordValue As String
    Private confirmPasswordValue As String
    Private emailValue As String

    <Required()> _
    <DisplayName("Benutzername")> _
    Public Property UserName() As String
        Get
            Return userNameValue
        End Get
        Set(ByVal value As String)
            userNameValue = value
        End Set
    End Property

    <Required()> _
    <DataType(DataType.EmailAddress)> _
    <DisplayName("E-Mail-Adresse")> _
    Public Property Email() As String
        Get
            Return emailValue
        End Get
        Set(ByVal value As String)
            emailValue = value
        End Set
    End Property

    <Required()> _
    <ValidatePasswordLength()> _
    <DataType(DataType.Password)> _
    <DisplayName("Kennwort")> _
    Public Property Password() As String
        Get
            Return passwordValue
        End Get
        Set(ByVal value As String)
            passwordValue = value
        End Set
    End Property

    <Required()> _
    <DataType(DataType.Password)> _
    <DisplayName("Kennwort bestätigen")> _
    Public Property ConfirmPassword() As String
        Get
            Return confirmPasswordValue
        End Get
        Set(ByVal value As String)
            confirmPasswordValue = value
        End Set
    End Property
End Class
#End Region

#Region "Dienste"
' Der FormsAuthentication-Typ ist versiegelt und enthält statische Member, weshalb
' Komponententests des Codes, von dem die Member aufgerufen werden, nicht ganz einfach sind. Von der Schnittstellen- und Helper-Klasse weiter unten wird veranschaulicht,
' wie ein abstrakter Wrapper für einen solchen Typ erstellt wird, um dafür zu sorgen, dass für den AccountController-
' Code Komponententests ausgeführt werden können.

Public Interface IMembershipService
    ReadOnly Property MinPasswordLength() As Integer

    Function ValidateUser(ByVal userName As String, ByVal password As String) As Boolean
    Function CreateUser(ByVal userName As String, ByVal password As String, ByVal email As String) As MembershipCreateStatus
    Function ChangePassword(ByVal userName As String, ByVal oldPassword As String, ByVal newPassword As String) As Boolean
End Interface

Public Class AccountMembershipService
    Implements IMembershipService

    Private ReadOnly _provider As MembershipProvider

    Public Sub New()
        Me.New(Nothing)
    End Sub

    Public Sub New(ByVal provider As MembershipProvider)
        _provider = If(provider, Membership.Provider)
    End Sub

    Public ReadOnly Property MinPasswordLength() As Integer Implements IMembershipService.MinPasswordLength
        Get
            Return _provider.MinRequiredPasswordLength
        End Get
    End Property

    Public Function ValidateUser(ByVal userName As String, ByVal password As String) As Boolean Implements IMembershipService.ValidateUser
        If String.IsNullOrEmpty(userName) Then Throw New ArgumentException("Der Wert darf nicht NULL oder leer sein.", "userName")
        If String.IsNullOrEmpty(password) Then Throw New ArgumentException("Der Wert darf nicht NULL oder leer sein.", "password")

        Return _provider.ValidateUser(userName, password)
    End Function

    Public Function CreateUser(ByVal userName As String, ByVal password As String, ByVal email As String) As MembershipCreateStatus Implements IMembershipService.CreateUser
        If String.IsNullOrEmpty(userName) Then Throw New ArgumentException("Der Wert darf nicht NULL oder leer sein.", "userName")
        If String.IsNullOrEmpty(password) Then Throw New ArgumentException("Der Wert darf nicht NULL oder leer sein.", "password")
        If String.IsNullOrEmpty(email) Then Throw New ArgumentException("Der Wert darf nicht NULL oder leer sein.", "email")

        Dim status As MembershipCreateStatus
        _provider.CreateUser(userName, password, email, Nothing, Nothing, True, Nothing, status)
        Return status
    End Function

    Public Function ChangePassword(ByVal userName As String, ByVal oldPassword As String, ByVal newPassword As String) As Boolean Implements IMembershipService.ChangePassword
        If String.IsNullOrEmpty(userName) Then Throw New ArgumentException("Der Wert darf nicht NULL oder leer sein.", "username")
        If String.IsNullOrEmpty(oldPassword) Then Throw New ArgumentException("Der Wert darf nicht NULL oder leer sein.", "oldPassword")
        If String.IsNullOrEmpty(newPassword) Then Throw New ArgumentException("Der Wert darf nicht NULL oder leer sein.", "newPassword")

        ' In bestimmten Fehlerszenarios wird von der zugrunde liegenden ChangePassword()-Methode
        ' nicht "false" zurückgegeben, sondern eine Ausnahme ausgelöst.
        Try
            Dim currentUser As MembershipUser = _provider.GetUser(userName, True)
            Return currentUser.ChangePassword(oldPassword, newPassword)
        Catch ex As ArgumentException
            Return False
        Catch ex As MembershipPasswordException
            Return False
        End Try
    End Function
End Class

Public Interface IFormsAuthenticationService
    Sub SignIn(ByVal userName As String, ByVal createPersistentCookie As Boolean)
    Sub SignOut()
End Interface

Public Class FormsAuthenticationService
    Implements IFormsAuthenticationService

    Public Sub SignIn(ByVal userName As String, ByVal createPersistentCookie As Boolean) Implements IFormsAuthenticationService.SignIn
        If String.IsNullOrEmpty(userName) Then Throw New ArgumentException("Der Wert darf nicht NULL oder leer sein.", "userName")

        FormsAuthentication.SetAuthCookie(userName, createPersistentCookie)
    End Sub

    Public Sub SignOut() Implements IFormsAuthenticationService.SignOut
        FormsAuthentication.SignOut()
    End Sub
End Class
#End Region

#Region "Validierung"
Public NotInheritable Class AccountValidation
    Public Shared Function ErrorCodeToString(ByVal createStatus As MembershipCreateStatus) As String
        ' Unter "http://go.microsoft.com/fwlink/?LinkID=177550" finden Sie
        ' eine vollständige Liste mit Statuscodes.
        Select Case createStatus
            Case MembershipCreateStatus.DuplicateUserName
                Return "Der Benutzername ist bereits vorhanden. Geben Sie einen anderen Benutzernamen ein."

            Case MembershipCreateStatus.DuplicateEmail
                Return "Für diese E-Mail-Adresse ist bereits ein Benutzername vorhanden. Geben Sie eine andere E-Mail-Adresse ein."

            Case MembershipCreateStatus.InvalidPassword
                Return "Das angegebene Kennwort ist ungültig. Geben Sie einen gültigen Kennwortwert ein."

            Case MembershipCreateStatus.InvalidEmail
                Return "Die angegebene E-Mail-Adresse ist ungültig. Überprüfen Sie den Wert, und wiederholen Sie den Vorgang."

            Case MembershipCreateStatus.InvalidAnswer
                Return "Die angegebene Kennwortabrufantwort ist ungültig. Überprüfen Sie den Wert, und wiederholen Sie den Vorgang."

            Case MembershipCreateStatus.InvalidQuestion
                Return "Die angegebene Kennwortabruffrage ist ungültig. Überprüfen Sie den Wert, und wiederholen Sie den Vorgang."

            Case MembershipCreateStatus.InvalidUserName
                Return "Der angegebene Benutzername ist ungültig. Überprüfen Sie den Wert, und wiederholen Sie den Vorgang."

            Case MembershipCreateStatus.ProviderError
                Return "Vom Authentifizierungsanbieter wurde ein Fehler zurückgegeben. Überprüfen Sie die Eingabe, und wiederholen Sie den Vorgang. Sollte das Problem weiterhin bestehen, wenden Sie sich an den zuständigen Systemadministrator."

            Case MembershipCreateStatus.UserRejected
                Return "Die Benutzererstellungsanforderung wurde abgebrochen. Überprüfen Sie die Eingabe, und wiederholen Sie den Vorgang. Sollte das Problem weiterhin bestehen, wenden Sie sich an den zuständigen Systemadministrator."

            Case Else
                Return "Unbekannter Fehler. Überprüfen Sie die Eingabe, und wiederholen Sie den Vorgang. Sollte das Problem weiterhin bestehen, wenden Sie sich an den zuständigen Systemadministrator."
        End Select
    End Function
End Class

<AttributeUsage(AttributeTargets.Class, AllowMultiple:=True, Inherited:=False)> _
Public NotInheritable Class PropertiesMustMatchAttribute
    Inherits ValidationAttribute

    Private Const _defaultErrorMessage As String = "'{0}' und '{1}' stimmen nicht überein."
    Private ReadOnly _confirmProperty As String
    Private ReadOnly _originalProperty As String
    Private ReadOnly _typeId As New Object()

    Public Sub New(ByVal originalProperty As String, ByVal confirmProperty As String)
        MyBase.New(_defaultErrorMessage)

        _originalProperty = originalProperty
        _confirmProperty = confirmProperty
    End Sub

    Public ReadOnly Property ConfirmProperty() As String
        Get
            Return _confirmProperty
        End Get
    End Property

    Public ReadOnly Property OriginalProperty() As String
        Get
            Return _originalProperty
        End Get
    End Property

    Public Overrides ReadOnly Property TypeId() As Object
        Get
            Return _typeId
        End Get
    End Property

    Public Overrides Function FormatErrorMessage(ByVal name As String) As String
        Return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, OriginalProperty, ConfirmProperty)
    End Function

    Public Overrides Function IsValid(ByVal value As Object) As Boolean
        Dim properties As PropertyDescriptorCollection = TypeDescriptor.GetProperties(value)
        Dim originalValue = properties.Find(OriginalProperty, True).GetValue(value)
        Dim confirmValue = properties.Find(ConfirmProperty, True).GetValue(value)
        Return Object.Equals(originalValue, confirmValue)
    End Function
End Class

<AttributeUsage(AttributeTargets.Field Or AttributeTargets.Property, AllowMultiple:=False, Inherited:=True)> _
Public NotInheritable Class ValidatePasswordLengthAttribute
    Inherits ValidationAttribute

    Private Const _defaultErrorMessage As String = "'{0}' muss mindestens {1} Zeichen lang sein."
    Private ReadOnly _minCharacters As Integer = Membership.Provider.MinRequiredPasswordLength

    Public Sub New()
        MyBase.New(_defaultErrorMessage)
    End Sub

    Public Overrides Function FormatErrorMessage(ByVal name As String) As String
        Return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, name, _minCharacters)
    End Function

    Public Overrides Function IsValid(ByVal value As Object) As Boolean
        Dim valueAsString As String = TryCast(value, String)
        Return (valueAsString IsNot Nothing) AndAlso (valueAsString.Length >= _minCharacters)
    End Function
End Class
#End Region
