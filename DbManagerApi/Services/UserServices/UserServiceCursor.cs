using Azure;
using DbManagerApi.Services.Interfaces;
using FluentResults;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Models.ModelsDTO;
using Microsoft.EntityFrameworkCore;
using MR.AspNetCore.Pagination;
using MR.EntityFrameworkCore.KeysetPagination;
using System.Linq.Expressions;

namespace DbManagerApi.Services.UserServices
{
    public class UserServiceCursor :
        UserService,
        IGetKeySetPaginationAsync<UserResponseDTO>
    {
        private IPaginationService _paginationService;
        public UserServiceCursor(SpellTestDbContext context, IPaginationService paginationService) 
            : base(context)
        {
            _paginationService = paginationService;
        }

        public Task<string> GetCursorBase64StringAsync(UserResponseDTO? cursorElement, string? propName)
        {
            return Task.FromResult(
                cursorElement?.Id.ToString() ?? "No more content..."
                );
        }

        public async Task<Result<KeysetPaginationResult<UserResponseDTO>>> GetKeysetPaginationAsync(string? after, string? propName, int? limit, int? Id, bool? reverse)
        {
            {
                int afterInt;
                if (int.TryParse(after, out afterInt) && afterInt > await _context.Users.MaxAsync(m => m.Id))
                {
                    int MaxId = await _context.Users.MaxAsync(m => m.Id);
                    after = MaxId.ToString();
                }
            }
            IQueryable<User> query = _context.Users;

            if (Id is not null)
            {
                query = query.Where(m => m.Id == Id);
            }


            Func<IQueryable<User>, IQueryable<UserResponseDTO>> map = (tIn) => tIn.Select(m => MapToDTO(m));

            KeysetQueryModel queryModel = new KeysetQueryModel()
            {

                Size = limit ?? 20,
                After = after,
            };

            Action<KeysetPaginationBuilder<User>> actionKeysetPaginationBuilder;
            try
            {
                actionKeysetPaginationBuilder = CreateActionKeysetPaginationBuilder(propName, reverse);
            }
            catch
            {
                actionKeysetPaginationBuilder = CreateActionKeysetPaginationBuilder(nameof(User.Id), reverse);
            }

            KeysetPaginationResult<UserResponseDTO> result = await _paginationService.KeysetPaginateAsync(
                query,
                CreateActionKeysetPaginationBuilder(propName, reverse),
                async id => await _context.Users.FindAsync(int.Parse(id)),
                map,
                queryModel: queryModel
            );

            return Result.Ok(result);
        }

        private Action<KeysetPaginationBuilder<User>> CreateActionKeysetPaginationBuilder(string? propName, bool? reverse)
        {
            var parameter = Expression.Parameter(typeof(User), "m");
            var property = Expression.PropertyOrField(parameter, propName ?? nameof(User.Id));
            var propertyType = property.Type;

            var lambdaType = typeof(Func<,>).MakeGenericType(typeof(User), propertyType);
            var lambda = Expression.Lambda(lambdaType, property, parameter);

            return builder =>
            {
                var ascendingMethod = typeof(KeysetPaginationBuilder<User>)
                    .GetMethods()
                    .First(m => m.Name == "Ascending" && m.IsGenericMethod)
                    .MakeGenericMethod(propertyType);

                var descendingMethod = typeof(KeysetPaginationBuilder<User>)
                    .GetMethods()
                    .First(m => m.Name == "Descending" && m.IsGenericMethod)
                    .MakeGenericMethod(propertyType);

                var propertyId = Expression.Lambda<Func<User, int>>(
                    Expression.PropertyOrField(parameter, nameof(User.Id)),
                    parameter
                );

                if (reverse == true)
                {
                    descendingMethod.Invoke(builder, new object[] { lambda });
                    builder.Descending(propertyId);
                }
                else
                {
                    ascendingMethod.Invoke(builder, new object[] { lambda });
                    builder.Ascending(propertyId);
                }
            };
        }
    
    }
}
