using System.Collections.Generic;
using System.Threading.Tasks;
using Ianf.Fittrack.Services.Domain;
using Ianf.Fittrack.Services.Errors;
using LanguageExt;

namespace Ianf.Fittrack.Services.Interfaces
{
    public interface IMatchEventRepository
    {
        Task<Either<IEnumerable<Error>, int>> AddNewMatchEventAsync(MatchEvent matchEvent);
        Task<Either<IEnumerable<Error>, List<MatchEvent>>> GetAllMatchEventsByUserIdAsync(UserId userId, MatchId matchId);
        Task<Either<IEnumerable<Error>, int>> DeleteMatchEventAsync(MatchEvent matchEvent);
    }
}
