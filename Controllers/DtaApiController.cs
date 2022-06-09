using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers;

public class DtaApiController : BaseApiController
{
    private readonly IRmsService _rmsService;

    public DtaApiController(IRmsService rmsService)
    {
        _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
    }

    /****************************************************************
    * FETCH ALL DTAs linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtp_id:int}/accesses")]
    [SwaggerOperation(Tags = new[] { "Data transfer access endpoint" })]

    public async Task<IActionResult> GetDtaList(int dtp_id)
    {
        if (await _rmsService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDTPResponse<Dta>);
        }
        var dtas = await _rmsService.GetAllDtasAsync(dtp_id);
        if (dtas == null || dtas.Count == 0)
        {
            return Ok(NoAttributesResponse<Dta>("No Dtas were found."));
        }
        return Ok(new ApiResponse<Dta>()
        {
            Total = dtas.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = dtas
        });
    }

    /****************************************************************
    * FETCH a particular DTA linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtp_id:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer access endpoint"})]
    
    public async Task<IActionResult> GetDta(int dtp_id, int id)
    {
        if (await _rmsService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDTPResponse<Dta>);
        }
        var dta = await _rmsService.GetDtaAsync(id);
        if (dta == null) 
        {
            return Ok(NoAttributesResponse<Dta>("No DTA with that id found."));
        }       
        return Ok(new ApiResponse<Dta>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Dta> { dta }
        });
    }

    /****************************************************************
    * CREATE a new DTA, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtp_id:int}/accesses")]
    [SwaggerOperation(Tags = new []{"Data transfer access endpoint"})]
    
    public async Task<IActionResult> CreateDta(int dtp_id, 
         [FromBody] Dta dtaContent)
    {
        if (await _rmsService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDTPResponse<Dta>);
        }
        dtaContent.DtpId = dtp_id;
        var dta = await _rmsService.CreateDtaAsync(dtaContent);
        if (dta == null)
        {
            return Ok(ErrorInActionResponse<Dta>("Error during DTA creation."));
        }    
        return Ok(new ApiResponse<Dta>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Dta>() { dta }
        });
    }

    /****************************************************************
    * UPDATE a DTA, linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtp_id:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer access endpoint"})]
    
    public async Task<IActionResult> UpdateDta(int dtp_id, int id, 
           [FromBody] Dta dtaContent)
    {
        if (await _rmsService.DtpAttributeDoesNotExistAsync(dtp_id, "DTA", id))
        {
            return Ok(ErrorInActionResponse<Dta>("No agreement with that id found for specified DTP."));
        }
        var updatedDta = await _rmsService.UpdateDtaAsync(id, dtaContent);
        if (updatedDta == null) 
        {
            return Ok(ErrorInActionResponse<Dta>("Error during DTA update."));
        }   
        return Ok(new ApiResponse<Dta>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<Dta>() { updatedDta }
        });
    }

    /****************************************************************
    * DELETE a specified DTA, linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtp_id:int}/accesses/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer access endpoint"})]
    
    public async Task<IActionResult> DeleteDta(int dtp_id, int id)
    {
        if (await _rmsService.DtpAttributeDoesNotExistAsync(dtp_id, "DTA", id))
        {
            return Ok(ErrorInActionResponse<Dta>("No agreement with that id found for specified DTP."));
        }
        var count = await _rmsService.DeleteDtaAsync(id);
        return Ok(new ApiResponse<Dtp>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>(){"DTA has been removed."}, Data = null
        });
    }
}