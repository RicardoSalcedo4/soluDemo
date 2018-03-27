using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Data;
using clUtilitario;

namespace clDato
{
    public class dConexion
    {

        public Database Conexion(String pConnectionString, Boolean pDesencriptarClave)
        {
            String ls_Clave;
            Database lconfig_db = DatabaseFactory.CreateDatabase(pConnectionString);
            DbConnectionStringBuilder lsb = lconfig_db.DbProviderFactory.CreateConnectionStringBuilder();
            ls_Clave = lsb["Password"].ToString();
            if (pDesencriptarClave)
            {
                ls_Clave = new Seguridad().Desencriptar(ls_Clave);
            }           
            GenericDatabase newDb = new GenericDatabase(lsb.ToString(), lconfig_db.DbProviderFactory);
            Database ldb_Conexion = newDb;
            return ldb_Conexion;
        }


        public Database Conexion(String pConnectionString, String pServidor, String pBaseDatos, String pNombreUsuario, String pClave)
        {
            Database lconfig_db = DatabaseFactory.CreateDatabase(pConnectionString);
            DbConnectionStringBuilder lsb = lconfig_db.DbProviderFactory.CreateConnectionStringBuilder();
            lsb["Data Source"] = pServidor;
            lsb["Initial Catalog"] = pBaseDatos;
            lsb["User ID"] = pNombreUsuario;
            lsb["Password"] = pClave;
            GenericDatabase newDb = new GenericDatabase(lsb.ToString(), lconfig_db.DbProviderFactory);
            Database ldb_Conexion = newDb;
            return ldb_Conexion;
        }

        public static DbParameter CreaParametroCursor(string NombreCursor)
        {
            OracleParameter param = new OracleParameter(NombreCursor, OracleType.Cursor, 0, ParameterDirection.Output, true, 0, 0, String.Empty, DataRowVersion.Current, Convert.DBNull);
            return param;
        }

       

    }
}
