using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Minesweeper.Models.Home
{
    public class Matriz
    {
            public string Size { get; set; }

            public List<Datos> campo { get; set; }     
        public class Datos
        {

            public int Fila { get; set; }

            public int Columna { get; set; }

            public string valor { get; set; }
        }
    }
}