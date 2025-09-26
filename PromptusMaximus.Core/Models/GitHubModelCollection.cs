using System.Collections;

namespace PromptusMaximus.Core.Models;

/// <summary>
/// Represents a collection of GitHub models that implements the array structure from the schema.
/// </summary>
public class GitHubModelCollection : IEnumerable<GitHubModel>
{
    private readonly List<GitHubModel> _models = new();

    /// <summary>
    /// Gets the models in the collection.
    /// </summary>
    public IReadOnlyList<GitHubModel> Models => _models.AsReadOnly();

    /// <summary>
    /// Gets the number of models in the collection.
    /// </summary>
    public int Count => _models.Count;

    /// <summary>
    /// Adds a model to the collection.
    /// </summary>
    /// <param name="model">The model to add.</param>
    public void Add(GitHubModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        _models.Add(model);
    }

    /// <summary>
    /// Adds multiple models to the collection.
    /// </summary>
    /// <param name="models">The models to add.</param>
    public void AddRange(IEnumerable<GitHubModel> models)
    {
        ArgumentNullException.ThrowIfNull(models);
        _models.AddRange(models);
    }

    /// <summary>
    /// Removes a model from the collection.
    /// </summary>
    /// <param name="model">The model to remove.</param>
    /// <returns>True if the model was removed, false otherwise.</returns>
    public bool Remove(GitHubModel model)
    {
        return _models.Remove(model);
    }

    /// <summary>
    /// Clears all models from the collection.
    /// </summary>
    public void Clear()
    {
        _models.Clear();
    }

    /// <summary>
    /// Gets a model by its index.
    /// </summary>
    /// <param name="index">The index of the model.</param>
    /// <returns>The model at the specified index.</returns>
    public GitHubModel this[int index] => _models[index];

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator for the collection.</returns>
    public IEnumerator<GitHubModel> GetEnumerator()
    {
        return _models.GetEnumerator();
    }

    /// <summary>
    /// Returns an enumerator that iterates through the collection.
    /// </summary>
    /// <returns>An enumerator for the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Finds models by publisher.
    /// </summary>
    /// <param name="publisher">The publisher name.</param>
    /// <returns>A collection of models from the specified publisher.</returns>
    public IEnumerable<GitHubModel> GetByPublisher(string publisher)
    {
        return _models.Where(m => string.Equals(m.Publisher, publisher, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Finds models by capability.
    /// </summary>
    /// <param name="capability">The capability to search for.</param>
    /// <returns>A collection of models that support the specified capability.</returns>
    public IEnumerable<GitHubModel> GetByCapability(string capability)
    {
        return _models.Where(m => m.Capabilities.Contains(capability, StringComparer.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Finds models by tag.
    /// </summary>
    /// <param name="tag">The tag to search for.</param>
    /// <returns>A collection of models that have the specified tag.</returns>
    public IEnumerable<GitHubModel> GetByTag(string tag)
    {
        return _models.Where(m => m.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase));
    }
}