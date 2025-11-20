using ResourceSystem;

namespace BuildingSystem
{
    public class Extercom : Furniture
    {
        private ExtercomSO ExtercomSo => (ExtercomSO)PlaceableSo;

        public override void OnConstructed()
        {
            base.OnConstructed();
            ResourceManager.Instance.Set(ResourceType.ContactPoint,
                ResourceManager.Instance.Get(ResourceType.ContactPoint) + ExtercomSo.ContactPoint);
        }
        
        public override void OnDemolished()
        {
            base.OnDemolished();
            ResourceManager.Instance.Set(ResourceType.ContactPoint,
                ResourceManager.Instance.Get(ResourceType.ContactPoint) - ExtercomSo.ContactPoint);
        }
    }
}