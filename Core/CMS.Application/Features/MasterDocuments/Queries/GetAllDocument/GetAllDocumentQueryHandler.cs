using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS.Application.Contracts.Persistence;
using CMS.Domain.Entities;
using MediatR;

namespace CMS.Application.Features.MasterDocuments.Queries.GetAllDocument
{
    public class GetAllDocumentQueryHandler : IRequestHandler<GetAllDocumentQuery, (IEnumerable<MasterDocument> , int )>
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly ICacheService _cacheService;
        public GetAllDocumentQueryHandler(IDocumentRepository documentRepository, ICacheService cacheService)
        {
            _documentRepository = documentRepository;
            _cacheService = cacheService;
        }
        public  async Task<(IEnumerable<MasterDocument> , int )> Handle(GetAllDocumentQuery request, CancellationToken cancellationToken)
        {
            //string cacheKey = $"Documents_{request.pageNumber}_{request.pageSize}";

            ////getting from cache
            //var cachedDocument = await _cacheService.GetAsync<(IEnumerable<MasterDocument> docs, int totalCount)>(cacheKey);
            //if (cachedDocument.docs != null && cachedDocument.totalCount != null)
            //{
            //    return (cachedDocument.docs, cachedDocument.totalCount);

            //}

            ////not in cache then fetching from repo
            //var docu = await _documentRepository.GetAllDocuments(request.pageNumber, request.pageSize);

            ////storing in cache
            //await _cacheService.SetAsync(cacheKey, docu, TimeSpan.FromMilliseconds(1));

            //return (docu.docs, docu.totalCount);

            return await _documentRepository.GetAllDocuments(request.pageNumber, request.pageSize);
        }
    }
}
