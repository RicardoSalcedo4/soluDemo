using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.Common;
using clEntidad;
namespace clDato
{
    public class dUsuarioSede
    {
        public Database idb_conexion;

     

        /// <summary>
        /// Insertar Sede del usuario
        /// </summary>
        /// <param name="pEmpresa">Codigo de la empresa</param>
        /// <param name="pUsuario">Codigo del usuario</param>
        /// <param name="pSede">Codigo de la sede</param>
        /// <returns>[0]: 1=OK ó -1=Error ; [1]: Mensaje</returns>
        public String[] Insertar(eUsuarioSede pUsuarioSede, DbTransaction ptrans_conexion)
        {
            String[] lValor = new String[2];
            String lExeValor, lExeMensaje;
            lValor[0] = "-1";
            lValor[1] = "";

            try
            {
                if (idb_conexion.GetType().Name == "OracleDatabase")
                {
                    using (DbCommand cmd = idb_conexion.GetStoredProcCommand("DEMO.SP_USUARIO_SEDE_INSERTAR"))
                    {
                        idb_conexion.AddInParameter(cmd, "XX_EMPRESA", DbType.Int32, pUsuarioSede.CodEmpresa);
                        idb_conexion.AddInParameter(cmd, "XX_USUARIO", DbType.Int32, pUsuarioSede.CodUsuario);
                        idb_conexion.AddInParameter(cmd, "XX_SEDE", DbType.Int32, pUsuarioSede.CodSede);

                        idb_conexion.AddOutParameter(cmd, "YY_VALOR", DbType.String, 2);
                        idb_conexion.AddOutParameter(cmd, "YY_MENSAJE", DbType.String, 4000);

                        idb_conexion.ExecuteNonQuery(cmd, ptrans_conexion);

                        lExeValor = idb_conexion.GetParameterValue(cmd, "YY_VALOR").ToString();
                        lExeMensaje = idb_conexion.GetParameterValue(cmd, "YY_MENSAJE").ToString();

                        if (lExeMensaje != "1")
                        {
                            lValor[0] = "-1";
                            lValor[1] = lExeMensaje;
                            return lValor;
                        }

                        lValor[0] = "1";
                        lValor[1] = lExeMensaje;
                        return lValor;

                    }
                }
                else
                {
                    lValor[0] = "-1";
                    lValor[1] = "Base de datos sin definir: " + idb_conexion.GetType().Name;
                    return lValor;
                }

            }
            catch (Exception ex)
            {
                lValor[0] = "-1";
                lValor[1] = ex.Message;
                return lValor;
            }


        }

    }
}
