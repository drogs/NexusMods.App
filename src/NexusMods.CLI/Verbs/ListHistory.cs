using NexusMods.CLI.DataOutputs;
using NexusMods.DataModel.Loadouts.Markers;

namespace NexusMods.CLI.Verbs;

// ReSharper disable once ClassNeverInstantiated.Global
/// <summary>
/// Lists the history of a loadout
/// </summary>
public class ListHistory : AVerb<LoadoutMarker>
{
    private readonly IRenderer _renderer;

    /// <summary>
    /// DI constructor
    /// </summary>
    /// <param name="configurator"></param>
    public ListHistory(Configurator configurator) => _renderer = configurator.Renderer;

    /// <inheritdoc />
    public static VerbDefinition Definition => new("list-history", "Lists the history of a loadout",
        new OptionDefinition[]
        {
            new OptionDefinition<LoadoutMarker>("l", "loadout", "Loadout to load")
        });

    /// <inheritdoc />
    public async Task<int> Run(LoadoutMarker loadout, CancellationToken token)
    {
        var rows = loadout.History()
            .Select(list => new object[] { list.LastModified, list.ChangeMessage, list.Mods.Count, list.DataStoreId })
            .ToList();

        await _renderer.Render(new Table(new[] { "Date", "Change Message", "Mod Count", "Id" }, rows));
        return 0;
    }
}
