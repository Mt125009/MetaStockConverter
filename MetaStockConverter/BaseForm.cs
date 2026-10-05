using System;
using System.Drawing;
using System.Windows.Forms;

namespace MetaStockConverter
{
    public abstract class BaseForm : Form
    {
        protected BaseForm()
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath)
                ?? throw new InvalidOperationException("Impossibile caricare l'icona dell'applicazione.");
        }
    }
}