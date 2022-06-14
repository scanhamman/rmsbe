using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpObjectsApiController : BaseApiController
{
    private readonly IRmsTransferService _rmsService;

    public DtpObjectsApiController(IRmsTransferService rmsService)
    {
        _rmsService = rmsService ?? throw new ArgumentNullException(nameof(rmsService));
    }
    
    /****************************************************************
    * FETCH ALL objects linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtp_id:int}/objects")]
    [SwaggerOperation(Tags = new []{"Data transfer process objects endpoint"})]
    
    public async Task<IActionResult> GetDtpObjectList(int dtp_id)
    {
        if (await _rmsService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDTPResponse<DtpObject>);
        }
        var dtpObjects = await _rmsService.GetAllDtpObjectsAsync(dtp_id);
        if (dtpObjects == null || dtpObjects.Count == 0)
        {
            return Ok(NoAttributesResponse<DtpObject>("No objects were found for the specified DTP."));
        }   
        return Ok(new ApiResponse<DtpObject>()
        {
            Total = dtpObjects.Count, StatusCode = Ok().StatusCode, Messages = null,
            Data = dtpObjects
        });
    }

    /****************************************************************
    * FETCH a particular object, linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtp_id:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process objects endpoint"})]
    
    public async Task<IActionResult> GetDtpObject(int dtp_id, int id)
    {
        if (await _rmsService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDTPResponse<DtpObject>);
        }
        var dtpObj = await _rmsService.GetDtpObjectAsync(id);
        if (dtpObj == null) 
        {
            return Ok(NoAttributesResponse<DtpObject>("No DTP object with that id found."));
        }        
        return Ok(new ApiResponse<DtpObject>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DtpObject>() { dtpObj }
        });
    }

    /****************************************************************
    * CREATE a new object, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtp_id:int}/objects/{sd_oid}")]
    [SwaggerOperation(Tags = new []{"Data transfer process objects endpoint"})]
    
    public async Task<IActionResult> CreateDtpObject(int dtp_id, string sd_oid,
           [FromBody] DtpObject dtpObjectContent)
    {
        if (await _rmsService.DtpDoesNotExistAsync(dtp_id))
        {
            return Ok(NoDTPResponse<DtpObject>);
        }
        dtpObjectContent.DtpId = dtp_id;
        dtpObjectContent.ObjectId = sd_oid;
        var dtpObj = await _rmsService.CreateDtpObjectAsync(dtpObjectContent);
        if (dtpObj == null)
        {
            return Ok(ErrorInActionResponse<DtpObject>("Error during DTP object creation."));
        }    
        return Ok(new ApiResponse<DtpObject>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DtpObject>() { dtpObj }
        });
    }

    /****************************************************************
    * UPDATE an object, linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtp_id:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process objects endpoint"})]
    
    public async Task<IActionResult> UpdateDtpObject(int dtp_id, int id, 
        [FromBody] DtpObject dtpObjectContent)
    {
        if (await _rmsService.DtpAttributeDoesNotExistAsync(dtp_id, "DTPObject", id))
        {
            return Ok(ErrorInActionResponse<DtpObject>("No object with that id found for specified DTP."));
        }
        var updatedDtpObject = await _rmsService.UpdateDtpObjectAsync(id, dtpObjectContent);
        if (updatedDtpObject == null)
        {
            return Ok(ErrorInActionResponse<DtpObject>("Error during DTP object update."));
        }      
        return Ok(new ApiResponse<DtpObject>()
        {
            Total = 1, StatusCode = Ok().StatusCode, Messages = null,
            Data = new List<DtpObject>() { updatedDtpObject }
        });
    }

    /****************************************************************
    * DELETE a specified object, linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtp_id:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"Data transfer process objects endpoint"})]
    
    public async Task<IActionResult> DeleteDtpObject(int dtp_id, int id)
    {
        if (await _rmsService.DtpAttributeDoesNotExistAsync(dtp_id, "DTPObject", id))
        {
            return Ok(ErrorInActionResponse<DtpObject>("No object with that id found for specified DTP."));
        }
        var count = await _rmsService.DeleteDtpObjectAsync(id);
        return Ok(new ApiResponse<DtpObject>()
        {
            Total = count, StatusCode = Ok().StatusCode,
            Messages = new List<string>(){"DTP object has been removed."}, Data = null
        });
    }
}