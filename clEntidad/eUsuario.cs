using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace clEntidad
{
    public class eUsuario
    {

        public Int32 Codigo{get;set;}
        public Int32 CodEmpresa { get; set; }
        public String NombreUsuario{get;set;}
        public String NombreCompleto{get;set;}
        public String Clave{get;set;}
        public DateTime FechaRegistro{get;set;}
        public Int32 EstadoActivo{get;set;}
        public List<eSede> ItemSede = new List<eSede>();
 

    }





}
