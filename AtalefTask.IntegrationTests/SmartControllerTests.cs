using AtalefTask.DTOs;
using AtalefTask.Infrastructure;
using AtalefTask.Models;
using AtalefTask.ViewModels;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace AtalefTask.IntegrationTests
{
    public class SmartControllerTests : IClassFixture<WebFixture>
    {
        private HttpClient client;

        public SmartControllerTests(WebFixture fixture)
        {
            this.client = fixture.Client;
        }
        #region Create()
        [Fact]
        public async Task Create_FailValidationBecauseOfEmptyValue()
        {
            var response = await client.PostAsJsonAsync("/api/Smart/", new SmartMatchViewModel
            {
                UserId = 1001,
                UniqueValue = ""
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task Create_FailValidationBecauseOfNegativeUserId()
        {
            var response = await client.PostAsJsonAsync("/api/Smart/", new SmartMatchViewModel
            {
                UserId = -11,
                UniqueValue = "xxx"
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Create_WithExitingUserId()
        {
            var response = await client.PostAsJsonAsync("/api/Smart/", new SmartMatchViewModel { 
                UserId = 1001,
                UniqueValue = "abra"
            });
            var body = await Deserialize<RestException>(response);

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            Assert.Equal("User already exists", body.Message);
        }

        [Fact]
        public async Task Create_WithExitingUniqueValue()
        {
            var response = await client.PostAsJsonAsync("/api/Smart/", new SmartMatchViewModel
            {
                UserId = 10013,
                UniqueValue = "qwerty"
            });
            var body = await Deserialize<RestException>(response);

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            Assert.Equal("Value already exists", body.Message);
        }

        [Fact]
        public async Task Create_ReturnsStatusOK()
        {
            string UNIQUE_VALUE = "unique_string_from_universe";
            int USER_ID = 10011;

            var response = await client.PostAsJsonAsync("/api/Smart/", new SmartMatchViewModel
            {
                UserId = USER_ID,
                UniqueValue = UNIQUE_VALUE
            });
            var body = await Deserialize<SmartMatchDTO>(response);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(USER_ID, body.UserId);
            Assert.Equal(UNIQUE_VALUE, body.UniqueValue);
        }
        #endregion

        #region Update()
        private const int UPDATE_ID = 1;
        [Fact]
        public async Task Update_WithExitingUniqueValue()
        {   
            var response = await client.PutAsJsonAsync($"/api/Smart/{UPDATE_ID}", 
                new SmartMatchViewModel
                {
                    UserId = 1001,
                    UniqueValue = "asdad"
                });
            var body = await Deserialize<RestException>(response);

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
            Assert.Equal("Value already exists", body.Message);
        }

        [Fact]
        public async Task Update_FailValidationBecauseOfEmptyValue()
        {
            var response = await client.PutAsJsonAsync($"/api/Smart/{UPDATE_ID}", 
                new SmartMatchViewModel
                {
                    UserId = 1001,
                    UniqueValue = ""
                });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_FailValidationBecauseOfNegativeUserId()
        {
            var response = await client.PutAsJsonAsync($"/api/Smart/{UPDATE_ID}", new SmartMatchViewModel
            {
                UserId = -1,
                UniqueValue = "Update_FailValidationBecauseOfNegativeUserId"
            });

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_ItemNotFound()
        {
            var response = await client.PutAsJsonAsync($"/api/Smart/{int.MaxValue}", new SmartMatchViewModel
            {
                UserId = 1001,
                UniqueValue = "Update_ItemNotFound"
            });

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Update_45ItemsWithSuccess()
        {
            Random random = new Random();
            var requests = Data.DBItems.Skip(2).Take(45).Select(x =>
                client.PutAsJsonAsync($"/api/Smart/{x.Id}", 
                new SmartMatchViewModel
                    {
                        UserId = x.UserId,
                        UniqueValue = Data.GenerateRandomString(random, 8)
                    })
            );
            
            var responses = await Task.WhenAll(requests);

            int OKStatusCount = responses.Count(x => x.IsSuccessStatusCode);
            Assert.Equal(requests.Count(), OKStatusCount);
        }
        #endregion

        #region Delete()
        [Fact]
        public async Task Delete_ItemNotFound()
        {
            var response = await client.DeleteAsync($"/api/Smart/{int.MaxValue}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_RemoveRowFromDB()
        {
            var targetItem = Data.DBItems.LastOrDefault();

            var response = await client.DeleteAsync($"/api/Smart/{targetItem.Id}");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        #endregion

        private async Task<T> Deserialize<T>(HttpResponseMessage response)
        {
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}