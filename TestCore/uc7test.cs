using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TestCore
{
    [TestFixture]
    public class UC7_GroceryListShareTests
    {
        private Mock<IGroceryListRepository> _groceryListRepositoryMock;
        private Mock<IFileSaverService> _fileSaverServiceMock;
        private GroceryListService _service;

        [SetUp]
        public void Setup()
        {
            _groceryListRepositoryMock = new Mock<IGroceryListRepository>();
            _fileSaverServiceMock = new Mock<IFileSaverService>();

            _service = new GroceryListService(
                _groceryListRepositoryMock.Object,
                _fileSaverServiceMock.Object
            );
        }

        [Test]
        public async Task ShareGroceryList_WithItems_SavesFile()
        {
            var items = new List<GroceryListItem>
            {
                new(1, 1, 101, 2),
                new(2, 1, 102, 1)
            };

            await _service.ShareGroceryList(items, CancellationToken.None);

            _fileSaverServiceMock.Verify(f => f.SaveFileAsync(
                "Boodschappen.json",
                It.Is<string>(s => s.Contains("101") && s.Contains("102")),
                It.IsAny<CancellationToken>()
            ), Times.Once);
        }

        [Test]
        public async Task ShareGroceryList_WithNoItems_DoesNotSaveFile()
        {
            var items = new List<GroceryListItem>();

            await _service.ShareGroceryList(items, CancellationToken.None);

            _fileSaverServiceMock.Verify(f => f.SaveFileAsync(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()
            ), Times.Never);
        }
    }
}
