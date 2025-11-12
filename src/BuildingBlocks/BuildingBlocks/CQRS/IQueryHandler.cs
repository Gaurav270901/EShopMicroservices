using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace BuildingBlocks.CQRS
{
    // we are declaring common interface here which can handle any query to retrive data 
    // when any class extend this interface it need to pass query as tquery type and response as tresponse
    // ex. GetUserByIdHandler : IQueryHandler<UserQuery , UserDto>
    public interface IQueryHandler<in TQuery , TResponse> : IRequestHandler<TQuery , TResponse>
        where TQuery : IQuery<TResponse>
        where TResponse : notnull
    {
    }
}
