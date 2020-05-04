Public Class FormInputCode

    Private Sub ButtonRegist_Click(sender As Object, e As EventArgs) Handles ButtonRegist.Click
        Try
            Me.DialogResult = Windows.Forms.DialogResult.OK
            Me.Hide()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub FormInputCode_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            TextBoxCode.Focus()
        Catch ex As Exception

        End Try
    End Sub
End Class