Imports System.IO
Imports System.Xml
Public Class Form2
    Dim EdgeFolderLocation As String, backup, ask_before_operation As Boolean
    REM 卸载关卡
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If ListBox1.SelectedItem <> "" Then
            If ask_before_operation Then
                If MsgBox("确定要卸载此关卡吗？此操作无法自动还原。", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "提示") = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If
            If EdgeFolderLocation = "" Then
                MsgBox("你还没确定edge.exe位置！", MsgBoxStyle.Exclamation, "出错啦")
                Exit Sub
            Else
                Dim filename As String = ListBox1.SelectedItem
                Dim uninstall_succeed As Boolean = Not (Level_uninstall(filename))
                If uninstall_succeed Then
                    ListBox1.Items.Remove(ListBox1.SelectedItem)
                    MsgBox("卸载成功。", MsgBoxStyle.Information, "提示")
                Else
                    MsgBox("未成功卸载。此关卡可能已被卸载。", MsgBoxStyle.Exclamation, "提示")
                End If
            End If
        End If
    End Sub
    REM 从mapping.xml移除关卡
    Private Function Level_uninstall(filename As String) As Boolean
        'xml存在确认
        If Not (File.Exists(EdgeFolderLocation + "\levels\mapping.xml")) Then
            MsgBox("未在指定的位置找到mapping.xml，请确认edge.exe路径是否有误。", MsgBoxStyle.Exclamation, "出错了")
            Return True '出现异常
        End If
        If backup Then
            File.Copy(EdgeFolderLocation + "\levels\mapping.xml", EdgeFolderLocation + "\levels\mapping.xml.bak", True)
        End If
        'xml存在确认完成
        'xml编码检测
        Try
            Dim doc As New XmlDocument
            doc.Load(EdgeFolderLocation + "\levels\mapping.xml")
        Catch ex As Exception
            If MsgBox("读取mapping.xml时出现错误，这可能是xml的编码问题。是否尝试修改编码？注意：根据你的设置，原文件可能不会被备份。", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "出错了") = MsgBoxResult.Yes Then
                '修改编码
                Try
                    Dim s() As String = File.ReadAllLines(EdgeFolderLocation + "\levels\mapping.xml")
                    s(0) = "<?xml version=" + Chr(34) + "1.0" + Chr(34) + " encoding=" + Chr(34) + "utf-8" + Chr(34) + "?>"
                    File.WriteAllLines(EdgeFolderLocation + "\levels\mapping.xml", s)
                Catch ex1 As Exception
                    MsgBox("修改编码过程中出现错误，请尝试手动修改编码至UTF-8。若要了解更多，请到搜索引擎搜索" + Chr(34) + "记事本改编码格式" + Chr(34) + "。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex1.ToString, MsgBoxStyle.Critical, "出错了")
                    Return True '出现异常
                End Try
                '修改完成
                '修改完成后，再次检测xml是否能正常打开
                Try
                    Dim doc1 As New XmlDocument
                    doc1.Load(EdgeFolderLocation + "\levels\mapping.xml")
                Catch ex2 As Exception
                    MsgBox("读取mapping.xml时仍然出现错误，请确认mapping.xml的完整性。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex2.ToString, MsgBoxStyle.Critical, "出错了")
                    Return True '出现异常
                End Try
            Else
                MsgBox("由于您拒绝了修改请求，卸载过程被中止。", MsgBoxStyle.Exclamation, "卸载中止")
                Return True '出现异常
            End If
        End Try
        'xml编码检测完成
        'xml修改
        Try
            Dim xsettings As New XmlReaderSettings
            xsettings.IgnoreComments = True '忽略注释,防止读取节点时出错   
            Dim xreader As XmlReader = XmlReader.Create(EdgeFolderLocation + "\levels\mapping.xml", xsettings)
            Dim xdoc As New XmlDocument
            xdoc.Load(xreader)
            xreader.Close()
            Dim node_levels As XmlNode = xdoc.SelectSingleNode("levels") '读取根节点levels
            Dim node_list_levels As XmlNodeList = node_levels.ChildNodes '获取levels的所有子节点
            Dim node_bonus_find As Boolean = False '用于判断是否找到bonus节点
            For Each xn As XmlNode In node_list_levels '遍历所有子节点
                If xn.Name = "bonus" Then '当子节点为bonus时
                    node_bonus_find = True '已找到bonus节点
                    Dim node_list_bonus As XmlNodeList = xn.ChildNodes '获取bonus的所有子节点
                    For Each xn1 As XmlNode In node_list_bonus '遍历所有子节点
                        Dim attribute_list_level As XmlAttributeCollection = xn1.Attributes() '获取这个level节点的所有属性
                        Dim attribute_filename_existed As XmlAttribute = attribute_list_level.ItemOf("filename") '获取filename属性值
                        If attribute_filename_existed.Value = filename Then
                            xn.RemoveChild(xn1)
                            xdoc.Save(EdgeFolderLocation + "\levels\mapping.xml")
                            Return False '修改成功
                        End If
                    Next
                    Exit For
                End If
            Next
            If Not (node_bonus_find) Then
                MsgBox("未在mapping.xml中找到bonus节点，请确认mapping.xml的完整性。", MsgBoxStyle.Exclamation, "出错了")
                Return True '出现异常
            End If
        Catch ex As Exception
            MsgBox("操作mapping.xml时发生错误。由于此错误是意料之外的，为了保证程序的正常运行，建议您退出并重新打开此程序。" + vbCrLf + vbCrLf + "可能导致此错误发生的原因：" + vbCrLf + "1.程序权限不足，请尝试获取管理员权限；" + vbCrLf + "2.操作请求被安全程序拦截；" + vbCrLf + "3.其他程序正在占用mapping.xml；" + vbCrLf + "4.mapping.xml结构不完整。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex.ToString, MsgBoxStyle.Critical, "出错了")
            Return True '出现异常
        End Try
        Return True '如果能运行到这里，说明并没有找到对应关卡索引，因此相当于修改失败
        'xml修改完成
    End Function
    REM 删除关卡
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If ListBox1.SelectedItem <> "" Then
            If ask_before_operation Then
                If MsgBox("确定要删除此关卡吗？此操作无法自动还原。", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "提示") = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If
            If EdgeFolderLocation = "" Then
                MsgBox("你还没确定edge.exe位置！", MsgBoxStyle.Exclamation, "出错啦")
                Exit Sub
            Else
                Dim filename As String = ListBox1.SelectedItem
                Dim delete_succeed As Boolean = False
                Dim uninstall_succeed As Boolean = Not (Level_uninstall(filename))
                If File.Exists(EdgeFolderLocation + "\levels\" + filename + ".bin") Then
                    Try
                        File.Delete(EdgeFolderLocation + "\levels\" + filename + ".bin")
                        delete_succeed = True
                    Catch ex As Exception
                        MsgBox("删除关卡文件时发生错误。" + vbCrLf + vbCrLf + "可能导致此错误发生的原因：" + vbCrLf + "1.程序权限不足，请尝试获取管理员权限；" + vbCrLf + "2.操作请求被安全程序拦截；" + vbCrLf + "3.其他程序正在占用此关卡文件；" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex.ToString, MsgBoxStyle.Critical, "出错了")
                    End Try
                End If
                If delete_succeed Then
                    ListBox1.Items.Remove(ListBox1.SelectedItem)
                    If uninstall_succeed Then
                        MsgBox("删除成功。", MsgBoxStyle.Information, "提示")
                    Else
                        MsgBox("删除成功，但在删除过程中遇到了问题。" + vbCrLf + vbCrLf + "未能移除mapping.xml中此关卡的索引，但在levels文件夹下找到了对应的关卡文件。", MsgBoxStyle.Information, "提示")
                    End If
                Else
                    If uninstall_succeed Then
                        ListBox1.Items.Remove(ListBox1.SelectedItem)
                        MsgBox("删除成功，但在删除过程中遇到了问题。" + vbCrLf + vbCrLf + "成功移除了mapping.xml中此关卡的索引，但在levels文件夹下未能找到对应的关卡文件。", MsgBoxStyle.Information, "提示")
                    Else
                        MsgBox("未成功删除。该关卡可能已被删除。", MsgBoxStyle.Exclamation, "提示")
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        EdgeFolderLocation = Form1.EdgeFolderLocation
        backup = Form1.backup
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            ask_before_operation = True
        Else
            ask_before_operation = False
        End If
    End Sub
    REM 读取mapping.xml
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ListBox1.Items.Clear()
        If EdgeFolderLocation = "" Then
            MsgBox("你还没确定edge.exe位置！", MsgBoxStyle.Exclamation, "出错啦")
            Exit Sub
        Else
            'xml存在确认
            If Not (File.Exists(EdgeFolderLocation + "\levels\mapping.xml")) Then
                MsgBox("未在指定的位置找到mapping.xml，请确认edge.exe路径是否有误。", MsgBoxStyle.Exclamation, "出错了")
                Exit Sub
            End If
            'xml存在确认完成
            'xml编码检测
            Try
                Dim doc As New XmlDocument
                doc.Load(EdgeFolderLocation + "\levels\mapping.xml")
            Catch ex As Exception
                If MsgBox("读取mapping.xml时出现错误，这可能是xml的编码问题。是否尝试修改编码？注意：根据你的设置，原文件可能不会被备份。", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "出错了") = MsgBoxResult.Yes Then
                    '修改编码
                    If backup Then
                        File.Copy(EdgeFolderLocation + "\levels\mapping.xml", EdgeFolderLocation + "\levels\mapping.xml.bak", True)
                    End If
                    Try
                        Dim s() As String = File.ReadAllLines(EdgeFolderLocation + "\levels\mapping.xml")
                        s(0) = "<?xml version=" + Chr(34) + "1.0" + Chr(34) + " encoding=" + Chr(34) + "utf-8" + Chr(34) + "?>"
                        File.WriteAllLines(EdgeFolderLocation + "\levels\mapping.xml", s)
                    Catch ex1 As Exception
                        MsgBox("修改编码过程中出现错误，请尝试手动修改编码至UTF-8。若要了解更多，请到搜索引擎搜索" + Chr(34) + "记事本改编码格式" + Chr(34) + "。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex1.ToString, MsgBoxStyle.Critical, "出错了")
                        Exit Sub
                    End Try
                    '修改完成
                    '修改完成后，再次检测xml是否能正常打开
                    Try
                        Dim doc1 As New XmlDocument
                        doc1.Load(EdgeFolderLocation + "\levels\mapping.xml")
                    Catch ex2 As Exception
                        MsgBox("读取mapping.xml时仍然出现错误，请确认mapping.xml的完整性。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex2.ToString, MsgBoxStyle.Critical, "出错了")
                        Exit Sub
                    End Try
                Else
                    MsgBox("由于您拒绝了修改请求，读取过程被中止。", MsgBoxStyle.Exclamation, "读取中止")
                    Exit Sub
                End If
            End Try
            'xml编码检测完成
            'xml读取
            Try
                Dim xsettings As New XmlReaderSettings
                xsettings.IgnoreComments = True '忽略注释,防止读取节点时出错   
                Dim xreader As XmlReader = XmlReader.Create(EdgeFolderLocation + "\levels\mapping.xml", xsettings)
                Dim xdoc As New XmlDocument
                xdoc.Load(xreader)
                xreader.Close()
                Dim node_levels As XmlNode = xdoc.SelectSingleNode("levels") '读取根节点levels
                Dim node_list_levels As XmlNodeList = node_levels.ChildNodes '获取levels的所有子节点
                Dim node_bonus_find As Boolean = False '用于判断是否找到bonus节点
                For Each xn As XmlNode In node_list_levels '遍历所有子节点
                    If xn.Name = "bonus" Then '当子节点为bonus时
                        node_bonus_find = True '已找到bonus节点
                        Dim node_list_bonus As XmlNodeList = xn.ChildNodes '获取bonus的所有子节点
                        For Each xn1 As XmlNode In node_list_bonus '遍历所有子节点
                            Dim attribute_list_level As XmlAttributeCollection = xn1.Attributes() '获取这个level节点的所有属性
                            Dim attribute_filename_existed As XmlAttribute = attribute_list_level.ItemOf("filename") '获取filename属性值
                            ListBox1.Items.Add(attribute_filename_existed.Value)
                        Next
                        Exit For
                    End If
                Next
                If Not (node_bonus_find) Then
                    MsgBox("未在mapping.xml中找到bonus节点，请确认mapping.xml的完整性。", MsgBoxStyle.Exclamation, "出错了")
                    Exit Sub
                End If
            Catch ex As Exception
                MsgBox("操作mapping.xml时发生错误。由于此错误是意料之外的，为了保证程序的正常运行，建议您退出并重新打开此程序。" + vbCrLf + vbCrLf + "可能导致此错误发生的原因：" + vbCrLf + "1.程序权限不足，请尝试获取管理员权限；" + vbCrLf + "2.操作请求被安全程序拦截；" + vbCrLf + "3.其他程序正在占用mapping.xml；" + vbCrLf + "4.mapping.xml结构不完整。" + vbCrLf + vbCrLf + "错误的详细信息：" + vbCrLf + ex.ToString, MsgBoxStyle.Critical, "出错了")
                Exit Sub
            End Try
            'xml读取完成
        End If
        If ListBox1.Items.Count > 0 Then
            Button3.Enabled = True
            Button4.Enabled = True
        End If
    End Sub
End Class