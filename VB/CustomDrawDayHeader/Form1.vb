Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.IO
#Region "#usings"
Imports DevExpress.XtraScheduler
Imports DevExpress.XtraScheduler.Drawing
Imports System.Drawing.Drawing2D
#End Region ' #usings

Namespace CustomDrawDayHeader
    Partial Public Class Form1
        Inherits Form

        Private Const aptDataFileName As String = "..\..\Data\appointments.xml"
        Private Const resDataFileName As String = "..\..\Data\resources.xml"

        Public Sub New()
            InitializeComponent()

           schedulerControl1.Start = New Date(2008, 7, 11)
           FillData()
        End Sub

        #Region "FillData"
        Private Sub FillData()
            Dim customNameMapping As New AppointmentCustomFieldMapping("CustomName", "CustomName")
            Dim customStatusMapping As New AppointmentCustomFieldMapping("CustomStatus", "CustomStatus")
            schedulerStorage1.Appointments.CustomFieldMappings.Add(customNameMapping)
            schedulerStorage1.Appointments.CustomFieldMappings.Add(customStatusMapping)
            FillResourcesStorage(schedulerStorage1.Resources.Items, resDataFileName)
            FillAppointmentsStorage(schedulerStorage1.Appointments.Items, aptDataFileName)
        End Sub

        Private Shared Function GetFileStream(ByVal fileName As String) As Stream
            Return (New StreamReader(fileName)).BaseStream
        End Function

        Private Shared Sub FillAppointmentsStorage(ByVal c As AppointmentCollection, ByVal fileName As String)
            Using stream As Stream = GetFileStream(fileName)
                c.ReadXml(stream)
                stream.Close()
            End Using
        End Sub

        Private Shared Sub FillResourcesStorage(ByVal c As ResourceCollection, ByVal fileName As String)
            Using stream As Stream = GetFileStream(fileName)
                c.ReadXml(stream)
                stream.Close()
            End Using
        End Sub
        #End Region

        Private Sub schedulerStorage_AppointmentsChanged(ByVal sender As Object, ByVal e As PersistentObjectsEventArgs) Handles schedulerStorage1.AppointmentsChanged, schedulerStorage1.AppointmentsInserted, schedulerStorage1.AppointmentsDeleted
            schedulerStorage1.Appointments.Items.WriteXml(aptDataFileName)
        End Sub

        Private Sub cb_CustomDraw_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cb_CustomDraw.CheckedChanged
            If cb_CustomDraw.Checked Then
                AddHandler schedulerControl1.CustomDrawDayHeader, AddressOf schedulerControl1_CustomDrawDayHeader
            Else
                RemoveHandler schedulerControl1.CustomDrawDayHeader, AddressOf schedulerControl1_CustomDrawDayHeader
            End If
            schedulerControl1.Refresh()

        End Sub
#Region "#customdrawdayheader"
Private Sub schedulerControl1_CustomDrawDayHeader(ByVal sender As Object, ByVal e As CustomDrawObjectEventArgs)
    Dim header As DayHeader = TryCast(e.ObjectInfo, DayHeader)
    ' Draws the outer rectangle.
    e.Cache.FillRectangle(New LinearGradientBrush(e.Bounds, Color.LightBlue, Color.Blue, LinearGradientMode.Vertical), e.Bounds)
    Dim innerRect As Rectangle = Rectangle.Inflate(e.Bounds, -2, -2)
    ' Draws the inner rectangle.
    e.Cache.FillRectangle(New LinearGradientBrush(e.Bounds, Color.Blue, Color.LightSkyBlue, LinearGradientMode.Vertical), innerRect)
    ' Draws the header's caption.
    e.Cache.DrawString(header.Caption, header.Appearance.HeaderCaption.Font, New SolidBrush(Color.White), innerRect, header.Appearance.HeaderCaption.GetStringFormat())
    e.Handled = True
End Sub
#End Region ' #customdrawdayheader
    End Class
End Namespace