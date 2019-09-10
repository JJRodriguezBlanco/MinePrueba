using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Minesweeper.Models.Home
{
    public class Matrix
    {
            public string Size { get; set; }

            public List<Field> Fields { get; set; }     
        public class Field
        {

            public int Row { get; set; }

            public int Column { get; set; }

            public string Value { get; set; }
        }
    }
}