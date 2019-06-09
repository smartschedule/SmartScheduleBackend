namespace SmartSchedule.Application.Friends.Queries.GetUserFriendRequests
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using SmartSchedule.Application.DAL.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Friends.Queries;
    using SmartSchedule.Application.DTO.User;

    public class GetUserFriendRequestsQuery : IRequest<FriendsListResponse>
    {
        public IdRequest Data { get; set; }

        public GetUserFriendRequestsQuery(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<GetUserFriendRequestsQuery, FriendsListResponse>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }
            public async Task<FriendsListResponse> Handle(GetUserFriendRequestsQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var friendRequestList = await _uow.FriendsRepository.GetPendingUserFriends(data.Id, cancellationToken);
                var friendsViewModel = new FriendsListResponse
                {
                    Users = new List<UserLookupModel>()
                };

                foreach (var item in friendRequestList)
                {
                    var user = item.Type == Domain.Enums.FriendshipTypes.pending_first_second ?
                        item.SecoundUser : item.FirstUser;

                    friendsViewModel.Users.Add(_mapper.Map<UserLookupModel>(user));
                }

                return friendsViewModel;
            }
        }
    }
}
