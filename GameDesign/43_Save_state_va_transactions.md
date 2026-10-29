# Save state và transaction contract — v0.1

Đây là contract đề xuất cho programmer/QA, không phải implementation hoặc bằng chứng crash-safe. Repo gameplay đã được xóa theo yêu cầu; tài liệu nằm ngoài repo. Cần chọn storage engine và thực nghiệm atomicity trước production.

## Save envelope

`SaveEnvelope { schemaVersion, contentVersion, campaignId, revision, savedAtMetadata, payload, checksum }`. `savedAtMetadata` để trình bày, không dùng tính `now`; `payload.now` là campaign minute. Checksum phát hiện corruption, không phải bảo mật. `contentVersion` khóa bộ định nghĩa dùng khi tính event/assessment; snapshot immutable lưu version reference để thay dữ liệu sau update không âm thầm đổi kết quả cũ.

Payload tối thiểu: `world {year, now, locationId, positionAnchor, unlockedLocations, calendarEntries, knownEntries, npcAvailability}`; `player {energy, wallet, reservations, inventory}`; `activities {activeRun, interruptedRuns, elapsed, energyApplied, milestones, pendingDecision}`; `academics {programVersion, courses, attempts, attendance, assignments, exams, creditLedger}`; `skills {nodes, evidenceRevisions, projects}`; `relationships {dimensions, conflicts, npcKnowledge, memories}`; `events {instances, processedKeys, pendingQueue, rngState}`; `career {cvDraft, applications, offers, acceptedOffer}`; `terminal {graduationCommit, handoff, ceremony, endingCommit}`. UI panel/pose cần thiết để resume được lưu cùng phiên, không suy từ vị trí mặc định.

IDs phải stable, unique trong namespace, references trỏ ID tồn tại hoặc `unknownContent` khi chưa có data; không silent drop record. Map progress 0–100, energy 0–100, available money ≥0 ngoài khoản nợ được author; `revision` tăng khi transaction committed. Runtime definitions và state không phải cùng object để sửa definition không đổi snapshot lịch sử.

## Transaction invariant

`command {commandId, expectedRevision, type, payload}` → validate trên snapshot → tính candidate + ledger/events → atomic persist(candidate) → publish candidate/UI. Mọi effect liên quan nằm cùng candidate; `commandId` đã commit thì trả kết quả trước, không apply lần hai. ExpectedRevision stale → không ghi, trả conflict để reload/re-preview. Cancel preview không ghi; retry cùng ID sau save lỗi chỉ được commit một lần. Persist lỗi không publish state hoặc hiệu ứng âm thanh/UI có tính xác nhận.

Ví dụ mua bữa: reserve 8M khi bắt đầu là transaction A; completion là transaction B capture reservation + memory + energy + time. B fail giữ state sau A và reservation; resume/retry hoặc cancel release, không vừa trả tiền vừa thiếu memory. Một transaction không được coi thành công chỉ vì UI đã nhận input.

## Clock và scheduler

Lưu `now`, activeRun, elapsed, energyApplied, pendingDecision, scheduler cursor `(timestamp, phase, priority, eventId)` và processed occurrence keys. Xử lý `(oldNow,newNow]` theo `02` với exception boarding trong `29`; sau mỗi decision bắt buộc pause và persist cursor. Resume không chạy lại các occurrence đã commit. Không tự sinh event trong skip ngoài calendar; preview phải lưu source revision, destination và danh sách known outcomes theo `02`.

Với activity cho phép resume, lưu từng interruptedRun cùng runId, elapsed và energyApplied riêng; tối đa một run đang running, không ghi đè run đang gián đoạn khi hoạt động khác bắt đầu. Một activity gián đoạn có thể resume cùng runId; elapsed và energyApplied là cumulative, không tính lại từ đầu. `now` không được lùi khi cancel activity đã tiến thời gian. Cancel preview time-skip không tiến clock. `boardingDecision` chờ user, save/load không tự đóng cửa, theo `29`.

## Assessments và progression

Attempt lưu question IDs/order, version/rubric, answers, profile, remainingMs, phase và submissionId; policy `40`. Submit atomically ghi đáp án, score, result và ledger effect phù hợp; không cộng course credit nếu chưa resolve môn. Course pass creditLedger dùng khóa `(programId,courseId)`; retry/retake sau pass không cộng thêm. `unknown` phân biệt `failed|missed`, không biến missing definition thành số 0. Giữ assignment snapshot của courseAttemptId đúng theo `30`.

Graduation preview giữ programDefinitionVersion và academicRevision; commit chỉ khi cùng revision và eligible. Handoff gắn graduationCommitId; ceremony và ending có IDs riêng, không tự replay khi load. Offer acceptance khóa `(campaignId,acceptedOffer)` và atomically đánh dấu offers khác, không tự tạo first-day commit.

## Restore và migration

Load: kiểm tra checksum/schema/content compatibility → migrate trên bản sao trong memory → validate invariants → persist bản migrate atomically → publish. Nếu version mới hơn client, checksum sai, thiếu content hoặc migration fail: giữ bản gốc, hiển thị lỗi/khả năng khôi phục, không reset campaign hay đặt missing records thành pass/fail. Backup snapshot version trước migration; không ghi đè duy nhất bản nguồn khi chưa có persisted result. RNG seed/state được giữ, không re-roll outcome đã commit.

## Kiểm tra đề xuất

- Hai lệnh cùng commandId và hai lệnh khác ID nhưng cùng expectedRevision; không double capture/credit/offer.
- Crash hoặc lỗi trước persist, sau persist trước publish; reload trả snapshot committed duy nhất.
- Save/load giữa activity, boarding decision, assessment timer và NPC dialog; đúng clock, position, pose/UI, reservation và pending choice.
- Skip cancel/save failure; calendar occurrence đúng một lần; missing rubric/unknown không thành fail/pass.
- Migration fail, save mới hơn client, corrupted checksum và contentId thiếu: giữ bản gốc.
- Graduation/first-job/ending commit idempotent, actor knowledge không bị lộ qua recap.

Các ca trên chưa chạy. Không có thiết kế format/implementation, số lượng command/state definitions đầy đủ hoặc bằng chứng engine atomicity; chưa tuyên bố production-ready.
