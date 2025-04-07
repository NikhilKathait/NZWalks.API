using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Collections.Generic;

namespace NZWalks.API.Controllers
{
    // https://localhost:7056/api/regions
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository,
            IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }


        // GET ALL REGIONS
        // GET : https://localhost:7056/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get Data From Database - Domain models
            // New: Get Data from Region Repository which will get data from Database.
            // Implemented Repository Pattern
            var regionsDomain = await regionRepository.GetAllAsync();

            // Map Domain Models to DTOs using Mapper
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

            // Return DTOs back to client
            return Ok(regionsDto);
        }


        // GET SINGLE REGION (Get Region By ID)
        // GET : https://localhost:7056/api/regions/{id}
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // Get Region Domain Model From Database
            // Implemented Repository Pattern
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            // Return Dto back to client
            return Ok(mapper.Map<List<RegionDto>>(regionDomain));
        }


        // POST To Create New Region
        // POST : https://localhost:7056/api/regions
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // Map / Convert DTO to Domain Model using mapper
            var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);

            // Implemented Repository pattern
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            // Map Domain Model back to DTO

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }


        // UPDATE REGION (Update Region By ID)
        // PUT : https://localhost:7056/api/regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            // Map DTO to Domain Model using mapper
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Convert Domain Model to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);

            return Ok(regionDto);
        }


        // DELETE REGION 
        // DELETE : https://localhost:7056/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // Implemented Repository Pattern
            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Return Deleted Region back
            // Map Domain Model to Dto
            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            return Ok(regionDto);
        }
    }
}
