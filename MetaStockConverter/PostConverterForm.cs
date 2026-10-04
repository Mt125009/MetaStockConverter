using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace MetaStockConverter
{
    public class PostConverterForm : Form
    {
        string[,] _converterLines;
        public PostConverterForm(string[,] converterLines)
        {
            _converterLines = converterLines;
            InitializeComponent();
            label2.Text += converterLines.GetLength(0).ToString();
            LoadDataGrid(converterLines);
        }
        
        private void LoadDataGrid(string[,] converterLines)
        {
            try
            {
                var dataTable = new DataTable();
                for (int i = 0; i < converterLines.GetLength(1)-1; i++)
                {
                    dataTable.Columns.Add(converterLines[0, i]);
                }
                for (int j = 1; j < converterLines.GetLength(0); j++)
                {
                    var dataRow = dataTable.NewRow();
                    for (int i = 0; i < converterLines.GetLength(1)-1; i++)
                    {
                        dataRow[i] = converterLines[j, i];
                    }
                    dataTable.Rows.Add(dataRow);
                }
                dataGrid1.DataSource = dataTable;
            }
            catch (Exception e)
            {
                MessageBox.Show("Errore durante il caricamento dei dati nella griglia: " + e.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FileSaver(string filePath)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                {
                    for (int j = 0; j < _converterLines.GetLength(0); j++)
                    {
                        string line = "";
                        for (int i = 0; i < _converterLines.GetLength(1); i++)
                        {
                            line += _converterLines[j, i];
                            if (i < _converterLines.GetLength(1) - 1)
                            {
                                line += ","; // Tab-separated values
                            }
                        }
                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Errore durante il salvataggio del file: " + e.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1MinSize = 200;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2MinSize = 900;
            this.splitContainer1.Size = new System.Drawing.Size(1182, 553);
            this.splitContainer1.SplitterDistance = 278;
            this.splitContainer1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoEllipsis = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(0, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(278, 342);
            this.label3.TabIndex = 3;
            this.label3.Text = "I dati sono stati convertiti in formato MetaStock compatibile. Controllare che la" + " conversione sia stata effettuata correttamente prima di procedere.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 453);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(278, 100);
            this.panel1.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(148, 37);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Salva";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Location = new System.Drawing.Point(41, 37);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Annulla";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(278, 111);
            this.label1.TabIndex = 1;
            this.label1.Text = "Risultato conversione";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dataGrid1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 47);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(900, 506);
            this.panel3.TabIndex = 2;
            // 
            // dataGrid1
            // 
            this.dataGrid1.AllowNavigation = false;
            this.dataGrid1.AllowSorting = false;
            this.dataGrid1.DataMember = "";
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(0, 0);
            this.dataGrid1.Margin = new System.Windows.Forms.Padding(10);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.PreferredColumnWidth = 100;
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.Size = new System.Drawing.Size(900, 506);
            this.dataGrid1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(900, 47);
            this.panel2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(651, 47);
            this.label2.TabIndex = 0;
            this.label2.Text = "Righe convertite:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PostConverterForm
            // 
            this.ClientSize = new System.Drawing.Size(1182, 553);
            this.Controls.Add(this.splitContainer1);
            this.Name = "PostConverterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Risultato conversione";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label label3;

        private System.Windows.Forms.DataGrid dataGrid1;

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.Panel panel2;

        private System.Windows.Forms.Button button2;

        private System.Windows.Forms.Panel panel1;

        private System.Windows.Forms.Button button1;

        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.SplitContainer splitContainer1;

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                try
                {
                    FileSaver(filePath);
                    MessageBox.Show("File salvato correttamente.", "Salvataggio completato", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Errore durante il salvataggio del file: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}