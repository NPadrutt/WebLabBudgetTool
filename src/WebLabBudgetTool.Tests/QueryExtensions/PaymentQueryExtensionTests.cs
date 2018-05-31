using System;
using System.Collections.Generic;
using System.Linq;
using WebLabBudgetTool.Entities;
using WebLabBudgetTool.QueryExtension;
using Xunit;

namespace WebLabBudgetTool.Tests.QueryExtensions
{
    public class PaymentQueryExtensionTests
    {
        [Fact]
        public void IsNotCleared()
        {
            // Arrange
            var paymentListQuery = new List<Payment>
                {
                    new Payment {Id = 1, IsCleared = true},
                    new Payment {Id = 2, IsCleared = false},
                    new Payment {Id = 3, IsCleared = false}
                }
                .AsQueryable();

            // Act
            var resultList = paymentListQuery.AreNotCleared().ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(2, resultList[0].Id);
            Assert.Equal(3, resultList[1].Id);
        }

        [Fact]
        public void IsCleared()
        {
            // Arrange
            var paymentListQuery = new List<Payment>
                {
                    new Payment {Id = 1, IsCleared = true},
                    new Payment {Id = 2, IsCleared = false},
                    new Payment {Id = 3, IsCleared = false}
                }
                .AsQueryable();

            // Act
            var resultList = paymentListQuery.AreCleared().ToList();

            // Assert
            Assert.Equal(1, resultList.Count);
            Assert.Equal(1, resultList[0].Id);
        }

        [Fact]
        public void HasDateLargerEqualsThan()
        {
            // Arrange
            var date = DateTime.Now;
            var paymentListQuery = new List<Payment>
                {
                    new Payment {Id = 1, Date = date.AddDays(-1)},
                    new Payment {Id = 2, Date = date},
                    new Payment {Id = 3, Date = date.AddDays(1)}
                }
                .AsQueryable();

            // Act
            var resultList = paymentListQuery.HasDateLargerEqualsThan(date).ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(2, resultList[0].Id);
            Assert.Equal(3, resultList[1].Id);
        }

        [Fact]
        public void HasDateSmallerEqualsThan()
        {
            // Arrange
            var date = DateTime.Now;
            var paymentListQuery = new List<Payment>
                {
                    new Payment {Id = 1, Date = date.AddDays(-1)},
                    new Payment {Id = 2, Date = date},
                    new Payment {Id = 3, Date = date.AddDays(1)}
                }
                .AsQueryable();

            // Act
            var resultList = paymentListQuery.HasDateSmallerEqualsThan(date).ToList();

            // Assert
            Assert.Equal(2, resultList.Count);
            Assert.Equal(1, resultList[0].Id);
            Assert.Equal(2, resultList[1].Id);
        }

        [Fact]
        public void HasAccountId()
        {
            // Arrange
            var paymentListQuery = new List<Payment>
                {
                    new Payment {Id = 1, AccountId = 4 },
                    new Payment {Id = 2, AccountId = 4 },
                    new Payment {Id = 3, AccountId = 1 },
                    new Payment {Id = 4, AccountId = 3 },

                }
                .AsQueryable();

            // Act
            var resultList = paymentListQuery.HasAccountId(1).ToList();

            // Assert
            Assert.Equal(1, resultList.Count);
            Assert.Equal(3, resultList[0].Id);
        }
    }
}
