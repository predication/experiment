Public Class ExpandoTest

    Public Function NonExistantProperty() As String
         Dim result As String
        result = String.Empty
        Dim item As Object
        item = New Object()
        item.Name = "Brian"
        result = item.Name
        Return result
    End Function

End Class
