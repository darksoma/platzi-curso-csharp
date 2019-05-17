using System;

namespace CoreEscuela.Entidades
{
    public abstract class ObjetoEscuelaBase
    {
        public string UniqueId { get; private set; } = Guid.NewGuid().ToString();
        public string Nombre { get; set; }

        public ObjetoEscuelaBase()
        {
            
        }
    }
}