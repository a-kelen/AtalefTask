using AtalefTask.Models;

namespace AtalefTask.IntegrationTests
{
    internal class Data
    {
        internal static List<SmartMatchItem> InitItems = GenerateSmartMatchItems(3, 50);
        internal static List<SmartMatchItem> DBItems = null!;

        private static List<SmartMatchItem> GenerateSmartMatchItems(int from, int to)
        {
            var random = new Random();
            List<SmartMatchItem> items = new List<SmartMatchItem>
            {
                new SmartMatchItem { UserId = 1001, UniqueValue = "qwerty" },
                new SmartMatchItem { UserId = 1002, UniqueValue = "asdad" },
            };
                
            items.AddRange(Enumerable.Range(from, to - from + 1)
                .Select(i => new SmartMatchItem
                {
                    UniqueValue = GenerateRandomString(random, 8),
                    UserId = 1000 + i,
                    Date = DateTimeOffset.UtcNow.AddHours(random.Next(-365, 0))
                })
                .ToList()
            );

            return items;
        }

        public static string GenerateRandomString(Random random, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}
