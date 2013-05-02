' Hinweis: Anweisungen zum Aktivieren des klassischen Modus von IIS6 oder IIS7 
' finden Sie unter "http://go.microsoft.com/?LinkId=9394802".

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Shared Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")

        ' Von "MapRoute" werden die folgenden Parameter in der folgenden Reihenfolge akzeptiert:
        ' (1) Routenname
        ' (2) URL mit Parametern
        ' (3) Parameterstandardwerte
        routes.MapRoute( _
            "Default", _
            "{controller}/{action}/{id}", _
            New With {.controller = "Home", .action = "Index", .id = UrlParameter.Optional} _
        )

    End Sub

    Sub Application_Start()
        AreaRegistration.RegisterAllAreas()

        RegisterRoutes(RouteTable.Routes)
    End Sub
End Class
