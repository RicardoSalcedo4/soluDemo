using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using clEntidad;
using clDato;
using System.Data.Common;
using System.Data;
namespace clLogica
{
    public class lUsuario
    {

        dUsuario idUsuario;
        dUsuarioSede idUsuarioSede;
        Database idb_conexion;
        DbConnection icnn_bd;

        public lUsuario()
        {
            idUsuario = new dUsuario();
            idUsuario.idb_conexion = idb_conexion;
            idUsuarioSede = new dUsuarioSede();
            idUsuarioSede.idb_conexion = idb_conexion;
        }

        /// <summary>
        /// Validar usuario y clave 
        /// </summary>
        /// <param name="pEmpresa">Codigo de la empresa</param>
        /// <param name="pNombreUsuario">Nombre del usuario</param>
        /// <param name="pClaveUsuario">Clave del usuario</param>
        /// <returns>[0]: 1=OK ó -1=Error ; [1]: Mensaje </returns>
        public String[] ValidarUsuarioClave(Int32 pEmpresa, String pNombreUsuario, String pClaveUsuario)
        {
            String[] lValor = new String[2];
            DataTable ldt_Usuarios;
            String[] lsReturn;          
            lValor[0] = "-1";
            lValor[1] = "";

            if (pEmpresa <= 0)
            {
                lValor[0] = "-1";
                lValor[1] = "Debe ingresar la Empresa";
                return lValor;
            }
            if (pNombreUsuario.Trim().Length == 0)
            {
                lValor[0] = "-1";
                lValor[1] = "Debe ingresar el nombre del usuario";
                return lValor;
            }
            if (pClaveUsuario.Trim().Length == 0)
            {
                lValor[0] = "-1";
                lValor[1] = "Debe ingresar la clave del usuario";
                return lValor;
            }

            try
            {
                lsReturn = idUsuario.Listar_xNombreUsuarioxClave(pEmpresa, pNombreUsuario, out ldt_Usuarios);
                if (lsReturn[0] != "1")
                {
                    lValor[0] = "-1";
                    lValor[1] = lsReturn[1];
                    return lValor;
                }

                if (ldt_Usuarios.Rows.Count == 0)
                {
                    lValor[0] = "-1";
                    lValor[1] = "Usuario no existe";
                    return lValor;
                }

                if (ldt_Usuarios.Rows.Count > 1)
                {
                    lValor[0] = "-1";
                    lValor[1] = "Hay mas de un usuario";
                    return lValor;
                }

                if (ldt_Usuarios.Rows[0]["CLAVE"].ToString() != pClaveUsuario)
                {
                    lValor[0] = "-1";
                    lValor[1] = "Clave invalida";
                    return lValor;
                }


                lValor[0] = "1";
                lValor[1] = ldt_Usuarios.Rows[0]["COD_USUARIO"].ToString();
            }
            catch (Exception ex)
            {
                lValor[0] = "-1";
                lValor[1] = "lUsuario.ValidarDatosUsuario: " + ex.Message;
                return lValor;
            }


            return lValor;
        }

        /// <summary>
        /// Insertar datos del usuario
        /// </summary>
        /// <returns>[0]: 1=OK ó -1=Error ; [1]: Mensaje</returns>
        public String[] Insertar(eUsuario peUsuario,List<eUsuarioSede> pListUsuSede )
        {
            DbTransaction ltrans1 = null;
            Int32 li_CodigoUsuario;
            String[] lValor = new String[2];
            String[] lResult = new String[2];
            lValor[0] = "-1";
            lValor[1] = "";

            //Validar datos
            if (peUsuario.CodEmpresa <= 0)
            {
                lValor[0] = "-1";
                lValor[1] = "Debe ingresar la Empresa";
                return lValor;
            }
            if (peUsuario.NombreUsuario.Trim().Length == 0)
            {
                lValor[0] = "-1";
                lValor[1] = "Debe ingresar el nombre del usuario";
                return lValor;
            }
            if (peUsuario.NombreCompleto.Trim().Length == 0)
            {
                lValor[0] = "-1";
                lValor[1] = "Debe ingresar el nombre del completo del usuario";
                return lValor;
            }
            if (peUsuario.Clave.Trim().Length == 0)
            {
                lValor[0] = "-1";
                lValor[1] = "Debe ingresar la clave del usuario";
                return lValor;
            }
            if (peUsuario.FechaRegistro <= DateTime.Parse("01/01/1901"))
            {
                lValor[0] = "-1";
                lValor[1] = "Debe ingresar la fecha de registro";
                return lValor;
            }


            //Grabar Datos
            try
            {
                icnn_bd = idb_conexion.CreateConnection();
                icnn_bd.Open();
                ltrans1 = icnn_bd.BeginTransaction();

              
                lResult = idUsuario.Insertar(peUsuario, ltrans1);
                if (lResult[0] != "1")
                {
                    vi_Commit_Rollback(ltrans1, false, icnn_bd);
                    lValor[0] = "-1";
                    lValor[1] = lResult[1];
                    return lValor;
                }
                li_CodigoUsuario = Int32.Parse(lResult[1]);

                foreach (eUsuarioSede RegSede in pListUsuSede)
                {                   
                    lResult = idUsuarioSede.Insertar(RegSede, ltrans1);
                    if (lResult[0] != "1")
                    {
                        vi_Commit_Rollback(ltrans1, false, icnn_bd);
                        lValor[0] = "-1";
                        lValor[1] = lResult[1];
                        return lValor;
                    }
                }

                vi_Commit_Rollback(ltrans1, true, icnn_bd);
                lValor[0] = "1";
                lValor[1] = li_CodigoUsuario.ToString();
                return lValor;
            }
            catch (Exception ex)
            {
                vi_Commit_Rollback(ltrans1, false, icnn_bd);
                lValor[0] = "-1";
                lValor[1] = "lUsuario.Insertar: " + ex.Message;
                return lValor;

            }

        }

        void vi_Commit_Rollback(DbTransaction ptrans_conexion1, Boolean pCommit, DbConnection pcnn_bd)
        {
            if (ptrans_conexion1 != null)
            {
                if (pCommit)
                {
                    ptrans_conexion1.Commit();
                }
                else
                {
                    ptrans_conexion1.Rollback();
                }

                ptrans_conexion1.Dispose();
                ptrans_conexion1 = null;
            }

            if (pcnn_bd != null)
            {
                pcnn_bd.Close();
                pcnn_bd.Dispose();
                pcnn_bd = null;
            }


        }

    

    }
}
