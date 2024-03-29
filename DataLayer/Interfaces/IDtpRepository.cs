using rmsbe.SysModels;
using rmsbe.DbModels;

namespace rmsbe.DataLayer.Interfaces;

public interface IDtpRepository
{
    /****************************************************************
    * Check functions - return a boolean that indicates if a record exists 
    ****************************************************************/
    
    Task<bool> DtpExists(int id);
    Task<bool> DtpAttributeExists(int dtpId, string typeName, int id);
    Task<bool> DtpDtaExists(int dtpId);
    Task<bool> DtpObjectExists(int dtpId, string sdOid);
    Task<bool> DtpObjectDatasetExists(int dtpId, string sdOid);
    Task<bool> DtpObjectAttributeExists(int dtpId, string sdOid, string typeName, int id);
     
    /****************************************************************
    * Fetch DTP / DTP entry data
    ****************************************************************/

    Task<IEnumerable<DtpInDb>> GetAllDtps();
    Task<IEnumerable<DtpEntryInDb>> GetAllDtpEntries();
    
    Task<IEnumerable<DtpInDb>> GetPaginatedDtpData(int pNum, int pSize);
    Task<IEnumerable<DtpEntryInDb>> GetPaginatedDtpEntries(int pNum, int pSize);
    
    Task<IEnumerable<DtpInDb>> GetFilteredDtpData(string titleFilter);
    Task<IEnumerable<DtpEntryInDb>> GetFilteredDtpEntries(string titleFilter);
    
    Task<IEnumerable<DtpInDb>> GetPaginatedFilteredDtpData(string titleFilter, int pNum, int pSize);
    Task<IEnumerable<DtpEntryInDb>> GetPaginatedFilteredDtpEntries(string titleFilter, int pNum, int pSize);
    
    Task<IEnumerable<DtpInDb>> GetRecentDtps(int n);  
    Task<IEnumerable<DtpEntryInDb>> GetRecentDtpEntries(int n);
    
    Task<IEnumerable<DtpInDb>> GetDtpsByOrg(int orgId);  
    Task<IEnumerable<DtpEntryInDb>> GetDtpEntriesByOrg(int orgId);
    
    /****************************************************************
    * Full DTP data (including attributes in other tables)
    ****************************************************************/
  
    Task<FullDtpInDb?> GetFullDtpById(int id);
    Task<int> DeleteFullDtp(int id);
    
    /****************************************************************
    * Dtp statistics
    ****************************************************************/

    Task<int> GetTotalDtps();
    Task<int> GetTotalFilteredDtps(string titleFilter);
    Task<int> GetCompletedDtps();
    Task<IEnumerable<StatisticInDb>> GetDtpsByStatus();

    /****************************************************************
    * DTP record data
    ****************************************************************/
    
    Task<DtpInDb?> GetDtp(int dtpId); 
    Task<DtpOutInDb?> GetOutDtp(int dtpId); 
    
    Task<DtpInDb?> CreateDtp(DtpInDb dtpContent);
    Task<DtpInDb?> UpdateDtp(DtpInDb dtpContent);
    Task<int> DeleteDtp(int dtpId); 
    
    /****************************************************************
    * DTP Studies
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DtpStudyInDb>> GetAllDtpStudies(int dtpId);
    Task<IEnumerable<DtpStudyOutInDb>> GetAllOutDtpStudies(int dtpId);
    
    Task<DtpStudyInDb?> GetDtpStudy(int id); 
    Task<DtpStudyOutInDb?> GetOutDtpStudy(int id); 
    
    // Update data
    Task<DtpStudyInDb?> CreateDtpStudy(DtpStudyInDb dtpStudyContent);
    Task<DtpStudyInDb?> UpdateDtpStudy(DtpStudyInDb dtpStudyContent);
    Task<int> DeleteDtpStudy(int id); 
    
    /****************************************************************
    * DTP Objects
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DtpObjectInDb>> GetAllDtpObjects(int dtpId);
    Task<IEnumerable<DtpObjectOutInDb>> GetAllOutDtpObjects(int dtpId);
    
    Task<DtpObjectInDb?> GetDtpObject(int id); 
    Task<DtpObjectOutInDb?> GetOutDtpObject(int id); 
    
    // Update data
    Task<DtpObjectInDb?> CreateDtpObject(DtpObjectInDb dtpObjectContent);
    Task<DtpObjectInDb?> UpdateDtpObject(DtpObjectInDb dtpObjectContent);
    Task<int> DeleteDtpObject(int id); 
    
    /****************************************************************
    * DTAs
    ****************************************************************/
    
    // Fetch data
    Task<DtaInDb?> GetDta(int dtpId); 
    Task<DtaOutInDb?> GetOutDta(int dtpId); 
    
    // Update data
    Task<DtaInDb?> CreateDta(DtaInDb dtaContent);
    Task<DtaInDb?> UpdateDta(DtaInDb dtaContent);
    Task<int> DeleteDta(int dtpId); 
    
    /****************************************************************
    * DTP datasets
    ****************************************************************/

    // Fetch data
    Task<DtpDatasetInDb?> GetDtpDataset(int dtpId, string sdOid); 
    Task<DtpDatasetOutInDb?> GetOutDtpDataset(int dtpId, string sdOid); 
    
    // Update data
    Task<DtpDatasetInDb?> CreateDtpDataset(DtpDatasetInDb dtpDatasetContent);
    Task<DtpDatasetInDb?> UpdateDtpDataset(DtpDatasetInDb dtpDatasetContent);
    Task<int> DeleteDtpDataset(int dtpId, string sdOid);  

    /****************************************************************
    * DTP Access pre-requisites
    ****************************************************************/
    
    // Fetch data
    Task<IEnumerable<DtpPrereqInDb>> GetAllDtpPrereqs(int dtpId, string sdOid);
    Task<IEnumerable<DtpPrereqOutInDb>> GetAllOutDtpPrereqs(int dtpId, string sdOid);
    
    Task<DtpPrereqInDb?> GetDtpPrereq(int id); 
    Task<DtpPrereqOutInDb?> GetOutDtpPrereq(int id); 
    
    // Update data
    Task<DtpPrereqInDb?> CreateDtpPrereq(DtpPrereqInDb dtpPrereqContent);
    Task<DtpPrereqInDb?> UpdateDtpPrereq(DtpPrereqInDb dtpPrereqContent);
    Task<int> DeleteDtpPrereq(int ide);  
    
   /****************************************************************
    * DTP notes
    ****************************************************************/

    // Fetch data
    Task<IEnumerable<DtpNoteInDb>> GetAllDtpNotes(int dtpId);
    Task<IEnumerable<DtpNoteOutInDb>> GetAllOutDtpNotes(int dtpId);
    
    Task<DtpNoteInDb?> GetDtpNote(int id); 
    Task<DtpNoteOutInDb?> GetOutDtpNote(int id); 
    
    // Update data
    Task<DtpNoteInDb?> CreateDtpNote(DtpNoteInDb dtpNoteContent);
    Task<DtpNoteInDb?> UpdateDtpNote(DtpNoteInDb dtpNoteContent);
    Task<int> DeleteDtpNote(int id); 

    /****************************************************************
    * DTP people
    ****************************************************************/
    
    // Fetch data 
    Task<IEnumerable<DtpPersonInDb>> GetAllDtpPeople(int dtpId);
    Task<IEnumerable<DtpPersonOutInDb>> GetAllOutDtpPeople(int dtpId);
    
    Task<DtpPersonInDb?> GetDtpPerson(int id); 
    Task<DtpPersonOutInDb?> GetOutDtpPerson(int id); 
    
    // Update data
    Task<DtpPersonInDb?> CreateDtpPerson(DtpPersonInDb dtpPeopleContent);
    Task<DtpPersonInDb?> UpdateDtpPerson(DtpPersonInDb dtpPeopleContent);
    Task<int> DeleteDtpPerson(int id);  
    
}
