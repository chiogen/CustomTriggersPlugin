using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using Serilog;

namespace CustomTriggersPlugin;

internal class TextToSpeechService : IDisposable
{

    private Plugin Plugin { get; }
    private SpeechSynthesizer Synthesizer { get; } = new();

    public string Voice
    {
        get
        {
            return Synthesizer.Voice.Name;
        }
    }
    public List<InstalledVoice> InstalledVoices { get; init; } = [];
    public Queue<string> MessageQueue { get; } = [];

    internal TextToSpeechService(Plugin plugin)
    {
        Plugin = plugin;

        InstalledVoices = [.. Synthesizer.GetInstalledVoices()];

        SelectVoiceFromConfig(this, new EventArgs());

        Plugin.Configuration.OnSaved += SelectVoiceFromConfig;
        Synthesizer.SpeakCompleted += OnSpeakComplete;
    }

    public void Refresh()
    {
        InstalledVoices.Clear();
        InstalledVoices.AddRange(Synthesizer.GetInstalledVoices());
    }

    public void Dispose()
    {
        Synthesizer.SpeakAsyncCancelAll();
        Synthesizer.Dispose();
    }

    public void Speak(string message)
    {
        try
        {
            Synthesizer.Volume = Plugin.Configuration.Volume;

            if (Plugin.Configuration.Debug)
                Log.Debug($"Sending message to synthesizer: {message}");


            if (Plugin.Configuration.EnableSoundQueue)
            {
                MessageQueue.Enqueue(message);
                TryPlayNext(); // Attempt to play if nothing else is playing
            }
            else
            {
                Synthesizer.SpeakAsyncCancelAll();
                Synthesizer.SpeakAsync(message);
            }
        }
        catch (Exception ex)
        {
            Log.Error("Error during tts playback", ex);
        }
    }

    private void SelectVoiceFromConfig(object? sender, EventArgs e)
    {
        try
        {
            if (Plugin.Configuration.Voice != null)
            {
                Synthesizer.SelectVoice(Plugin.Configuration.Voice);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to apply voice from configuration.");
        }
    }

    private void OnSpeakComplete(object? sender, SpeakCompletedEventArgs ev)
    {
        if (!Plugin.Configuration.EnableSoundQueue)
            return;

        TryPlayNext();
    }
    private void TryPlayNext()
    {
        if (Synthesizer.State == SynthesizerState.Ready && MessageQueue.TryDequeue(out string? nextMessage) && nextMessage != null)
            Synthesizer.SpeakAsync(nextMessage);
    }

}

