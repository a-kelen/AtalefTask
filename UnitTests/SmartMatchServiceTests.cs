using AtalefTask.Infrastructure;
using AtalefTask.Models;
using AtalefTask.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtalefTask.UnitTests
{
    public class SmartMatchServiceTests
    {
        [Fact]
        [Trait("method", "Create")]
        public async Task Create_UserExists_ThrowConflictExceptionWithTransactionRollback()
        {
            var dbMock = new DBMock();
            var contextMock = dbMock.ContextMock;
            var transactionMock = dbMock.TransactionMock;
            var item = new SmartMatchItem { UserId = 1, UniqueValue = "Argfd#d" };
            var service = new SmartMatchService(contextMock.Object);

            await Assert.ThrowsAsync<ConflictException>(() => service.Create(item));
            transactionMock.Verify(m => m.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        [Trait("method", "Create")]
        public async Task Create_ValueExists_ThrowConflictExceptionWithTransactionRollback()
        {
            var dbMock = new DBMock();
            var contextMock = dbMock.ContextMock;
            var transactionMock = dbMock.TransactionMock;
            var item = new SmartMatchItem { UserId = 1, UniqueValue = "Argfd#d" };
            var service = new SmartMatchService(contextMock.Object);

            await Assert.ThrowsAsync<ConflictException>(() => service.Create(item));
            transactionMock.Verify(m => m.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        [Trait("method", "Create")]
        public async Task Create_Return_SaveAndReturnCreatedItem()
        {
            var dbMock = new DBMock();
            var contextMock = dbMock.ContextMock;
            var item = new SmartMatchItem { UserId = 1, UniqueValue = "Argfd#d" };
            var service = new SmartMatchService(contextMock.Object);

            var result = await service.Create(item);
            
            Assert.NotNull(result);
        }
    }
}
