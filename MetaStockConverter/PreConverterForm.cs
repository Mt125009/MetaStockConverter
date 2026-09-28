using System;
using System.Drawing;
using System.Windows.Forms;

namespace MetaStockConverter
{
    public class PreConverterForm : Form
    {
        public PreConverterForm()
        {
            InitializeComponent();
            //Text = "Validazione dati";
            //Width = 400;
            //Height = 200;
            //StartPosition = FormStartPosition.CenterScreen;
    }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PreConverterForm
            // 
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "PreConverterForm";
            this.Text = "Apertura file";
            this.ResumeLayout(false);
        }
    }
}