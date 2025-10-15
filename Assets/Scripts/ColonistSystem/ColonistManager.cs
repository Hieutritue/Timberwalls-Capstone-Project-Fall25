using System.Collections.Generic;

namespace DefaultNamespace.ColonistSystem
{
    public class ColonistManager : MonoSingleton<ColonistManager>
    {
        public List<Colonist> Colonists { get; set; } = new List<Colonist>();
    }
}