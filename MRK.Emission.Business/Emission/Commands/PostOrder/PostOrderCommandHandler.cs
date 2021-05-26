using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MRK.Emission.Business.Emission.Commands.PostOrder
{
    public class PostOrderCommandHandler : IRequestHandler<PostOrderCommandRequest, PostOrderCommandResponse>
    {
        public Task<PostOrderCommandResponse> Handle(PostOrderCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
