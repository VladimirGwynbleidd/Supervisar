<Serializable>
Public Class EntidadSicod
    Public Property ID As Integer
    Public Property DSC As String
End Class


<Serializable>
Public Class EntidadTipo
    Public Property IdEntidad As Integer
    Public Property IdTipoEntidad As Integer
    Public Property dscTipoEntidad As String
End Class

<Serializable>
Public Class SubEntidad
    Public Property IdSubEntidad As Integer
    Public Property DscSubEntidad As String
End Class

<Serializable>
Public Class TipoSubEntidad
    Public Property IdTipoEntidad As Integer
    Public Property DscTipoEntidad As String
    Public Property IdSubEntidad As Integer
    Public Property IdItem As Integer
End Class