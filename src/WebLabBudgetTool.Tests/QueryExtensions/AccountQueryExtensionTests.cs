
using System.Collections.Generic;
using System.Linq;
using WebLabBudgetTool.Entities;
using WebLabBudgetTool.QueryExtension;
using Xunit;

namespace WebLabBudgetTool.Tests.QueryExtensions
{
    public class AccountQueryExtensionTests
    {
        [Fact]
        public void AreNotExcluded()
        {
            // Arrange
            var accountListQuery = new List<Account>
                {
                    new Account {Id = 1, IsExcluded = true},
                    new Account {Id = 2, IsExcluded = false},
                    new Account {Id = 3, IsExcluded = false}
                }
                .AsQueryable();

            // Act
            var resultList = accountListQuery.AreNotExcluded().ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(2, resultList[0].Id);
            Assert.Equal(3, resultList[1].Id);
        }

        [Fact]
        public void AreExcluded()
        {
            // Arrange
            var accountListQuery = new List<Account>
                {
                    new Account {Id = 1, IsExcluded = true},
                    new Account {Id = 2, IsExcluded = false},
                    new Account {Id = 3, IsExcluded = false}
                }
                .AsQueryable();

            // Act
            var resultList = accountListQuery.AreExcluded().ToList();

            // Assert
            Assert.Equal(1, resultList.Count);
            Assert.Equal(1, resultList[0].Id);
        }

        [Fact]
        public void NameEquals()
        {
            // Arrange
            var accountListQuery = new List<Account>
                {
                    new Account {Id = 1, Name = "Foo"},
                    new Account {Id = 2, Name = "Income"},
                    new Account {Id = 3, Name = "SomethingElse"}
                }
                .AsQueryable();

            // Act
            var resultList = accountListQuery.NameEquals("Income").ToList();

            // Assert
            Assert.Equal(1, resultList.Count);
            Assert.Equal(2, resultList[0].Id);
        }

        [Fact]
        public void OrderByName()
        {
            // Arrange
            var accountListQuery = new List<Account>
                {
                    new Account {Id = 1, Name = "Foo"},
                    new Account {Id = 2, Name = "Alber"},
                    new Account {Id = 3, Name = "Thom"}
                }
                .AsQueryable();

            // Act
            var resultList = accountListQuery.OrderByName().ToList();

            // Assert
            Assert.Equal(3, resultList.Count);
            Assert.Equal(2, resultList[0].Id);
            Assert.Equal(1, resultList[1].Id);
            Assert.Equal(3, resultList[2].Id);
        }
    }
}
