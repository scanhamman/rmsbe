using rmsbe.SysModels;
using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IDupRepository
{
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/
    
    Task<bool> DupExists(int id);
    Task<bool> DupAttributeExists(int dupId, string typeName, int id);
    Task<bool> DupDuaExists(int dupId);
    Task<bool> DupObjectExists(int dupId, string sdOid);
    Task<bool> DupObjectAttributeExists(int dtpId, string sdOid, string typeName, int id);
    
    /****************************************************************
    * Fetch DUP / DUP entry data
    ****************************************************************/
    
    Task<IEnumerable<DupInDb>> GetAllDups();
    Task<IEnumerable<DupEntryInDb>> GetAllDupEntries();
    
    Task<IEnumerable<DupInDb>> GetPaginatedDupData(int pNum, int pSize);
    Task<IEnumerable<DupEntryInDb>> GetPaginatedDupEntries(int pNum, int pSize);
    
    Task<IEnumerable<DupInDb>> GetFilteredDupData(string titleFilter);
    Task<IEnumerable<DupEntryInDb>> GetFilteredDupEntries(string titleFilter);
    
    Task<IEnumerable<DupInDb>> GetPaginatedFilteredDupData(string titleFilter, int pNum, int pSize);
    Task<IEnumerable<DupEntryInDb>> GetPaginatedFilteredDupEntries(string titleFilter, int pNum, int pSize);
     
    Task<IEnumerable<DupInDb>> GetRecentDups(int n);   
    Task<IEnumerable<DupEntryInDb>> GetRecentDupEntries(int n);
    
    Task<IEnumerable<DupInDb>> GetDupsByOrg(int orgId);   
    Task<IEnumerable<DupEntryInDb>> GetDupEntriesByOrg(int orgId);
   
    
    /****************************************************************
    * Full DUP data (including attributes in other tables)
    ****************************************************************/
  
    Task<FullDupInDb?> GetFullDupById(int id);
    Task<int> DeleteFullDup(int id);
    
    /****************************************************************
    * Dup statistics
    ****************************************************************/

    Task<int> GetTotalDups();
    Task<int> GetTotalFilteredDups(string titleFilter);
    Task<int> GetCompletedDups();
    Task<IEnumerable<StatisticInDb>> GetDupsByStatus();

    /****************************************************************
    * DUP record data
    ****************************************************************/
    
    Task<DupInDb?> GetDup(int dupId); 
    Task<DupOutInDb?> GetOutDup(int dupId); 
    
    Task<DupInDb?> CreateDup(DupInDb dupContent);
    Task<DupInDb?> UpdateDup(DupInDb dupContent);
    Task<int> DeleteDup(int dupId); 
  
    /****************************************************************
    * DUP Studies
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DupStudyInDb>> GetAllDupStudies(int dupId);
    Task<IEnumerable<DupStudyOutInDb>> GetAllOutDupStudies(int dupId);
    
    Task<DupStudyInDb?> GetDupStudy(int id); 
    Task<DupStudyOutInDb?> GetOutDupStudy(int id); 
    
    // Update data
    Task<DupStudyInDb?> CreateDupStudy(DupStudyInDb dupStudyContent);
    Task<DupStudyInDb?> UpdateDupStudy(DupStudyInDb dupStudyContent);
    Task<int> DeleteDupStudy(int id); 
    
    /****************************************************************
    * DUP Objects
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DupObjectInDb>> GetAllDupObjects(int dupId);
    Task<IEnumerable<DupObjectOutInDb>> GetAllOutDupObjects(int dupId);
    
    Task<DupObjectInDb?> GetDupObject(int id); 
    Task<DupObjectOutInDb?> GetOutDupObject(int id); 
    
    // Update data
    Task<DupObjectInDb?> CreateDupObject(DupObjectInDb dupObjectContent);
    Task<DupObjectInDb?> UpdateDupObject(DupObjectInDb dupObjectContent);
    Task<int> DeleteDupObject(int id); 
    
    /****************************************************************
    * DUAs
    ****************************************************************/
    
    // Fetch data
    Task<DuaInDb?> GetDua(int dupId); 
    Task<DuaOutInDb?> GetOutDua(int dupId); 
    
    // Update data
    Task<DuaInDb?> CreateDua(DuaInDb dtaContent);
    Task<DuaInDb?> UpdateDua(DuaInDb dtaContent);
    Task<int> DeleteDua(int dupId); 
    
    /****************************************************************
    * DUP Access pre-requisites
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<DupPrereqInDb>> GetAllDupPrereqs(int dupId, string sdOid);
    Task<IEnumerable<DupPrereqOutInDb>> GetAllOutDupPrereqs(int dupId, string sdOid);
    
    Task<DupPrereqInDb?> GetDupPrereq(int id); 
    Task<DupPrereqOutInDb?> GetOutDupPrereq(int id); 
    
    // Update data
    Task<DupPrereqInDb?> CreateDupPrereq(DupPrereqInDb dupPrereqContent);
    Task<DupPrereqInDb?> UpdateDupPrereq(DupPrereqInDb dupPrereqContent);
    Task<int> DeleteDupPrereq(int ide);  

    /****************************************************************
     * DUP notes
     ****************************************************************/

    // Fetch data
    Task<IEnumerable<DupNoteInDb>> GetAllDupNotes(int dupId);
    Task<IEnumerable<DupNoteOutInDb>> GetAllOutDupNotes(int dupId);
    
    Task<DupNoteInDb?> GetDupNote(int id); 
    Task<DupNoteOutInDb?> GetOutDupNote(int id); 
    
    // Update data
    Task<DupNoteInDb?> CreateDupNote(DupNoteInDb dupNoteContent);
    Task<DupNoteInDb?> UpdateDupNote(DupNoteInDb dupNoteContent);
    Task<int> DeleteDupNote(int id); 

    /****************************************************************
    * DUP people
    ****************************************************************/

    // Fetch data 
    Task<IEnumerable<DupPersonInDb>> GetAllDupPeople(int dpId);
    Task<IEnumerable<DupPersonOutInDb>> GetAllOutDupPeople(int dtpId);
    
    Task<DupPersonInDb?> GetDupPerson(int id); 
    Task<DupPersonOutInDb?> GetOutDupPerson(int id); 
    
    // Update data
    Task<DupPersonInDb?> CreateDupPerson(DupPersonInDb dupPeopleContent);
    Task<DupPersonInDb?> UpdateDupPerson(DupPersonInDb dupPeopleContent);
    Task<int> DeleteDupPerson(int id);  
    
    
    /****************************************************************
    * Secondary use
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<DupSecondaryUseInDb>> GetAllSecUses(int dupId);
    Task<DupSecondaryUseInDb?> GetSecUse(int dupId); 
    
    // Update data
    Task<DupSecondaryUseInDb?> CreateSecUse(DupSecondaryUseInDb dtaContent);
    Task<DupSecondaryUseInDb?> UpdateSecUse(DupSecondaryUseInDb dtaContent);
    Task<int> DeleteSecUse(int id); 
    
}
