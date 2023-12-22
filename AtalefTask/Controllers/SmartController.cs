using AtalefTask.DTOs;
using AtalefTask.Models;
using AtalefTask.Services.Interfaces;
using AtalefTask.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AtalefTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmartController : ControllerBase
    {
        private ISmartMatchService smartMatchService;
        private IMapper mapper;

        public SmartController(ISmartMatchService smartMatchService, IMapper mapper)
        {
            this.smartMatchService = smartMatchService;
            this.mapper = mapper;
        }
        /// <summary>
        /// Save the User`s unique value
        /// </summary>
        /// <param name="viewModel">The ViewModel of item.</param>
        /// <returns>The created item with the specified ID.</returns>
        /// <response code="200">Returns the created item.</response>
        /// <response code="409">If User or Value already exists.</response>
        [HttpPost]
        public async Task<ActionResult<SmartMatchDTO>> Create([FromBody]SmartMatchViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SmartMatchItem item = mapper.Map<SmartMatchItem>(viewModel);
            item = await smartMatchService.Create(item);

            return mapper.Map<SmartMatchDTO>(item);

        }

        /// <summary>
        /// Update the User`s Item value
        /// </summary>
        /// <param name="id">The item ID.</param>
        /// <param name="viewModel">The ViewModel of item.</param>
        /// <returns>The updated item with the specified ID.</returns>
        /// <response code="200">Returns the created item.</response>
        /// <response code="409">If User or Value already exists.</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<SmartMatchDTO>> Update([FromRoute]int id, [FromBody]SmartMatchViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SmartMatchItem item = mapper.Map<SmartMatchItem>(viewModel);
            item = await smartMatchService.Update(id, item);

            return mapper.Map<SmartMatchDTO>(item);
        }

        /// <summary>
        /// Delete the User`s Item
        /// </summary>
        /// <param name="id">The item ID.</param>
        /// <param name="viewModel">The ViewModel of item.</param>
        /// <returns>Deteltion result.</returns>
        /// <response code="200">Returns the boolean if row was deleted.</response>
        /// <response code="404">If item not found.</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> Delete([FromRoute] int id)
        {
            return await smartMatchService.Delete(id);
        }
    }
}
