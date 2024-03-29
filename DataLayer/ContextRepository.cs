using rmsbe.DbModels;
using rmsbe.DataLayer.Interfaces;
using rmsbe.Helpers.Interfaces;
using Npgsql;
using Dapper.Contrib;
using Dapper;

namespace rmsbe.DataLayer;

public class ContextRepository : IContextRepository
{
    private readonly string _dbCtxConnString;
    private readonly string _dbRmsConnString;

    public ContextRepository(ICreds creds)
    {
        _dbCtxConnString = creds.GetConnectionString("context");
        _dbRmsConnString = creds.GetConnectionString("rms");
    }
    
    /****************************************************************
    * Check functions for organisations
    ****************************************************************/
    
    public async Task<bool> OrgExists(int id)
    {
        string sqlString = $@"select exists (select 1 from lup.organisations
                              where id = {id.ToString()})";
        await using var conn = new NpgsqlConnection(_dbRmsConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }
    
    /****************************************************************
    * FETCH org records - simply from organisations table
    ****************************************************************/
    
    // All organisations (id, name from relevant org names)
    public async Task<IEnumerable<OrgTableDataInDb>> GetOrgsTableData()
    {
        var sqlString = $@"Select id, default_name from lup.organisations
                           order by default_name";
        await using var conn = new NpgsqlConnection(_dbRmsConnString);
        return await conn.QueryAsync<OrgTableDataInDb>(sqlString);
    }
    
    // All organisations containing string (id, name)
    public async Task<IEnumerable<OrgTableDataInDb>> GetFilteredOrgsTableData(string filter)
    {
        var sqlString = $@"Select id, default_name from lup.organisations
                           where default_name ilike '%{filter}%'
                           order by default_name";
        await using var conn = new NpgsqlConnection(_dbRmsConnString);
        return await conn.QueryAsync<OrgTableDataInDb>(sqlString);
    }
    
    /****************************************************************
    * FETCH org records - from orgs_to_search table
    ****************************************************************/
    
    // All organisations (id, name from relevant org names)
    public async Task<IEnumerable<OrgSimpleInDb>> GetOrgs()
    {
        var sqlString = $@"Select id, name from lup.orgs_to_search
                           order by name";
        await using var conn = new NpgsqlConnection(_dbRmsConnString);
        return await conn.QueryAsync<OrgSimpleInDb>(sqlString);
    }
    
    // All organisations containing string (id, name)
    public async Task<IEnumerable<OrgSimpleInDb>> GetFilteredOrgs(string filter)
    {
        var sqlString = $@"Select id, name from lup.orgs_to_search
                           where name ilike '%{filter}%'
                           order by name";
        await using var conn = new NpgsqlConnection(_dbRmsConnString);
        return await conn.QueryAsync<OrgSimpleInDb>(sqlString);
    }
    
    /****************************************************************
    * FETCH org records - from orgs_to_search table, and with
    * non-default names indicating the default and org id
    ****************************************************************/
    
    // All organisation names (id, name, org_id, default_name)
    public async Task<IEnumerable<OrgWithNamesInDb>> GetOrgNames()
    {
        var sqlString = $@"Select s.id, s.org_id,
                           case when s.qualifier_id <> 1 then name ||' (--> '|| g.default_name||')'  
                           else name end as name,
                           g.default_name
                           from lup.orgs_to_search s
                           inner join lup.organisations g
                           on s.org_id = g.id
                           order by s.name";
        await using var conn = new NpgsqlConnection(_dbRmsConnString);
        return await conn.QueryAsync<OrgWithNamesInDb>(sqlString);
    }
    
    // All organisation names containing string (id, name, org id, default name)
    public async Task<IEnumerable<OrgWithNamesInDb>> GetFilteredOrgNames(string filter)
    {
        var sqlString = $@"Select s.id, s.org_id,
                           case when s.qualifier_id <> 1 then name ||' (--> '|| g.default_name||')'  
                           else name end as name,
                           g.default_name
                           from lup.orgs_to_search s
                           inner join lup.organisations g
                           on s.org_id = g.id
                           where s.name ilike '%{filter}%'
                           order by s.name";
        await using var conn = new NpgsqlConnection(_dbRmsConnString);
        return await conn.QueryAsync<OrgWithNamesInDb>(sqlString);
    }
    
    
    /****************************************************************
    * Check functions for languages
    ****************************************************************/

    public async Task<bool> LangCodeExists(string code)
    {
        string sqlString = $@"select exists (select 1 from lup.language_codes 
                              where code = '{code}')";
        await using var conn = new NpgsqlConnection(_dbCtxConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    public async Task<bool> LangNameExists(string name, string nameLang)
    {
        string fieldName = GetFieldname(nameLang);
        string sqlString = $@"select exists (select 1 from lup.language_codes 
                              where {fieldName} = '{name}')";
        await using var conn = new NpgsqlConnection(_dbCtxConnString);
        return await conn.ExecuteScalarAsync<bool>(sqlString);
    }

    
    /****************************************************************
    * FETCH lang codes, names (by language, en, de, fr) en=default
    ****************************************************************/

    public async Task<IEnumerable<LangCodeInDb>> GetLangCodes(string nameLang)
    {
        string fieldName = GetFieldname(nameLang);
        var sqlString = $@"Select code, {fieldName} as name from lup.language_codes
                           order by {fieldName} ";
        await using var conn = new NpgsqlConnection(_dbCtxConnString);
        return await conn.QueryAsync<LangCodeInDb>(sqlString);
    }

    public async Task<IEnumerable<LangCodeInDb>> GetMajorLangCodes(string nameLang)
    {
        string fieldName = GetFieldname(nameLang);
        var sqlString = $@"Select code, {fieldName} as name from lup.language_codes
                           where is_major = true
                           order by {fieldName}";
        await using var conn = new NpgsqlConnection(_dbCtxConnString);
        return await conn.QueryAsync<LangCodeInDb>(sqlString);
    }

    private string GetFieldname(string nameLang)
    {
        var langNameField = "lang_name_en";  // default
        if (nameLang.ToLower().StartsWith("fr"))
        {
            langNameField = "lang_name_fr";
        }
        else if (nameLang.ToLower().StartsWith("de"))
        {
            langNameField = "lang_name_de";
        }
        return langNameField;
    }

  
    /****************************************************************
    * FETCH lang details 
    ****************************************************************/

    public async Task<LangDetailsInDb?> GetLangDetailsFromCode(string code)
    {
        var sqlString = $"select * from lup.language_codes where code = '{code}'";
        await using var conn = new NpgsqlConnection(_dbCtxConnString);
        return await conn.QueryFirstOrDefaultAsync<LangDetailsInDb>(sqlString);
    }

    public async Task<LangDetailsInDb?> GetLangDetailsFromName(string name, string nameLang)
    {
        var fieldName = GetFieldname(nameLang);
        var sqlString = $"select * from lup.language_codes where {fieldName} = '{name}'";
        await using var conn = new NpgsqlConnection(_dbCtxConnString);
        return await conn.QueryFirstOrDefaultAsync<LangDetailsInDb>(sqlString);
    }


}
