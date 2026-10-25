# Audio inventory — 25/10/2026

No recorded WAV/MP3/OGG/AIFF/FLAC asset found under `mobile-2d/Assets`. Existing source: `Assets/Scripts/DormSound.cs`, seven deterministic mono 16 kHz procedural factories, each 1 second. No downloads, paid generation or voice-over.

All additions are procedural approximations for prototype staging. Crowd is texture without speech; lake/street are wind beds; applause, projector and travel arrival are stylized placeholders, not recordings. Concrete step can also represent tile/slipper/stair timing, but material-specific realism is pending listening polish. BGM slots contain no audio and intentionally return null; their nominal duration of 1 is schema metadata, not a completed clip.

Rights/provenance: existing DormSound labels itself original procedural foley. New synthesis consists only of deterministic oscillators/noise authored in this task, with no samples or external composition. This records origin rather than inventing an external license grant. JSON holds per-cue provenance, duration, loop, volume and status.

| Cue | Group | Source | Seconds | Loop | Gain | Status |
|---|---|---|---:|---|---:|---|
| water | A_AMBIENCE | DormSound.CreateWater | 1 | True | 0.28 | existing_procedural |
| typing | A_STUDY | DormSound.CreateTyping | 1 | True | 0.28 | existing_procedural |
| writing | A_STUDY | DormSound.CreateWriting | 1 | True | 0.32 | existing_procedural |
| footsteps | A_TRAVEL | DormSound.CreateFootsteps | 1 | True | 0.35 | existing_procedural |
| bus_engine | A_TRAVEL | DormSound.CreateBusEngine | 1 | True | 0.3 | existing_procedural |
| hotpot | A_FRIENDS | DormSound.CreateHotpot | 1 | True | 0.3 | existing_procedural |
| glass_clink | A_FRIENDS | DormSound.CreateGlassClink | 1 | False | 0.4 | existing_procedural |
| amb_station | A_AMBIENCE | synth:crowd | 4 | True | 0.25 | procedural_placeholder |
| amb_dorm | A_AMBIENCE | synth:fan | 4 | True | 0.25 | procedural_placeholder |
| amb_classroom | A_AMBIENCE | synth:hum | 4 | True | 0.25 | procedural_placeholder |
| amb_canteen | A_AMBIENCE | synth:crowd | 4 | True | 0.25 | procedural_placeholder |
| amb_lab | A_AMBIENCE | synth:hum | 4 | True | 0.25 | procedural_placeholder |
| amb_street | A_AMBIENCE | synth:wind | 4 | True | 0.25 | procedural_placeholder |
| amb_lake | A_AMBIENCE | synth:wind | 4 | True | 0.25 | procedural_placeholder |
| amb_ceremony | A_AMBIENCE | synth:crowd | 4 | True | 0.25 | procedural_placeholder |
| amb_binh_dong | A_AMBIENCE | synth:wind | 4 | True | 0.25 | procedural_placeholder |
| step | A_TRAVEL | synth:door | 0.16 | False | 0.18 | procedural_placeholder |
| stairs_step | A_TRAVEL | synth:door | 0.18 | False | 0.22 | procedural_placeholder |
| bag | A_TRAVEL | synth:cloth | 0.55 | False | 0.45 | procedural_placeholder |
| door | A_TRAVEL | synth:door | 0.65 | False | 0.4 | procedural_placeholder |
| key | A_TRAVEL | synth:keys | 0.4 | False | 0.3 | procedural_placeholder |
| bus_arrival | A_TRAVEL | synth:hum | 1.2 | False | 0.3 | procedural_placeholder |
| paper | A_STUDY | synth:cloth | 0.35 | False | 0.3 | procedural_placeholder |
| click | A_STUDY | synth:click | 0.08 | False | 0.28 | procedural_placeholder |
| compile | A_STUDY | synth:click | 0.18 | False | 0.3 | procedural_placeholder |
| test_pass | A_STUDY | synth:success | 0.4 | False | 0.35 | procedural_placeholder |
| error | A_STUDY | synth:error | 0.35 | False | 0.35 | procedural_placeholder |
| submit | A_STUDY | synth:success | 0.45 | False | 0.35 | procedural_placeholder |
| fan_switch | A_LIFE | synth:click | 0.12 | False | 0.3 | procedural_placeholder |
| wipe | A_LIFE | synth:cloth | 0.65 | False | 0.4 | procedural_placeholder |
| alarm | A_LIFE | synth:alarm | 2 | False | 0.35 | procedural_placeholder |
| snooze | A_LIFE | synth:click | 0.15 | False | 0.3 | procedural_placeholder |
| phone_vibrate | A_LIFE | synth:phone | 1.2 | False | 0.35 | procedural_placeholder |
| phone_accept | A_LIFE | synth:click | 0.15 | False | 0.3 | procedural_placeholder |
| phone_end | A_LIFE | synth:error | 0.2 | False | 0.2 | procedural_placeholder |
| money | A_LIFE | synth:cloth | 0.3 | False | 0.35 | procedural_placeholder |
| usb | A_LIFE | synth:click | 0.15 | False | 0.3 | procedural_placeholder |
| screwdriver | A_LIFE | synth:keys | 0.6 | False | 0.25 | procedural_placeholder |
| guard_keys | A_STEALTH | synth:keys | 0.9 | False | 0.4 | procedural_placeholder |
| unplug | A_STEALTH | synth:click | 0.15 | False | 0.35 | procedural_placeholder |
| lift_pot | A_STEALTH | synth:door | 0.3 | False | 0.25 | procedural_placeholder |
| cupboard | A_STEALTH | synth:door | 0.5 | False | 0.3 | procedural_placeholder |
| timer | A_STEALTH | synth:click | 0.08 | False | 0.25 | procedural_placeholder |
| knock | A_FRIENDS | synth:knock | 0.8 | False | 0.4 | procedural_placeholder |
| food_package | A_FRIENDS | synth:cloth | 0.55 | False | 0.4 | procedural_placeholder |
| camera | A_FRIENDS | synth:click | 0.18 | False | 0.45 | procedural_placeholder |
| greeting_cloth | A_FRIENDS | synth:cloth | 0.7 | False | 0.35 | procedural_placeholder |
| printer | A_THESIS_END | synth:printer | 2 | False | 0.35 | procedural_placeholder |
| projector | A_THESIS_END | synth:hum | 1 | False | 0.3 | procedural_placeholder |
| applause | A_THESIS_END | synth:applause | 2 | False | 0.3 | procedural_placeholder |
| degree_paper | A_THESIS_END | synth:cloth | 0.5 | False | 0.35 | procedural_placeholder |
| light_switch | A_THESIS_END | synth:click | 0.12 | False | 0.4 | procedural_placeholder |
| final_door | A_THESIS_END | synth:door | 0.8 | False | 0.35 | procedural_placeholder |
| ui_hover | A_UI | synth:click | 0.05 | False | 0.15 | procedural_placeholder |
| ui_confirm | A_UI | synth:click | 0.1 | False | 0.3 | procedural_placeholder |
| ui_cancel | A_UI | synth:error | 0.12 | False | 0.2 | procedural_placeholder |
| ui_error | A_UI | synth:error | 0.3 | False | 0.3 | procedural_placeholder |
| ui_complete | A_UI | synth:success | 0.45 | False | 0.35 | procedural_placeholder |
| ui_reward | A_UI | synth:success | 0.5 | False | 0.35 | procedural_placeholder |
| ui_lock | A_UI | synth:click | 0.15 | False | 0.25 | procedural_placeholder |
| ui_unlock | A_UI | synth:success | 0.25 | False | 0.3 | procedural_placeholder |
| patrol_step | A_STEALTH | synth:door | 0.2 | False | 0.22 | procedural_placeholder |
| bgm_student | A_BGM | pending | 1 | True | 0.5 | pending_music |
| bgm_gaming | A_BGM | pending | 1 | True | 0.5 | pending_music |
| bgm_decline | A_BGM | pending | 1 | True | 0.5 | pending_music |
| bgm_livelihood | A_BGM | pending | 1 | True | 0.5 | pending_music |
| bgm_lake | A_BGM | pending | 1 | True | 0.5 | pending_music |
| bgm_farewell | A_BGM | pending | 1 | True | 0.5 | pending_music |
| bgm_recovery | A_BGM | pending | 1 | True | 0.5 | pending_music |
| bgm_thesis | A_BGM | pending | 1 | True | 0.5 | pending_music |
| bgm_defense | A_BGM | pending | 1 | True | 0.5 | pending_music |
| bgm_graduation | A_BGM | pending | 1 | True | 0.5 | pending_music |
| bgm_acoustic_credits | A_BGM | pending | 1 | True | 0.5 | pending_music |
