namespace UserManagementService.Src.Services.Implements
{
    using System.Collections.Generic;
    using AutoMapper;
    using UserManagementService.Src.Services.Interfaces;

    public class MapperService : IMapperService
    {
        private readonly IMapper _mapper;

        public MapperService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _mapper.Map<TDestination>(source);
        }

        public List<TDestination> MapList<TSource, TDestination>(List<TSource> sourceItems)
        {
            var mappedObjects = sourceItems.Select(x => _mapper.Map<TDestination>(x)).ToList();
            return mappedObjects;
        }
    }
}