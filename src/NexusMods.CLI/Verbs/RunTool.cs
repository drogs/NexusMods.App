using NexusMods.DataModel.Abstractions;
using NexusMods.DataModel.Games;
using NexusMods.DataModel.Loadouts.Markers;

namespace NexusMods.CLI.Verbs;

// ReSharper disable once ClassNeverInstantiated.Global
/// <summary>
/// Run a tool with a loadout
/// </summary>
public class RunTool : AVerb<ITool, LoadoutMarker>
{
    private readonly IRenderer _renderer;
    private readonly IToolManager _toolManager;

    /// <summary>
    /// DI constructor
    /// </summary>
    /// <param name="configurator"></param>
    /// <param name="toolManager"></param>
    public RunTool(Configurator configurator, IToolManager toolManager)
    {
        _renderer = configurator.Renderer;
        _toolManager = toolManager;
    }

    /// <inheritdoc />
    public static VerbDefinition Definition => new("run",
        "Run a tool with a loadout", new OptionDefinition[]
        {
            new OptionDefinition<ITool>("t", "tool", "Tool to run"),
            new OptionDefinition<LoadoutMarker>("l", "loadout", "Loadout to run the tool with")
        });

    /// <inheritdoc />
    public async Task<int> Run(ITool tool, LoadoutMarker loadout, CancellationToken token)
    {
        await _renderer.WithProgress(token, async () =>
        {
            await _toolManager.RunTool(tool, loadout.Value, token:token);
            return 0;
        });
        return 0;
    }
}
