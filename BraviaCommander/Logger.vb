''' <summary>
''' 動作ログを残すクラス
''' ファイルに書き込むタイプ
''' </summary>
''' <remarks></remarks>
Public Class Logger
    Public Enum Type
        Exception
        Information
    End Enum
    Public FilePath As String = ""  'ログファイルパス
    Public MaxLine As Long = 1000  '書き込む最大行

    ''' <summary>
    ''' コンストラクター
    ''' ログファイルパスを受け付ける
    ''' </summary>
    ''' <param name="strFilePath"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal strFilePath As String)
        Me.FilePath = strFilePath
    End Sub


    ''' <summary>
    ''' ログのテキストを書き込む
    ''' </summary>
    ''' <param name="LoglineText"></param>
    ''' <remarks></remarks>
    Public Sub addLine(Type As Type, LoglineText As String)
        '追加するテキストを作成
        LoglineText = Type.ToString & vbTab & Now.ToString("yyyy-MM-dd HH:mm:ss") & vbTab & LoglineText
        '末尾の改行削除
        Dim Foot As String = LoglineText.Substring(LoglineText.Length - 1, 1)
        If Foot = vbCr Or Foot = vbLf Or Foot = vbCrLf Then
            LoglineText = LoglineText.Substring(0, LoglineText.Length - 1)
        End If
        Console.WriteLine(LoglineText)
        '追記
        Using StreamWriter As New System.IO.StreamWriter(FilePath, True, System.Text.Encoding.UTF8)
            StreamWriter.WriteLine(LoglineText)
        End Using
        '行数カウント
        Dim Lines As New List(Of String)
        Using StreamReader As New System.IO.StreamReader(FilePath, System.Text.Encoding.UTF8)
            While StreamReader.Peek > -1
                Lines.Add(StreamReader.ReadLine)
            End While
        End Using
        '行数オーバー
        If Lines.Count >= MaxLine Then
            '頭の行を飛ばして上書き書き込み
            Using StreamWriter As New System.IO.StreamWriter(FilePath, False, System.Text.Encoding.UTF8)
                For i = Lines.Count - MaxLine + 1 To Lines.Count - 1
                    StreamWriter.WriteLine(Lines(i))
                Next
            End Using
        End If
    End Sub

End Class
