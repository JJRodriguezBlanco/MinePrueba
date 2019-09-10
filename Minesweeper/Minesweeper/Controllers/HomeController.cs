using Minesweeper.DataAccess;
using Minesweeper.Models;
using Minesweeper.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Minesweeper.Models.Home.Matrix;

namespace Minesweeper.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Input = "Input";

            ViewBag.Output = "Output";

            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textArea">Son los datos introducidos por el usuario por pantalla</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Calculation(string textArea)
        {
            try
            {
                // Differentiate the introduced lines, removing the empty lines
                var lines = textArea.Split('\n').Where(x => x != "").ToList();

                // Save the data entered by the user in BBDD 
                Savebbdd(lines);

                // Get the data entered by the user of the database
                lines = Getbbdd();

                // Pass the data to a structure to differentiate the different matrices and the X and Y position of the boxes.
                var listMatrix = TransformationToListMatrix(lines);

                // Change the points for the number of bombs around.
                ChangePointsToNumberOfBombs(listMatrix);

                // We pass the data to a string to display correctly in the TextArea.
                var result = TransformationToString(listMatrix);

                return Json(new JsonReturnMine { IsOk= true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new JsonReturnMine { IsOk = false, data = ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        #region Private

        private void Savebbdd(List<string> lineas)
        {
            using (var ddbb = new MinesweeperEntities())
            {      
                var table = ddbb.Input_Mine;

                ddbb.Database.ExecuteSqlCommand("TRUNCATE TABLE Input_Mine");

                for (var d = 0; d < lineas.Count; d++)
                {
                    var entity = new Input_Mine()
                    {
                        Id = d,
                        Input = lineas[d]
                    };
                    table.Add(entity);
                }

                ddbb.SaveChanges();          
            }
        }

        private List<string> Getbbdd()
        {
            using (var ddbb = new MinesweeperEntities())
            {
                var dto = ddbb.Input_Mine.Select(x=> x.Input).ToList();
                return dto;     
            }
        }

        private List<Matrix> TransformationToListMatrix(List<string> lines)
        {

            var listMatrix = new List<Matrix>();
            var matriz = new Matrix();
            var fila = 0;
            foreach (var a in lines)
            {
                if (a.IndexOf('.') == -1 && a.IndexOf('*') == -1)
                {
                    if (lines[0] != a)
                        listMatrix.Add(matriz);

                    matriz = new Matrix();
                    matriz.Size = a;
                    matriz.Fields = new List<Field>();
                    fila = 0;
                }
                else
                {
                    for (var i = 0; i < a.Length; i++)
                    {
                        var dato = new Field()
                        {
                            Row = fila,
                            Column = i,
                            Value = a[i].ToString()
                        };
                        matriz.Fields.Add(dato);

                    }
                    fila++;
                }
            }

            if (!listMatrix.Any()) listMatrix.Add(matriz);

            return listMatrix;
        }

        private void ChangePointsToNumberOfBombs(List<Matrix> listMatrix)
        {
            //Go through the Matrix List
            foreach (var m in listMatrix)
            {
                //Go through the matrices to calculate how many bombs are around
                foreach (var a in m.Fields)
                {
                    a.Value = NumberOfBombs(a, m.Fields);
                }
            }
        }

        private string NumberOfBombs(Field a, List<Field> campo)
        {
            var row = a.Row;
            var column = a.Column;
            var value = a.Value;

            if (value == "*")
            {
                return value;
            }
            else
            {
                var listField = new List<Field>();
                listField.Add(campo.Find(x => x.Row == row - 1 && x.Column == column - 1));
                listField.Add(campo.Find(x => x.Row == row - 1 && x.Column == column));
                listField.Add(campo.Find(x => x.Row == row - 1 && x.Column == column + 1));

                listField.Add(campo.Find(x => x.Row == row && x.Column == column + 1));
                listField.Add(campo.Find(x => x.Row == row && x.Column == column - 1));

                listField.Add(campo.Find(x => x.Row == row + 1 && x.Column == column - 1));
                listField.Add(campo.Find(x => x.Row == row + 1 && x.Column == column));
                listField.Add(campo.Find(x => x.Row == row + 1 && x.Column == column + 1));

                var countBombs = listField.Where(x => x != null).Count(x=> x.Value == "*");

                return countBombs.ToString();
            }
        }

        private string TransformationToString(List<Matrix> listMatrix)
        {
            var result = "";

            for (var m = 0; m < listMatrix.Count; m++)
            {
                var numField = m + 1;
                result = result + "Field #" + numField + "\n";
                for (var f = 0; f < int.Parse(listMatrix[m].Size.Substring(0, 1)); f++)
                {
                    var row = listMatrix[m].Fields.Where(x => x.Row == f).Select(x => x.Value).ToList();
                    result = result + string.Join("", row) + "\n";
                }
            }

            return result;
        }

        #endregion

    }



}