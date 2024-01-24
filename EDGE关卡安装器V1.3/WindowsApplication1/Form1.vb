Imports System.IO
Imports System.Xml
Imports System.Security.Principal
Public Class Form1
    Public EdgeFolderLocation As String, backup As Boolean
    Dim bin_name, BinLocation, EsoLocation As String, bonus_ahead, is_admin As Boolean
    REM 确认、检测、修改mapping.xml
    Private Function XML_Modify(number As Byte) As Boolean
        'xml存在确认
        If Not (File.Exists(EdgeFolderLocation + "\levels\mapping.xml")) Then
            Select Case number
                Case 4 '拖动安装
                    TextBox4.Text = "安装失败：未找到mapping.xml。" + vbCrLf + TextBox4.Text
                Case 2 '直接安装
                    TextBox2.Text = "安装失败：未找到mapping.xml。" + vbCrLf + TextBox2.Text
                Case 3 '选择安装
                    TextBox3.Text = "安装失败：未找到mapping.xml。" + vbCrLf + TextBox3.Text
            End Select
            MsgBox("未在指定的位置找到mapping.xml，请确认edge.exe路径是否有误。", MsgBoxStyle.Exclamation, "出错了")
            Return True '出现异常
        End If
        If backup Then
            Select Case number
                Case 4 '拖动安装
                    TextBox4.Text = "备份mapping.xml......" + vbCrLf + TextBox4.Text
                Case 2 '直接安装
                    TextBox2.Text = "备份mapping.xml......" + vbCrLf + TextBox2.Text
                Case 3 '选择安装
                    TextBox3.Text = "备份mapping.xml......" + vbCrLf + TextBox3.Text
            End Select
            File.Copy(EdgeFolderLocation + "\levels\mapping.xml", EdgeFolderLocation + "\levels\mapping.xml.bak", True)
        End If
        'xml存在确认完成
        'xml编码检测
        Try
            Dim doc As New XmlDocument
            doc.Load(EdgeFolderLocation + "\levels\mapping.xml")
        Catch ex As Exception
            If MsgBox("读取mapping.xml时出现错误，这可能是xml的编码问题。是否尝试修改编码？注意：根据你的设置，原文件可能不会被备份。", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "出错了") = MsgBoxResult.Yes Then
                Select Case number
                    Case 4 '拖动安装
                        TextBox4.Text = "修改mapping.xml编码......" + vbCrLf + TextBox4.Text
                    Case 2 '直接安装
                        TextBox2.Text = "修改mapping.xml编码......" + vbCrLf + TextBox2.Text
                    Case 3 '选择安装
                        TextBox3.Text = "修改mapping.xml编码......" + vbCrLf + TextBox3.Text
                End Select
                '修改编码
                Try
                    Dim s() As String = File.ReadAllLines(EdgeFolderLocation + "\levels\mapping.xml")
                    s(0) = "<?xml version=" + Chr(34) + "1.0" + Chr(34) + " encoding=" + Chr(34) + "utf-8" + Chr(34) + "?>"
                    File.WriteAllLines(EdgeFolderLocation + "\levels\mapping.xml", s)
                Catch ex1 As Exception
                    Select Case number
                        Case 4 '拖动安装
                            TextBox4.Text = "安装失败：修改mapping.xml编码失败。" + vbCrLf + TextBox4.Text
                        Case 2 '直接安装
                            TextBox2.Text = "安装失败：修改mapping.xml编码失败。" + vbCrLf + TextBox2.Text
                        Case 3 '选择安装
                            TextBox3.Text = "安装失败：修改mapping.xml编码失败。" + vbCrLf + TextBox3.Text
                    End Select
                    MsgBox("修改编码过程中出现错误，请尝试手动修改编码至UTF-8。若要了解更多，请到搜索引擎搜索" + Chr(34) + "记事本改编码格式" + Chr(34) + "。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex1.ToString, MsgBoxStyle.Critical, "出错了")
                    Return True '出现异常
                End Try
                '修改完成
                Select Case number
                    Case 4 '拖动安装
                        TextBox4.Text = "修改完成，尝试重新读取mapping.xml......" + vbCrLf + TextBox4.Text
                    Case 2 '直接安装
                        TextBox2.Text = "修改完成，尝试重新读取mapping.xml......" + vbCrLf + TextBox2.Text
                    Case 3 '选择安装
                        TextBox3.Text = "修改完成，尝试重新读取mapping.xml......" + vbCrLf + TextBox3.Text
                End Select
                '修改完成后，再次检测xml是否能正常打开
                Try
                    Dim doc1 As New XmlDocument
                    doc1.Load(EdgeFolderLocation + "\levels\mapping.xml")
                Catch ex2 As Exception
                    Select Case number
                        Case 4 '拖动安装
                            TextBox4.Text = "安装失败：读取mapping.xml时出现错误。" + vbCrLf + TextBox4.Text
                        Case 2 '直接安装
                            TextBox2.Text = "安装失败：读取mapping.xml时出现错误。" + vbCrLf + TextBox2.Text
                        Case 3 '选择安装
                            TextBox3.Text = "安装失败：读取mapping.xml时出现错误。" + vbCrLf + TextBox3.Text
                    End Select
                    MsgBox("读取mapping.xml时仍然出现错误，请确认mapping.xml的完整性。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex2.ToString, MsgBoxStyle.Critical, "出错了")
                    Return True '出现异常
                End Try
            Else
                Select Case number
                    Case 4 '拖动安装
                        TextBox4.Text = "安装失败：读取mapping.xml时出现错误。" + vbCrLf + TextBox4.Text
                    Case 2 '直接安装
                        TextBox2.Text = "安装失败：读取mapping.xml时出现错误。" + vbCrLf + TextBox2.Text
                    Case 3 '选择安装
                        TextBox3.Text = "安装失败：读取mapping.xml时出现错误。" + vbCrLf + TextBox3.Text
                End Select
                MsgBox("由于您拒绝了修改请求，安装过程被中止。", MsgBoxStyle.Exclamation, "安装中止")
                Return True '出现异常
            End If
        End Try
        'xml编码检测完成
        'xml修改
        Try
            Select Case number
                Case 4 '拖动安装
                    TextBox4.Text = "修改mapping.xml......" + vbCrLf + TextBox4.Text
                Case 2 '直接安装
                    TextBox2.Text = "修改mapping.xml......" + vbCrLf + TextBox2.Text
                Case 3 '选择安装
                    TextBox3.Text = "修改mapping.xml......" + vbCrLf + TextBox3.Text
            End Select
            Dim xsettings As New XmlReaderSettings
            xsettings.IgnoreComments = True '忽略注释,防止读取节点时出错   
            Dim xreader As XmlReader = XmlReader.Create(EdgeFolderLocation + "\levels\mapping.xml", xsettings)
            Dim xdoc As New XmlDocument
            xdoc.Load(xreader)
            xreader.Close()
            Dim node_levels As XmlNode = xdoc.SelectSingleNode("levels") '读取根节点levels
            Dim node_list_levels As XmlNodeList = node_levels.ChildNodes '获取levels的所有子节点
            Dim node_bonus_find As Boolean = False '用于判断是否找到bonus节点
            Dim filename_existed As Boolean = False '用于判断正在安装的关卡是否已在mapping.xml中存在
            For Each xn As XmlNode In node_list_levels '遍历所有子节点
                If xn.Name = "bonus" Then '当子节点为bonus时
                    node_bonus_find = True '已找到bonus节点
                    Dim node_list_bonus As XmlNodeList = xn.ChildNodes '获取bonus的所有子节点
                    For Each xn1 As XmlNode In node_list_bonus '遍历所有子节点
                        Dim attribute_list_level As XmlAttributeCollection = xn1.Attributes() '获取这个level节点的所有属性
                        Dim attribute_filename_existed As XmlAttribute = attribute_list_level.ItemOf("filename") '获取filename属性值
                        If attribute_filename_existed.Value = bin_name Then '如果正在安装的关卡已在mapping.xml中存在
                            filename_existed = True
                            Exit For
                        End If
                    Next
                    If filename_existed Then
                        Select Case number
                            Case 4 '拖动安装
                                TextBox4.Text = "注意：此关卡之前已在bonus levels安装过。此次安装只会覆盖原关卡文件，不会修改mapping.xml。" + vbCrLf + TextBox4.Text
                            Case 2 '直接安装
                                TextBox2.Text = "注意：此关卡之前已在bonus levels安装过。此次安装只会覆盖原关卡文件，不会修改mapping.xml。" + vbCrLf + TextBox2.Text
                            Case 3 '选择安装
                                TextBox3.Text = "注意：此关卡之前已在bonus levels安装过。此次安装只会覆盖原关卡文件，不会修改mapping.xml。" + vbCrLf + TextBox3.Text
                        End Select
                        Exit For
                    Else
                        Dim node_level As XmlElement = xdoc.CreateElement("level") '创建新元素(节点)level
                        Dim attribute_filename As XmlAttribute = xdoc.CreateAttribute("filename") '创建新属性filename
                        attribute_filename.InnerText = bin_name '设置属性值
                        node_level.SetAttributeNode(attribute_filename) '将属性设置为元素level的属性
                        If bonus_ahead Then
                            xn.PrependChild(node_level) '在bonus节点下增加新子节点level,位置为头部
                        Else
                            xn.AppendChild(node_level) '在bonus节点下增加新子节点level,位置为尾部
                        End If
                        xdoc.Save(EdgeFolderLocation + "\levels\mapping.xml")
                        Exit For
                    End If
                End If
            Next
            If node_bonus_find Then
                Return False '未出现异常
            Else
                Select Case number
                    Case 4 '拖动安装
                        TextBox4.Text = "安装失败：在mapping.xml中未找到bonus节点。" + vbCrLf + TextBox4.Text
                    Case 2 '直接安装
                        TextBox2.Text = "安装失败：在mapping.xml中未找到bonus节点。" + vbCrLf + TextBox2.Text
                    Case 3 '选择安装
                        TextBox3.Text = "安装失败：在mapping.xml中未找到bonus节点。" + vbCrLf + TextBox3.Text
                End Select
                MsgBox("未在mapping.xml中找到bonus节点，请确认mapping.xml的完整性。", MsgBoxStyle.Exclamation, "出错了")
                Return True '出现异常
            End If
        Catch ex As Exception
            Select Case number
                Case 4 '拖动安装
                    TextBox4.Text = "安装失败：操作mapping.xml时遇到异常。" + vbCrLf + TextBox4.Text
                Case 2 '直接安装
                    TextBox2.Text = "安装失败：操作mapping.xml时遇到异常。" + vbCrLf + TextBox2.Text
                Case 3 '选择安装
                    TextBox3.Text = "安装失败：操作mapping.xml时遇到异常。" + vbCrLf + TextBox3.Text
            End Select
            MsgBox("操作mapping.xml时发生错误。由于此错误是意料之外的，为了保证程序的正常运行，建议您退出并重新打开此程序。" + vbCrLf + vbCrLf + "可能导致此错误发生的原因：" + vbCrLf + "1.程序权限不足，请尝试获取管理员权限；" + vbCrLf + "2.操作请求被安全程序拦截；" + vbCrLf + "3.其他程序正在占用mapping.xml；" + vbCrLf + "4.mapping.xml结构不完整。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex.ToString, MsgBoxStyle.Critical, "出错了")
            Return True '出现异常
        End Try
        'xml修改完成
    End Function
    REM 拖动安装
    Private Sub Drag_Install()
        Try
            TextBox4.Text = ""
            TextBox4.Text = "检测拖入文件的类型......" + vbCrLf + TextBox4.Text
            If Mid(BinLocation, InStrRev(BinLocation, ".") + 1) = "bin" Then
                If TextBox1.Text = "" Then
                    TextBox4.Text = "安装失败：未确定edge.exe位置。" + vbCrLf + TextBox4.Text
                    MsgBox("你还没确定edge.exe位置！", MsgBoxStyle.Exclamation, "出错啦")
                Else
                    TextBox4.Text = "查找.eso文件......" + vbCrLf + TextBox4.Text
                    Dim di As New DirectoryInfo(Mid(BinLocation, 1, InStrRev(BinLocation, "\") - 1))
                    Dim EsoFiles As FileInfo()
                    EsoFiles = di.GetFiles("*.eso")
                    If EsoFiles.Length = 0 Then
                        '啥也不干
                    ElseIf EsoFiles.Length > 1 Then
                        TextBox4.Text = "安装失败：无法确定目标.eso文件。" + vbCrLf + TextBox4.Text
                        MsgBox(".eso文件太多啦！我不知道到底选哪个TAT", MsgBoxStyle.Exclamation, "出错啦")
                        Exit Sub
                    Else
                        TextBox4.Text = "复制.eso文件中，文件名：" + EsoFiles(0).ToString + vbCrLf + TextBox4.Text
                        EsoLocation = Mid(BinLocation, 1, InStrRev(BinLocation, "\")) + EsoFiles(0).ToString
                        File.Copy(EsoLocation, EdgeFolderLocation + "\models\" + EsoFiles(0).ToString, True)
                    End If
                    TextBox4.Text = "复制.bin文件中，文件名：" + Mid(BinLocation, InStrRev(BinLocation, "\") + 1) + vbCrLf + TextBox4.Text
                    File.Copy(BinLocation, EdgeFolderLocation + "\levels\" + Mid(BinLocation, InStrRev(BinLocation, "\") + 1), True)
                    TextBox4.Text = "准备修改mapping.xml......" + vbCrLf + TextBox4.Text
                    bin_name = Mid(BinLocation, InStrRev(BinLocation, "\") + 1, InStrRev(BinLocation, ".") - InStrRev(BinLocation, "\") - 1)
                    If XML_Modify(4) Then
                        Exit Sub
                    End If
                    TextBox4.Text = "安装完成！完成时的系统时间为：" + Now.TimeOfDay.ToString + vbCrLf + TextBox4.Text
                End If
            ElseIf Mid(BinLocation, InStrRev(BinLocation, ".") + 1) = "zip" Then
                TextBox4.Text = "安装失败：拖入的文件非.bin文件。" + vbCrLf + TextBox4.Text
                MsgBox("解压完再来找我！", MsgBoxStyle.Exclamation, "文件格式不对")
            ElseIf Mid(BinLocation, InStrRev(BinLocation, ".") + 1) = "eso" Then
                TextBox4.Text = "安装失败：拖入的文件非.bin文件。" + vbCrLf + TextBox4.Text
                MsgBox("行了行了我知道这关有.eso文件了！", MsgBoxStyle.Exclamation, "文件格式不对")
            ElseIf Mid(BinLocation, InStrRev(BinLocation, ".") + 1) = "edgemod" Then
                TextBox4.Text = "安装失败：拖入的文件非.bin文件。" + vbCrLf + TextBox4.Text
                MsgBox("读题！.edgemod格式的关卡用EdgeTool安装！或者解压完再用我也行。所以我是备胎，对吗？", MsgBoxStyle.Exclamation, "文件格式不对")
            Else
                TextBox4.Text = "安装失败：拖入的文件非.bin文件。" + vbCrLf + TextBox4.Text
                MsgBox("拖的什么鬼玩意，我不认识！", MsgBoxStyle.Exclamation, "文件格式不对")
            End If
        Catch ex As Exception
            TextBox4.Text = "安装失败：操作文件时发生异常。" + vbCrLf + TextBox4.Text
            MsgBox("安装关卡时出现错误。由于此错误是意料之外的，为了保证程序的正常运行，建议您退出并重新打开此程序。" + vbCrLf + vbCrLf + "可能导致此错误发生的原因：" + vbCrLf + "1.edge.exe的路径出错；" + vbCrLf + "2.程序权限不足，请尝试获取管理员权限；" + vbCrLf + "3.操作请求被安全程序拦截。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex.ToString, MsgBoxStyle.Critical, "出错了")
        End Try
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Try
            OpenFileDialog1.ShowDialog()
            If OpenFileDialog1.FileName <> "" Then
                TextBox1.Text = OpenFileDialog1.FileName
                EdgeFolderLocation = Mid(TextBox1.Text, 1, InStrRev(TextBox1.Text, "\") - 1)
                Dim fs As New FileStream(Application.StartupPath + "\EDGE位置.txt", FileMode.Open, FileAccess.Write)
                Dim fsw As New StreamWriter(fs)
                fsw.WriteLine(TextBox1.Text)
                fsw.WriteLine(EdgeFolderLocation)
                fsw.Close()
                fs.Close()
                Button6.Enabled = True
            End If
        Catch ex As Exception
            MsgBox("选择或保存路径时发生错误。由于此错误是意料之外的，为了保证程序的正常运行，建议您退出并重新打开此程序。" + vbCrLf + vbCrLf + "可能导致此错误发生的原因：" + vbCrLf + "1.程序权限不足，请尝试获取管理员权限；" + vbCrLf + "2.操作请求被安全程序拦截；" + vbCrLf + "3.其他程序正在占用EDGE位置.txt。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex.ToString, MsgBoxStyle.Critical, "出错了")
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Application.Exit()
    End Sub
    REM 直接安装
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Try
            TextBox2.Text = ""
            If TextBox1.Text = "" Then
                TextBox2.Text = "安装失败：未确定edge.exe位置。" + vbCrLf + TextBox2.Text
                MsgBox("你还没确定edge.exe位置！", MsgBoxStyle.Exclamation, "出错啦")
            Else
                TextBox2.Text = "查找.bin和.eso文件......" + vbCrLf + TextBox2.Text
                Dim di As New DirectoryInfo(Application.StartupPath)
                Dim BinFiles As FileInfo()
                Dim EsoFiles As FileInfo()
                BinFiles = di.GetFiles("*.bin")
                EsoFiles = di.GetFiles("*.eso")
                If BinFiles.Length = 0 Then
                    TextBox2.Text = "安装失败：找不到.bin文件。" + vbCrLf + TextBox2.Text
                    MsgBox("我找不到.bin文件TAT", MsgBoxStyle.Exclamation, "出错啦")
                ElseIf BinFiles.Length > 1 Then
                    TextBox2.Text = "安装失败：无法确定目标.bin文件。" + vbCrLf + TextBox2.Text
                    MsgBox(".bin文件太多啦！我不知道到底选哪个TAT", MsgBoxStyle.Exclamation, "出错啦")
                Else
                    If EsoFiles.Length = 0 Then
                        '啥也不干
                    ElseIf EsoFiles.Length > 1 Then
                        TextBox2.Text = "安装失败：无法确定目标.eso文件。" + vbCrLf + TextBox2.Text
                        MsgBox(".eso文件太多啦！我不知道到底选哪个TAT", MsgBoxStyle.Exclamation, "出错啦")
                        Exit Sub
                    Else
                        TextBox2.Text = "复制.eso文件中，文件名：" + EsoFiles(0).ToString + vbCrLf + TextBox2.Text
                        EsoLocation = Application.StartupPath + "\" + EsoFiles(0).ToString
                        File.Copy(EsoLocation, EdgeFolderLocation + "\models\" + EsoFiles(0).ToString, True)
                    End If
                    TextBox2.Text = "复制.bin文件中，文件名：" + BinFiles(0).ToString + vbCrLf + TextBox2.Text
                    BinLocation = Application.StartupPath + "\" + BinFiles(0).ToString
                    File.Copy(BinLocation, EdgeFolderLocation + "\levels\" + BinFiles(0).ToString, True)
                    TextBox2.Text = "准备修改mapping.xml......" + vbCrLf + TextBox2.Text
                    bin_name = Mid(BinLocation, InStrRev(BinLocation, "\") + 1, InStrRev(BinLocation, ".") - InStrRev(BinLocation, "\") - 1)
                    If XML_Modify(2) Then
                        Exit Sub
                    End If
                    TextBox2.Text = "安装完成！完成时的系统时间为：" + Now.TimeOfDay.ToString + vbCrLf + TextBox2.Text
                End If
            End If
        Catch ex As Exception
            TextBox2.Text = "安装失败：操作文件时发生异常。" + vbCrLf + TextBox2.Text
            MsgBox("安装关卡时出现错误。由于此错误是意料之外的，为了保证程序的正常运行，建议您退出并重新打开此程序。" + vbCrLf + vbCrLf + "可能导致此错误发生的原因：" + vbCrLf + "1.edge.exe的路径出错；" + vbCrLf + "2.程序权限不足，请尝试获取管理员权限；" + vbCrLf + "3.操作请求被安全程序拦截。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex.ToString, MsgBoxStyle.Critical, "出错了")
        End Try
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            bonus_ahead = False
            backup = True
            If Dir(Application.StartupPath + "\EDGE位置.txt") = "" Then
                Dim fs As New FileStream(Application.StartupPath + "\EDGE位置.txt", FileMode.Create)
                fs.Close()
            Else
                Dim fs2 As New FileStream(Application.StartupPath + "\EDGE位置.txt", FileMode.Open, FileAccess.Read)
                Dim fs2r As New StreamReader(fs2)
                TextBox1.Text = fs2r.ReadLine
                EdgeFolderLocation = fs2r.ReadLine
                fs2r.Close()
                fs2.Close()
                Button6.Enabled = True
            End If
            If TextBox1.Text = "" Then
                Button6.Enabled = False
            End If
        Catch ex As Exception
            MsgBox("读取EDGE位置.txt出现错误。由于此错误是意料之外的，为了保证程序的正常运行，建议您退出并重新打开此程序。" + vbCrLf + vbCrLf + "可能导致此错误发生的原因：" + vbCrLf + "1.程序权限不足，请尝试获取管理员权限；" + vbCrLf + "2.操作请求被安全程序拦截；" + vbCrLf + "3.文件内容被篡改。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex.ToString, MsgBoxStyle.Critical, "出错了")
        End Try
        Try
            Dim identity As WindowsIdentity = WindowsIdentity.GetCurrent
            Dim principal As New WindowsPrincipal(identity)
            Dim isRunasAdmin As Boolean = principal.IsInRole(WindowsBuiltInRole.Administrator)
            If isRunasAdmin Then
                is_admin = True
            Else
                is_admin = False
            End If
        Catch ex As Exception
            MsgBox("我只是例行在这里加一个异常捕获，这都能被你遇到，真是见了鬼了。别管这个错误了，继续运行程序吧。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex.ToString, MsgBoxStyle.Critical, "坏了，这也能出错")
        End Try
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked Then
            bonus_ahead = False
        End If
    End Sub

    Private Sub TextBox4_DragDrop(sender As Object, e As DragEventArgs) Handles TextBox4.DragDrop
        Try
            Dim filepath As String() = e.Data.GetData(DataFormats.FileDrop)
            For Each fp As String In filepath
                BinLocation = fp
                Drag_Install()
            Next
        Catch ex As Exception
            TextBox4.Text = "读取路径时出错了！也许你拖动文件的轨迹不够完美......" + vbCrLf + TextBox4.Text
            MsgBox("读取拖入的文件路径时出错。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex.ToString, MsgBoxStyle.Critical, "出错了")
        End Try
    End Sub

    Private Sub TextBox4_DragEnter(sender As Object, e As DragEventArgs) Handles TextBox4.DragEnter
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub
    REM 放个彩蛋玩玩
    Private Sub RadioButton2_Click(sender As Object, e As EventArgs) Handles RadioButton2.Click
        Static colorful_egg As Byte = 0
        colorful_egg += 1
        Select Case colorful_egg
            Case 1
                '啥也不干
            Case 2
                MsgBox("叫你不要再点我了，没听见啊！", MsgBoxStyle.Exclamation, "怎么回事")
            Case 3
                MsgBox("叫你不要再点我了，眼睛瞎啊！", MsgBoxStyle.Exclamation, "怎么回事")
            Case 4
                MsgBox("叫你不要再点我了，没脑子啊！", MsgBoxStyle.Exclamation, "怎么回事")
            Case 5
                MsgBox("叫你不要再点我了，有毛病啊！", MsgBoxStyle.Exclamation, "怎么回事")
            Case 6
                MsgBox("叫你不要再点我了......", MsgBoxStyle.Exclamation, "怎么回事")
            Case 7
                MsgBox("有本事你给我点100次！", MsgBoxStyle.Exclamation, "你......")
            Case 100
                MsgBox("不跟你玩了，我直接退出！", MsgBoxStyle.Exclamation, "算你狠")
                Application.Exit()
            Case Else
                MsgBox("有本事你给我点100次！" + vbCrLf + vbCrLf + "已点" + CStr(colorful_egg) + "次", MsgBoxStyle.Exclamation, "我倒要看看你有多大能耐")
        End Select
    End Sub

    Private Sub 关于ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 关于ToolStripMenuItem.Click
        MsgBox("程序名称：EDGE关卡安装器(EDGE Levels Installer)" + vbCrLf + "版本：V1.3" + vbCrLf + "作者：fkby48" + vbCrLf + vbCrLf + "程序说明和更新日志请参阅随程序一同提供的文本文件。感谢您的使用！", MsgBoxStyle.Information, "关于")
    End Sub

    Private Sub 通过UAC获取ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 通过UAC获取ToolStripMenuItem.Click
        If is_admin Then
            MsgBox("此程序已获得管理员权限，无需进行该操作。", MsgBoxStyle.Information, "提示")
        Else
            Dim psi As New ProcessStartInfo()
            psi.FileName = Application.ExecutablePath
            psi.Verb = "runas"
            Try
                Process.Start(psi)
                Application.Exit()
            Catch ex As Exception
                MsgBox("获取管理员权限时出错。你可以尝试手动授予管理员权限：关闭程序，右键单击此程序文件，在弹出的菜单中选择" + Chr(34) + "以管理员方式运行" + Chr(34) + "。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex.ToString, MsgBoxStyle.Critical, "出错了")
            End Try
        End If
    End Sub

    Private Sub 联系开发者ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 联系开发者ToolStripMenuItem.Click
        MsgBox("QQ：1158436640" + vbCrLf + "邮箱：y970310@126.com", MsgBoxStyle.Information, "联系开发者")
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Form2.Show()
    End Sub

    Private Sub 为什么要获取ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 为什么要获取ToolStripMenuItem.Click
        MsgBox("此程序需要访问外部文件（待安装的关卡文件、EDGE目录下的部分文件）来完成安装操作。若此程序的权限等级较低，容易因为权限不足而无法读取，触发异常。授予此程序管理员权限可以减少该问题出现的概率。当然，如果在默认配置下没有出现任何问题，你也可以不授予管理员权限。" + vbCrLf + vbCrLf + "注意：Windows XP系统在管理员账户下会默认以管理员身份启动程序。", MsgBoxStyle.Information, "为什么要获取管理员权限？")
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            backup = True
        Else
            backup = False
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton2.Checked Then
            bonus_ahead = True
            MsgBox("你bonus levels一定没有全部通关！什么？通了？关我啥事！" + vbCrLf + "无论如何，不要再点我了！", MsgBoxStyle.Information, "让我猜猜")
        End If
    End Sub
    REM 选择.xml文件位置
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Try
            OpenFileDialog2.ShowDialog()
            If OpenFileDialog2.FileName <> "" Then
                BinLocation = OpenFileDialog2.FileName
                Button5.Enabled = True
            Else
                MsgBox("你给我个空位置干啥？玩我呢！", MsgBoxStyle.Exclamation, "喂喂喂")
            End If
        Catch ex As Exception
            MsgBox("选择路径时发生错误。请确保操作请求没有被安全程序拦截。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex.ToString, MsgBoxStyle.Critical, "出错了")
        End Try
    End Sub
    REM 选择安装
    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Try
            TextBox3.Text = ""
            If TextBox1.Text = "" Then
                TextBox3.Text = "安装失败：未确定edge.exe位置。" + vbCrLf + TextBox3.Text
                MsgBox("你还没确定edge.exe位置！", MsgBoxStyle.Exclamation, "出错啦")
            Else
                TextBox3.Text = "查找.eso文件......" + vbCrLf + TextBox3.Text
                Dim di As New DirectoryInfo(Mid(BinLocation, 1, InStrRev(BinLocation, "\") - 1))
                Dim EsoFiles As FileInfo()
                EsoFiles = di.GetFiles("*.eso")
                If EsoFiles.Length = 0 Then
                    '啥也不干
                ElseIf EsoFiles.Length > 1 Then
                    TextBox3.Text = "安装失败：无法确定目标.eso文件。" + vbCrLf + TextBox3.Text
                    MsgBox(".eso文件太多啦！我不知道到底选哪个TAT", MsgBoxStyle.Exclamation, "出错啦")
                    Exit Sub
                Else
                    TextBox3.Text = "复制.eso文件中，文件名：" + EsoFiles(0).ToString + vbCrLf + TextBox3.Text
                    EsoLocation = Mid(BinLocation, 1, InStrRev(BinLocation, "\")) + EsoFiles(0).ToString
                    File.Copy(EsoLocation, EdgeFolderLocation + "\models\" + EsoFiles(0).ToString, True)
                End If
                TextBox3.Text = "复制.bin文件中，文件名：" + Mid(BinLocation, InStrRev(BinLocation, "\") + 1) + vbCrLf + TextBox3.Text
                File.Copy(BinLocation, EdgeFolderLocation + "\levels\" + Mid(BinLocation, InStrRev(BinLocation, "\") + 1), True)
                TextBox3.Text = "准备修改mapping.xml......" + vbCrLf + TextBox3.Text
                bin_name = Mid(BinLocation, InStrRev(BinLocation, "\") + 1, InStrRev(BinLocation, ".") - InStrRev(BinLocation, "\") - 1)
                If XML_Modify(3) Then
                    Exit Sub
                End If
                TextBox3.Text = "安装完成！完成时的系统时间为：" + Now.TimeOfDay.ToString + vbCrLf + TextBox3.Text
            End If
        Catch ex As Exception
            TextBox3.Text = "安装失败：操作文件时发生异常。" + vbCrLf + TextBox3.Text
            MsgBox("安装关卡时出现错误。由于此错误是意料之外的，为了保证程序的正常运行，建议您退出并重新打开此程序。" + vbCrLf + vbCrLf + "可能导致此错误发生的原因：" + vbCrLf + "1.edge.exe的路径出错；" + vbCrLf + "2.程序权限不足，请尝试获取管理员权限；" + vbCrLf + "3.操作请求被安全程序拦截。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex.ToString, MsgBoxStyle.Critical, "出错了")
        End Try
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Try
            Process.Start("explorer.exe", EdgeFolderLocation)
        Catch ex As Exception
            MsgBox("启动文件资源管理器时发生错误，请确保此请求未被安全程序拦截。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex.ToString, MsgBoxStyle.Critical, "出错了")
        End Try
    End Sub
End Class
