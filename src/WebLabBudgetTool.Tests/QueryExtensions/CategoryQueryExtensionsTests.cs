using System.Collections.Generic;
using System.Linq;
using WebLabBudgetTool.Entities;
using WebLabBudgetTool.QueryExtension;
using Xunit;

namespace WebLabBudgetTool.Tests.QueryExtensions
{
    public class CategoryQueryExtensionsTests
    {
        [Fact]
        public void NameNotNull()
        {
            // Arrange
            var categoryQueryList = new List<Category>
                {
                    new Category {Id = 1, Name = null},
                    new Category {Id = 2, Name = null},
                    new Category {Id = 3, Name = "Rent"}
                }
                .AsQueryable();

            // Act
            var resultList = categoryQueryList.NameNotNull().ToList();

            // Assert
            Assert.Equal(1, resultList.Count);
            Assert.Equal(3, resultList[0].Id);
        }

        [Fact]
        public void NameContains()
        {
            // Arrange
            var categoryQueryList = new List<Category>
                {
                    new Category {Id = 1, Name = "abc123de"},
                    new Category {Id = 2, Name = "123"},
                    new Category {Id = 3, Name = "Rent"}
                }
                .AsQueryable();

            // Act
            var resultList = categoryQueryList.NameContains("123").ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(1, resultList[0].Id);
            Assert.Equal(2, resultList[1].Id);
        }
        
        [Fact]
        public void NameContainsOrderByName()
        {
            // Arrange
            var categoryQueryList = new List<Category>
                {
                    new Category {Id = 1, Name = "c"},
                    new Category {Id = 2, Name = "d"},
                    new Category {Id = 3, Name = "a"}
                }
                .AsQueryable();

            // Act
            var resultList = categoryQueryList.OrderByName().ToList();

            // Assert
            Assert.Equal(3, resultList.Count);
            Assert.Equal(3, resultList[0].Id);
            Assert.Equal(1, resultList[1].Id);
            Assert.Equal(2, resultList[2].Id);
        }
    }
}
