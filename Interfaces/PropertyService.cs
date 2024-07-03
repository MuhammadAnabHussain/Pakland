using Pakland.Data;
using Pakland.Models;

namespace Pakland.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _context;
        // Other dependencies as needed

        public PropertyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public PropertyDetails GetPropertyById(int id)
        {
            return _context.PropertyDetails.Find(id);
        }

        public void UpdateProperty(PropertyDetails property)
        {
            _context.PropertyDetails.Update(property);
            _context.SaveChanges();
        }

        // Implement other methods as needed
    }
}
