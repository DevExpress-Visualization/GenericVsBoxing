using DevExpress.XtraCharts;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Analizer {
    public partial class MainForm : Form {
        string rootFolder; 

        public MainForm() {
            InitializeComponent();

            //Workaround
            XYDiagram diagram = (XYDiagram)chartControl.Diagram;
            for (int i = 0; i < 4; i++) {
                ((XYDiagramSeriesViewBase)chartControl.Series[4 + i].View).AxisY = diagram.SecondaryAxesY[0];
                ((XYDiagramSeriesViewBase)chartControl.Series[8 + i].View).AxisY = diagram.SecondaryAxesY[1];
            }

            rootFolder = Directory.GetParent(Application.ExecutablePath).Parent.Parent.Parent.FullName;
        }
        void btnRun_Click(object sender, EventArgs e) {
            RunProcess(rootFolder + @"\Bin\Tests\Full\Release\Tests.exe");
            RunProcess(rootFolder + @"\Bin\Tests\Core\Release\netcoreapp3.1\TestsCore.exe");
            LoadData();
        }
        void RunProcess(string file) {
            if (!File.Exists(file)) {
                MessageBox.Show(string.Format("The {0} file is not exists\r\nPlease build the solution in the Release mode.", file), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            ProcessStartInfo info = new ProcessStartInfo(file);
            info.WorkingDirectory = Path.GetDirectoryName(file);
            Process process = Process.Start(info);
            process.WaitForExit();
        }
        void btnLoad_Click(object sender, EventArgs e) {
            LoadData();
        }
        void LoadData() {
            foreach (Series series in chartControl.Series)
                series.Points.Clear();
            LoadData("Core");
            LoadData("Full");
        }
        void LoadData(string name) {
            string actualPath = rootFolder + @"\Data\" + name + ".xml";
            if (!File.Exists(actualPath)) {
                MessageBox.Show(string.Format("The {0} file is not exists\r\nPlease press the \"Run Tests\" button.", actualPath), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            XmlDocument document = new XmlDocument();
            document.Load(actualPath);
            foreach (XmlNode child in document.DocumentElement.ChildNodes) {
                string testType = string.Empty;
                string dataType = string.Empty;
                int itemsCount = 0;
                int milliseconds = 0;
                foreach (XmlNode attribute in child.ChildNodes) {
                    switch (attribute.Name) {
                        case "TestType":
                            testType = attribute.InnerText;
                            break;
                        case "DataType":
                            dataType = attribute.InnerText;
                            break;
                        case "ItemsCount":
                            itemsCount = int.Parse(attribute.InnerText);
                            break;
                        case "Milliseconds":
                            milliseconds = int.Parse(attribute.InnerText);
                            break;
                    }
                }
                Series series = chartControl.GetSeriesByName(dataType + testType + name);
                if (series != null)
                    series.Points.AddPoint(itemsCount, milliseconds);
            }
        }
    }
}
