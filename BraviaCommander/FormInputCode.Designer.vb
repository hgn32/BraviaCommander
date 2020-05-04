<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormInputCode
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナーで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナーで必要です。
    'Windows フォーム デザイナーを使用して変更できます。  
    'コード エディターを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ButtonRegist = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBoxCode = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'ButtonRegist
        '
        Me.ButtonRegist.Location = New System.Drawing.Point(15, 54)
        Me.ButtonRegist.Name = "ButtonRegist"
        Me.ButtonRegist.Size = New System.Drawing.Size(254, 23)
        Me.ButtonRegist.TabIndex = 0
        Me.ButtonRegist.Text = "登録"
        Me.ButtonRegist.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(13, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(256, 12)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "テレビの画面に表示されているコードを入力してください"
        '
        'TextBoxCode
        '
        Me.TextBoxCode.Location = New System.Drawing.Point(15, 29)
        Me.TextBoxCode.Name = "TextBoxCode"
        Me.TextBoxCode.Size = New System.Drawing.Size(254, 19)
        Me.TextBoxCode.TabIndex = 2
        '
        'FormInputCode
        '
        Me.AcceptButton = Me.ButtonRegist
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(285, 92)
        Me.Controls.Add(Me.TextBoxCode)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ButtonRegist)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormInputCode"
        Me.Text = "InputCode"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ButtonRegist As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBoxCode As System.Windows.Forms.TextBox
End Class
