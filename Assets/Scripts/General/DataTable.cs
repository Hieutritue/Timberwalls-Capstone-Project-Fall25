using System.Collections.Generic;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.ColonistSystem.AfflictionSystem;
using General;
using UnityEngine.Serialization;

namespace DefaultNamespace.General
{
    public class DataTable : MonoSingleton<DataTable>
    {
        public GeneralNumberSO GeneralNumberSo;
        public ColonistActionCollectionSo ColonistActionCollectionSo;
        public BuildingsCollectionSo BuildingsCollectionSo;
        public List<ColonistSO> ColonistSos;
        public List<AfflictionSO> AfflictionSos;
    }
}