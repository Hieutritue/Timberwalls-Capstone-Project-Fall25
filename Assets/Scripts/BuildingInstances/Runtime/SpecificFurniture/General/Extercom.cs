using ResourceSystem;

namespace BuildingSystem
{
    public class Extercom : Furniture
    {
        private ExtercomSO ExtercomSo => (ExtercomSO)PlaceableSo;

        public override void Constructed()
        {
            base.Constructed();
            ResourceManager.Instance.Set(ResourceType.ContactPoint,
                ResourceManager.Instance.Get(ResourceType.ContactPoint) + ExtercomSo.ContactPoint);
        }
        
        public override void Demolished()
        {
            base.Demolished();
            ResourceManager.Instance.Set(ResourceType.ContactPoint,
                ResourceManager.Instance.Get(ResourceType.ContactPoint) - ExtercomSo.ContactPoint);
        }
    }
}