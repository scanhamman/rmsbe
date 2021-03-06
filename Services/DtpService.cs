using rmsbe.SysModels;
using rmsbe.DbModels;
using rmsbe.Services.Interfaces;
using rmsbe.DataLayer.Interfaces;

namespace rmsbe.Services;

public class DtpService : IDtpService
{
    private readonly IDtpRepository _dtpRepository;
    private readonly ILookupService _lupService;
    private List<Lup> _lookups;

    public DtpService(IDtpRepository dtpRepository, ILookupService lupService)
    {
        _dtpRepository = dtpRepository ?? throw new ArgumentNullException(nameof(dtpRepository));
        _lupService = lupService ?? throw new ArgumentNullException(nameof(lupService));
        _lookups = new List<Lup>();
    }
    
    /****************************************************************
    * Check functions 
    ****************************************************************/
    
    // Check if DTP exists
    public async Task<bool> DtpExists (int id) 
           => await _dtpRepository.DtpExists(id);
    
    // Check if attribute exists on this DTP
    public async Task<bool> DtpAttributeExists(int dtpId, string typeName, int id)
           => await _dtpRepository.DtpAttributeExists(dtpId, typeName, id);

    // Check if DTP / object combination exists
    public async Task<bool> DtpObjectExists(int dtpId, string sdOid)
           => await _dtpRepository.DtpObjectExists(dtpId, sdOid);
   
    // Check if pre-req exists on this DTP / object
    public async Task<bool> DtpObjectAttributeExists (int dtpId, string sdOid, string typeName, int id) 
           => await _dtpRepository.DtpObjectAttributeExists(dtpId, sdOid, typeName, id); 
    
    
    /****************************************************************
    * All DTPs / DTP entries
    ****************************************************************/
    
    public async Task<List<Dtp>?> GetAllDtps() {
        var dtpsInDb = (await _dtpRepository.GetAllDtps()).ToList();
        return (!dtpsInDb.Any()) ? null 
            : dtpsInDb.Select(r => new Dtp(r)).ToList();
    }
    
    public async Task<List<DtpEntry>?> GetAllDtpEntries(){ 
        var dtpEntriesInDb = (await _dtpRepository.GetAllDtpEntries()).ToList();
        return !dtpEntriesInDb.Any() ? null 
            : dtpEntriesInDb.Select(r => new DtpEntry(r)).ToList();
    }
    
    /****************************************************************
    * Paginated DTPs / DTP entries
    ****************************************************************/
    
    public async Task<List<Dtp>?> GetPaginatedDtpData(PaginationRequest validFilter)
    {
        var pagedDtpsInDb = (await _dtpRepository
            .GetPaginatedDtpData(validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedDtpsInDb.Any() ? null 
            : pagedDtpsInDb.Select(r => new Dtp(r)).ToList();
    }
    
    public async Task<List<DtpEntry>?> GetPaginatedDtpEntries(PaginationRequest validFilter)
    {
        var pagedDtpEntriesInDb = (await _dtpRepository
            .GetPaginatedDtpEntries(validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedDtpEntriesInDb.Any() ? null 
            : pagedDtpEntriesInDb.Select(r => new DtpEntry(r)).ToList();
    }

    /****************************************************************
    * Filtered DTPs / DTP entries
    ****************************************************************/    
    
    public async Task<List<Dtp>?> GetFilteredDtpRecords(string titleFilter)
    {
        var filteredDtpsInDb = (await _dtpRepository
            .GetFilteredDtpData(titleFilter)).ToList();
        return !filteredDtpsInDb.Any() ? null 
            : filteredDtpsInDb.Select(r => new Dtp(r)).ToList();
    }
    
    public async Task<List<DtpEntry>?> GetFilteredDtpEntries(string titleFilter)
    {
        var filteredDtpEntriesInDb = (await _dtpRepository
            .GetFilteredDtpEntries(titleFilter)).ToList();
        return !filteredDtpEntriesInDb.Any() ? null 
            : filteredDtpEntriesInDb.Select(r => new DtpEntry(r)).ToList();
    }
    
    /****************************************************************
    * Paginated and filtered DTPs / DTP entries
    ****************************************************************/

    public async Task<List<Dtp>?> GetPaginatedFilteredDtpRecords(string titleFilter,
        PaginationRequest validFilter)
    {
        var pagedFilteredDtpsInDb = (await _dtpRepository
            .GetPaginatedFilteredDtpData(titleFilter, validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedFilteredDtpsInDb.Any() ? null 
            : pagedFilteredDtpsInDb.Select(r => new Dtp(r)).ToList();
    }
    
    public async Task<List<DtpEntry>?> GetPaginatedFilteredDtpEntries(string titleFilter,
        PaginationRequest validFilter)
    {
        var pagedFilteredDtpEntriesInDb = (await _dtpRepository
            .GetPaginatedFilteredDtpEntries(titleFilter, validFilter.PageNum, validFilter.PageSize)).ToList();
        return !pagedFilteredDtpEntriesInDb.Any() ? null 
            : pagedFilteredDtpEntriesInDb.Select(r => new DtpEntry(r)).ToList();
    }
    
    /****************************************************************
    * Recent DTPs / DTP entries
    ****************************************************************/
    
    public async Task<List<Dtp>?> GetRecentDtps(int n) {
        var recentDtpsInDb = (await _dtpRepository.GetRecentDtps(n)).ToList();
        return (!recentDtpsInDb.Any()) ? null 
            : recentDtpsInDb.Select(r => new Dtp(r)).ToList();
    }
   
    public async Task<List<DtpEntry>?> GetRecentDtpEntries(int n){ 
        var recentDtpEntriesInDb = (await _dtpRepository.GetRecentDtpEntries(n)).ToList();
        return !recentDtpEntriesInDb.Any() ? null 
            : recentDtpEntriesInDb.Select(r => new DtpEntry(r)).ToList();
    }
    
    /****************************************************************
    * DTPs / DTP entries by Organisation
    ****************************************************************/
    
    public async Task<List<Dtp>?> GetDtpsByOrg(int orgId) {
        var dtpsByOrgInDb = (await _dtpRepository.GetDtpsByOrg(orgId)).ToList();
        return (!dtpsByOrgInDb.Any()) ? null 
            : dtpsByOrgInDb.Select(r => new Dtp(r)).ToList();
    }
   
    public async Task<List<DtpEntry>?> GetDtpEntriesByOrg(int orgId){ 
        var dtpEntriesByOrgInDb = (await _dtpRepository.GetDtpEntriesByOrg(orgId)).ToList();
        return !dtpEntriesByOrgInDb.Any() ? null 
            : dtpEntriesByOrgInDb.Select(r => new DtpEntry(r)).ToList();
    }
    
    /****************************************************************
    * Get single DTP record
    ****************************************************************/   
    
    public async Task<Dtp?> GetDtp(int dtpId) {
        var dtpInDb = await _dtpRepository.GetDtp(dtpId);
        return dtpInDb == null ? null : new Dtp(dtpInDb);
    }
 
    /****************************************************************
    * Update DTP records
    ****************************************************************/   
    
    public async Task<Dtp?> CreateDtp(Dtp dtpContent) {
        var dtpInDb = new DtpInDb(dtpContent);
        var res = await _dtpRepository.CreateDtp(dtpInDb);
        return res == null ? null : new Dtp(res);
    }

    public async Task<Dtp?> UpdateDtp(int aId, Dtp dtpContent) {
        var dtpInDb = new DtpInDb(dtpContent) { id = aId };
        var res = await _dtpRepository.UpdateDtp(dtpInDb);
        return res == null ? null : new Dtp(res);
    }

    public async Task<int> DeleteDtp(int dtpId)
           => await _dtpRepository.DeleteDtp(dtpId);
    
    
    /****************************************************************
    * Full DTP data (including attributes in other tables)
    ****************************************************************/
    
    // Fetch data
    public async Task<FullDtp?> GetFullDtpById(int id){ 
        FullDtpInDb? fullDtpInDb = await _dtpRepository.GetFullDtpById(id);
        return fullDtpInDb == null ? null : new FullDtp(fullDtpInDb);
    }
    
    // Delete data
    public async Task<int> DeleteFullDtp(int id) 
        => await _dtpRepository.DeleteFullDtp(id);

    /****************************************************************
    * Statistics
    ****************************************************************/

    public async Task<Statistic> GetTotalDtps()
    {
        int res = await _dtpRepository.GetTotalDtps();
        return new Statistic("Total", res);
    }
    
    public async Task<Statistic> GetTotalFilteredDtps(string titleFilter)
    {
        int res = await _dtpRepository.GetTotalFilteredDtps(titleFilter);
        return new Statistic("TotalFiltered", res);
    }
    
    public async Task<List<Statistic>?> GetDtpsByStatus()
    {
        var res = (await _dtpRepository.GetDtpsByStatus()).ToList();
        if (await ResetLookups("dtp-status-types"))
        {
            return !res.Any()
                ? null
                : res.Select(r => new Statistic(LuTypeName(r.stat_type), r.stat_value)).ToList();
        }
        return null;
    }
    
    public async Task<List<Statistic>> GetDtpsByCompletion()
    {
        int total = await _dtpRepository.GetTotalDtps();
        int completed = await _dtpRepository.GetCompletedDtps();
        return new List<Statistic>()
        {
            new Statistic("Total", total),
            new Statistic("Incomplete", total - completed)
        };
    }
    
    private string LuTypeName(int n)
    {
        foreach (var p in _lookups.Where(p => n == p.Id))
        {
            return p.Name ?? "null name in matching lookup!";
        }
        return "not known";
    }

    private async Task<bool> ResetLookups(string typeName)
    {
        _lookups = new List<Lup>();  // reset to empty list
        _lookups = await _lupService.GetLookUpValues(typeName);
        return _lookups.Count > 0 ;
    }

    
    /****************************************************************
    * DTP Studies
    ****************************************************************/

    // Fetch data
    public async Task<List<DtpStudy>?> GetAllDtpStudies(int dtpId) {
        var dtpStudiesInDb = (await _dtpRepository.GetAllDtpStudies(dtpId)).ToList();
        return (!dtpStudiesInDb.Any()) ? null 
            : dtpStudiesInDb.Select(r => new DtpStudy(r)).ToList();
    }

    public async Task<DtpStudy?> GetDtpStudy(int id) {
        var dtpStudyInDb = await _dtpRepository.GetDtpStudy(id);
        return dtpStudyInDb == null ? null : new DtpStudy(dtpStudyInDb);
    }
 
    // Update data
    public async Task<DtpStudy?> CreateDtpStudy(DtpStudy dtpStudyContent) {
        var dtpStudyInDb = new DtpStudyInDb(dtpStudyContent);
        var res = await _dtpRepository.CreateDtpStudy(dtpStudyInDb);
        return res == null ? null : new DtpStudy(res);
    }

    public async Task<DtpStudy?> UpdateDtpStudy(int aId, DtpStudy dtpStudyContent) {
        var dtpStudyContentInDb = new DtpStudyInDb(dtpStudyContent) { id = aId };
        var res = await _dtpRepository.UpdateDtpStudy(dtpStudyContentInDb);
        return res == null ? null : new DtpStudy(res);
    }

    public async Task<int> DeleteDtpStudy(int id)
           => await _dtpRepository.DeleteDtpStudy(id);

    /****************************************************************
    * DTP Objects
    ****************************************************************/

    // Fetch data
    public async Task<List<DtpObject>?> GetAllDtpObjects(int dtpId) {
        var dtpObjectsInDb = (await _dtpRepository.GetAllDtpObjects(dtpId)).ToList();
        return (!dtpObjectsInDb.Any()) ? null 
            : dtpObjectsInDb.Select(r => new DtpObject(r)).ToList();
    }

    public async Task<DtpObject?> GetDtpObject(int id) {
        var dtpObjectInDb = await _dtpRepository.GetDtpObject(id);
        return dtpObjectInDb == null ? null : new DtpObject(dtpObjectInDb);
    }
 
    // Update data
    public async Task<DtpObject?> CreateDtpObject(DtpObject dtpObjectContent) {
        var dtpObjectInDb = new DtpObjectInDb(dtpObjectContent);
        var res = await _dtpRepository.CreateDtpObject(dtpObjectInDb);
        return res == null ? null : new DtpObject(res);
    }

    public async Task<DtpObject?> UpdateDtpObject(int aId,DtpObject dtpObjectContent)
    {
        var dtpObjectContentInDb = new DtpObjectInDb(dtpObjectContent) { id = aId };
        var res = await _dtpRepository.UpdateDtpObject(dtpObjectContentInDb);
        return res == null ? null : new DtpObject(res);
    }

    public async Task<int> DeleteDtpObject(int id)
           => await _dtpRepository.DeleteDtpObject(id);
    
    /****************************************************************
    * DTAs
    ****************************************************************/
    
    // Fetch data
    public async Task<List<Dta>?> GetAllDtas(int dtpId) {
        var dtasInDb = (await _dtpRepository.GetAllDtas(dtpId)).ToList();
        return (!dtasInDb.Any()) ? null 
            : dtasInDb.Select(r => new Dta(r)).ToList();
    }

    public async Task<Dta?> GetDta(int id) {
        var dtaInDb = await _dtpRepository.GetDta(id);
        return dtaInDb == null ? null : new Dta(dtaInDb);
    }
 
    // Update data
    public async Task<Dta?> CreateDta(Dta dtaContent) {
        var dtaInDb = new DtaInDb(dtaContent);
        var res = await _dtpRepository.CreateDta(dtaInDb);
        return res == null ? null : new Dta(res);
    }

    public async Task<Dta?> UpdateDta(int aId,Dta dtaContent) {
        var dtaInDb = new DtaInDb(dtaContent) { id = aId };
        var res = await _dtpRepository.UpdateDta(dtaInDb);
        return res == null ? null : new Dta(res);
    }

    public async Task<int> DeleteDta(int id)
           => await _dtpRepository.DeleteDta(id);
    
    /***********************************************************
    * DTP datasets
    ****************************************************************/
    
    // Fetch data

    public async Task<DtpDataset?> GetDtpDataset(int id) {
        var dtpDatasetInDb = await _dtpRepository.GetDtpDataset(id);
        return dtpDatasetInDb == null ? null : new DtpDataset(dtpDatasetInDb);
    }
 
    // Update data
    public async Task<DtpDataset?> CreateDtpDataset(DtpDataset dtpDatasetContent) {
        var dtpDatasetInDb = new DtpDatasetInDb(dtpDatasetContent);
        var res = await _dtpRepository.CreateDtpDataset(dtpDatasetInDb);
        return res == null ? null : new DtpDataset(res);
    }

    public async Task<DtpDataset?> UpdateDtpDataset(int aId, DtpDataset dtpDatasetContent) {
        var dtpDatasetContentInDb = new DtpDatasetInDb(dtpDatasetContent) { id = aId };
        var res = await _dtpRepository.UpdateDtpDataset(dtpDatasetContentInDb);
        return res == null ? null : new DtpDataset(res);
    }

    public async Task<int> DeleteDtpDataset(int id)
           => await _dtpRepository.DeleteDtpDataset(id);
    
    /****************************************************************
    * DTP pre-requisites met
    ****************************************************************/
    
    // Fetch data
    public async Task<List<DtpPrereq>?> GetAllDtpPrereqs(int dtpId, string sdOid) {
        var dtpPrereqsInDb = (await _dtpRepository.GetAllDtpPrereqs(dtpId, sdOid)).ToList();
        return (!dtpPrereqsInDb.Any()) ? null 
            : dtpPrereqsInDb.Select(r => new DtpPrereq(r)).ToList();
    }

    public async Task<DtpPrereq?> GetDtpPrereq(int id) {
        var dtpPrereqInDb = await _dtpRepository.GetDtpPrereq(id);
        return dtpPrereqInDb == null ? null : new DtpPrereq(dtpPrereqInDb);
    }
 
    // Update data
    public async Task<DtpPrereq?> CreateDtpPrereq(DtpPrereq dtpPrereqContent) {
        var dtpPrereqInDb = new DtpPrereqInDb(dtpPrereqContent);
        var res = await _dtpRepository.CreateDtpPrereq(dtpPrereqInDb);
        return res == null ? null : new DtpPrereq(res);
    }

    public async Task<DtpPrereq?> UpdateDtpPrereq(int aId, DtpPrereq dtpPrereqContent) {
        var dtpPrereqInDb = new DtpPrereqInDb(dtpPrereqContent) { id = aId };
        var res = await _dtpRepository.UpdateDtpPrereq(dtpPrereqInDb);
        return res == null ? null : new DtpPrereq(res);
    }

    public async Task<int> DeleteDtpPrereq(int id)
        => await _dtpRepository.DeleteDtpPrereq(id);


    /****************************************************************
    * DTP notes
    ****************************************************************/

    // Fetch data
    public async Task<List<DtpNote>?> GetAllDtpNotes(int dtpId) {
        var dtpNotesInDb = (await _dtpRepository.GetAllDtpNotes(dtpId)).ToList();
        return (!dtpNotesInDb.Any()) ? null 
            : dtpNotesInDb.Select(r => new DtpNote(r)).ToList();
    }

    public async Task<DtpNote?> GetDtpNote(int id) {
        var dtpNoteInDb = await _dtpRepository.GetDtpNote(id);
        return dtpNoteInDb == null ? null : new DtpNote(dtpNoteInDb);
    }
 
    // Update data
    public async Task<DtpNote?> CreateDtpNote(DtpNote dtpNoteContent) {
        var dtpNoteInDb = new DtpNoteInDb(dtpNoteContent);
        var res = await _dtpRepository.CreateDtpNote(dtpNoteInDb);
        return res == null ? null : new DtpNote(res);
    }

    public async Task<DtpNote?> UpdateDtpNote(int aId, DtpNote dtpNoteContent) {
        var dtpNoteContentInDb = new DtpNoteInDb(dtpNoteContent) { id = aId };
        var res = await _dtpRepository.UpdateDtpNote(dtpNoteContentInDb);
        return res == null ? null : new DtpNote(res);
    }

    public async Task<int> DeleteDtpNote(int id)
        => await _dtpRepository.DeleteDtpNote(id);


    /****************************************************************
    * DTP people
    ****************************************************************/
    
    // Fetch data 
    public async Task<List<DtpPerson>?> GetAllDtpPeople(int dtpId) {
        var dtpPeopleInDb = (await _dtpRepository.GetAllDtpPeople(dtpId)).ToList();
        return (!dtpPeopleInDb.Any()) ? null 
            : dtpPeopleInDb.Select(r => new DtpPerson(r)).ToList();
    }

    public async Task<DtpPerson?> GetDtpPerson(int id) {
        var dtpPersonInDb = await _dtpRepository.GetDtpPerson(id);
        return dtpPersonInDb == null ? null : new DtpPerson(dtpPersonInDb);
    }
 
    // Update data
    public async Task<DtpPerson?> CreateDtpPerson(DtpPerson dtpPersonContent) {
        var dtpPersonInDb = new DtpPersonInDb(dtpPersonContent);
        var res = await _dtpRepository.CreateDtpPerson(dtpPersonInDb);
        return res == null ? null : new DtpPerson(res);
    }

    public async Task<DtpPerson?> UpdateDtpPerson(int aId, DtpPerson dtpPersonContent) {
        var dtpPersonInDb = new DtpPersonInDb(dtpPersonContent) { id = aId };
        var res = await _dtpRepository.UpdateDtpPerson(dtpPersonInDb);
        return res == null ? null : new DtpPerson(res);
    }

    public async Task<int> DeleteDtpPerson(int id)
        => await _dtpRepository.DeleteDtpPerson(id);
    

}