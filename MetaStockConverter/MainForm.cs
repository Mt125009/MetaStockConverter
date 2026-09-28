using System;
using System.Drawing;
using System.Windows.Forms;

namespace MetaStockConverter
{
    public class MainForm : Form
    {
        public MainForm()
        {
            Text = "MetaStock Converter";
            Width = 600;
            Height = 300;
            StartPosition = FormStartPosition.CenterScreen;

            Label label = new Label();
            label.Text = "MetaStock Converter";
            label.Font = new Font("Arial", 18);
            label.AutoSize = true;
            label.Location = new Point(30, 30);

            Button button = new Button();
            button.Text = "Test";
            button.Location = new Point(30, 90);
            button.Click += Button_Click;

            Controls.Add(label);
            Controls.Add(button);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "La GUI funziona!",
                "Test",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }
    }
}namespace MetaStockConverter
{
    public class MainForm
    {
        
    }
}