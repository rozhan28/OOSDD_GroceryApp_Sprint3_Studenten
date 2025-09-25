using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace TestCore
{
    [TestFixture]
    public class UC8_GroceryListSearchTests
    {
        private GroceryListService _service;

        [SetUp]
        public void Setup()
        {
            var groceryListRepositoryMock = new Mock<IGroceryListRepository>();
            var fileSaverServiceMock = new Mock<IFileSaverService>();

            _service = new GroceryListService(
                groceryListRepositoryMock.Object,
                fileSaverServiceMock.Object
            );
        }

        public static IEnumerable<TestCaseData> SearchCases
        {
            get
            {
                yield return new TestCaseData("an", 2, new[] { "Banana", "Orange" })
                    .SetName("Zoekterm 'an' moet Banana en Orange vinden");
                yield return new TestCaseData(null, 3, new[] { "Apple", "Banana", "Orange" })
                    .SetName("Lege zoekterm moet alle beschikbare producten teruggeven");
                yield return new TestCaseData("xyz", 0, new string[0])
                    .SetName("Niet-bestaande zoekterm moet lege lijst teruggeven");
            }
        }

        [TestCaseSource(nameof(SearchCases))]
        public void SearchProducts_WithVariousQueries_ReturnsCorrectResults(string? searchTerm, int expectedCount, string[] expectedNames)
        {
            var allProducts = new List<Product>
            {
                new(1, "Apple", 10),
                new(2, "Banana", 10),
                new(3, "Orange", 10),
                new(4, "Milk", 10),
                new(5, "Bread", 0) 
            };

            var itemsOnList = new List<GroceryListItem>
            {
                new(1, 1, 4, 1)
            };

            var results = _service.SearchProducts(allProducts, searchTerm, itemsOnList);

            Assert.AreEqual(expectedCount, results.Count, "Aantal resultaten klopt niet");
            CollectionAssert.AreEquivalent(expectedNames, results.Select(p => p.Name), "Productnamen komen niet overeen");
        }
    }
}
