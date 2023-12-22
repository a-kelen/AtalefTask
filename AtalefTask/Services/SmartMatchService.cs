using AtalefTask.Infrastructure;
using AtalefTask.Models;
using AtalefTask.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace AtalefTask.Services
{
    public class SmartMatchService : ISmartMatchService
    {
        private ApplicationContext context;

        public SmartMatchService(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<SmartMatchItem> Create(SmartMatchItem item)
        {
            var strategy = context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable))
                {
                    try
                    {
                        bool existUser = await context.SmartMatchResult
                            .AnyAsync(x => x.UserId == item.UserId);
                        bool existValue = await context.SmartMatchResult
                            .AnyAsync(x => x.UniqueValue == item.UniqueValue);

                        if (existUser)
                        {
                            throw new ConflictException("User already exists");
                        }
                        if (existValue)
                        {
                            throw new ConflictException("Value already exists");
                        }

                        item.Date = DateTimeOffset.UtcNow;
                        var createdEntity = await context.SmartMatchResult.AddAsync(item);

                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return createdEntity.Entity;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            });
        }

        public async Task<SmartMatchItem> Update(int id, SmartMatchItem item)
        {
            var strategy = context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable))
                {
                    try
                    {
                        SmartMatchItem? existingItem = await context.SmartMatchResult
                            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == item.UserId);
                        bool existValue = await context.SmartMatchResult
                            .AnyAsync(x => x.UniqueValue == item.UniqueValue);


                        if (existingItem == null)
                        {
                            throw new NotFoundException("Item not found");
                        }

                        if (existValue && existingItem?.UniqueValue != item.UniqueValue)
                        {
                            throw new ConflictException("Value already exists");
                        }

                        existingItem.UniqueValue = item.UniqueValue;
                        existingItem.Date = DateTimeOffset.UtcNow;
                        var updatedEntity = context.SmartMatchResult.Update(existingItem);

                        await context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return updatedEntity.Entity;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            });
        }

        public async Task<bool> Delete(int id)
        {
            var strategy = context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable))
                {
                    try
                    {
                        SmartMatchItem? existingItem = await context.SmartMatchResult
                            .FirstOrDefaultAsync(x => x.Id == id);

                        if (existingItem == null)
                        {
                            throw new NotFoundException("Item not found");
                        }

                        context.SmartMatchResult.Remove(existingItem!);
                        int result = await context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return result > 0;
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            });
        }
    }
}
