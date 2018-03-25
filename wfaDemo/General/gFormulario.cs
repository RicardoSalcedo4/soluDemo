using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace wfaDemo.General
{
    public class gFormulario
    {
        Form iFormulario;

        public gFormulario(Form pFormuario)
        {
           iFormulario = pFormuario;           
           iFormulario.BackColor = System.Drawing.Color.FromArgb(160,170,190);
        
        }

        public void vi_KeyDown()
        {
            iFormulario.KeyPreview = true;
            iFormulario.KeyDown += new KeyEventHandler(pFormuario_KeyDown);        
        }

        void pFormuario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                iFormulario.Close();
            }
        }
    }
}
