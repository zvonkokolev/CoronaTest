using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoronaTest.Core.Entities;
using CoronaTest.Core.Interfaces;
using CoronaTest.Core.DTOs;
using CoronaTest.Api.Dtos;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace CoronaTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize()]
    public class ExaminationsController : ControllerBase
    {
        //private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ExaminationsController(IUnitOfWork unitOfWork) //, IMapper mapper
        {
            //_mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// returns all examinations DTO's
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TestsDto>>> GetExaminations()
        {
            var examinations = await _unitOfWork.Examinations.GetAllExaminationsDtosAsync();
            if(examinations == null)
            {
                return NotFound();
            }
            return Ok(examinations);
        }

        /// <summary>
        /// Returns the examination dto by given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ExaminationsDto>> GetExamination(int id)
        {
            TestsDto examination = await _unitOfWork.Examinations.GetExaminationDtoByIdAsync(id);

            if (examination == null)
            {
                return NotFound();
            }

            return Ok(examination);
        }

        /// <summary>
        /// Updates the given examination
        /// </summary>
        /// <param name="id"></param>
        /// <param name="examination"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutExamination(int id, Examination examination)
        {
            if (id != examination.Id)
            {
                return BadRequest();
            }

            _unitOfWork.Examinations.UpdateExamination(examination);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExaminationExistsAsync(id))
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
        /// Posts a new examination into the database
        /// </summary>
        /// <param name="examination"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<Examination>> PostExamination(Examination examination)
        {
            await _unitOfWork.Examinations.AddExaminationAsync(examination);
            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Message);
            }
            return CreatedAtAction("GetExamination", new { id = examination.Id }, examination);
        }

        /// <summary>
        /// Removes examinations by given id
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
        public async Task<IActionResult> DeleteExamination(int id)
        {
            var examination = await _unitOfWork.Examinations.GetExaminationByIdAsync(id);
            if (examination == null)
            {
                return NotFound();
            }

            _unitOfWork.Examinations.RemoveExamination(examination);
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

        private async Task<bool> ExaminationExistsAsync(int id)
        {
            return await _unitOfWork.Examinations.GetExaminationByIdAsync(id) != null;
        }
    }
}
