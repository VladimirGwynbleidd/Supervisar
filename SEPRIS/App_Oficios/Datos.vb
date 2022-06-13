Public Class Datos
    Public Property id As Integer
    Public Property valor As String

    Sub New()

    End Sub

    Sub New(ByVal _id As Integer, ByVal _valor As String)
        Me.id = _id
        Me.valor = _valor

    End Sub

End Class
