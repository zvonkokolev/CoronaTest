using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoronaTest.Core.Entities;
using CoronaTest.Persistence;
using CoronaTest.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace CoronaTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ParticipantsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// returns all participants
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Participant>>> GetParticipants()
        {
            var participants = await _unitOfWork.Participants
                .GetAllParticipantsAsync();
            if (participants == null)
            {
                return NotFound();
            }
            return Ok(participants);
        }

        /// <summary>
        /// Returns the participant by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Participant>> GetParticipant(int id)
        {
            var participant = await _unitOfWork.Participants
                .GetParticipantByIdAsync(id);

            if (participant == null)
            {
                return NotFound();
            }

            return Ok(participant);
        }

        /// <summary>
        /// Updates the given participant
        /// </summary>
        /// <param name="id"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutParticipant(int id, Participant participant)
        {
            if (id != participant.Id)
            {
                return BadRequest();
            }

            _unitOfWork.Participants
                .UpdateParticipantsData(participant);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ParticipantExists(id))
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
        /// Posts a new participant into the database
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Participant>> PostParticipant(Participant participant)
        {
            await _unitOfWork.Participants
                .AddParticipantAsync(participant);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }         
            return CreatedAtAction("GetParticipant", new { id = participant.Id }, participant);
        }

        /// <summary>
        /// Removes participant by given id
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
        public async Task<IActionResult> DeleteParticipant(int id)
        {
            var participant = await _unitOfWork.Participants
                .GetParticipantByIdAsync(id);
            if (participant == null)
            {
                return NotFound();
            }

            await _unitOfWork.Participants
                .RemoveParticipantAsync(participant.Id);
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

        private async Task<bool> ParticipantExists(int id)
        {
            return await _unitOfWork.Participants
                .GetParticipantByIdAsync(id) != null;
        }
    }
}
