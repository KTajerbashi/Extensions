using AutoMapper;
using Extensions.ObjectMappers.Abstractions;

namespace Extensions.ObjectMappers;

/// <summary>
/// Provides an implementation of <see cref="IMapperAdapter"/> using AutoMapper.
/// This adapter abstracts the AutoMapper dependency and offers a clean,
/// testable way to perform object-to-object mappings within the application.
/// </summary>
/// <remarks>
/// This implementation aligns with Clean Architecture principles by avoiding
/// direct AutoMapper usage across the application layers. Instead, it offers
/// a consistent mapping interface that can be easily mocked during unit testing.
/// </remarks>
public class ObjectMapperAdapter(IMapper mapper) : IMapperAdapter
{
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Maps an object of type <typeparamref name="TSource"/> to an instance
    /// of <typeparamref name="TDestination"/> using AutoMapper.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object.</typeparam>
    /// <typeparam name="TDestination">The type of the destination object.</typeparam>
    /// <param name="source">The source object to map.</param>
    /// <returns>
    /// A new <typeparamref name="TDestination"/> instance populated with values
    /// from <paramref name="source"/>.
    /// </returns>
    public TDestination Map<TSource, TDestination>(TSource source)
        => _mapper.Map<TDestination>(source);

    /// <summary>
    /// Maps a sequence of <typeparamref name="TSource"/> objects to a sequence of
    /// <typeparamref name="TDestination"/> objects using AutoMapper.
    /// </summary>
    /// <typeparam name="TSource">The type of the source elements.</typeparam>
    /// <typeparam name="TDestination">The type of the destination elements.</typeparam>
    /// <param name="source">The collection of source objects.</param>
    /// <returns>
    /// An <see cref="IEnumerable{TDestination}"/> containing mapped objects.
    /// </returns>
    public IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source)
        => _mapper.Map<IEnumerable<TDestination>>(source);

    /// <summary>
    /// Maps a list of <typeparamref name="TSource"/> objects to a list of
    /// <typeparamref name="TDestination"/> objects using AutoMapper.
    /// </summary>
    /// <typeparam name="TSource">The type of the source elements.</typeparam>
    /// <typeparam name="TDestination">The type of the destination elements.</typeparam>
    /// <param name="source">The list of source objects to map.</param>
    /// <returns>
    /// A <see cref="List{TDestination}"/> containing mapped objects.
    /// </returns>
    public List<TDestination> Map<TSource, TDestination>(List<TSource> source)
        => _mapper.Map<List<TDestination>>(source);
}

