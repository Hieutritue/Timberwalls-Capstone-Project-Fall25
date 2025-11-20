using System.Collections.Generic;

public enum BuildingCategory
{
    Construction,
    Living,
    Resource,
    Science,
    Medical,
    Entertainment,
    Defense,
}

public static class BuildingCategoryExtensions
{
    public static List<BuildingSubCategory> GetSubcategories(this BuildingCategory category)
    {
        switch (category)
        {
            case BuildingCategory.Living:
                return new List<BuildingSubCategory>()
                    { BuildingSubCategory.Bedroom, BuildingSubCategory.Bathroom, BuildingSubCategory.Kitchen };
            case BuildingCategory.Resource:
                return new List<BuildingSubCategory>()
                    { BuildingSubCategory.Food, BuildingSubCategory.Extracting, BuildingSubCategory.Refining };
            case BuildingCategory.Science:
                return new List<BuildingSubCategory>()
                    { BuildingSubCategory.Contacting, BuildingSubCategory.Researching, BuildingSubCategory.Flying };
            case BuildingCategory.Entertainment:
                return new List<BuildingSubCategory>()
                    { BuildingSubCategory.Decorating, BuildingSubCategory.Playing };
            case BuildingCategory.Construction:
                return new List<BuildingSubCategory>()
                    { BuildingSubCategory.Construction };
            case BuildingCategory.Medical:
                return new List<BuildingSubCategory>()
                    { BuildingSubCategory.Medical };
            case BuildingCategory.Defense:
                return new List<BuildingSubCategory>()
                    { BuildingSubCategory.Defense };
            default:
                return new List<BuildingSubCategory>() { };
        }
    }
}

public enum BuildingSubCategory
{
    //Construction
    Construction,

    //Living
    Bedroom,
    Bathroom,
    Kitchen,

    //Resource
    Extracting,
    Refining,

    //Science
    Contacting,
    Researching,
    Flying,

    //Medical
    Medical,

    //Entertainment
    Decorating,
    Playing,

    //Defense
    Defense,
    
    //Resource
    Food,
}