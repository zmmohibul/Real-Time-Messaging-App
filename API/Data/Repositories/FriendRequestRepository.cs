using API.Dtos.FriendRequest;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories;

public class FriendRequestRepository : IFriendRequestRepository
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public FriendRequestRepository(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<FriendRequestDto>> GetAllRequestsForUser(int userId)
    {
        return await _dataContext.FriendRequests
            .AsNoTracking()
            .Where(fr => fr.RequestToId == userId)
            .ProjectTo<FriendRequestDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<FriendRequest> GetRequestsById(int requestId)
    {
        return await _dataContext.FriendRequests.FindAsync(requestId);
    }

    public async Task<bool> CreateRequest(AppUser requestFromUser, AppUser requestToUser)
    {
        var request = new FriendRequest(requestFromUser, requestToUser);

        _dataContext.FriendRequests.Add(request);
        return await _dataContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteFriendRequestById(int requestId)
    {
        var request = await _dataContext.FriendRequests
            .SingleOrDefaultAsync(fr => fr.Id == requestId);

        if (request == null)
        {
            return false;
        }

        _dataContext.FriendRequests.Remove(request);
        return await _dataContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> RequestExist(int requestFromUserId, int requestToUserId)
    {
        var req = await _dataContext.FriendRequests
            .FirstOrDefaultAsync(fr => fr.RequestFromId == requestFromUserId && fr.RequestToId == requestToUserId);
        
        return req != null;
    }
}