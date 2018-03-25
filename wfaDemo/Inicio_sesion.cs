using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace wfaDemo
{
    public partial class Inicio_sesion : Form
    {
        public Inicio_sesion()
        {
            InitializeComponent();
        }

        private void btoCerrar_Click(object sender, EventArgs e)
        {
            vi_cerrar();      
        }

        private void btoAceptar_Click(object sender, EventArgs e)
        {
            vi_aceptar();
        }

        void vi_aceptar()
        {
            MessageBox.Show("Ok");
        }

        void vi_cerrar()
        {
            this.Close();  
        }

        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtClave.Focus();
                txtClave.Select();
            }
        }

        private void txtClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                vi_aceptar();
            }
        }

        

        private void Inicio_sesion_Load(object sender, EventArgs e)
        {
           General.gFormulario lgFormulario = new General.gFormulario(this);
           lgFormulario.vi_KeyDown();
        }
     
    }
}
