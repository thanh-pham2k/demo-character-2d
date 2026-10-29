# Event definition và runtime contract — đề xuất v0.1

Không phải API đã tồn tại. Schema này nhằm chuyển event prose sang data để writer, programmer và QA cùng đối chiếu. Trước runtime phải có parser, schema/version, id catalog, calendar, localization và validation; chưa có các thành phần này.

## Definition

`EventDefinition`: `id`, `contentVersion`, `titleKey`, `category` (scheduled/conditional/NPC/location/career/consequence), `yearRange`, `calendarRef`, `locationIds`, `trigger`, `precondition`, `requiredFlags`, `forbiddenFlags`, `actors`, `playerGoalKey`, `conflictKey`, `priority`, `cooldown`, `repeatPolicy`, `variants`, `choices`, `miss`, `recoveryRefs`, `followUpRefs`, `callbackRefs`, `sourceRefs`.

`trigger` mô tả event hoặc calendar occurrence, không tự tạo clock time. `precondition` là predicate AST, trả true/false/unknown; unknown phải đánh dấu data thiếu, không mặc định true. Các actor có role, availability predicate, required/optional và knowledge requirements. Một variant có predicate, priority, dialogueKey; khi nhiều variant đúng chọn priority giảm rồi ID; không có variant thì fallback author hoặc blocked_data. Không bộc lộ private facts mà actor không biết.

`ChoiceDefinition`: `id`, `labelKey`, `availabilityPredicate`, `preview {duration,cost,knownRisks}`, `startCommand`, `onCompleteEffects`, `onInterrupt`, `followUpRefs`. Effect có `type`, `target`, `value`, `condition`, `idempotencyKey`; cho phép explicit `none`. Cần phân loại time, academic, relationship, career, money/energy, flags, memory; absence không được hiểu là tự tính. Một memory effect ghi actors thực, outcome, visibility và `knownBy`.

`EventInstance`: `instanceId`, `definitionId/version`, `occurrenceId`, `state=eligible|offered|running|resolved|expired|blocked_data`, `selectedChoiceId`, `timestamps`, `pendingRunId`, `appliedEffectKeys`, `outcome`, `knowledgeAtTrigger`. InstanceId theo campaign + occurrence + definition; không dùng chỉ event ID cho event có thể lặp. Cooldown và repeat dùng instance ledger, không đọc tên dialogue.

## Thứ tự và kết quả

1. Scheduler nhận occurrence đã author; kiểm tra time, location và actor; availability unknown → blocked_data, false → chờ đến hết cửa sổ nếu event cho phép.
2. Eligible và priority/queue xác định theo `02`; một choice-required event pause clock, không tự random ra choice. `offered` lưu snapshot và revision.
3. Player chọn sau preview; revalidate predicate/revision. Nếu action có duration, tạo activityRun với cost/reservation; nếu 0 phút, commit outcome qua transaction `43`.
4. `onCompleteEffects` apply atomically theo keys; interruption áp dụng policy rõ, không tự tính completion; `miss` xử lý hết window khi chưa resolve, không giả định relationship penalty.
5. Callback chỉ eligible khi source effect/memory thật tồn tại và actor có knowledge; các follow-up IDs phải có definition và window nếu cần, không tự tạo nội dung mới.

## Ví dụ dữ liệu rút gọn — invitation buổi sáng

```yaml
id: EV_Y1_LONG_INVITE
contentVersion: draft-1
category: npc
calendarRef: LONG_INVITE
locationIds: [LOC_ROOM]
trigger: {kind: enter_location_during_window}
precondition: {all: [{flag: met_long, equals: true}, {actor_available: LONG}]}
requiredFlags: [met_long]
forbiddenFlags: [invite_resolved]
actors: [{id: LONG, required: true}]
priority: 50
repeatPolicy: once_per_occurrence
choices:
  - id: play_one
    preview: {durationMinutes: 15, energyCost: 3, moneyCost: 0}
    startCommand: {activityId: A_GAME_LONG_QUICK}
    onCompleteEffects:
      - {type: set_flag, target: invite_resolved, value: played}
      - {type: increment, target: relationship.LONG.trust, value: 2}
      - {type: create_memory, target: memory_long_game_d2, actors: [PLAYER, LONG]}
      - {type: enable_event, target: EV_Y1_LONG_REPLAN}
    onInterrupt: {keepElapsedCost: true, applyCompletion: false}
  - id: not_now
    preview: {durationMinutes: 0, energyCost: 0, moneyCost: 0}
    onCompleteEffects: [{type: set_flag, target: invite_resolved, value: declined}]
    onInterrupt: null
miss: {outcome: expired, effects: []}
```

Đây là minh họa, chưa đủ schema production: thiếu keys bản địa hóa, actor knowledge, calendar occurrence, idempotency keys cụ thể và variant; `leave` trong `10` cũng chưa được ghi. Không coi YAML này là event database đầy đủ hoặc input hợp lệ.

## Authoring validation cần có

- ID unique cùng contentVersion; calendar/location/actor/flag/activity/effect/memory/follow-up tham chiếu đúng type; mọi event phải có fallback/miss hoặc lý do không thể miss.
- Choice có cost/interrupt/complete rõ; 0 phút không tạo vòng vô hạn; exclusive options không được commit cùng instance.
- Không có actor bắt buộc vô lịch, cửa sổ rỗng, callback không có producer, dead-end bắt buộc không có recovery, duplicate credit hoặc cycle không có điểm vào.
- Text/risk preview chỉ dùng facts player biết; NPC variant chỉ dùng knowledge phù hợp. `unknown` phải được bảo toàn khi thiếu data.
- Test scheduler boundary, ordering, retry/save failure, determinism và migration trên dataset thực; các bước này chưa chạy.
