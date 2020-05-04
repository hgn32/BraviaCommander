Module ModuleMain
    Sub Main()
        Dim Setting As New SettingManager(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "setting.xml"))
        Dim Logger As New Logger(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "runing.log"))
        Dim Bravia As New Bravia
        Try
            '設定読み込む
            Setting.InitializeSetting()
            Setting.LoadFromFile()
            'Setting.SaveToFile()
            Bravia.IPAddr = Setting.Value.IPAddr
            Bravia.MACAddr = Setting.Value.MACAddr
            Bravia.ClientID = Setting.Value.ClientID
            Bravia.NickName = Setting.Value.NickName
            Bravia.Cookies = Setting.Value.Cookies
            Bravia.ClientIDCode = Setting.Value.ClientIDCode

            'コマンドライン引数を配列で取得する
            If System.Environment.GetCommandLineArgs().Length <= 0 Then
                Throw New Exception("引数(処理)が未指定です")
            End If
            Dim First As Boolean = True
            For Each CommandLineArg As String In System.Environment.GetCommandLineArgs()
                If First Then
                    First = False
                    Continue For
                End If
                Dim Command As String = CommandLineArg.ToLower
                Dim Response As New Bravia.Response
                If Command = "setup" Then
                    Response = Bravia.Setup()
                    Setting.Value.ClientID = Bravia.ClientID
                    Setting.Value.NickName = Bravia.NickName
                    Setting.Value.Cookies = Bravia.Cookies
                    Setting.Value.ClientIDCode = Bravia.ClientIDCode
                    Setting.SaveToFile()
                Else
                    For i As Integer = 1 To Setting.Value.Retry
                        Response = Bravia.Send(Command)
                        If Bravia.CookiesUpdate Then
                            Setting.Value.Cookies = Bravia.Cookies
                            Setting.SaveToFile()
                        End If
                    Next
                End If
                Logger.addLine(BraviaCommander.Logger.Type.Information, String.Format("{1}{0}Response:{2}{0}Cookie:{3}{0}Body:{4}", vbTab, Command, Response.Code, Bravia.CookiesUpdate, Response.Body.Replace(vbLf, "")))
            Next
        Catch ex As Exception
            Logger.addLine(BraviaCommander.Logger.Type.Exception, ex.Message)
        End Try
    End Sub

End Module
