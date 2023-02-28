using System.Collections.ObjectModel;

namespace PCLazyLocker;

public class KeysCombination
{
    private Keys _previousKey = Keys.None;

    private int _keyAwaitingIndex = 0;

    private readonly ReadOnlyCollection<Keys> _keysCombination;
    private readonly Action _combinationPressedHandler;
    private readonly Action? _missedKeyHandler;

    public KeysCombination(
        ReadOnlyCollection<Keys> keys,
        Action combinationPressedHandler,
        Action? missedKeyHandler = null)
    {
        _keysCombination = keys;
        _combinationPressedHandler = combinationPressedHandler;
        _missedKeyHandler = missedKeyHandler;
    }

#pragma warning disable CS1998
    public async Task StartMonitoringAsync()
    {
#pragma warning restore CS1998
        InputKeysMonitor.KeyPressed += CheckPressedKey;

#pragma warning disable CS4014
        Task.Run(() =>
        {
            while (true)
            {
                if (IsCombinationPressed())
                    _combinationPressedHandler.Invoke();

                Thread.Sleep(Config.POLLING_DELAY_MS);
            }
        });
#pragma warning restore CS4014
    }

    private void CheckPressedKey(Keys pressedKey)
    {
        if (!InputKeysMonitor.IsKeyboardKey(pressedKey)
         || pressedKey == _previousKey)
            return;

        if (pressedKey == _keysCombination[_keyAwaitingIndex])
            _keyAwaitingIndex++;
        else
        {
            _keyAwaitingIndex = 0;
            _missedKeyHandler?.Invoke();
        }

        _previousKey = pressedKey;
    }

    private bool IsCombinationPressed()
    {
        if (_keyAwaitingIndex == _keysCombination.Count)
        {
            _keyAwaitingIndex = 0;
            return true;
        }

        return false;
    }
}
