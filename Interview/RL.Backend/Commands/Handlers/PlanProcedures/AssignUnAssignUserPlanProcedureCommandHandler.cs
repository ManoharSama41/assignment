using MediatR;
using Microsoft.EntityFrameworkCore;
using RL.Backend.Exceptions;
using RL.Backend.Models;
using RL.Data;

namespace RL.Backend.Commands.Handlers.PlanProcedures
{
    public class AssignUnAssignUserPlanProcedureCommandHandler : IRequestHandler<AssignUnAssignUserPlanProcedureCommand, ApiResponse<Unit>>
    {
        private readonly RLContext _context;
        public AssignUnAssignUserPlanProcedureCommandHandler(RLContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse<Unit>> Handle(AssignUnAssignUserPlanProcedureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //Validate request
                if (request.PlanId < 1)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid PlanId"));
                if (request.ProcedureId < 1)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid ProcedureId"));
                if (request.UserId < 1)
                    return ApiResponse<Unit>.Fail(new BadRequestException("Invalid UserId"));

                var planProcedure = await _context.PlanProcedures
                    .Include(pp => pp.AssignedUsers)
                    .FirstOrDefaultAsync(pp => pp.PlanId == request.PlanId && pp.ProcedureId == request.ProcedureId);

                if (planProcedure is null)
                    return ApiResponse<Unit>.Fail(new NotFoundException($"PlanProcedure not found with PlanId: {request.PlanId} and ProcedureId: {request.ProcedureId}"));

                var user = await _context.Users.FirstOrDefaultAsync(user => user.UserId == request.UserId);

                if (user is null)
                    return ApiResponse<Unit>.Fail(new NotFoundException($"UserId: {request.UserId} not found"));

                var userExistence = planProcedure.AssignedUsers.Any(au => au.UserId == request.UserId);

                if (request.AssignmentType == AssignmentType.Assign)
                {
                    if (userExistence)
                        return ApiResponse<Unit>.Fail(new BadRequestException($"PlanProcedure with PlanId: {request.PlanId} and ProcedureId: {request.ProcedureId} already assigned to  UserId :{request.UserId}"));
                    else
                    {
                        planProcedure.AssignedUsers.Add(user);
                    }
                }

                if (request.AssignmentType == AssignmentType.UnAssign)
                {
                    if (!userExistence)
                        return ApiResponse<Unit>.Fail(new BadRequestException($"PlanProcedure with PlanId: {request.PlanId} and ProcedureId: {request.ProcedureId} and UserId :{request.UserId} assignment does not exists"));
                    else
                    {
                        planProcedure.AssignedUsers.Remove(user);
                    }
                }

                await _context.SaveChangesAsync();

                return ApiResponse<Unit>.Succeed(new Unit());
            }
            catch (Exception ex)
            {
                return ApiResponse<Unit>.Fail(ex);
            }
        }
    }
}
