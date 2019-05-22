using System;
using System.Linq;
using System.Collections.Generic;
using CoreEscuela.Entidades;

namespace CoreEscuela.App
{
    public class Reporteador
    {
        Dictionary<LlavesDiccionario, IEnumerable<ObjetoEscuelaBase>> _diccionario;
        public Reporteador(Dictionary<LlavesDiccionario, IEnumerable<ObjetoEscuelaBase>> dicObsEsc)
        {
            if(dicObsEsc == null)
            {
                throw new ArgumentNullException(nameof(dicObsEsc));
            }
            _diccionario = dicObsEsc;
        }

        public IEnumerable<Evaluación> GetListaEvaluaciones(){

            if(_diccionario.TryGetValue(LlavesDiccionario.Evaluaciones, out IEnumerable<ObjetoEscuelaBase> lista))
            {
                return lista.Cast<Evaluación>();
            }
            {
                return new List<Evaluación>();
            }
        }
        
        public IEnumerable<string> GetListaAsignaturas(out IEnumerable<Evaluación> ListaEvaluaciones)
        {

            ListaEvaluaciones = GetListaEvaluaciones();
            return (from Evaluación ev in ListaEvaluaciones
                   select ev.Asignatura.Nombre).Distinct();
        }

        public IEnumerable<string> GetListaAsignaturas()
        {
            return GetListaAsignaturas(out var dummy);
        }

        public Dictionary<string, IEnumerable<Evaluación>> GetListaEvaluacionesXAsig()
        {
            var dicrta = new Dictionary<string, IEnumerable<Evaluación>>();
            var listaAsig = GetListaAsignaturas(out var listaEval);
            
            foreach(var asig in listaAsig)
            {
                var evalsAsig =  from eval in listaEval
                                where eval.Asignatura.Nombre == asig
                                select eval;

                dicrta.Add(asig, evalsAsig);
            }
            
            return dicrta; 
        }
    }
}