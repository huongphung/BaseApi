using Application.Interface.Databases;
using AutoMapper;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public abstract class BaseHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected readonly IMapper _mapper;
        protected readonly ICasperReadContext _contextRead;
        protected readonly ICasperWriteContext _contextWrite;
        protected readonly ILogger _log = Log.ForContext<BaseHandler<TRequest, TResponse>>();

        protected BaseHandler(IMapper mapper,
            ICasperWriteContext contextWrite,
            ICasperReadContext contextRead
        )
        {
            _mapper = mapper;
            _contextWrite = contextWrite;
            _contextRead = contextRead;
        }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}
