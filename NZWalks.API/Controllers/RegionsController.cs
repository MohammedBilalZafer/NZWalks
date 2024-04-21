using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            //Get data from Database
            var regionsDomain = await regionRepository.GetAllAsync();

            //Map Domain models to DTOs
            
            //Show DTOs
            return Ok(mapper.Map<List<RegionDto>>(regionsDomain));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetById([FromRoute]Guid id) 
        {
            //Get Region Domain Model from Database
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if(regionDomain == null)
            {
                return NotFound();
            }

            

            //Return DTO to client
            return Ok(mapper.Map<RegionDto>(regionDomain));
        }

        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //Map Dtos into Domain Models
            var regionDomain = mapper.Map<Region>(addRegionRequestDto);

            //Use Domain Model to create region
            regionDomain = await regionRepository.CreateAsync(regionDomain);

            //Map Domain model back to DTO
            var regionDto = mapper.Map<RegionDto>(regionDomain);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map DTO to Domain Model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            //check if region exists
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            if (regionDomainModel == null)
            {
                return NotFound();
            }


            //Convert Domain Model to DTO
     
            return Ok(mapper.Map<UpdateRegionRequestDto>(regionDomainModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles ="Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) 
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }
    }
}
