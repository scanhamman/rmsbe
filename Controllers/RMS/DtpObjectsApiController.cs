using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DtpObjectsApiController : BaseApiController
{
    private readonly IDtpService _dtpService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;

    public DtpObjectsApiController(IDtpService dtpService)
    {
        _dtpService = dtpService ?? throw new ArgumentNullException(nameof(dtpService));
        _parType = "DTP"; _parIdType = "id"; _entityType = "DtpObject";
        _attType = "DTP object"; _attTypes = "DTP objects";
    }
    
    /****************************************************************
    * FETCH ALL objects linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtpId:int}/objects")]
    [SwaggerOperation(Tags = new []{"DTP objects endpoint"})]
    
    public async Task<IActionResult> GetDtpObjectList(int dtpId)
    {
        if (await _dtpService.DtpExists(dtpId)) {
            var dtpObjects = await _dtpService.GetAllDtpObjects(dtpId);
            return dtpObjects != null
                ? Ok(ListSuccessResponse(dtpObjects.Count, dtpObjects))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtpId.ToString()));    
    }
    
    /****************************************************************
    * FETCH ALL objects linked to a specified DTP, with foreign key names
    ****************************************************************/

    [HttpGet("data-transfers/with-fk-names/{dtpId:int}/objects")]
    [SwaggerOperation(Tags = new []{"DTP objects endpoint"})]
    
    public async Task<IActionResult> GetDtpObjectListWfn(int dtpId)
    {
        if (await _dtpService.DtpExists(dtpId)) {
            var dtpObjectsWfn = await _dtpService.GetAllOutDtpObjects(dtpId);
            return dtpObjectsWfn != null
                ? Ok(ListSuccessResponse(dtpObjectsWfn.Count, dtpObjectsWfn))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtpId.ToString()));    
    }
    
    /****************************************************************
    * FETCH a particular object, linked to a specified DTP
    ****************************************************************/

    [HttpGet("data-transfers/{dtpId:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"DTP objects endpoint"})]
    
    public async Task<IActionResult> GetDtpObject(int dtpId, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            var dtpObj = await _dtpService.GetDtpObject(id);
            return dtpObj != null
                ? Ok(SingleSuccessResponse(new List<DtpObject>() { dtpObj }))
                : Ok(ErrorResponse("r", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }
   
    /****************************************************************
    * FETCH a particular object, linked to a specified DTP, with foreign key names
    ****************************************************************/

    [HttpGet("data-transfers/with-fk-names/{dtpId:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"DTP objects endpoint"})]
    
    public async Task<IActionResult> GetDtpObjectWfn(int dtpId, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            var dtpObjWfn = await _dtpService.GetOutDtpObject(id);
            return dtpObjWfn != null
                ? Ok(SingleSuccessResponse(new List<DtpObjectOut>() { dtpObjWfn }))
                : Ok(ErrorResponse("r", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }
    
    /****************************************************************
    * CREATE a new object, linked to a specified DTP
    ****************************************************************/

    [HttpPost("data-transfers/{dtpId:int}/objects/{sdOid}")]
    [SwaggerOperation(Tags = new []{"DTP objects endpoint"})]
    
    public async Task<IActionResult> CreateDtpObject(int dtpId, string sdOid,
           [FromBody] DtpObject dtpObjectContent)
    {
        if (await _dtpService.DtpExists(dtpId)) {
            dtpObjectContent.DtpId = dtpId;    // ensure this is the case
            dtpObjectContent.SdOid = sdOid;
            var dtpObj = await _dtpService.CreateDtpObject(dtpObjectContent);
            return dtpObj != null
                ? Ok(SingleSuccessResponse(new List<DtpObject>() { dtpObj }))
                : Ok(ErrorResponse("c", _attType, _parType, dtpId.ToString(), dtpId.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, dtpId.ToString()));  
    }  
    
    /****************************************************************
    * UPDATE an object, linked to a specified DTP
    ****************************************************************/

    [HttpPut("data-transfers/{dtpId:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"DTP objects endpoint"})]
    
    public async Task<IActionResult> UpdateDtpObject(int dtpId, int id, 
        [FromBody] DtpObject dtpObjectContent)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            dtpObjectContent.DtpId = dtpId;  // ensure this is the case
            dtpObjectContent.Id = id;
            var updatedDtpObject = await _dtpService.UpdateDtpObject(dtpObjectContent);
            return updatedDtpObject != null
                ? Ok(SingleSuccessResponse(new List<DtpObject>() { updatedDtpObject }))
                : Ok(ErrorResponse("u", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }

    /****************************************************************
    * DELETE a specified object, linked to a specified DTP
    ****************************************************************/

    [HttpDelete("data-transfers/{dtpId:int}/objects/{id:int}")]
    [SwaggerOperation(Tags = new []{"DTP objects endpoint"})]
    
    public async Task<IActionResult> DeleteDtpObject(int dtpId, int id)
    {
        if (await _dtpService.DtpAttributeExists(dtpId, _entityType, id)) {
            var count = await _dtpService.DeleteDtpObject(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, dtpId.ToString(), id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, dtpId.ToString(), id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, dtpId.ToString(), id.ToString()));
    }
}