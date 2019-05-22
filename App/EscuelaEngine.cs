using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.Entidades;
using CoreEscuela.Util;

namespace CoreEscuela.App
{
    public sealed class EscuelaEngine
    {
        public Escuela Escuela { get; set; }

        public EscuelaEngine()
        {

        }

        public void Inicializar()
        {
            Escuela = new Escuela("Platzi Academy", 2012, TiposEscuela.Primaria,
            ciudad: "Bogotá", pais: "Colombia"
            );

            CargarCursos();
            CargarAsignaturas();
            CargarEvaluaciones();
            Console.WriteLine("---");

        }

        public void ImprimirDiccionario(Dictionary<LlavesDiccionario, IEnumerable<ObjetoEscuelaBase>> dic, bool imprVal = false)
        {
            foreach(var obj in dic)
            {
                Printer.WriteTitle(obj.Key.ToString());
                foreach(var val in obj.Value)
                {
                    switch(obj.Key)
                    {
                        case LlavesDiccionario.Evaluaciones:
                            if(imprVal)
                            {
                                Console.WriteLine(val);
                            }
                        break;
                        
                        case LlavesDiccionario.Escuela:
                            Console.WriteLine("Escuela: " + val);
                        break;
                        
                        case LlavesDiccionario.Alumnos:
                            Console.WriteLine("Alumno: " + val.Nombre);
                        break;
                        
                        case LlavesDiccionario.Cursos:
                            var curtmp = val as Curso;
                            if(curtmp!= null)
                            {
                                int count = ((Curso)val).Alumnos.Count;
                                Console.WriteLine("Curso: " + val.Nombre + " Cantidad alumnos: " + count);
                            }
                        break;

                        default:
                            Console.WriteLine(val);
                        break;
                    }
                }
            }
        }

        public Dictionary<LlavesDiccionario, IEnumerable<ObjetoEscuelaBase>> GetDiccionarioObjetos()
        {
            var diccionario = new Dictionary<LlavesDiccionario, IEnumerable<ObjetoEscuelaBase>>();

            diccionario.Add(LlavesDiccionario.Escuela, new [] {Escuela});
            diccionario.Add(LlavesDiccionario.Cursos, Escuela.Cursos.Cast<ObjetoEscuelaBase>());
            var listaTmp = new List<Evaluación>();
            var listaTmpAs = new List<Asignatura>();
            var listaTmpAl = new List<Alumno>();

            foreach(var cur in Escuela.Cursos)
            {
                listaTmpAl.AddRange(cur.Alumnos);
                listaTmpAs.AddRange(cur.Asignaturas);

                
                foreach(var alumno in cur.Alumnos)
                {
                    listaTmp.AddRange(alumno.Evaluaciones);
                }
            }
            diccionario.Add(LlavesDiccionario.Evaluaciones, listaTmp.Cast<ObjetoEscuelaBase>());
            diccionario.Add(LlavesDiccionario.Asignaturas, listaTmpAs.Cast<ObjetoEscuelaBase>());
            diccionario.Add(LlavesDiccionario.Alumnos, listaTmpAl.Cast<ObjetoEscuelaBase>());
            
            
            return diccionario;
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
            bool traeEvaluaciones = true,
            bool traeAlumnos = true,
            bool traeAsignaturas = true,
            bool traeCursos = true
        ){
            return GetObjetosEscuela(out int dummy, out dummy, out dummy, out dummy);
        }

        public IReadOnlyList<ObjetoEscuelaBase> GetObjetosEscuela(
            out int conteoEvaluaciones,
            out int conteoAlumnos,
            out int conteoAsignaturas,
            out int conteoCursos,
            bool traeEvaluaciones = true,
            bool traeAlumnos = true,
            bool traeAsignaturas = true,
            bool traeCursos = true
        )
        {
            conteoEvaluaciones = conteoAlumnos = conteoAsignaturas = conteoCursos = 0;
            var listaObj = new List<ObjetoEscuelaBase>();
            listaObj.Add(Escuela);
            
            if (traeCursos)
                listaObj.AddRange(Escuela.Cursos);
                conteoCursos += Escuela.Cursos.Count;

            foreach (var curso in Escuela.Cursos)
            {
                if(traeAsignaturas)
                    listaObj.AddRange(curso.Asignaturas);
                    conteoAsignaturas += curso.Asignaturas.Count;
                
                if(traeAlumnos)
                    listaObj.AddRange(curso.Alumnos);
                    conteoAlumnos += curso.Alumnos.Count;

                if (traeEvaluaciones){
                    foreach (var alumno in curso.Alumnos)
                    {
                        listaObj.AddRange(alumno.Evaluaciones);
                        conteoEvaluaciones +=alumno.Evaluaciones.Count;
                    }
                }
            }

            return listaObj;
        }

        #region metodos de carga
    
        private void CargarEvaluaciones()
        {
            var rnd = new Random(System.Environment.TickCount);

            foreach (var curso in Escuela.Cursos)
            {
                foreach (var asignatura in curso.Asignaturas)
                {
                    foreach (var alumno in curso.Alumnos)
                    {

                        for (int i = 0; i < 5; i++)
                        {
                            var ev = new Evaluación
                            {
                                Asignatura = asignatura,
                                Nombre = $"{asignatura.Nombre} Ev#{i + 1}",
                                Nota = (float)Math.Round(5 * rnd.NextDouble(),2),
                                Alumno = alumno
                            };
                            alumno.Evaluaciones.Add(ev);
                        }
                    }
                }
            }

        }

        private void CargarAsignaturas()
        {
            foreach (var curso in Escuela.Cursos)
            {
                var listaAsignaturas = new List<Asignatura>(){
                            new Asignatura{Nombre="Matemáticas"} ,
                            new Asignatura{Nombre="Educación Física"},
                            new Asignatura{Nombre="Castellano"},
                            new Asignatura{Nombre="Ciencias Naturales"}
                };
                curso.Asignaturas = listaAsignaturas;
            }
        }

        private List<Alumno> GenerarAlumnosAlAzar(int cantidad)
        {
            string[] nombre1 = { "Alba", "Felipa", "Eusebio", "Farid", "Donald", "Alvaro", "Nicolás" };
            string[] apellido1 = { "Ruiz", "Sarmiento", "Uribe", "Maduro", "Trump", "Toledo", "Herrera" };
            string[] nombre2 = { "Freddy", "Anabel", "Rick", "Murty", "Silvana", "Diomedes", "Nicomedes", "Teodoro" };

            var listaAlumnos = from n1 in nombre1
                               from n2 in nombre2
                               from a1 in apellido1
                               select new Alumno { Nombre = $"{n1} {n2} {a1}" };

            return listaAlumnos.OrderBy((al) => al.UniqueId).Take(cantidad).ToList();
        }

        private void CargarCursos()
        {
            Escuela.Cursos = new List<Curso>(){
                        new Curso(){ Nombre = "101", Jornada = TiposJornada.Mañana },
                        new Curso() {Nombre = "201", Jornada = TiposJornada.Mañana},
                        new Curso{Nombre = "301", Jornada = TiposJornada.Mañana},
                        new Curso(){ Nombre = "401", Jornada = TiposJornada.Tarde },
                        new Curso() {Nombre = "501", Jornada = TiposJornada.Tarde},
            };

            Random rnd = new Random();
            foreach (var c in Escuela.Cursos)
            {
                int cantRandom = rnd.Next(5, 20);
                c.Alumnos = GenerarAlumnosAlAzar(cantRandom);
            }
        }
    }
    #endregion
}