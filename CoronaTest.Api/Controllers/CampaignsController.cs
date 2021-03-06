using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using System.ComponentModel.DataAnnotations;
using CoronaTest.Core.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace CoronaTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]
    public class CampaignsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CampaignsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// returns all campaigns DTO's
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<KampagneDto>>> GetCampaigns()
        {
            var campaigns = await _unitOfWork.Campaigns.GetAllCampaignsDtosAsync();
            if (campaigns == null)
            {
                return NotFound();
            }
            return Ok(campaigns);
        }

        /// <summary>
        /// Returns the camapaign dto by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Campaign>> GetCampaign(int id)
        {
            var campaign = await _unitOfWork.Campaigns.GetCampaignByIdAsync(id);

            if (campaign == null)
            {
                return NotFound();
            }

            return Ok(campaign);
        }

        /// <summary>
        /// Updates the given campaign
        /// </summary>
        /// <param name="id"></param>
        /// <param name="campaign"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCampaign(int id, Campaign campaign)
        {
            if (id != campaign.Id)
            {
                return BadRequest();
            }

            _unitOfWork.Campaigns.UpdateCampaignsData(campaign);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CampaignExistsAsync(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Posts a new campaign into the database
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Campaign>> PostCampaign(Campaign campaign)
        {
            await _unitOfWork.Campaigns.AddCampaignAsync(campaign);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            return CreatedAtAction("GetCampaign", new { id = campaign.Id }, campaign);
        }

        /// <summary>
        /// Removes campaign by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCampaign(int id)
        {
            var campaign = await _unitOfWork.Campaigns.GetCampaignByIdAsync(id);
            if (campaign == null)
            {
                return NotFound();
            }

            await _unitOfWork.Campaigns.RemoveCampaignAsync(campaign.Id);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            return NoContent();
        }

        private async Task<bool> CampaignExistsAsync(int id)
        {
            return await _unitOfWork.Campaigns.GetCampaignByIdAsync(id) != null;
        }
    }
}
