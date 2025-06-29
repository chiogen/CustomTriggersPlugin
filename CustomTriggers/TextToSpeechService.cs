using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using Serilog;

namespace CustomTriggersPlugin;

internal class TextToSpeechService : IDisposable
{

    private Plugin Plugin { get; }
    private SpeechSynthesizer Synthesizer { get; } = new();

    public List<string> InstalledVoices { get; } = [];

    internal TextToSpeechService(Plugin plugin)
    {
        Plugin = plugin;

        foreach (var v in Synthesizer.GetInstalledVoices())
            InstalledVoices.Add(v.VoiceInfo.Name);
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

            Synthesizer.SpeakAsyncCancelAll();
            Synthesizer.SpeakAsync(message);
        }
        catch (Exception ex)
        {
            Log.Error("Error during tts playback", ex);
        }
    }

}

