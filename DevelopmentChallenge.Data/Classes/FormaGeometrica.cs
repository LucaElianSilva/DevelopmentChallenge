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
        public const int Portugues = 3;

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
            var traducciones = GetTraducciones();
            if (!formas.Any())
            {
                sb.Append("<h1>"+ traducciones["listaVaciaFormas"].FirstOrDefault(x => x.Key == idioma).Value +"</h1>");
            }
            else
            {
                // Hay por lo menos una forma
                // HEADER
                sb.Append("<h1>"+ traducciones["reporteFormas"].FirstOrDefault(x => x.Key == idioma).Value + "</h1>");

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
                sb.Append(listFormas.Count() + " " + traducciones["formas"].FirstOrDefault(x => x.Key == idioma).Value + " ");
                sb.Append((traducciones["perimetro"].FirstOrDefault(x => x.Key == idioma).Value) + " " + (perimetros).ToString("#.##") + " ");
                sb.Append((traducciones["perimetro"].FirstOrDefault(x => x.Key == idioma).Value) + " " + (areas).ToString("#.##"));
            }

            return sb.ToString();
        }

        private static string ObtenerLinea(int cantidad, decimal area, decimal perimetro, int tipo, int idioma)
        {
            if (cantidad > 0)
            {
                var traducciones = GetTraducciones();
                return $"{cantidad} {TraducirForma(tipo, cantidad, idioma)} | " + (traducciones["area"].FirstOrDefault(x => x.Key == idioma).Value) + " {"
                    + (traducciones["area"].FirstOrDefault(x => x.Key == idioma).Value) + ":#.##} | " 
                    + (traducciones["perimetro"].FirstOrDefault(x => x.Key == idioma).Value) + " {" + (traducciones["perimetro"].FirstOrDefault(x => x.Key == idioma).Value) + ":#.##} <br/>";
            }

            return string.Empty;
        }

        private static string TraducirForma(int tipo, int cantidad, int idioma)
        {
            var traducciones = GetTraducciones();
            switch (tipo)
            {
                case Cuadrado:
                    return cantidad == 1 ? (traducciones["cuadrado"].FirstOrDefault(x => x.Key == idioma).Value) : (traducciones["cuadrados"].FirstOrDefault(x => x.Key == idioma).Value);
                case Circulo:
                    return cantidad == 1 ? (traducciones["circulo"].FirstOrDefault(x => x.Key == idioma).Value) : (traducciones["circulos"].FirstOrDefault(x => x.Key == idioma).Value);
                case TrianguloEquilatero: 
                    return cantidad == 1 ? (traducciones["triangulo"].FirstOrDefault(x => x.Key == idioma).Value) : (traducciones["triangulos"].FirstOrDefault(x => x.Key == idioma).Value);
                case Trapecio:
                    return cantidad == 1 ? (traducciones["trapecio"].FirstOrDefault(x => x.Key == idioma).Value) : (traducciones["trapecios"].FirstOrDefault(x => x.Key == idioma).Value);
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
                    decimal ladoOblicuo = (decimal)Math.Sqrt((double)((_altura * _altura) + (ladoNoParalelo * ladoNoParalelo)));
                    return (_ladoA + _ladoB) + (2 * ladoOblicuo);
                default:
                    throw new ArgumentOutOfRangeException(@"Forma desconocida");
            }
        }


        public static Dictionary<string, Dictionary<int, string>> GetTraducciones()
        {
            Dictionary<string, Dictionary<int, string>> Traducciones = new Dictionary<string, Dictionary<int, string>>()
            {
                {
                    "listaVaciaFormas", new Dictionary<int, string>
                    {
                        { Castellano, "Lista vacía de formas!" },
                        { Portugues, "relatório de formas geométricas" },
                        { Ingles, "Empty list of shapes!" }
                    }
                },
                {
                    "reporteFormas", new Dictionary<int, string>
                    {
                        { Castellano, "Reporte de Formas" },
                        { Portugues, "Relatório de formas" },
                        { Ingles, "Shapes report" }
                    }
                },
                {
                    "formas", new Dictionary<int, string>
                    {
                        { Castellano, "formas" },
                        { Portugues, "formas" },
                        { Ingles, "shapes" }
                    }
                },
                {
                    "perimetro", new Dictionary<int, string>
                    {
                        { Castellano, "Perimetro" },
                        { Portugues, "Perímetro" },
                        { Ingles, "Perimeter" }
                    }
                },
                {
                    "area", new Dictionary<int, string>
                    {
                        { Castellano, "Area" },
                        { Portugues, "Área" },
                        { Ingles, "Area" }
                    }
                },
                {
                    "cuadrado", new Dictionary<int, string>
                    {
                        { Castellano, "Cuadrado" },
                        { Portugues, "Quadrado" },
                        { Ingles, "Square" }
                    }
                },
                {
                    "circulo", new Dictionary<int, string>
                    {
                        { Castellano, "Círculo" },
                        { Portugues, "Círculo" },
                        { Ingles, "Circle" }
                    }
                },
                {
                    "triangulo", new Dictionary<int, string>
                    {
                        { Castellano, "Triángulo" },
                        { Portugues, "Triângulo" },
                        { Ingles, "Triángulo" }
                    }
                },
                {
                    "trapecio", new Dictionary<int, string>
                    {
                        { Castellano, "Trapecio" },
                        { Portugues, "Trapézio" },
                        { Ingles, "Trapeze" }
                    }
                },
                {
                    "cuadrados", new Dictionary<int, string>
                    {
                        { Castellano, "Cuadrados" },
                        { Portugues, "Quadrados" },
                        { Ingles, "Squares" }
                    }
                },
                {
                    "circulos", new Dictionary<int, string>
                    {
                        { Castellano, "Círculos" },
                        { Portugues, "Círculo" },
                        { Ingles, "Circles" }
                    }
                },
                {
                    "triangulos", new Dictionary<int, string>
                    {
                        { Castellano, "Triángulos" },
                        { Portugues, "Triângulo" },
                        { Ingles, "Triangles" }
                    }
                },
                {
                    "trapecios", new Dictionary<int, string>
                    {
                        { Castellano, "Trapecios" },
                        { Portugues, "Trapézio" },
                        { Ingles, "Trapezoids" }
                    }
                },
            };

            return Traducciones;
        }
    }
}
