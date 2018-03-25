using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace wfaDemo.General
{
    public class gValidacion
    {
        ErrorProvider ierrp_Validar;
        String is_Mensaje;

        public void ValidarTextBoxVacio(TextBox pTextBox, ErrorProvider pErrorProvider, string pMensaje)
        {
            ierrp_Validar = pErrorProvider;
            is_Mensaje = pMensaje;  
            pTextBox.Validating += new System.ComponentModel.CancelEventHandler(pTextBox_Validating);
        }

        void pTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox lTexbox = (TextBox)sender;

            if (string.IsNullOrEmpty(lTexbox.Text.Trim()))
            {
                ierrp_Validar.SetError(lTexbox, is_Mensaje);
            }
            else
            {
                ierrp_Validar.SetError(lTexbox, "");
            }
        }



    }
}
