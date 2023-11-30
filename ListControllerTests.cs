using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ToDoApp.Controllers;
using ToDoApp.Data;
using ToDoApp.Domain;
using ToDoApp.Models;
using ToDoAppTests.Models;

namespace ToDoAppTests
{
    public class ListControllerTests
    {
        private readonly ListController _sut;
        private readonly ILogger<ListController> _logger = Substitute.For<ILogger<ListController>>();
        private readonly IToDoListOperations _toDoListOperations = Substitute.For<IToDoListOperations>();
        public ListControllerTests()
        {
            _sut = new ListController(_logger, _toDoListOperations);
        }
        [Fact]
        public async Task GetToDoLists_ShouldReturnToDoLists_WhenListsExists()
        {
            // Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            var fakeList2 = FakeListEntity.GenerateListEntity(3).Generate();
            var fakeList3 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoListOperations.GetToDoLists().Returns(Task.FromResult(new List<ListEntity> { fakeList1, fakeList2, fakeList3 }));
            // Act
            var result = await _sut.GetToDoLists();
            var actualList = (result.Result as OkObjectResult)?.Value as List<ListEntity>;
            // Assert
            Assert.Equal(new List<ListEntity> { fakeList1, fakeList2, fakeList3 }, actualList);
        }
        [Fact]
        public async Task GetToDoLists_ShouldReturnBadRequestObjectResult_WhenTheresNoLists()
        {
            // Arrange
            _toDoListOperations.GetToDoLists().Returns(Task.FromResult(new List<ListEntity> {}));
            // Act
            var result = await _sut.GetToDoLists();
            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        [Fact]
        public async Task GetToDoLists_ShouldReturnBadRequestObjectResult_WhenListsNull()
        {
            // Arrange
            _toDoListOperations.GetToDoLists().Returns(Task.FromResult<List<ListEntity>>(null));
            // Act
            var result = await _sut.GetToDoLists();
            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        [Fact]
        public async Task GetToDoListByName_ShouldReturnListObject_WhenListExists()
        {
            // Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoListOperations.GetListByName("somelistname").Returns(Task.FromResult(fakeList1));
            // Act
            var result = await _sut.GetToDoListByName("somelistname");
            var actualList = (result.Result as OkObjectResult)?.Value as ListEntity;
            // Assert
            Assert.Equal(fakeList1, actualList);
        }
        [Fact]
        public async Task GetToDoListByName_ShouldReturnBadRequestObjectResult_WhenListDoesNotExists()
        {
            // Arrange
            _toDoListOperations.GetListByName("somelistname").Returns(Task.FromResult<ListEntity>(null));
            // Act
            var result = await _sut.GetToDoListByName("somelistname");
            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        [Fact]
        public async Task GetToDoListByName_ShouldReturnBadRequestObjectResult_WhenListNameIsEmpty()
        {
            // Arrange
            
            // Act
            var result = await _sut.GetToDoListByName("");
            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        [Fact]
        public async Task UpdateListName_ShouldReturnOkObjectResult_WhenListNameUpdated()
        {
            // Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoListOperations.UpdateListName(fakeList1.Name,"Somelistname").Returns(Task.FromResult(true));
            // Act
            var result = await _sut.UpdateListName(fakeList1.Name, "Somelistname");
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task UpdateListName_ShouldReturnBadRequestObjectResult_WhenListNameUpdateFailed()
        {
            // Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoListOperations.UpdateListName(fakeList1.Name, "Somelistname").Returns(Task.FromResult(false));
            // Act
            var result = await _sut.UpdateListName(fakeList1.Name, "Somelistname");
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Theory]
        [InlineData("", "Somelistname")]
        [InlineData("Somelistname", "")]    
        public async Task UpdateListName_ShouldReturnBadRequestObjectResult_WhenParametersAreNullOrEmpty(string listId, string newListName)
        {
            // Arrange

            // Act
            var result = await _sut.UpdateListName(listId, newListName);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task CreateToDoList_ShouldReturnOkObjectResult_WhenListCreated()
        {
            // Arrange
            _toDoListOperations.CreateList("Somelistname").Returns(Task.FromResult(true));
            // Act
            var result = await _sut.CreateToDoList("Somelistname");
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task CreateToDoList_ShouldReturnBadRequestObjectResult_WhenListCreationFailed()
        {
            // Arrange
            _toDoListOperations.CreateList("Somelistname").Returns(Task.FromResult(false));
            // Act
            var result = await _sut.CreateToDoList("Somelistname");
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task CreateToDoList_ShouldReturnBadRequestObjectResult_WhenParameterIsNullOrEmpty()
        {
            // Arrange

            // Act
            var result = await _sut.CreateToDoList("");
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task DeleteToDoList_ShouldReturnOkObjectResult_WhenParameterIsNullOrEmpty()
        {
            // Arrange

            // Act
            var result = await _sut.CreateToDoList("");
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task DeleteToDoList_ShouldReturnBadRequestObjectResult_WhenListDeletionFailed()
        {
            // Arrange
            _toDoListOperations.DeleteList("Somelistname").Returns(Task.FromResult(false));
            // Act
            var result = await _sut.DeleteToDoList("Somelistname");
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task DeleteToDoList_ShouldReturnBadRequestObjectResult_WhenParameterIsNullOrEmpty()
        {
            // Arrange

            // Act
            var result = await _sut.DeleteToDoList("");
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task MergeToDoLists_ShouldReturnOkObjectResult_WhenListsMerged()
        {
            // Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            var fakeList2 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoListOperations.MergeLists(fakeList1.Name, fakeList2.Name).Returns(Task.FromResult(true));
            // Act
            var result = await _sut.MergeLists(fakeList1.Name,fakeList2.Name);
            // Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task MergeToDoLists_ShouldReturnBadRequestObjectResult_WhenListMergeFailed()
        {
            // Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            var fakeList2 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoListOperations.MergeLists(fakeList1.Name, fakeList2.Name).Returns(Task.FromResult(false));
            // Act
            var result = await _sut.MergeLists(fakeList1.Name, fakeList2.Name);
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Theory]
        [InlineData("", "Somelistname")]
        [InlineData("Somelistname", "")]
        [InlineData("", "")]
        public async Task MergeToDoLists_ShouldReturnBadRequestObjectResult_WhenListNamesNullOrEmpty(string firstListName, string secondListName)
        {
            // Arrange

            // Act
            var result = await _sut.MergeLists(firstListName, secondListName);
            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}