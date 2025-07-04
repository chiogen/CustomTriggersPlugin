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

    public List<string> InstalledVoices { get; } = [];
    public Queue<string> MessageQueue { get; } = [];

    internal TextToSpeechService(Plugin plugin)
    {
        Plugin = plugin;

        foreach (var v in Synthesizer.GetInstalledVoices())
            InstalledVoices.Add(v.VoiceInfo.Name);

        Synthesizer.SpeakCompleted += OnSpeakComplete;
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

    public void OnSpeakComplete(object? sender, SpeakCompletedEventArgs ev)
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

