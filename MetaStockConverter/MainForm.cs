using System;
using System.Drawing;
using System.Windows.Forms;

namespace MetaStockConverter
{
    public class MainForm : Form
    {
        private TextBox txtFile;
        private Button btnBrowse;
        private Button btnConvert;

        public MainForm()
        {
            Text = "MetaStock Converter";
            Width = 600;
            Height = 300;
            StartPosition = FormStartPosition.CenterScreen;

            Label label = new Label();
            label.Text = "File .asc MultiCharts:";
            label.AutoSize = true;
            label.Location = new Point(30, 30);

            txtFile = new TextBox();
            txtFile.Location = new Point(30, 60);
            txtFile.Width = 420;

            btnBrowse = new Button();
            btnBrowse.Text = "Seleziona asc...";
            btnBrowse.Location = new Point(460, 58);
            btnBrowse.Width = 100;
            btnBrowse.Click += BtnBrowse_Click;

            btnConvert = new Button();
            btnConvert.Text = "Converti";
            btnConvert.Location = new Point(30, 110);
            btnConvert.Width = 120;
            btnConvert.Click += BtnConvert_Click;

            Controls.Add(label);
            Controls.Add(txtFile);
            Controls.Add(btnBrowse);
            Controls.Add(btnConvert);
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Seleziona file asc MultiCharts";
            dialog.Filter = "File asc (*.asc)|*.asc|Tutti i file (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = dialog.FileName;
            }
        }

        private void BtnConvert_Click(object sender, EventArgs e)
        {
            if (txtFile.Text == "")
            {
                MessageBox.Show(
                    "Seleziona prima un file asc.",
                    "MetaStock Converter",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            PreConverterForm preConverterForm = new PreConverterForm();
            preConverterForm.ShowDialog();
            
            MessageBox.Show(
                "File selezionato:\r\n" + txtFile.Text,
                "Test conversione",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}