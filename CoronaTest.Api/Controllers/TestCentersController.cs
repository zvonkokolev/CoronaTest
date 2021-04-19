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
using System.ComponentModel.DataAnnotations;
using CoronaTest.Core.DTOs;
using CoronaTest.Api.Dtos;

namespace CoronaTest.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestCentersController : ControllerBase
	{
		private readonly IUnitOfWork _unitOfWork;

		public TestCentersController(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		/// <summary>
		/// returns all test centers DTO's
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<IEnumerable<ZentrumDto>>> GetTestCenters()
		{
			List<ZentrumDto> testCenters = await _unitOfWork.TestCenters.GetAllTestCentersDtosAsync();
			if (testCenters == null)
			{
				return NotFound();
			}
			return Ok(testCenters);
		}

		/// <summary>
		/// Returns the test center dto by given id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult<ZentrumDto>> GetTestCenter(int id)
		{
			ZentrumDto testCenter = await _unitOfWork.TestCenters.GetTestCenterDtoByIdAsync(id);

			if (testCenter == null)
			{
				return NotFound();
			}

			return Ok(testCenter);
		}

		/// <summary>
		/// Updates the given test center
		/// </summary>
		/// <param name="id"></param>
		/// <param name="testCenter"></param>
		/// <returns></returns>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> PutTestCenter(int id, TestCenter testCenter)
		{
			if (id != testCenter.Id)
			{
				return BadRequest();
			}

			_unitOfWork.TestCenters.UpdateTestCentersData(testCenter);

			try
			{
				await _unitOfWork.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!await TestCenterExistsAsync(id))
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
		/// Posts a new test center into the database
		/// </summary>
		/// <param name="testCenter"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<TestCenter>> PostTestCenter(ZentrumDto testCenter)
		{
			await _unitOfWork.TestCenters.AddTestCenterAsync(testCenter);
			try
			{
				await _unitOfWork.SaveChangesAsync();
			}
			catch (ValidationException e)
			{
				return BadRequest(e.Message);
			}
			return NoContent();
			//return CreatedAtAction("GetTestCenter", new { id = testCenter.Id }, testCenter);
		}

		/// <summary>
		/// Removes test center by given id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteTestCenter(int id)
		{
			var testCenter = await _unitOfWork.TestCenters.GetTestCenterByIdAsync(id);
			if (testCenter == null)
			{
				return NotFound();
			}

			await _unitOfWork.TestCenters.RemoveTestCenterAsync(testCenter.Id);
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

		private async Task<bool> TestCenterExistsAsync(int id)
		{
			return await _unitOfWork.TestCenters.GetTestCenterByIdAsync(id) != null;
		}
	}
}
