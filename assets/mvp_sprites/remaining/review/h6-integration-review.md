# H6 identity and home reaction integration

C00 classmate is bound in saved H6 scene and its NPC sprite writer. Classroom dialogue/prompt identifies a classmate; Đạm reacts at404 after the first completed school day. Historical scene object names are retained to preserve references.

The home beat owns a journey lock until conversation completion. A persistent `story:h6-told-dam` flag prevents repeated playback after checkpoint restore. Completing this flag requires completed lessons, return trip and safeRoom. Pure checkpoint checks passed in Unity; the full Play Mode suite is being rerun.

Initial regression caught an automation race: movement started before the newly scheduled home dialogue. The route now waits for that authored conversation and asserts its persistent flag before continuing. Screenshots record the actual classmate and404 conversation.

This covers the identity correction and home bridge only, not the entire45mission production flow.

Fresh Play Mode regression finished 2026-10-25T10:55:36.0886730Z: PASS506checks,0errors. Includes first404reaction, one-time story commit, classroom classmate route and checkpoint resumes. Scope remains the existing Year1 campaign and interaction foundation, not all45production missions.
