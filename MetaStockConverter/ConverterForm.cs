using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MetaStockConverter
{
    public class ConverterForm : BaseForm
    {
        // Global variable to hold the file lines
        private readonly string _filePath;
        String[,] _fileLines; // Original Data
        String[,] _convertedLines; // Converted Data

        public ConverterForm(String filePath)
        {
            // Set the form properties
            InitializeComponent();

            _filePath = filePath;

            Shown += ConverterForm_Shown;
        }

        private void ConverterForm_Shown(object sender, EventArgs e)
        {
            // Write the file to the matrix array
            _fileLines = OpenFile(_filePath);
            // Initialize the converted lines array with the same number of rows as the original file lines and 8 columns
            _convertedLines = new String[_fileLines.GetLength(0), _fileLines.GetLength(1) + 1];
            // Call the Converter method to process the file lines
            Boolean result = Converter();
            if (result)
            {
                MessageBox.Show("Conversione completata con successo.\nI dati convertiti sono disponibili nella finestra successiva.", "Info", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                PostConverterForm postConverterForm = new PostConverterForm(_convertedLines);
                postConverterForm.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Errore durante la conversione.", "Errore", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Close();
            }
        }

        private String[,] OpenFile(String filePath)
        {
            try
            {
                String[] lines = File.ReadAllLines(filePath);
                // Create a 2D array to hold the data
                // The first dimension is the number of lines, the second is the number of columns (7 in this case)
                String[,] matrix = new String[lines.Length, 8];

                foreach (String line in lines)
                {
                    // Process each line and populate the 2D array
                    String[] columns = line.Split(',');
                    for (int i = 0; i < 7; i++)
                    {
                        matrix[Array.IndexOf(lines, line), i] = columns[i];
                    }
                }

                return matrix;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante la lettura del file: " + ex.Message, "Errore", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }
        }

        private Boolean Converter()
        {
            int totalCell = _fileLines.GetLength(0) * _fileLines.GetLength(1);
            int splitCell = totalCell / 9;
            int processedCell = 0;
            progressBar1.Maximum = totalCell;
            progressBar1.Value = processedCell;
            try
            {
                
                LabelUpdater(HeadLbl, Color.Yellow);
                // Insert the correct header in the first row of the fileLines array
                if (ApplyHeader())
                {
                    processedCell += splitCell;
                    FormUpdater(processedCell, headBox);
                    LabelUpdater(HeadLbl, Color.Green);
                }

                LabelUpdater(SymbolLbl, Color.Yellow);
                // Convert the symbols in the first column of the fileLines array
                if (ConvertSymbol())
                {
                    processedCell += splitCell;
                    FormUpdater(processedCell, symbolBox);
                    LabelUpdater(SymbolLbl, Color.Green);
                }

                LabelUpdater(perLbl, Color.Yellow);
                // Insert the <PER> tag
                if (InsertPerTag())
                {
                    processedCell += splitCell;
                    FormUpdater(processedCell, perBox);
                    LabelUpdater(perLbl, Color.Green);
                }

                LabelUpdater(dateLbl, Color.Yellow);
                // Convert date to YYYYMMGG
                if (ConvertDate())
                {
                    processedCell += splitCell;
                    FormUpdater(processedCell, dateBox);
                    LabelUpdater(dateLbl, Color.Green);
                    
                }

                LabelUpdater(openLbl, Color.Yellow);
                // Convert open
                if (ConvertOpen())
                {
                    processedCell += splitCell;
                    FormUpdater(processedCell, openBox);
                    LabelUpdater(openLbl, Color.Green);
                   
                }
    
                LabelUpdater(maxLbl, Color.Yellow);
                // Convert high
                if (ConvertHigh())
                {
                    processedCell += splitCell;
                    FormUpdater(processedCell, highBox);
                    LabelUpdater(maxLbl, Color.Green);
                }

                LabelUpdater(minLbl, Color.Yellow); 
                // Convert low
                if (ConvertLow())
                {
                    processedCell += splitCell;
                    FormUpdater(processedCell, lowBox);
                    LabelUpdater(minLbl, Color.Green);
                    
                }

                LabelUpdater(closeLbl, Color.Yellow);
                // Convert close
                if (ConvertClose())
                {
                    processedCell += splitCell;
                    FormUpdater(processedCell, closeBox);
                    LabelUpdater(closeLbl, Color.Green);
                    
                }

                LabelUpdater(totalLbl, Color.Yellow);
                // Convert TotalVolume
                if (ConvertTotalVolume())
                {
                    processedCell += splitCell;
                    FormUpdater(processedCell, totalBox);
                    LabelUpdater(totalLbl, Color.Green); 
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante la conversione: " + ex.Message, "Errore", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }

        private Boolean ApplyHeader()
        {
            try
            {
                if (String.Equals(_fileLines[0, 0]?.Trim(), "\"Symbol\"", StringComparison.OrdinalIgnoreCase) &&
                    _fileLines[0, 1] == "\"Date\"" &&
                    _fileLines[0, 2] == "\"Open\"" &&
                    _fileLines[0, 3] == "\"High\"" &&
                    _fileLines[0, 4] == "\"Low\"" &&
                    _fileLines[0, 5] == "\"Close\"" &&
                    _fileLines[0, 6] == "\"TotalVolume\"")
                {
                    _convertedLines[0, 0] = "<TICKER>";
                    _convertedLines[0, 1] = "<PER>";
                    _convertedLines[0, 2] = "<DTYYYYMMDD>";
                    _convertedLines[0, 3] = "<OPEN>";
                    _convertedLines[0, 4] = "<HIGH>";
                    _convertedLines[0, 5] = "<LOW>";
                    _convertedLines[0, 6] = "<CLOSE>";
                    _convertedLines[0, 7] = "<VOL>";

                    return true;
                }

                MessageBox.Show("Il file non contiene l'intestazione corretta.", "Errore", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                LabelUpdater(HeadLbl, Color.Red);
                throw   new Exception("Intestazione non valida.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante l'applicazione dell'intestazione: " + ex.Message, "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LabelUpdater(HeadLbl, Color.DarkRed);
                throw new Exception("Intestazione non valida.");
            }
        }

        private Boolean ConvertSymbol()
        {
            try
            {
                for (int i = 1; i < _fileLines.GetLength(0); i++)
                {
                    if (_fileLines[i, 0] != null)
                    {
                        _convertedLines[i, 0] = _fileLines[i, 0];
                    }
                    else
                    {
                        MessageBox.Show("Simbolo mancante alla riga " + (i + 1) + ".", "Errore", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        LabelUpdater(HeadLbl, Color.Red);
                        throw new Exception("Simbolo mancante alla riga " + (i + 1) + ".");
                    }
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Errore durante la conversione dei simboli: " + e.Message, "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LabelUpdater(HeadLbl, Color.DarkRed);
                throw new Exception("Errore durante la conversione dei simboli: " + e.Message);
            }
        }

        private Boolean InsertPerTag()
        {
            try
            {
                for (int i = 1; i < _fileLines.GetLength(0); i++)
                {
                    _convertedLines[i, 1] = "D";
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante l'inserimento del tag <PER>: " + ex.Message, "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LabelUpdater(HeadLbl, Color.DarkRed);
                throw new Exception("Errore durante l'inserimento del tag <PER>: " + ex.Message);
            }
        }

        private Boolean ConvertDate()
        {
            try
            {
                for (int i = 1; i < _fileLines.GetLength(0); i++)
                {
                    if (_fileLines[i, 1] != null)
                    {
                        DateTime date;
                        if (DateTime.TryParse(_fileLines[i, 1], out date))
                        {
                            _convertedLines[i, 2] = date.ToString("yyyyMMdd");
                        }
                        else
                        {
                            MessageBox.Show("Formato data non valido alla riga " + (i + 1) + ": " + _fileLines[i, 1],
                                "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            LabelUpdater(HeadLbl, Color.Red);
                            throw new Exception("Formato data non valido alla riga " + (i + 1) + ": " + _fileLines[i, 1]);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Data mancante alla riga " + (i + 1) + ".", "Errore", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        LabelUpdater(HeadLbl, Color.Red);
                        throw new Exception("Data mancante alla riga " + (i + 1) + ".");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante la conversione delle date: " + ex.Message, "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LabelUpdater(HeadLbl, Color.DarkRed);
                throw new Exception("Errore durante la conversione delle date: " + ex.Message);
            }
        }

        private Boolean ConvertOpen()
        {
            try
            {
                for (int i = 1; i < _fileLines.GetLength(0); i++)
                {
                    if (_fileLines[i, 2] != null)
                    {
                        _convertedLines[i, 3] = _fileLines[i, 2];
                    }
                    else
                    {
                        MessageBox.Show("Valore Open mancante alla riga " + (i + 1) + ".", "Errore",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        LabelUpdater(HeadLbl, Color.Red);
                        throw new Exception("Valore Open mancante alla riga " + (i + 1) + ".");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante la conversione dei valori Open: " + ex.Message, "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LabelUpdater(HeadLbl, Color.DarkRed);
                throw new Exception("Errore durante la conversione dei valori Open: " + ex.Message);
            }
        }

        private Boolean ConvertHigh()
        {
            try
            {
                for (int i = 1; i < _fileLines.GetLength(0); i++)
                {
                    if (_fileLines[i, 3] != null)
                    {
                        _convertedLines[i, 4] = _fileLines[i, 3];
                    }
                    else
                    {
                        MessageBox.Show("Valore High mancante alla riga " + (i + 1) + ".", "Errore",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        LabelUpdater(HeadLbl, Color.Red);
                        throw new Exception("Valore High mancante alla riga " + (i + 1) + ".");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante la conversione dei valori High: " + ex.Message, "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LabelUpdater(HeadLbl, Color.DarkRed);
                throw new Exception("Errore durante la conversione dei valori High: " + ex.Message);
            }
        }

        private Boolean ConvertLow()
        {
            try
            {
                for (int i = 1; i < _fileLines.GetLength(0); i++)
                {
                    if (_fileLines[i, 4] != null)
                    {
                        _convertedLines[i, 5] = _fileLines[i, 4];
                    }
                    else
                    {
                        MessageBox.Show("Valore Low mancante alla riga " + (i + 1) + ".", "Errore",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        LabelUpdater(HeadLbl, Color.Red);
                        throw new Exception("Valore Low mancante alla riga " + (i + 1) + ".");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante la conversione dei valori Low: " + ex.Message, "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LabelUpdater(HeadLbl, Color.DarkRed);
                throw new Exception("Errore durante la conversione dei valori Low: " + ex.Message);
            }
        }

        private Boolean ConvertClose()
        {
            try
            {
                for (int i = 1; i < _fileLines.GetLength(0); i++)
                {
                    if (_fileLines[i, 5] != null)
                    {
                        _convertedLines[i, 6] = _fileLines[i, 5];
                    }
                    else
                    {
                        MessageBox.Show("Valore Close mancante alla riga " + (i + 1) + ".", "Errore",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        LabelUpdater(HeadLbl, Color.Red);
                        throw new Exception("Valore Close mancante alla riga " + (i + 1) + ".");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante la conversione dei valori Close: " + ex.Message, "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LabelUpdater(HeadLbl, Color.DarkRed);
                throw new Exception("Errore durante la conversione dei valori Close: " + ex.Message);
            }
        }

        private Boolean ConvertTotalVolume()
        {
            try
            {
                for (int i = 1; i < _fileLines.GetLength(0); i++)
                {
                    if (_fileLines[i, 6] != null)
                    {
                        _convertedLines[i, 7] = _fileLines[i, 6];
                    }
                    else
                    {
                        MessageBox.Show("Valore TotalVolume mancante alla riga " + (i + 1) + ".", "Errore",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        LabelUpdater(HeadLbl, Color.Red);
                        throw new Exception("Valore TotalVolume mancante alla riga " + (i + 1) + ".");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore durante la conversione dei valori TotalVolume: " + ex.Message, "Errore",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                LabelUpdater(HeadLbl, Color.DarkRed);
                throw new Exception("Errore durante la conversione dei valori TotalVolume: " + ex.Message);
            }
        }

        private void FormUpdater(int processedCell, CheckBox checkBox)
        {
            progressBar1.Value = processedCell;
            checkBox.Checked = true;
        }
        
        private void LabelUpdater(Label label, Color color)
        {
            label.ForeColor = color;
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.totalBox = new System.Windows.Forms.CheckBox();
            this.closeBox = new System.Windows.Forms.CheckBox();
            this.lowBox = new System.Windows.Forms.CheckBox();
            this.highBox = new System.Windows.Forms.CheckBox();
            this.openBox = new System.Windows.Forms.CheckBox();
            this.dateBox = new System.Windows.Forms.CheckBox();
            this.perBox = new System.Windows.Forms.CheckBox();
            this.headBox = new System.Windows.Forms.CheckBox();
            this.symbolBox = new System.Windows.Forms.CheckBox();
            this.HeadLbl = new System.Windows.Forms.Label();
            this.SymbolLbl = new System.Windows.Forms.Label();
            this.perLbl = new System.Windows.Forms.Label();
            this.dateLbl = new System.Windows.Forms.Label();
            this.openLbl = new System.Windows.Forms.Label();
            this.maxLbl = new System.Windows.Forms.Label();
            this.closeLbl = new System.Windows.Forms.Label();
            this.totalLbl = new System.Windows.Forms.Label();
            this.minLbl = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular,
                System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(50);
            this.label1.Size = new System.Drawing.Size(482, 128);
            this.label1.TabIndex = 1;
            this.label1.Text = "Conversione in MetaStock...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.UseWaitCursor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 131);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(458, 23);
            this.progressBar1.TabIndex = 2;
            this.progressBar1.UseWaitCursor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.totalBox, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.closeBox, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lowBox, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.highBox, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.openBox, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.dateBox, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.perBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.headBox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.symbolBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.HeadLbl, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.SymbolLbl, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.perLbl, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.dateLbl, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.openLbl, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.maxLbl, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.closeLbl, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.totalLbl, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.minLbl, 1, 6);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(88, 185);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(305, 180);
            this.tableLayoutPanel1.TabIndex = 3;
            this.tableLayoutPanel1.UseWaitCursor = true;
            // 
            // totalBox
            // 
            this.totalBox.Enabled = false;
            this.totalBox.Location = new System.Drawing.Point(3, 163);
            this.totalBox.Name = "totalBox";
            this.totalBox.Size = new System.Drawing.Size(34, 14);
            this.totalBox.TabIndex = 15;
            this.totalBox.UseVisualStyleBackColor = true;
            this.totalBox.UseWaitCursor = true;
            // 
            // closeBox
            // 
            this.closeBox.Enabled = false;
            this.closeBox.Location = new System.Drawing.Point(3, 143);
            this.closeBox.Name = "closeBox";
            this.closeBox.Size = new System.Drawing.Size(34, 14);
            this.closeBox.TabIndex = 14;
            this.closeBox.UseVisualStyleBackColor = true;
            this.closeBox.UseWaitCursor = true;
            // 
            // lowBox
            // 
            this.lowBox.Enabled = false;
            this.lowBox.Location = new System.Drawing.Point(3, 123);
            this.lowBox.Name = "lowBox";
            this.lowBox.Size = new System.Drawing.Size(34, 14);
            this.lowBox.TabIndex = 12;
            this.lowBox.UseVisualStyleBackColor = true;
            this.lowBox.UseWaitCursor = true;
            // 
            // highBox
            // 
            this.highBox.Enabled = false;
            this.highBox.Location = new System.Drawing.Point(3, 103);
            this.highBox.Name = "highBox";
            this.highBox.Size = new System.Drawing.Size(22, 14);
            this.highBox.TabIndex = 10;
            this.highBox.UseVisualStyleBackColor = true;
            this.highBox.UseWaitCursor = true;
            // 
            // openBox
            // 
            this.openBox.Enabled = false;
            this.openBox.Location = new System.Drawing.Point(3, 83);
            this.openBox.Name = "openBox";
            this.openBox.Size = new System.Drawing.Size(22, 14);
            this.openBox.TabIndex = 8;
            this.openBox.UseVisualStyleBackColor = true;
            this.openBox.UseWaitCursor = true;
            // 
            // dateBox
            // 
            this.dateBox.Enabled = false;
            this.dateBox.Location = new System.Drawing.Point(3, 63);
            this.dateBox.Name = "dateBox";
            this.dateBox.Size = new System.Drawing.Size(22, 14);
            this.dateBox.TabIndex = 6;
            this.dateBox.UseVisualStyleBackColor = true;
            this.dateBox.UseWaitCursor = true;
            // 
            // perBox
            // 
            this.perBox.Enabled = false;
            this.perBox.Location = new System.Drawing.Point(3, 43);
            this.perBox.Name = "perBox";
            this.perBox.Size = new System.Drawing.Size(22, 14);
            this.perBox.TabIndex = 4;
            this.perBox.UseVisualStyleBackColor = true;
            this.perBox.UseWaitCursor = true;
            // 
            // headBox
            // 
            this.headBox.Enabled = false;
            this.headBox.Location = new System.Drawing.Point(3, 3);
            this.headBox.Name = "headBox";
            this.headBox.Size = new System.Drawing.Size(22, 14);
            this.headBox.TabIndex = 0;
            this.headBox.UseVisualStyleBackColor = true;
            this.headBox.UseWaitCursor = true;
            // 
            // symbolBox
            // 
            this.symbolBox.Enabled = false;
            this.symbolBox.Location = new System.Drawing.Point(3, 23);
            this.symbolBox.Name = "symbolBox";
            this.symbolBox.Size = new System.Drawing.Size(34, 14);
            this.symbolBox.TabIndex = 1;
            this.symbolBox.UseVisualStyleBackColor = true;
            this.symbolBox.UseWaitCursor = true;
            // 
            // HeadLbl
            // 
            this.HeadLbl.Location = new System.Drawing.Point(43, 0);
            this.HeadLbl.Name = "HeadLbl";
            this.HeadLbl.Size = new System.Drawing.Size(262, 20);
            this.HeadLbl.TabIndex = 16;
            this.HeadLbl.Text = "Applicazione intestazione";
            this.HeadLbl.UseWaitCursor = true;
            // 
            // SymbolLbl
            // 
            this.SymbolLbl.Location = new System.Drawing.Point(43, 20);
            this.SymbolLbl.Name = "SymbolLbl";
            this.SymbolLbl.Size = new System.Drawing.Size(262, 20);
            this.SymbolLbl.TabIndex = 17;
            this.SymbolLbl.Text = "Conversione simboli";
            this.SymbolLbl.UseWaitCursor = true;
            // 
            // perLbl
            // 
            this.perLbl.Location = new System.Drawing.Point(43, 40);
            this.perLbl.Name = "perLbl";
            this.perLbl.Size = new System.Drawing.Size(262, 20);
            this.perLbl.TabIndex = 18;
            this.perLbl.Text = "Inserimento fascia temporale";
            this.perLbl.UseWaitCursor = true;
            // 
            // dateLbl
            // 
            this.dateLbl.Location = new System.Drawing.Point(43, 60);
            this.dateLbl.Name = "dateLbl";
            this.dateLbl.Size = new System.Drawing.Size(262, 20);
            this.dateLbl.TabIndex = 19;
            this.dateLbl.Text = "Conversione date";
            this.dateLbl.UseWaitCursor = true;
            // 
            // openLbl
            // 
            this.openLbl.Location = new System.Drawing.Point(43, 80);
            this.openLbl.Name = "openLbl";
            this.openLbl.Size = new System.Drawing.Size(262, 20);
            this.openLbl.TabIndex = 20;
            this.openLbl.Text = "Conversione aperture";
            this.openLbl.UseWaitCursor = true;
            // 
            // maxLbl
            // 
            this.maxLbl.Location = new System.Drawing.Point(43, 100);
            this.maxLbl.Name = "maxLbl";
            this.maxLbl.Size = new System.Drawing.Size(280, 20);
            this.maxLbl.TabIndex = 21;
            this.maxLbl.Text = "Conversione massimi";
            this.maxLbl.UseWaitCursor = true;
            // 
            // closeLbl
            // 
            this.closeLbl.Location = new System.Drawing.Point(43, 140);
            this.closeLbl.Name = "closeLbl";
            this.closeLbl.Size = new System.Drawing.Size(262, 20);
            this.closeLbl.TabIndex = 23;
            this.closeLbl.Text = "Conversione chiusure";
            this.closeLbl.UseWaitCursor = true;
            // 
            // totalLbl
            // 
            this.totalLbl.Location = new System.Drawing.Point(43, 160);
            this.totalLbl.Name = "totalLbl";
            this.totalLbl.Size = new System.Drawing.Size(262, 20);
            this.totalLbl.TabIndex = 24;
            this.totalLbl.Text = "Conversione volume totale";
            this.totalLbl.UseWaitCursor = true;
            // 
            // minLbl
            // 
            this.minLbl.Location = new System.Drawing.Point(43, 120);
            this.minLbl.Name = "minLbl";
            this.minLbl.Size = new System.Drawing.Size(262, 20);
            this.minLbl.TabIndex = 22;
            this.minLbl.Text = "Conversione minimi";
            this.minLbl.UseWaitCursor = true;
            // 
            // ConverterForm
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(482, 406);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.Location = new System.Drawing.Point(15, 15);
            this.Name = "ConverterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.UseWaitCursor = true;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Label totalLbl;

        private System.Windows.Forms.Label closeLbl;

        private System.Windows.Forms.Label minLbl;

        private System.Windows.Forms.Label maxLbl;

        private System.Windows.Forms.Label openLbl;

        private System.Windows.Forms.Label dateLbl;

        private System.Windows.Forms.Label SymbolLbl;
        private System.Windows.Forms.Label perLbl;

        private System.Windows.Forms.Label HeadLbl;

        private System.Windows.Forms.CheckBox totalBox;

        private System.Windows.Forms.CheckBox symbolBox;
        private System.Windows.Forms.CheckBox perBox;
        private System.Windows.Forms.CheckBox dateBox;
        private System.Windows.Forms.CheckBox openBox;
        private System.Windows.Forms.CheckBox highBox;
        private System.Windows.Forms.CheckBox lowBox;
        private System.Windows.Forms.CheckBox closeBox;

        private System.Windows.Forms.CheckBox headBox;

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;

        private System.Windows.Forms.ProgressBar progressBar1;

        private System.Windows.Forms.Label label1;
    }
}