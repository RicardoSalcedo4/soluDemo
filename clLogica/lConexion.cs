using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
namespace clLogica
{
    public class lConexion
    {
        public Database Conexion(String pConnectionString, Boolean pDesencriptarClave)
        {
           return new clDato.dConexion().Conexion(pConnectionString, pDesencriptarClave);
        }

        public Database Conexion(String pConnectionString, String pServidor, String pBaseDatos, String pNombreUsuario, String pClave)
        {
            return new clDato.dConexion().Conexion( pConnectionString, pServidor, pBaseDatos, pNombreUsuario, pClave);
        }
    }
}
