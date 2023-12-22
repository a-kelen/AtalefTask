using AtalefTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace AtalefTask.UnitTests
{
    public class DBMock
    {
        private Mock<ApplicationContext> contextMock;
        private Mock<IDbContextTransaction> transactionMock;

        public Mock<ApplicationContext> ContextMock { get { return contextMock; } }
        public Mock<IDbContextTransaction> TransactionMock { get { return transactionMock; } }

        public DBMock()
        {
            contextMock = new Mock<ApplicationContext>();
            var databaseMock = new Mock<DatabaseFacade>(contextMock.Object);
            var strategyMock = new Mock<IExecutionStrategy>();
            strategyMock
            .Setup(strategy => strategy.ExecuteAsync(
                It.IsAny<object>(),
                It.IsAny<Func<DbContext, object, CancellationToken, Task<object>>>(),
                It.IsAny<Func<DbContext, object, CancellationToken, Task<ExecutionResult<object>>>>(),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(
                (
                    object state,
                    Func<DbContext, object, CancellationToken, Task<SmartMatchItem>> operation,
                    Func<DbContext, object, CancellationToken, Task<SmartMatchItem>> verify,
                    CancellationToken ct
                ) =>
                {
                    return operation.Invoke(contextMock.Object, contextMock.Object, ct);
                }
            );

            transactionMock = new Mock<IDbContextTransaction>();
            databaseMock.Setup(x => x.CreateExecutionStrategy()).Returns(strategyMock.Object);
            databaseMock.Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(transactionMock.Object);
            contextMock.Setup(x => x.Database).Returns(databaseMock.Object);

            var entities = new List<SmartMatchItem>() {
                new SmartMatchItem { Id = 1, UserId = 1, UniqueValue = "qwerty" }
            };
            contextMock.Setup(x => x.SmartMatchResult).ReturnsDbSet(entities);
        }


        public void MockSmartMatchDbSet(IEnumerable<SmartMatchItem> entities)
        {
            contextMock.Setup(x => x.SmartMatchResult).ReturnsDbSet(entities);
        }
    }
}
