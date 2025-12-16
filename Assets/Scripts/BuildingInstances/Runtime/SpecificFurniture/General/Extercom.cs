using ResourceSystem;

namespace BuildingSystem
{
    public class Extercom : Furniture
    {
        private ExtercomSO ExtercomSo => (ExtercomSO)PlaceableSo;

        public void AddContactPoint()
        {
            ResourceManager.Instance.Set(ResourceType.ContactPoint,
                ResourceManager.Instance.Get(ResourceType.ContactPoint) + ExtercomSo.ContactPoint);
        }
        
        public void RemoveContactPoint()
        {
            ResourceManager.Instance.Set(ResourceType.ContactPoint,
                ResourceManager.Instance.Get(ResourceType.ContactPoint) - ExtercomSo.ContactPoint);
        }
    }
}