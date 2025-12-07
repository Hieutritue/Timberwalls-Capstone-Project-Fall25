using UnityEngine;

public class Test : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public SoundSource musicSource;     // e.g. Ambience / BG Music
    public SoundSource sfxSource;       // e.g. SFX Source

    void Update()
    {
        // -----------------------------
        // MUSIC TEST INPUTS
        // -----------------------------

        // 1 → play first track with fade-in + crossfade
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("▶ Test 1: Play \"track1\" (fade + crossfade)");
            musicSource.Play("01. Be Ready for it!", fadeIn: true, fadeOut: false, crossfade: true);
        }

        // 2 → play second track, no fade, instant
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("▶ Test 2: Play \"track2\" (NO fade, NO crossfade)");
            musicSource.Play("02. Gene's Rock A Bye", fadeIn: false, fadeOut: false, crossfade: true);
        }

        // -----------------------------
        // SFX TEST INPUTS
        // -----------------------------

        // 3 → play SFX with fade-in (rare but possible)
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("▶ Test 3: SFX \"click\" with fade-in");
            sfxSource.Play("28. Battery size D", fadeIn: true, fadeOut: false, crossfade: false);
        }

        // 4 → play another SFX without fade
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("▶ Test 4: SFX \"explosion\" instant");
            sfxSource.Play("30. Fly Flap", fadeIn: false, fadeOut: true, crossfade: false);
        }

        // -----------------------------
        // STOP / FADE-OUT
        // -----------------------------

        // F → fade-out current music track
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("⏸ Fade-out current MUSIC");
            musicSource.FadeOutAndStop();
        }
    }
}
