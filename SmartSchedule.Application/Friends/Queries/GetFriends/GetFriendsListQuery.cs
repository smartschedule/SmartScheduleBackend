namespace SmartSchedule.Application.Friends.Queries.GetFriends
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using MediatR;
    using SmartSchedule.Application.Interfaces.UoW;
    using SmartSchedule.Application.DTO.Common;
    using SmartSchedule.Application.DTO.Friends.Queries;
    using SmartSchedule.Application.DTO.User;

    public class GetFriendsListQuery : IRequest<FriendsListResponse>
    {
        public IdRequest Data { get; set; }

        public GetFriendsListQuery(IdRequest data)
        {
            this.Data = data;
        }

        public class Handler : IRequestHandler<GetFriendsListQuery, FriendsListResponse>
        {
            private readonly IUnitOfWork _uow;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork uow, IMapper mapper)
            {
                _uow = uow;
                _mapper = mapper;
            }

            public async Task<FriendsListResponse> Handle(GetFriendsListQuery request, CancellationToken cancellationToken)
            {
                IdRequest data = request.Data;

                var friendsList = await _uow.FriendsRepository.GetFriends(data.Id, cancellationToken);
                var friendsViewModel = new FriendsListResponse
                {
                    Users = new List<UserLookupModel>()
                };

                foreach (var item in friendsList)
                {
                    var user = item.FirstUserId.Equals(data.Id) ? item.SecoundUser : item.FirstUser;
                    friendsViewModel.Users.Add(_mapper.Map<UserLookupModel>(user));
                }

                return friendsViewModel;
            }
        }
    }
}
