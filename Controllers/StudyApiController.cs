using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MdmService.Contracts.Responses;
using MdmService.DTO.Study;
using MdmService.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authentication;
using rmsbe.Contracts;

namespace rmsbe.Controllers
{
    public class StudyApiController : BaseApiController
    {
        private readonly IStudyRepository _studyRepository;

        public StudyApiController(IStudyRepository studyRepository)
        {
            _studyRepository = studyRepository ?? throw new ArgumentNullException(nameof(studyRepository));
        }
        
        [HttpGet("studies")]
        [SwaggerOperation(Tags = new []{"Study endpoint"})]
        public async Task<IActionResult> GetAllStudies()
        {
            var studies = await _studyRepository.GetAllStudies();
            if (studies == null)
                return Ok(new ApiResponse<StudyDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No studies have been found." },
                    Data = null
                });
            return Ok(new ApiResponse<StudyDto>()
            {
                Total = studies.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studies
            });
        }

        [HttpGet("studies/{sd_sid}")]
        [SwaggerOperation(Tags = new []{"Study endpoint"})]
        public async Task<IActionResult> GetStudyById(string sd_sid)
        {
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null) return Ok(new ApiResponse<StudyDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });

            var studyList = new List<StudyDto>() { study };
            return Ok(new ApiResponse<StudyDto>()
            {
                Total = studyList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyList
            });
        }

        [HttpPost("studies")]
        [SwaggerOperation(Tags = new []{"Study endpoint"})]
        public async Task<IActionResult> CreateStudy([FromBody] StudyDto studyDto)
        {
            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var study = await _studyRepository.CreateStudy(studyDto, accessToken);
            if (study == null)
                return Ok(new ApiResponse<StudyDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during study creation." },
                    Data = null
                });

            var studyList = new List<StudyDto>() { study };
            return Ok(new ApiResponse<StudyDto>()
            {
                Total = studyList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyList
            });
        }
        
        [HttpPut("studies/{sd_sid}")]
        [SwaggerOperation(Tags = new []{"Study endpoint"})]
        public async Task<IActionResult> UpdateStudy(string sd_sid, [FromBody] StudyDto studyDto)
        {
            studyDto.sd_sid ??= sd_sid;
            
            var study = await _studyRepository.GetStudyById(sd_sid);
            if (study == null)
                return Ok(new ApiResponse<StudyDto>()
                {
                    Total = 0,
                    StatusCode = NotFound().StatusCode,
                    Messages = new List<string>() { "No studies have been found." },
                    Data = null
                });

            var accessTokenRes = await HttpContext.GetTokenAsync("access_token");
            var accessToken = accessTokenRes?.ToString();

            var updatedStudy = await _studyRepository.UpdateStudy(studyDto, accessToken);
            if (updatedStudy == null)
                return Ok(new ApiResponse<StudyDto>()
                {
                    Total = 0,
                    StatusCode = BadRequest().StatusCode,
                    Messages = new List<string>() { "Error during study update." },
                    Data = null
                });

            var studyList = new List<StudyDto>() { updatedStudy };
            return Ok(new ApiResponse<StudyDto>()
            {
                Total = studyList.Count,
                StatusCode = Ok().StatusCode,
                Messages = null,
                Data = studyList
            });
        }

        [HttpDelete("studies/{sd_sid}")]
        [SwaggerOperation(Tags = new []{"Study endpoint"})]
        public async Task<IActionResult> DeleteStudy(string sd_sid)
        {
            var studyDto = await _studyRepository.GetStudyById(sd_sid);
            if (studyDto == null) return Ok(new ApiResponse<StudyDto>()
            {
                Total = 0,
                StatusCode = NotFound().StatusCode,
                Messages = new List<string>() { "No studies have been found." },
                Data = null
            });
            var count = await _studyRepository.DeleteStudy(sd_sid);
            return Ok(new ApiResponse<StudyDto>()
            {
                Total = count,
                StatusCode = Ok().StatusCode,
                Messages = new List<string>() { "Study has been removed." },
                Data = null
            });
        }
    }
}