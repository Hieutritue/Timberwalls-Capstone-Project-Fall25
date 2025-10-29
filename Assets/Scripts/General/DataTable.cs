using General;
using UnityEngine.Serialization;

namespace DefaultNamespace.General
{
    public class DataTable : MonoSingleton<DataTable>
    {
        public GeneralNumberSO GeneralNumberSo;
        [FormerlySerializedAs("PersonalActionCollectionSo")] public ColonistActionCollectionSo ColonistActionCollectionSo;
    }
}