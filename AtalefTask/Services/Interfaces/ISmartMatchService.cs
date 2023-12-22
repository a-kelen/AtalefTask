using AtalefTask.Models;

namespace AtalefTask.Services.Interfaces
{
    public interface ISmartMatchService
    {
        public Task<SmartMatchItem> Create(SmartMatchItem item);
        public Task<SmartMatchItem> Update(int id, SmartMatchItem item);
        public Task<bool> Delete(int id);
    }
}
