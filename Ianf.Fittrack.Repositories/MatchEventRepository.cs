using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Ianf.Fittrack.Services.Domain;
using Ianf.Fittrack.Services.Interfaces;
using Ianf.Fittrack.Services.Errors;
using System;
using LanguageExt;

namespace Ianf.Fittrack.Repositories
{
    public class MatchEventRepository : IMatchEventRepository
    {
        protected FittrackDbContext _dbContext { get; }

        public MatchEventRepository(FittrackDbContext context) => _dbContext = context;

        public async Task<Either<IEnumerable<Error>, List<MatchEvent>>> GetAllMatchEventsByUserIdAsync(UserId userId, MatchId matchId) =>
            await _dbContext.MatchEvents
                .Where(e => e.UserId == userId.Value)
                .Where(e => e.MatchId == matchId.Value)
                .Select(s => s.ToDomain())
                .ToListAsync();

        public Task<LanguageExt.Either<IEnumerable<Error>, int>> DeleteMatchEventAsync(MatchEvent matchEvent)
        {
            throw new System.NotImplementedException();
        }

        public async Task<LanguageExt.Either<IEnumerable<Error>, int>> AddNewMatchEventAsync(MatchEvent matchEvent)
        {
            try
            {
                var entity = matchEvent.ToEntity();
                _dbContext.MatchEvents.Add(entity);
                await _dbContext.SaveChangesAsync();
                return entity.Id;
            } 
            catch (Exception ex) 
            {
                return new List<Error> { new SqlError(ex.Message) };
            }
        }
    }
}