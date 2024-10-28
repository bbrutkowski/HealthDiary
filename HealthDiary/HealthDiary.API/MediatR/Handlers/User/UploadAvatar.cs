using CSharpFunctionalExtensions;
using HealthDiary.API.Context.DataContext;
using HealthDiary.API.Model.Main;
using MediatR;

namespace HealthDiary.API.MediatR.Handlers.User
{
    public static class UploadAvatar
    {
        public record UpdateAvatarRequest(int UserId, string Avatar) : IRequest<Result<bool>>;

        public sealed class Handler : IRequestHandler<UpdateAvatarRequest, Result<bool>>
        {
            private readonly DataContext _context;

            private const string UserNotFoundError = "User with given Id not found";
            private const string UpdateUserAvatarError = "Error occurred while updating user avatar";

            public Handler(DataContext context) => _context = context;

            public async Task<Result<bool>> Handle(UpdateAvatarRequest request, CancellationToken cancellationToken)
            {

                var avatarBytes = Convert.FromBase64String(request.Avatar);

                var newAvatar = new Avatar
                {
                    UserId = request.UserId,
                    Picture = avatarBytes
                };

                await _context.Avatars.AddAsync(newAvatar, cancellationToken);
                var changes = await _context.SaveChangesAsync(cancellationToken);

                return changes > 0 ? Result.Success(true) : Result.Failure<bool>(UpdateUserAvatarError);
            }
        }
    }
}
