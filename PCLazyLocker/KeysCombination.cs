using System.Collections.ObjectModel;

namespace PCLazyLocker;

public class KeysCombination
{
    private Keys _previousKey = Keys.None;

    private int _keyAwaitingIndex = 0;

    private readonly Action _handler;
    private readonly ReadOnlyCollection<Keys> _keysCombination;

    public KeysCombination(ReadOnlyCollection<Keys> keys, Action handler)
    {
        _keysCombination = keys;
        _handler = handler;
    }

#pragma warning disable CS1998 // async method lacks await 
    public async Task StartMonitoringAsync()
    {
#pragma warning restore CS1998
        InputKeysMonitor.KeyPressed += CheckPressedKey;

#pragma warning disable CS4014 // call is not awaited
        Task.Run(() =>
        {
            while (true)
                if (IsCombinationPressed())
                    _handler.Invoke();
        });
#pragma warning restore CS4014 // call is not awaited
    }

    private void CheckPressedKey(Keys pressedKey)
    {
        if (pressedKey == _previousKey)
            return;

        if (pressedKey == _keysCombination[_keyAwaitingIndex])
            _keyAwaitingIndex++;
        else
            _keyAwaitingIndex = 0;

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
