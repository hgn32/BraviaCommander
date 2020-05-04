''' <summary>
''' 設定を管理するクラス
''' 例）設定の書き込み・読み出し・初期化
''' </summary>
''' <remarks></remarks>
Public Class SettingManager
    ''' <summary>
    ''' 設定ファイルのパス
    ''' </summary>
    ''' <remarks></remarks>
    Property FilePath As String
    ''' <summary>
    ''' 設定ファイルのクラス
    ''' </summary>
    ''' <remarks></remarks>
    Property Value As New SettingValue

    ''' <summary>
    ''' コンストラクタ
    ''' パス : strFilePath
    ''' </summary>
    ''' <param name="strFilePath">ファイルパス</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal strFilePath As String)
        Me.FilePath = strFilePath
    End Sub

    ''' <summary>
    ''' 設定の値を初期化する
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InitializeSetting()
        Value = New SettingValue
    End Sub

    ''' <summary>
    ''' 設定ファイルが存在するか
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExistSettingFile() As Boolean
        If System.IO.File.Exists(FilePath) = False Then
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' 設定を書き出す
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SaveToFile()
        Dim serializer = New System.Runtime.Serialization.DataContractSerializer(Me.Value.GetType)
        Dim settings As New Xml.XmlWriterSettings()
        settings.Encoding = New System.Text.UTF8Encoding(False) 'BOMなしUTF8
        Using XmlWriter As Xml.XmlWriter = Xml.XmlWriter.Create(FilePath, settings)
            serializer.WriteObject(XmlWriter, Me.Value)
        End Using
    End Sub

    ''' <summary>
    ''' 設定を読み込む
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub LoadFromFile()
        If System.IO.File.Exists(FilePath) = False Then
            SaveToFile()
        Else
            Dim serializer = New System.Runtime.Serialization.DataContractSerializer(Me.Value.GetType)
            Using XmlReader As Xml.XmlReader = Xml.XmlReader.Create(FilePath)
                Me.Value = serializer.ReadObject(XmlReader)
            End Using
        End If
    End Sub
End Class
Public Class SettingValue
    Public IPAddr As String = "" '192.168.0.3
    Public MACAddr As String = "" '10:4F:A8:B7:AD:D6
    Public UseStatiocIPAdder As Boolean = True
    Public ClientID As String = "" 'NAS01:d9f9b7d4-ac99-4a7b-92bb-887cf9b233fa"
    Public NickName As String = "" 'NAS01
    Public Retry As Integer = 5
    Public Cookies As New List(Of Bravia.Cookie)
    Public ClientIDCode As String = "1426"
End Class
