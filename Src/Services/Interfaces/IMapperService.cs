namespace UserManagementService.Src.Services.Interfaces
{
        public interface IMapperService
    {
        public TDestination Map<TSource, TDestination>(TSource source);

        public List<TDestination> MapList<TSource, TDestination>(List<TSource> sourceItems);
    }
}