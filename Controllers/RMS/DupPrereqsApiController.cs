using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using rmsbe.SysModels;
using rmsbe.Services.Interfaces;

namespace rmsbe.Controllers.RMS;

public class DupPrereqsApiController : BaseApiController
{
    private readonly IDupService _dupService;
    private readonly string _parType, _parIdType;
    private readonly string _attType, _attTypes, _entityType;
    
    public DupPrereqsApiController(IDupService dupService)
    {
        _dupService = dupService ?? throw new ArgumentNullException(nameof(dupService));
        _parType = "Object"; _parIdType = "sd_oid"; _entityType = "DupPrereq";
        _attType = "object prerequisite"; _attTypes = "object prerequisites"; 
    }
    
    /****************************************************************
    * FETCH ALL pre-requisites linked to a specified DUP / Object
    ****************************************************************/
    
    [HttpGet("data-uses/{dupId:int}/objects/{sdOid}/prereqs")]
    [SwaggerOperation(Tags = new []{"DUP prereqs endpoint"})]
    
    public async Task<IActionResult> GetDupPrereqList(int dupId, string sdOid)
    {
        if (await _dupService.DupObjectExists(dupId, sdOid)) {
            var dupPrereqs = await _dupService.GetAllDupPrereqs(dupId, sdOid);
            return dupPrereqs != null    
                ? Ok(ListSuccessResponse(dupPrereqs.Count, dupPrereqs))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));    
    }
    
    /****************************************************************
    * FETCH ALL pre-requisite records, for a specified object / DUP,
    * with names of foreign key entities
    ****************************************************************/
    
    [HttpGet("data-uses/with-fk-names/{dupId:int}/objects/{sdOid}/prereqs")]
    [SwaggerOperation(Tags = new []{"DUP object pre-requisites endpoint"})]
    
    public async Task<IActionResult> GetDupPrereqListWfn(int dupId, string sdOid)
    {
        if (await _dupService.DupObjectExists(dupId, sdOid)) {
            var dupPrereqsWfn = await _dupService.GetAllOutDupPrereqs(dupId, sdOid);
            return dupPrereqsWfn != null    
                ? Ok(ListSuccessResponse(dupPrereqsWfn.Count, dupPrereqsWfn))
                : Ok(NoAttributesResponse(_attTypes));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));    
    }
    
    /****************************************************************
    * FETCH a specific pre-requisite met record, on a specified DUP / Object
    ****************************************************************/
    
    [HttpGet("data-uses/{dupId:int}/objects/{sdOid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"DUP prereqs endpoint"})]
    
    public async Task<IActionResult> GetDupPrereq(int dupId, string sdOid, int id)
    {
        if (await _dupService.DupObjectAttributeExists(dupId, sdOid, _entityType, id)) {
            var dupPrereq = await _dupService.GetDupPrereq(id);
            return dupPrereq != null
                ? Ok(SingleSuccessResponse(new List<DupPrereq>() { dupPrereq }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }

    /****************************************************************
    * FETCH a particular pre-requisite record, for a specified object,
    * with names of foreign key entities
    ****************************************************************/
    
    [HttpGet("data-uses/with-fk-names/{dupId:int}/objects/{sdOid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"DUP object pre-requisites endpoint"})]
    
    public async Task<IActionResult> GetDupPrereqWfn(int dupId, string sdOid, int id)
    {
        if (await _dupService.DupObjectAttributeExists (dupId, sdOid, _entityType, id)) {
            var dupPrereqWfn = await _dupService.GetOutDupPrereq(id);
            return dupPrereqWfn != null
                ? Ok(SingleSuccessResponse(new List<DupPrereqOut>() { dupPrereqWfn }))
                : Ok(ErrorResponse("r", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
    
    /****************************************************************
    * CREATE a pre-requisite record for a specified DUP / Object
    ****************************************************************/
    
    [HttpPost("data-uses/{dupId:int}/objects/{sdOid}/prereqs")]
    [SwaggerOperation(Tags = new []{"DUP prereqs endpoint"})]
    
    public async Task<IActionResult> CreateDupPrereq(int dupId, string sdOid, 
        [FromBody] DupPrereq dupPrereqContent)
    {
        if (await _dupService.DupObjectExists(dupId, sdOid)) {
            dupPrereqContent.DupId = dupId;   // ensure this is the case
            dupPrereqContent.SdOid = sdOid;
            var dupPrereq = await _dupService.CreateDupPrereq(dupPrereqContent);
            return dupPrereq != null
                ? Ok(SingleSuccessResponse(new List<DupPrereq>() { dupPrereq }))
                : Ok(ErrorResponse("c", _attType, _parType, dupId.ToString(), dupId.ToString()));
        }
        return Ok(NoParentResponse(_parType, _parIdType, sdOid));  
    }  
    
    /****************************************************************
    * UPDATE a pre-requisite met record, for a specified DUP / Object
    ****************************************************************/

    [HttpPut("data-uses/{dupId:int}/objects/{sdOid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"DUP prereqs endpoint"})]
    
    public async Task<IActionResult> UpdateDupPrereq(int dupId, string sdOid, int id, 
        [FromBody] DupPrereq dupPrereqContent)
    {
        if (await _dupService.DupObjectAttributeExists(dupId, sdOid, _entityType, id)) {
            dupPrereqContent.DupId = dupId;  // ensure this is the case
            dupPrereqContent.SdOid = sdOid;
            dupPrereqContent.Id = id;
            var updatedDupPrereq = await _dupService.UpdateDupPrereq(dupPrereqContent);
            return updatedDupPrereq != null
                ? Ok(SingleSuccessResponse(new List<DupPrereq>() { updatedDupPrereq }))
                : Ok(ErrorResponse("u", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
    
    /****************************************************************
    * DELETE a pre-requisite met record, for a specified DUP / Object
    ****************************************************************/
    
    [HttpDelete("data-uses/{dupId:int}/objects/{sdOid}/prereqs/{id:int}")]
    [SwaggerOperation(Tags = new []{"DUP prereqs endpoint"})]
    
    public async Task<IActionResult> DeleteDupPrereq(int dupId, string sdOid, int id)
    {
        if (await _dupService.DupObjectAttributeExists(dupId, sdOid, _entityType, id)) {
            var count = await _dupService.DeleteDupPrereq(id);
            return count > 0
                ? Ok(DeletionSuccessResponse(count, _attType, sdOid, id.ToString()))
                : Ok(ErrorResponse("d", _attType, _parType, sdOid, id.ToString()));
        }
        return Ok(NoParentAttResponse(_attType, _parType, sdOid, id.ToString()));
    }
}