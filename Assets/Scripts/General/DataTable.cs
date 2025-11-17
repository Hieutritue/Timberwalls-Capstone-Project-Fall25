using General;
using UnityEngine.Serialization;

namespace DefaultNamespace.General
{
    public class DataTable : MonoSingleton<DataTable>
    {
        public GeneralNumberSO GeneralNumberSo;
        public ColonistActionCollectionSo ColonistActionCollectionSo;
        public BuildingsCollectionSo BuildingsCollectionSo;
    }
}