namespace Extensions.ObjectMappers.Abstractions;

/// <summary>
/// Provides an abstraction layer over object-to-object mapping functionality.
/// This interface is typically implemented using a mapping tool such as AutoMapper.
/// It allows mapping between different object types in a clean and testable way,
/// consistent with Clean Architecture and DDD principles.
/// </summary>
public interface IMapperAdapter
{
    /// <summary>
    /// Maps an instance of <typeparamref name="TSource"/> to a new instance
    /// of <typeparamref name="TDestination"/>.
    /// </summary>
    /// <typeparam name="TSource">The source object type.</typeparam>
    /// <typeparam name="TDestination">The destination object type.</typeparam>
    /// <param name="source">The object to map from.</param>
    /// <returns>
    /// A new instance of <typeparamref name="TDestination"/> with mapped values
    /// from <paramref name="source"/>.
    /// </returns>
    TDestination Map<TSource, TDestination>(TSource source);

    /// <summary>
    /// Maps a sequence of <typeparamref name="TSource"/> objects to a collection
    /// of <typeparamref name="TDestination"/> objects.
    /// </summary>
    /// <typeparam name="TSource">The source element type.</typeparam>
    /// <typeparam name="TDestination">The destination element type.</typeparam>
    /// <param name="source">The sequence of objects to map from.</param>
    /// <returns>
    /// An <see cref="IEnumerable{TDestination}"/> containing mapped items.
    /// </returns>
    IEnumerable<TDestination> Map<TSource, TDestination>(IEnumerable<TSource> source);

    /// <summary>
    /// Maps a list of <typeparamref name="TSource"/> objects to a list of
    /// <typeparamref name="TDestination"/> objects.
    /// </summary>
    /// <typeparam name="TSource">The source element type.</typeparam>
    /// <typeparam name="TDestination">The destination element type.</typeparam>
    /// <param name="source">The list of objects to map from.</param>
    /// <returns>
    /// A <see cref="List{TDestination}"/> containing mapped items.
    /// </returns>
    List<TDestination> Map<TSource, TDestination>(List<TSource> source);
}
