using System.Collections.ObjectModel;

namespace PCLazyLocker;

public class PCLocker
{
    private bool _isLocked;

    private readonly ReadOnlyCollection<Keys> _keysCombination;
    private readonly ReadOnlyDictionary<Keys, Keys>? _keysAliases;
    private readonly CancellationTokenSource _cts;

    public PCLocker(
        ReadOnlyCollection<Keys> keysCombination,
        ReadOnlyDictionary<Keys, Keys>? keysAliases = null)
    {
        _isLocked = false;

        _keysCombination = keysCombination;
        _keysAliases = keysAliases;

        _cts = new CancellationTokenSource();
    }

    public async Task StartPollingAsync()
    {
        await KeyboardManager.SetKeysCombination(
                _keysCombination,
                () => _isLocked.Switch(),
                _cts.Token,
                _keysAliases);


    }

    public void CancelKeysCombinations()
    {
        _cts.Cancel();
    }
}
