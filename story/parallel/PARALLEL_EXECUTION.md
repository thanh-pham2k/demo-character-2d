# Phân công song song — 25/10/2026

Baseline: M01–M09 prototype819checks; 45mission/212event model;24sheet/178PNG đã có. Goal toàn bộ plan vẫn giữ nguyên.

Chat `tao asset` làm composition/integration, shared definition/save/UI và Unity QA tập trung. Các task con là chat riêng trong project, cùng working directory. Không dùng worktree vì user chưa yêu cầu.

## Hợp đồng chung

- Đọc AGENTS/rules và các plan hiện tại. Không reset/ghi đè dirty work của người khác; không commit nếu chưa được yêu cầu.
- Gameplay module implement `IProductionChapterModule`, nhận `ProductionChapterContext` (đã có tại Assets/Scripts/ProductionChapters). Dùng session production được inject; không tạo ví/save/controller demo khác. Phát event đúng current mission/step chỉ sau thao tác thật.
- Chỉ một module được composition root bật tại một thời điểm. State player lock có owner; cancel/pause/unload/reload khôi phục đúng renderer/physics/UI. Không auto-complete mission hoặc dùng nút Confirm cho mọi minigame.
- Mỗi task chỉ sửa namespace/folder được giao; không sửa core opening partials/builders, PlayerMovement/visual, context chung, missions.json, campaign store/model/checks, ASSET_BATCH_PLAN/progress hoặc scene chung. Ghi đề xuất thay đổi shared contract trong handoff của task.
- Scene builder/checker chỉ opt-in, không tự chạy lúc import. Không tự mở/refresh Unity, không start Play Mode, không touch shared Temp requests, không sửa ProjectSettings. Một editor dùng chung; coordinator compile/test sau khi các source ổn định. Tập trung code/module/builder/test riêng và báo giới hạn chưa kiểm chứng.
- Tái sử dụng 178output/27nhóm cũ; không gọi imagegen. Nếu thiếu pose thật sự, ghi ID/grid/ref/slicing proposal trước, không tự tạo ảnh.
- Tài liệu kế quả/handoff: story/parallel/<task>.md. Evidence/test outputs phải ở review/parallel/<task>/; không ghi đè report của task khác. Handoff ghi files, wiring entrypoint, event coverage, validation done/pending, shared changes needed.

## Phạm vi

| Task | Mission / việc | Folder độc quyền |
|---|---|---|
| year2 | M10–M15 gaming/alarm/stealth/C++/exam/portal | Scripts/ProductionChapters/Year2; Editor/ProductionChapters/Year2 |
| year3 | M16–M23 mother/jobs/repair/crush/lake/results | Scripts/ProductionChapters/Year3; Editor/ProductionChapters/Year3 |
| years45 | M24–M36 lab/CV/eligibility/farewells/freshmen/uninstall/recovery | Scripts/ProductionChapters/Years45; Editor/ProductionChapters/Years45 |
| year6 | M37–M45 thesis/demos/report/finalbug/defense/pack/degree/reunion/credits | Scripts/ProductionChapters/Year6; Editor/ProductionChapters/Year6 |
| art_polish | night matte/window, pose/contact/motion audit + reusable native fixes | Editor/ProductionArtPolish; AssetProduction/ParallelArt; review/parallel/art_polish |
| audio | cue inventory/director/foley/ambience wiring proposal | Scripts/ProductionAudio; Editor/ProductionAudio; Resources/ProductionAudio; review/parallel/audio |

Thứ tự tích hợp: contract → Year2 → Year3 → Years45 → Year6; art/audio làm đồng thời nhưng chỉ coordinator wire vào scene/chapter chung. Check từng module trước, full route ở mốc lớn; không chạy lại toàn tuyến cho layout/copy nhỏ.
