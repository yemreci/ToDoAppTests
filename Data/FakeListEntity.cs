using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoAppTests.Models
{
    public class FakeListEntity
    {
        public static Faker<ListEntity> GenerateListEntity(int num)
        {
            return new Faker<ListEntity>()
                .RuleFor(l => l.Name, f => f.Name.FullName())
                .RuleFor(l => l.Todos, f => GenerateTodoEntities(f, num)); // Change 3 to the desired number of todo items
        }
        private static List<ToDoEntity> GenerateTodoEntities(Faker f, int count)
        {
            var todoFaker = new Faker<ToDoEntity>()
                .RuleFor(t => t.Description, f => f.Random.Words())
                .RuleFor(t => t.IsComplete, f => f.Random.Bool());

            return todoFaker.Generate(count);
        }
    }
}
