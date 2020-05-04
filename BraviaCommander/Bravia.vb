Public Class Bravia
    Private IRCCCommands As New Dictionary(Of String, String) From {
        {"poweron", "AAAAAQAAAAEAAAAuAw=="},
        {"poweroff", "AAAAAQAAAAEAAAAvAw=="},
        {"hdmi1", "AAAAAgAAABoAAABaAw=="},
        {"hdmi2", "AAAAAgAAABoAAABbAw=="},
        {"hdmi3", "AAAAAgAAABoAAABcAw=="},
        {"hdmi4", "AAAAAgAAABoAAABdAw=="}
    }
    Public IPAddr As String = ""
    Public MACAddr As String = ""
    Public ClientID As String = ""
    Public ClientIDCode As String = "0000"
    Public NickName As String = ""
    Public Cookies As New List(Of Cookie)
    Public CookiesUpdate As Boolean = False

    Public Function Setup() As Response
        Dim URL As String = String.Format("http://{0}/sony/accessControl", Me.IPAddr)
        If ClientID = "" Then
            ClientID = Guid.NewGuid().ToString
        End If
        If NickName = "" Then
            NickName = System.Net.Dns.GetHostName()
        End If
        Dim Headers As New List(Of String)
        Dim Body As New Text.StringBuilder
        Body.AppendFormat("{{""method"":""actRegister"",""params"":[{{""clientid"":""{0}"",""nickname"":""{1}""}},[{{""function"":""WOL"",""value"":""no""}}]],""id"":{2},""version"":""1.0""}}", ClientID, NickName, ClientIDCode)
        '画面にコードを表示させる
        Dim Response As Response = _Send(URL, "application/json", Headers, Body.ToString)

        If Response.Code = 200 AndAlso Response.Body = "{""result"":[],""id"":3041}" Then
            '認証更新
            Return Response
        Else
            '新規
            Dim Form As New FormInputCode()
            If Form.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Dim Code As String = Form.TextBoxCode.Text
                ClientIDCode = Code
                Headers.Add(String.Format("Authorization: Basic {0}", Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(":" & Code))))
                Return _Send(URL, "application/json", Headers, Body.ToString)
            Else
                Throw New Exception("キャンセルされました")
            End If
        End If
    End Function

    Public Function Send(CommandKey As String) As Response
        If IRCCCommands.ContainsKey(CommandKey) = False Then
            Throw New Exception(String.Format("不明なCommand({0})", CommandKey))
        End If
        Dim URL As String = String.Format("http://{0}/sony/ircc", Me.IPAddr)
        Dim Headers As New List(Of String)
        Dim Body As New Text.StringBuilder
        Headers.Add("soapaction: ""urn:schemas-sony-com:service:IRCC:1#X_SendIRCC""")
        Body.Append("<?xml version=""1.0""?><s:Envelope xmlns:s=""http://schemas.xmlsoap.org/soap/envelope/"" s:encodingStyle=""http://schemas.xmlsoap.org/soap/encoding/""><s:Body>")
        Body.AppendFormat("<u:X_SendIRCC xmlns:u=""urn:schemas-sony-com:service:IRCC:1""><IRCCCode>{0}</IRCCCode></u:X_SendIRCC></s:Body></s:Envelope>", IRCCCommands(CommandKey))
        Return _Send(URL, "text/xml; charset=utf-8", Headers, Body.ToString)
    End Function

    Private Function _Send(URL As String, ContentType As String, Headers As List(Of String), Body As String) As Response
        Dim CookieContainer As New System.Net.CookieContainer
        For Each Cookie As Cookie In Cookies
            CookieContainer.Add(Cookie.ToNetCookie)
        Next

        Dim HttpWebRequest As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(URL), System.Net.HttpWebRequest)
        HttpWebRequest.Timeout = 10 * 1000
        HttpWebRequest.CachePolicy = New System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore)
        HttpWebRequest.CookieContainer = CookieContainer
        HttpWebRequest.Method = "POST"
        HttpWebRequest.ContentType = ContentType
        HttpWebRequest.Accept = ""
        For Each Header As String In Headers
            HttpWebRequest.Headers.Add(Header)
        Next
        Dim RequestByte() As Byte = System.Text.Encoding.UTF8.GetBytes(Body.ToString)
        HttpWebRequest.ContentLength = RequestByte.Length
        Using RequestStream As System.IO.Stream = HttpWebRequest.GetRequestStream()
            RequestStream.Write(RequestByte, 0, RequestByte.Length)
        End Using
        '結果取得
        Dim Response As New Response
        Try
            Dim HttpWebResponse As System.Net.HttpWebResponse = HttpWebRequest.GetResponse()
            Using StreamReader As System.IO.StreamReader = New System.IO.StreamReader(HttpWebResponse.GetResponseStream(), System.Text.Encoding.UTF8)
                Response.Code = HttpWebResponse.StatusCode
                Response.Body = StreamReader.ReadToEnd()
            End Using
            If HttpWebResponse.Cookies.Count > 0 Then
                CookiesUpdate = True
                Me.Cookies.Clear()
                For Each ResponseCookie As Net.Cookie In HttpWebResponse.Cookies
                    Me.Cookies.Add(New Cookie(ResponseCookie.Domain, ResponseCookie.Path, ResponseCookie.Name, ResponseCookie.Value))
                Next
            End If
        Catch ex As Net.WebException
            'WebExceptionは
            Dim errres As System.Net.HttpWebResponse = CType(ex.Response, System.Net.HttpWebResponse)
            Response.Code = errres.StatusCode
            Dim ResponseStream As IO.Stream = ex.Response.GetResponseStream()
            If ResponseStream.CanRead Then
                Using StreamReader As New IO.StreamReader(ResponseStream, Text.Encoding.UTF8)
                    Response.Body = StreamReader.ReadToEnd()
                End Using
            End If
        Catch ex As Exception
            'それ以外の例外は中断
            Throw
        End Try
        Return Response
    End Function

    Public Class Response
        Public Code As Integer = 0
        Public Body As String = ""
    End Class
    <System.Runtime.Serialization.DataContract> Public Class Cookie
        <System.Runtime.Serialization.DataMember> Public Domain As String = ""
        <System.Runtime.Serialization.DataMember> Public Path As String = ""
        <System.Runtime.Serialization.DataMember> Public Name As String = ""
        <System.Runtime.Serialization.DataMember> Public Value As String = ""

        Public Sub New(Domain As String, URL As String, Key As String, Value As String)
            Me.Domain = Domain
            Me.Path = URL
            Me.Name = Key
            Me.Value = Value
        End Sub
        Public Function ToNetCookie() As Net.Cookie
            Return New Net.Cookie(Me.Name, Me.Value, Me.Path, Me.Domain)
        End Function
    End Class
End Class