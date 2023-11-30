using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Controllers;
using ToDoApp.Data;
using ToDoApp.Domain;
using ToDoApp.Models;
using ToDoAppTests.Models;

namespace ToDoAppTests
{
    public class ToDoControllerTests
    {
        private readonly ToDoController _sut;
        private readonly ILogger<ToDoController> _logger = Substitute.For<ILogger<ToDoController>>();
        private readonly IToDoOperations _toDoOperations = Substitute.For<IToDoOperations>();
        public ToDoControllerTests()
        {
            _sut = new ToDoController(_logger, _toDoOperations);
        }
        [Fact]
        public async Task GetToDoItems_ShouldReturnToDoList_WhenListExists()
        {
            // Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoOperations.GetToDoItems(fakeList1.Name).Returns(Task.FromResult(new List<ToDoEntity>(fakeList1.Todos)));
            // Act
            var result = await _sut.GetToDoItems(fakeList1.Name);
            var actualList = (result.Result as OkObjectResult)?.Value as List<ToDoEntity>;
            // Assert
            Assert.Equal(new List<ToDoEntity>(fakeList1.Todos), actualList);
        }
        [Fact]
        public async Task GetToDoItems_ShouldReturnBadRequestObjectResult_WhenTheresNoListWithGivenName()
        {
            //Arrange
            _toDoOperations.GetToDoItems("somelistname").Returns(Task.FromResult(new List<ToDoEntity> { }));
            //Act
            var result = await _sut.GetToDoItems("somelistname");
            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        [Fact]
        public async Task GetToDoItems_ShouldReturnBadRequestObjectResult_WhenListNameIsNullOrEmpty()
        {
            //Arrange
            
            //Act
            var result = await _sut.GetToDoItems("");
            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        [Fact]
        public async Task GetToDoItem_ShouldReturnBadRequestObjectResult_WhenListNameIsNullOrEmpty()
        {
            //Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoOperations.GetToDoItem(fakeList1.Name, fakeList1.Todos[0].Id).Returns(Task.FromResult(fakeList1.Todos[0]));
            //Act
            var result = await _sut.GetToDoItem("", fakeList1.Todos[0].Id);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        [Fact]
        public async Task GetToDoItem_ShouldReturnBadRequestObjectResult_WhenResultIsNull()
        {
            //Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(0).Generate();
            _toDoOperations.GetToDoItem(fakeList1.Name, 0).Returns(Task.FromResult<ToDoEntity>(null));
            //Act
            var result = await _sut.GetToDoItem(fakeList1.Name, 0);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        [Fact]
        public async Task GetToDoItem_ShouldReturnResult_WhenItemExists()
        {
            //Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoOperations.GetToDoItem(fakeList1.Name, fakeList1.Todos[0].Id).Returns(Task.FromResult(fakeList1.Todos[0]));
            //Act
            var result = await _sut.GetToDoItem(fakeList1.Name, fakeList1.Todos[0].Id);
            var actualItem = (result.Result as OkObjectResult)?.Value as ToDoEntity;
            //Assert
            Assert.Equal(fakeList1.Todos[0],actualItem);
        }
        [Fact]
        public async Task CheckToDoMark_ShouldReturnBadRequestObjectResult_WhenListNameIsEmpty()
        {
            //Arrange
            
            //Act
            var result = await _sut.CheckToDoMark("",0);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task CheckToDoMark_ShouldReturnBadRequestObjectResult_WhenOperationFailed()
        {
            //Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoOperations.CheckToDoMark(fakeList1.Name, fakeList1.Todos[0].Id).Returns(Task.FromResult(false));
            //Act
            var result = await _sut.CheckToDoMark(fakeList1.Name,fakeList1.Todos[0].Id);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task CheckToDoMark_ShouldReturnOkObjectResult_WhenOperationSuccess()
        {
            //Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoOperations.CheckToDoMark(fakeList1.Name, fakeList1.Todos[0].Id).Returns(Task.FromResult(true));
            //Act
            var result = await _sut.CheckToDoMark(fakeList1.Name, fakeList1.Todos[0].Id);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task UnCheckToDoMark_ShouldReturnBadRequestObjectResult_WhenListNameIsEmpty()
        {
            //Arrange

            //Act
            var result = await _sut.UnCheckToDoMark("", 0);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UnCheckToDoMark_ShouldReturnBadRequestObjectResult_WhenOperationFailed()
        {
            //Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoOperations.UnCheckToDoMark(fakeList1.Name, fakeList1.Todos[0].Id).Returns(Task.FromResult(false));
            //Act
            var result = await _sut.UnCheckToDoMark(fakeList1.Name, fakeList1.Todos[0].Id);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UnCheckToDoMark_ShouldReturnOkObjectResult_WhenOperationSuccess()
        {
            //Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoOperations.UnCheckToDoMark(fakeList1.Name, fakeList1.Todos[0].Id).Returns(Task.FromResult(true));
            //Act
            var result = await _sut.UnCheckToDoMark(fakeList1.Name, fakeList1.Todos[0].Id);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task UpdateToDoDescription_ShouldReturnBadRequestObjectResult_WhenListNameIsEmpty()
        {
            //Arrange

            //Act
            var result = await _sut.UpdateToDoDescription("",0,"");
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UpdateToDoDescription_ShouldReturnBadRequestObjectResult_WhenOperationFailed()
        {
            //Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoOperations.UpdateToDoDescription(fakeList1.Name, fakeList1.Todos[0].Id, "somedesc").Returns(Task.FromResult(false));
            //Act
            var result = await _sut.UpdateToDoDescription(fakeList1.Name, fakeList1.Todos[0].Id, "somedesc");
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task UpdateToDoDescription_ShouldReturnOkObjectResult_WhenOperationSuccess()
        {
            //Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoOperations.UpdateToDoDescription(fakeList1.Name, fakeList1.Todos[0].Id, "somedesc").Returns(Task.FromResult(true));
            //Act
            var result = await _sut.UpdateToDoDescription(fakeList1.Name, fakeList1.Todos[0].Id, "somedesc");
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task AddToDo_ShouldReturnBadRequestObjectResult_WhenListNameIsNullOrEmpty()
        {
            //Arrange

            //Act
            var result = await _sut.AddToDo("",new ToDoDTO()
            {
                Description = "asd",
                IsComplete = true
            });
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task AddToDo_ShouldReturnBadRequestObjectResult_WhenOperationFailed()
        {
            //Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoOperations.AddToDo(fakeList1.Name, Arg.Is<ToDoEntity>(x => x.Description == "asd" && x.IsComplete == false)).Returns(Task.FromResult(false));
            //Act
            var result = await _sut.AddToDo(fakeList1.Name, new ToDoDTO() { Description = "asd", IsComplete = false });
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task AddToDo_ShouldReturnOkObjectResult_WhenOperationSuccess()
        {
            //Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            var item = new ToDoEntity() { Description = "asd", IsComplete = false };
            _toDoOperations.AddToDo(fakeList1.Name,Arg.Is<ToDoEntity>(x => x.Description == "asd" && x.IsComplete == false)).Returns(Task.FromResult(true));
            //Act
            var result = await _sut.AddToDo(fakeList1.Name, new ToDoDTO() { Description = "asd", IsComplete = false });
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public async Task DeleteToDoFromList_ShouldReturnBadRequestObjectResult_WhenListNameIsNullOrEmpty()
        {
            //Arrange

            //Act
            var result = await _sut.DeleteToDoFromList("", 0);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task DeleteToDoFromList_ShouldReturnBadRequestObjectResult_WhenOperationFailed()
        {
            //Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoOperations.DeleteToDo(fakeList1.Name, 0).Returns(Task.FromResult(false));
            //Act
            var result = await _sut.DeleteToDoFromList(fakeList1.Name, 0);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task DeleteToDoFromList_ShouldReturnOkObjectResult_WhenOperationSuccess()
        {
            //Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoOperations.DeleteToDo(fakeList1.Name, 0).Returns(Task.FromResult(true));
            //Act
            var result = await _sut.DeleteToDoFromList(fakeList1.Name, 0);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
        [Theory]
        [InlineData("", "Somelistname")]
        [InlineData("Somelistname", "")]
        [InlineData("", "")]
        public async Task MoveToDoToAnotherList_ShouldReturnBadRequestObjectResult_WhenListNameIsNullOrEmpty(string listName1, string listName2)
        {
            //Arrange

            //Act
            var result = await _sut.MoveToDoToAnotherList(listName1,0,listName2);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task MoveToDoToAnotherList_ShouldReturnBadRequestObjectResult_WhenOperationFailed()
        {
            //Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            var fakeList2 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoOperations.MoveToDo(fakeList1.Name, 0,fakeList2.Name).Returns(Task.FromResult(false));
            //Act
            var result = await _sut.MoveToDoToAnotherList(fakeList1.Name, 0,fakeList2.Name);
            //Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }
        [Fact]
        public async Task MoveToDoToAnotherList_ShouldReturnOkObjectResult_WhenOperationSuccess()
        {
            //Arrange
            var fakeList1 = FakeListEntity.GenerateListEntity(3).Generate();
            var fakeList2 = FakeListEntity.GenerateListEntity(3).Generate();
            _toDoOperations.MoveToDo(fakeList1.Name, 0, fakeList2.Name).Returns(Task.FromResult(true));
            //Act
            var result = await _sut.MoveToDoToAnotherList(fakeList1.Name, 0, fakeList2.Name);
            //Assert
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
