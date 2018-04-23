using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
#region #usings
using DevExpress.XtraScheduler;
using DevExpress.XtraScheduler.Drawing;
using System.Drawing.Drawing2D;
#endregion #usings

namespace CustomDrawDayHeader {
    public partial class Form1 : Form {
        const string aptDataFileName = @"..\..\Data\appointments.xml";
        const string resDataFileName = @"..\..\Data\resources.xml";
        
        public Form1() {
            InitializeComponent();
           
           schedulerControl1.Start = new DateTime(2008, 7, 11);
           FillData();
        }

        #region FillData
        void FillData() {
            AppointmentCustomFieldMapping customNameMapping = new AppointmentCustomFieldMapping("CustomName", "CustomName");
            AppointmentCustomFieldMapping customStatusMapping = new AppointmentCustomFieldMapping("CustomStatus", "CustomStatus");
            schedulerStorage1.Appointments.CustomFieldMappings.Add(customNameMapping);
            schedulerStorage1.Appointments.CustomFieldMappings.Add(customStatusMapping);
            FillResourcesStorage(schedulerStorage1.Resources.Items, resDataFileName);
            FillAppointmentsStorage(schedulerStorage1.Appointments.Items, aptDataFileName);
        }

        static Stream GetFileStream(string fileName) {
            return new StreamReader(fileName).BaseStream;
        }

        static void FillAppointmentsStorage(AppointmentCollection c, string fileName) {
            using (Stream stream = GetFileStream(fileName)) {
                c.ReadXml(stream);
                stream.Close();
            }
        }

        static void FillResourcesStorage(ResourceCollection c, string fileName) {
            using (Stream stream = GetFileStream(fileName)) {
                c.ReadXml(stream);
                stream.Close();
            }
        }
        #endregion

        private void schedulerStorage_AppointmentsChanged(object sender, PersistentObjectsEventArgs e) {
            schedulerStorage1.Appointments.Items.WriteXml(aptDataFileName);
        }

        private void cb_CustomDraw_CheckedChanged(object sender, EventArgs e) {
            if(cb_CustomDraw.Checked) {
                schedulerControl1.CustomDrawDayHeader += new DevExpress.XtraScheduler.CustomDrawObjectEventHandler(schedulerControl1_CustomDrawDayHeader);
            }
            else {
                schedulerControl1.CustomDrawDayHeader -= new DevExpress.XtraScheduler.CustomDrawObjectEventHandler(schedulerControl1_CustomDrawDayHeader);
            }
            schedulerControl1.Refresh();

        }
#region #customdrawdayheader
private void schedulerControl1_CustomDrawDayHeader(object sender, CustomDrawObjectEventArgs e) {
    DayHeader header = e.ObjectInfo as DayHeader;
    // Draws the outer rectangle.
    e.Cache.FillRectangle(new LinearGradientBrush(e.Bounds,
        Color.LightBlue, Color.Blue, LinearGradientMode.Vertical), e.Bounds);
    Rectangle innerRect = Rectangle.Inflate(e.Bounds, -2, -2);
    // Draws the inner rectangle.
    e.Cache.FillRectangle(new LinearGradientBrush(e.Bounds,
        Color.Blue, Color.LightSkyBlue, LinearGradientMode.Vertical), innerRect);
    // Draws the header's caption.
    e.Cache.DrawString(header.Caption, header.Appearance.HeaderCaption.Font,
        new SolidBrush(Color.White), innerRect,
        header.Appearance.HeaderCaption.GetStringFormat());
    e.Handled = true;
}
#endregion #customdrawdayheader
    }
}