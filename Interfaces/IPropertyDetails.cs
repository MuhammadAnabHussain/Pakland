using Pakland.Models;

namespace Pakland.Services
{
    public interface IPropertyService
    {
        PropertyDetails GetPropertyById(int id);
        void UpdateProperty(PropertyDetails property);
        // Other methods as needed
    }
}
