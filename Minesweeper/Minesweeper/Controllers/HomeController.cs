using Minesweeper.DataAccess;
using Minesweeper.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Minesweeper.Models.Home.Matriz;

namespace Minesweeper.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Calculo(string textArea)
        {
            var lineas = textArea.Split('\n').Where(x => x != "").ToList();

            //SaveInbbdd(lineas);

            //lineas = TakeInbbdd();

            var matrizInput = new List<Matriz>();
            var matriz = new Matriz();
            var fila = 0;
            foreach (var a in lineas)
            {
                if (a.IndexOf('.') == -1 && a.IndexOf('*') == -1)
                {
                    if (lineas[0] != a)
                        matrizInput.Add(matriz);

                    matriz = new Matriz();
                    matriz.Size = a;
                    matriz.campo = new List<Datos>();
                    fila = 0;
                }
                else { 
                    for (var i= 0; i < a.Length; i++)
                    {
                        var dato = new Datos() {
                            Fila = fila,
                            Columna = i,
                            valor = a[i].ToString()
                        };
                        matriz.campo.Add(dato);

                    }
                    fila ++;
                } 
            }

            if (!matrizInput.Any()) matrizInput.Add(matriz);

            // Tratar los puntos y pasarlos a numeros
            foreach (var m in matrizInput)
            { 
                foreach (var a in m.campo)
                {
                    a.valor = numeroBombas(a,m.campo);
                }
            }

            // Tratar pasar a json
            var result = "";
            for (var m = 0; m < matrizInput.Count; m++)
            {
                var numField = m + 1;
                result = result + "Field #" + numField + "\n";
                for(var f = 0; f < int.Parse(matrizInput[m].Size.Substring(0, 1)); f++)
                {
                    var row = matrizInput[m].campo.Where(x => x.Fila == f).Select(x => x.valor).ToList();
                    result = result + string.Join("", row) + "\n";
                }
                result = result + "\n";

            }
            return Json(result, JsonRequestBehavior.AllowGet);
             
        }

        private List<string> TakeInbbdd()
        {
            using (var ddbb = new MinesweeperEntities())
            {
                var dto = ddbb.Input_Mine.Select(x=> x.Input).ToList();
                return dto;     
            }
        }

        private void SaveInbbdd(List<string> lineas)
        {
            using (var ddbb = new MinesweeperEntities())
            {
                try{ 
                    var table = ddbb.Input_Mine;

                    ddbb.Database.ExecuteSqlCommand("TRUNCATE TABLE Input_Mine");


                    for (var d = 0; d < lineas.Count;d++)
                    {
                        var entity = new Input_Mine() {
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

        private string numeroBombas(Datos a, List<Datos> campo)
        {
            var fila = a.Fila;
            var columna = a.Columna;
            var valor = a.valor;

            if (valor == "*")
            {
                return valor;
            }
            else
            {
                var list2 = new List<Datos>();
                list2.Add(campo.Find(x => x.Fila == fila - 1 && x.Columna == columna - 1));
                list2.Add(campo.Find(x => x.Fila == fila - 1 && x.Columna == columna));
                list2.Add(campo.Find(x => x.Fila == fila - 1 && x.Columna == columna + 1));

                list2.Add(campo.Find(x => x.Fila == fila && x.Columna == columna + 1));
                list2.Add(campo.Find(x => x.Fila == fila && x.Columna == columna - 1));

                list2.Add(campo.Find(x => x.Fila == fila + 1 && x.Columna == columna - 1));
                list2.Add(campo.Find(x => x.Fila == fila + 1 && x.Columna == columna));
                list2.Add(campo.Find(x => x.Fila == fila + 1 && x.Columna == columna + 1));

                var count = list2.Where(x => x != null).Count(x=> x.valor == "*");

                return count.ToString();
            }



            throw new NotImplementedException();
        }

        public ActionResult About()
        {
            ViewBag.Input = "Input";

            ViewBag.Output = "Output";

            return View();
        }

    }



}