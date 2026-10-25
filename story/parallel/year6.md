# Year6 M37–M45 — source handoff

Trạng thái: **source ổn định để coordinator review/tích hợp; chưa compile C#, chưa Unity refresh, chưa Play Mode**. Không xác nhận full route, visual contact, mobile QA hay Finished runtime. Không imagegen, không sửa scene/core/context/definition/save/ProjectSettings/Temp requests. Dùng asset hiện có; không gửi tin sang chat khác.

## Files và entrypoint

- `mobile-2d/Assets/Scripts/ProductionChapters/Year6/Year6ChapterModule.cs` — `ProductionChapters.Year6.Year6ChapterModule : MonoBehaviour, IProductionChapterModule, IInteractable`, range 37–45.
- `Year6Activities.cs` — đủ 47 event theo đúng missions.json hiện tại, lựa chọn thao tác theo quy trình, distractors, confidence/rehearsal checkpoint, clock từ durable prefix.
- `Year6Workbench.cs` — mô hình graph/cổng mạng, telemetry, API authorization/input validation, crash/null fix, regression và report state. Đây là mô phỏng gameplay trong bộ nhớ, không chạy build/network thật ngoài game.
- `mobile-2d/Assets/Editor/ProductionChapters/Year6/Year6ModuleBuilder.cs` — menu **Production → Chapters → Year6 → Create owned module under selection**; tạo GO dưới selection và bind existing sprites. Chỉ thực thi khi coordinator gọi menu; không auto-run/import hook.
- `Year6Validation.cs` — menu **Production → Chapters → Year6 → Validate activities and workbench**. Chưa được chạy; log báo số assertions khi coordinator chạy thành công.
- Các `.meta` riêng đã tạo, kể cả hai folder Year6. Không thay `.meta` folder parent chung.
- `review/parallel/year6/check_source.py`, `source-contract.json` — evidence riêng, source hashes.

## Wiring cho composition root

1. Coordinator chạy builder opt-in trong scene integration; giữ module GO/component enabled. Không tạo `ProductionCampaignSession` mới.
2. Khi shared session tới M37, gọi `module.Configure(sharedContext)`. Module dựng stage con dưới `context.WorldRoot` (hoặc chính module nếu không có root), không sửa shared camera/fade/scene objects.
3. Gọi `Refresh()` khi context thay đổi. Module cũng tự refresh theo current step lúc active; callback commit đồng bộ được xử lý sau khi đóng IMGUI layout. Chỉ một chapter active.
4. `Interact()` hoặc nút tên activity mở workbench. Module acquire player state `Transitioning` với owner `this`; chỉ khi acquire thành công mới mở. Renderer của player được snapshot và tắt khi workbench mở, khôi phục khi đóng/suspend/disable/destroy, chỉ release interaction nếu còn thuộc module. Không sửa collider/physics trực tiếp.
5. `Suspend()` khi đổi chapter, unload hoặc composition pause toàn màn hình; có thể `Configure(context)` lại để resume từ durable step. Đóng UI/Escape tạm dừng credits; `Time.timeScale == 0` không nhận lựa chọn, không cộng thời gian credits. Uncommitted activity được làm lại khi reload, không tự commit. Save failure giữ kết quả và cung cấp **Thử lưu lại checkpoint**, không chạy lại effect/reward hoặc skip step.
6. Sau `finish_credits`, shared progress phát Finished; coordinator chịu trách nhiệm fade/menu/scene transition cuối. Module không tự bật menu demo hay đánh Finished trực tiếp.

## Event coverage

| Mission | Hoạt động thực thi trong prototype |
|---|---|
| M37 — 4 event | Lịch/GVHD, đề tài tự động hóa và chẩn đoán lỗi thiết bị mạng, hồ sơ, checklist nghiên cứu |
| M38 — 4 | Tài liệu có nguồn/phiên bản, tạo graph sensor→switch→diagnostic→dashboard, TLS443/SSH22 private, validate đường đi/cycle/cổng, backlog |
| M39 — 4 | Parse/input/timeout-retry/alert modules; online/offline/bad-payload tests; demo gây lỗi và phục hồi; lịch demo1 |
| M40 — 5 | Demo1, ghi cả auth/input feedback, patch token/quyền/input/secret, suite 401/403/400/200, đặt demo2 |
| M41 — 5 | Demo2 được kiểm security, report styles/mục lục/nguồn/120 trang, preview/in/lề gáy, bind, hạn nộp |
| M42 — 7 | Reproduce disconnect crash, trace DeviceSnapshot.Read, null/disconnected/cancel patch, regression+security, release/checksum/upload, receipt+USB backup, lịch 5 người |
| M43 — 5 | Sơ mi, cáp/input máy chiếu, slide/problem/design/demo/limits, 5 câu hỏi riêng hội đồng, nhận biên bản A sau pass |
| M44 — 6 | Gọi mẹ, chọn riêng laptop/sách/tua vít/chăn Lươn (đồ 3 freshmen là distractor bị từ chối), công tắc, ra/khép cửa, trả khóa tại quầy, thư mời |
| M45 — 7 | Nhận bằng ở A5, cha mẹ/hoa/ảnh, Đạm đã về, đủ 3 bạn, đổi đồ/travel Bình Đông, thứ tự 4 người/khoác vai/đi, xem credits 18 giây mới commit |

Không có `CompleteMission`/auto-pass button. Các thao tác là UI lựa chọn theo thứ tự với đáp án nhiễu; những nhóm network/security/crash/report thay đổi và kiểm tra mô hình thật trong bộ nhớ trước commit. Các cảnh hội thoại/travel/packing là **prototype sequence tương tác**, chưa phải animation nhân vật đi tự do hoặc drag/drop vật lý. Không mô tả đây là bản minigame/diễn xuất cuối cùng.

Model chung vẫn là nguồn rewards/facts duy nhất: module commit `context.Progress.CurrentMission/currentStep.id` sau hoạt động đạt; A/150 chỉ ở receive_grade_a sau pass_defense_qa; không cấp bằng ở M43; M44 không commit Finished; chỉ finish_credits sau degree/link_shoulders. Yêu cầu tiên quyết, khóa/receipt và inventory vẫn do model kiểm.

## Clock, retry và persistence

M42 dùng **đồng hồ công việc mô phỏng theo prefix sự kiện đã lưu**, không phải countdown wall-clock. 22:45 ban đầu → 23:00 reproduce → 23:15 log → 23:35 patch → 23:44:32 final tests; submit/receipt giữ 23:44:32 trước hạn23:45. Không bake giờ vào ảnh. Reload không làm lại thời gian của các event đã commit. Thao tác sai giảm confidence và retry local, không tiêu tốn giờ mô phỏng.

Nếu coordinator muốn countdown thời gian thực với timeout/resume hoặc fail-branch thay vì mô phỏng này, shared save cần cung cấp auxiliary state/clock có session/run identity; module chưa tự tạo sidecar save hay ví/session phụ. Đây là thay đổi contract cần quyết định trước QA deadline thực.

Q&A có 5 lượt tương ứng GVHD/chủ tịch/3 giám khảo. Sai reset thứ tự và giảm tự tin; hết 3 cần tập dượt để khôi phục. Không rollback event đã lưu hoặc cấp A trong retry. Credits chỉ cộng thời gian khi UI mở và game không pause; reload/suspend bắt đầu lại thời lượng xem của event chưa commit.

## Art reuse / staging

Builder bind C04/C05/P04/P05/E04/E05 và teacher, E03 advisor office, room404, light switch, P01 key, AN03 robe idle. Hội đồng dùng teacher + chair + examiner A/B/C = 5; P05 projector/table/podium; E05 giữ aspect theo height 8 units; A5 dùng robe/cha/mẹ/hoa; Bình Đông dùng 4 C05 arm poses. M44 chỉ đối tượng của Lươn thay đổi; ba bedding freshmen luôn còn; riêng stage của module tint tối sau công tắc.

Stage sprites là blockout reviewable. Chưa duyệt feet/pivot/contact khoác vai, mask foreground, travel walking, máy in/đóng gáy animation, cửa vật lý, real-time light của shared room, safe area/font Việt, audio, camera framing. Bốn vai cần coordinator/art polish kiểm khoảng cách bằng camera thực; không tự generate ảnh để chữa staging. Prior chapter world presentation cần composition root ẩn/restore theo ownership khi vào Year6 để không chồng stage. Module không tự tắt shared roots.

## Validation đã làm / cần làm

Đã chạy `python3 review/parallel/year6/check_source.py`: **72 source-contract checks pass**, 47/47 event IDs khớp thứ tự data, asset paths có thật, không second session/CompleteMission/import/refresh/Play hooks, source hash ghi trong JSON. Đây là kiểm tra source, **không phải C# compile hoặc gameplay test**.

Coordinator cần:

1. Compile Unity sau khi tất cả source song song ổn định; chạy menu `Validate activities and workbench` (positive sequence, unknown/wrong input, confidence retry, empty/unpatched test failures, graph/security/report/final-bug checks).
2. Chạy fixture shared session tại M37 với prefix M01–M36 hợp lệ, đi đủ47 events; kiểm 136→150 chỉ sau phản biện, receipt/key/freshmen inventory, M44 chưa Finished, degree/credits/M45 Finished.
3. Save-failure retry; close/reopen; pause during credits; disable/unload/destroy while owned interaction; reload giữa từng event (bench reconstruct từ event history). Không compile/Play bằng source checker.
4. Review stage camera/renderer restore/IMGUI trên desktop và mobile; integrate fade→credits→menu, shared world isolation/audio, door/packing contact animation.

Không sửa shared contract trong task này. Không commit git; workspace có nhiều dirty/untracked changes từ các task khác, đã giữ nguyên.
