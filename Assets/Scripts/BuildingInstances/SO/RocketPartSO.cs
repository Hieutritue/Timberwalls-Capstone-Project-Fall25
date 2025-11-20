namespace BuildingSystem
{
    public class RocketPartSO : PlaceableSO
    {
        public RocketPartType RocketPartType;
    }
    
    public enum RocketPartType
    {
        Hull,
        Engine,
    }
}