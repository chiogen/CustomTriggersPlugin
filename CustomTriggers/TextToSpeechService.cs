using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using Serilog;
using Dalamud.Plugin.Services;

namespace CustomTriggersPlugin;

internal class TextToSpeechService : IDisposable
{

    private Plugin Plugin { get; }
    private SpeechSynthesizer Synthesizer { get; } = new();

    public List<string> InstalledVoices { get; } = [];

    internal TextToSpeechService(Plugin plugin)
    {
        Plugin = plugin;

        //Synthesizer.SetOutputToDefaultAudioDevice();
        Synthesizer.Volume = 50;
        Synthesizer.Rate = 0;

        foreach (var v in Synthesizer.GetInstalledVoices())
            InstalledVoices.Add(v.VoiceInfo.Name);
    }

    public void Dispose()
    {
        Synthesizer.Dispose();
    }

    public void Speak(string message)
    {
        try
        {
            if (Plugin.Configuration.Debug)
                Log.Debug($"Sending message to synthesizer: {message}");

            //Synthesizer.SpeakAsyncCancelAll();
            Synthesizer.SpeakAsync(message);
        }
        catch (Exception ex)
        {
            Log.Error("Error during tts playback", ex);
        }
    }

}

