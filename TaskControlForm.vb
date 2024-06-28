Public Class TaskControlForm

    Dim TaskList As List(Of cTask)
    Dim VerticalBorders As Boolean = True

    Private Sub TaskControlForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        InitializeTasks()

        For Each tmpTask As cTask In TaskList

            Dim tmpTC = New TaskControl
            tmpTC.Task = tmpTask

            tmpTC.Initialize()
            tmpTC.VerticalBorders(VerticalBorders)

            AddHandler tmpTC.LB_Image.MouseMove, AddressOf TC_MouseMove
            AddHandler tmpTC.LB_Image.QueryContinueDrag, AddressOf TC_QueryContinueDrag

            FLP.Controls.Add(tmpTC)
            SetupAnchors()

        Next

    End Sub

    Private Sub TC_MouseMove(sender As Object, e As MouseEventArgs)
        If e.Button = MouseButtons.Left Then
            Dim tmpTC As Label = DirectCast(sender, Label)
            Dim tmpTask As TaskControl = tmpTC.Parent.Parent
            tmpTask.TLP.BackColor = Color.Orange
            tmpTC.DoDragDrop(tmpTC, DragDropEffects.Move)
        End If
    End Sub

    Private Sub TC_QueryContinueDrag(sender As Object, e As QueryContinueDragEventArgs)
        If e.Action = DragAction.Drop OrElse e.Action = DragAction.Cancel Then
            Dim tmpTC As Label = DirectCast(sender, Label)
            Dim tmpTask As TaskControl = tmpTC.Parent.Parent
            tmpTask.TLP.BackColor = Color.White
        End If
    End Sub

    Private Sub SetupAnchors()
        If FLP.Controls.Count > 0 Then
            For i = 0 To FLP.Controls.Count - 1
                Dim c As Control = FLP.Controls(i)
                If i = 0 Then
                    ' Its the first control, all subsequent controls follow
                    ' the anchor behavior of this control.

                    c.Width = FLP.Width - 2
                    c.Anchor = AnchorStyles.Left + AnchorStyles.Top

                    'If FLP_Richieste.VerticalScroll.Visible Then c.Width += -SystemInformation.VerticalScrollBarWidth
                Else

                    ' It is not the first control. Set its anchor to
                    ' copy the width of the first control in the list.
                    c.Anchor = AnchorStyles.Left + AnchorStyles.Right

                End If
            Next
        End If
    End Sub

    Private Sub InitializeTasks()

        TaskList = New List(Of cTask)

        Dim tmpTask As New cTask("Title")
        TaskList.Add(tmpTask)

        tmpTask = New cTask("Open\Save")
        TaskList.Add(tmpTask)

        tmpTask = New cTask("Save as 2D")
        TaskList.Add(tmpTask)

        tmpTask = New cTask("Save as 3D")
        TaskList.Add(tmpTask)

        tmpTask = New cTask("Activate and update all")
        TaskList.Add(tmpTask)

        tmpTask = New cTask("Property find\replace")
        TaskList.Add(tmpTask)

        tmpTask = New cTask("Update physical properties")
        TaskList.Add(tmpTask)

        tmpTask = New cTask("Variable add\edit\expose")
        TaskList.Add(tmpTask)

    End Sub

    Private Sub FLP_SizeChanged(sender As Object, e As EventArgs) Handles FLP.SizeChanged

        For i = 0 To FLP.Controls.Count - 1
            Dim c As Control = FLP.Controls(i)
            If i = 0 Then

                c.Width = FLP.Width - 2

            End If
        Next

    End Sub

    Private Sub FLP_DragEnter(sender As Object, e As DragEventArgs) Handles FLP.DragEnter
        e.Effect = If(e.Data.GetDataPresent(GetType(Label)), DragDropEffects.All, DragDropEffects.None)
    End Sub

    Private Sub FLP_DragOver(sender As Object, e As DragEventArgs) Handles FLP.DragOver
        Dim pt As Point = FLP.PointToClient(New Point(e.X, e.Y))
        Dim overTC As TaskControl = FLP.GetChildAtPoint(pt)

        If Not IsNothing(overTC) Then
            Dim TC As Label = DirectCast(e.Data.GetData(GetType(Label)), Label)
            If Not TC Is overTC.LB_Image Then
                Dim newIndex As Integer = FLP.Controls.IndexOf(overTC)
                If newIndex <> FLP.Controls.IndexOf(TC) Then
                    FLP.Controls.SetChildIndex(TC.Parent.Parent, newIndex)
                End If
            End If
        End If
    End Sub

End Class


