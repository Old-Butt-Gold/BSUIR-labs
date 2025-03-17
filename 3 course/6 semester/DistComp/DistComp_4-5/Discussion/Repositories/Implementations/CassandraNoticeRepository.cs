using Cassandra;
using Discussion.Models;
using Discussion.Repositories.Interfaces;
using ISession = Cassandra.ISession;

namespace Discussion.Repositories.Implementations;

public class CassandraNoticeRepository : INoticeRepository
{
    private readonly ISession _session;
    
    public CassandraNoticeRepository(ISession session)
    {
        _session = session;
    }
    
    private Notice MapNoteFromRow(Row row)
    {
        return new Notice
        {
            Id = row.GetValue<long>("id"),
            StoryId = row.GetValue<long>("story_id"),
            Content = row.GetValue<string>("content"),
        };
    }
    
    public async Task<IEnumerable<Notice>> GetAllAsync()
    {
        var query = "SELECT * FROM tbl_notice";
        var result = await _session.ExecuteAsync(new SimpleStatement(query));
        return result.Select(MapNoteFromRow).ToList();
    }

    public async Task<Notice?> GetByIdAsync(long id)
    {
        var query = "SELECT * FROM tbl_notice WHERE id = ?";
        var result = await _session.ExecuteAsync(new SimpleStatement(query, id));
        var row = result.FirstOrDefault();
        return row != null ? MapNoteFromRow(row) : null;
    }

    public async Task<Notice> CreateAsync(Notice entity)
    {
        var query = "INSERT INTO tbl_notice (id, story_id, content) VALUES (?, ?, ?)";
        var statement = await _session.PrepareAsync(query);
        var boundStatement = statement.Bind(entity.Id, entity.StoryId, entity.Content);
        var a = await _session.ExecuteAsync(boundStatement);
        foreach (var item in a.GetRows())
        {
            Console.WriteLine(item);
        }
        return entity;
    }

    public async Task<Notice?> UpdateAsync(Notice entity)
    {
        var existingNote = await GetByIdAsync(entity.Id);
        if (existingNote == null)
            return null;

        var query = "UPDATE tbl_notice SET story_id = ?, content = ? WHERE id = ?";
        var statement = await _session.PrepareAsync(query);
        var boundStatement = statement.Bind(entity.StoryId, entity.Content, entity.Id);
        await _session.ExecuteAsync(boundStatement);
        return entity;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var existingNote = await GetByIdAsync(id);
        if (existingNote == null)
            return false;

        var query = $"DELETE FROM tbl_notice WHERE id = ?";
        var statement = await _session.PrepareAsync(query);
        var boundStatement = statement.Bind(id);
        await _session.ExecuteAsync(boundStatement);
        return true;
    }
}