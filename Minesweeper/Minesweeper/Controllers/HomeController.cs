using Minesweeper.DataAccess;
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

        [HttpPost]
        public ActionResult Calculo(string textArea)
        {
            // Diferenciar las lineas introducidas, quitando las lineas vacias
            var lines = textArea.Split('\n').Where(x => x != "").ToList();

            //Guardamos en BBDD los datos introducidos por el usuario 
            Savebbdd(lines);

            //Obtenemos los datos Introduccidos por el usuario de la BBDD
            lines = Getbbdd();

            //Pasamos los datos a una estructura para diferencias las diferentes matrices y la posición X e Y de las casillas.
            var listMatrix = TransformationToListMatrix(lines);

            // Tratar los puntos y pasarlos a numero de bombas
            ChangePointsToNumberOfBombs(listMatrix);

            //Pamos los datos a un string para mostrarlo correctamente en el TextArea.
            var result = TransformationToString(listMatrix);

            return Json(result, JsonRequestBehavior.AllowGet);
             
        }

        #region Private

        private void Savebbdd(List<string> lineas)
        {
            using (var ddbb = new MinesweeperEntities())
            {
                try
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
                catch (Exception ex)
                {
                }
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
            //Recorremos la Lista de matrices
            foreach (var m in listMatrix)
            {
                //Recorremos la matrices para calcular cuantas bombas hay al rededor
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