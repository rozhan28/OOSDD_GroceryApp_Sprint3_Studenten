using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Text.Json;

namespace Grocery.Core.Services
{
    public class GroceryListService : IGroceryListService
    {
        private readonly IGroceryListRepository _groceryRepository;
        private readonly IFileSaverService _fileSaverService;

        public GroceryListService(IGroceryListRepository groceryRepository, IFileSaverService fileSaverService)
        {
            _groceryRepository = groceryRepository;
            _fileSaverService = fileSaverService;
        }

        public List<GroceryList> GetAll()
        {
            return _groceryRepository.GetAll();
        }

        public GroceryList Add(GroceryList item)
        {
            return _groceryRepository.Add(item);
        }

        public GroceryList? Delete(GroceryList item)
        {
            return _groceryRepository.Delete(item);
        }

        public GroceryList? Get(int id)
        {
            return _groceryRepository.Get(id);
        }

        public GroceryList? Update(GroceryList item)
        {
            return _groceryRepository.Update(item);
        }

        // 🔹 UC7 - Delen boodschappenlijst
        public async Task ShareGroceryList(IEnumerable<GroceryListItem> items, CancellationToken cancellationToken)
        {
            if (items == null || !items.Any())
                return;

            string jsonString = JsonSerializer.Serialize(items);
            await _fileSaverService.SaveFileAsync("Boodschappen.json", jsonString, cancellationToken);
        }

        // 🔹 UC8 - Zoek producten (op basis van allProducts en huidige lijst)
        public List<Product> SearchProducts(IEnumerable<Product> allProducts, string? searchTerm, IEnumerable<GroceryListItem> itemsOnList)
        {
            var availableProducts = allProducts
                .Where(p => itemsOnList.All(i => i.ProductId != p.Id) && p.Stock > 0);

            if (string.IsNullOrWhiteSpace(searchTerm))
                return availableProducts.ToList();

            return availableProducts
                .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
