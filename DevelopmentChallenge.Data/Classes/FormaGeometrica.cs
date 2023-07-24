/******************************************************************************************************************/
/******* ¿Qué pasa si debemos soportar un nuevo idioma para los reportes, o agregar más formas geométricas? *******/
/******************************************************************************************************************/

/*
 * TODO: 
 * Refactorizar la clase para respetar principios de la programación orientada a objetos.
 * Implementar la forma Trapecio/Rectangulo. 
 * Agregar el idioma Italiano (o el deseado) al reporte.
 * OPCIONAL: Se agradece la inclusión de nuevos tests unitarios para validar el comportamiento de la nueva funcionalidad agregada (los tests deben pasar correctamente al entregar la solución, incluso los actuales.)
 * Una vez finalizado, hay que subir el código a un repo GIT y ofrecernos la URL para que podamos utilizar la nueva versión :).
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevelopmentChallenge.Data.Classes
{
    public class FormaGeometrica
    {
        #region Formas

        public const int Cuadrado = 1;
        public const int TrianguloEquilatero = 2;
        public const int Circulo = 3;
        public const int Trapecio = 4;

        #endregion

        #region Idiomas

        public const int Castellano = 1;
        public const int Ingles = 2;

        #endregion

        private readonly decimal _ladoA;
        private readonly decimal _ladoB;
        private readonly decimal _altura;

        public int Tipo { get; set; }

        public FormaGeometrica(int tipo, decimal LadoA, decimal ladoB = 0, decimal altura = 0)
        {
            Tipo = tipo;
            _ladoA = LadoA;
            _ladoB = ladoB;
            _altura = altura;
        }

        public static string Imprimir(List<FormaGeometrica> formas, int idioma)
        {
            var sb = new StringBuilder();

            if (!formas.Any())
            {
                if (idioma == Castellano)
                    sb.Append("<h1>Lista vacía de formas!</h1>");
                else
                    sb.Append("<h1>Empty list of shapes!</h1>");
            }
            else
            {

                // Hay por lo menos una forma
                // HEADER
                if (idioma == Castellano)
                    sb.Append("<h1>Reporte de Formas</h1>");
                else
                    // default es inglés
                    sb.Append("<h1>Shapes report</h1>");

                Dictionary<int, List<FormaGeometrica>> listFormas = new Dictionary<int, List<FormaGeometrica>>();

                foreach (var forma in formas) 
                {
                    if (listFormas.ContainsKey(forma.Tipo))
                    {
                        listFormas[forma.Tipo].Add(forma);
                    }
                    else {
                        var newList = new List<FormaGeometrica>();
                        newList.Add(forma);
                        listFormas[forma.Tipo] = newList; 
                    }
                }

                decimal perimetros = 0;
                decimal areas = 0;

                foreach (var tipo in listFormas.Keys.ToList()) {
                    areas += listFormas[tipo].Select(x => x.CalcularArea()).Sum();
                    perimetros += listFormas[tipo].Select(x => x.CalcularPerimetro()).Sum();
                    sb.Append(ObtenerLinea(listFormas[tipo].Count(), listFormas[tipo].Select(x => x.CalcularArea()).Sum(), listFormas[tipo].Select(x => x.CalcularPerimetro()).Sum(), tipo, idioma));

                }

                // FOOTER
                sb.Append("TOTAL:<br/>");
                sb.Append(listFormas.Count() + " " + (idioma == Castellano ? "formas" : "shapes") + " ");
                sb.Append((idioma == Castellano ? "Perimetro " : "Perimeter ") + (perimetros).ToString("#.##") + " ");
                sb.Append("Area " + (areas).ToString("#.##"));
            }

            return sb.ToString();
        }

        private static string ObtenerLinea(int cantidad, decimal area, decimal perimetro, int tipo, int idioma)
        {
            if (cantidad > 0)
            {
                if (idioma == Castellano)
                    return $"{cantidad} {TraducirForma(tipo, cantidad, idioma)} | Area {area:#.##} | Perimetro {perimetro:#.##} <br/>";

                return $"{cantidad} {TraducirForma(tipo, cantidad, idioma)} | Area {area:#.##} | Perimeter {perimetro:#.##} <br/>";
            }

            return string.Empty;
        }

        private static string TraducirForma(int tipo, int cantidad, int idioma)
        {
            switch (tipo)
            {
                case Cuadrado:
                    if (idioma == Castellano) return cantidad == 1 ? "Cuadrado" : "Cuadrados";
                    else return cantidad == 1 ? "Square" : "Squares";
                case Circulo:
                    if (idioma == Castellano) return cantidad == 1 ? "Círculo" : "Círculos";
                    else return cantidad == 1 ? "Circle" : "Circles";
                case TrianguloEquilatero:
                    if (idioma == Castellano) return cantidad == 1 ? "Triángulo" : "Triángulos";
                    else return cantidad == 1 ? "Triangle" : "Triangles";
            }

            return string.Empty;
        }

        public decimal CalcularArea()
        {
            switch (Tipo)
            {
                case Cuadrado: return _ladoA * _ladoB;
                case Circulo: return (decimal)Math.PI * (_ladoA / 2) * (_ladoB / 2);
                case TrianguloEquilatero: return ((decimal)Math.Sqrt(3) / 4) * _ladoA * _ladoB;
                case Trapecio: return ((_ladoA + _ladoB) * _altura) / 2;
                default:
                    throw new ArgumentOutOfRangeException(@"Forma desconocida");
            }
        }

        public decimal CalcularPerimetro()
        {
            switch (Tipo)
            {
                case Cuadrado: return _ladoA * 4;
                case Circulo: return (decimal)Math.PI * _ladoA;
                case TrianguloEquilatero: return _ladoA * 3;
                case Trapecio:
                    decimal ladoNoParalelo = (_ladoA - _ladoB) / 2;
                    decimal ladoOblicuo = (decimal)Math.Sqrt((double)(_altura * _altura + ladoNoParalelo * ladoNoParalelo));
                    return _ladoA + _ladoB + 2 * ladoOblicuo;
                default:
                    throw new ArgumentOutOfRangeException(@"Forma desconocida");
            }
        }
    }
}
