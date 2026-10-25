# Năm 1 — báo cáo gameplay và visual

Ngày kiểm tra: 25/10/2026. Unity 6000.6.7f1, Room404_MVP, Play Mode trên Mac.
Trạng thái: **PASS — 506 kiểm tra, 0 lỗi**. Kết thúc UTC: 2026-10-25T10:55:36.0886730Z.
report.json là kết quả Play Mode cuối sau tích hợp C00, chuyện kể với Đạm,
bộ câu hỏi riêng, pivot NPC và các kiểm tra checkpoint/hủy/chống lặp.
Regression story kế tiếp cũng PASS (story-report.json).

## Phạm vi đã triển khai

- Nhận phòng, xí giường, dọn phòng, gặp bạn, lấy balo và bữa cơm lựa chọn 25k; trợ cấp 100k một lần.
- Sáu ngày, 12 buổi, sáu môn; bài tập/ôn/kiểm tra có câu riêng và retry. HK1 16, kết năm 32/150 tín chỉ quy đổi.
- Sau lớp phải ra H6, đón xe về, lên KTX và về phòng. Có kể chuyện với Đạm khi về phòng, lưu cờ chống lặp.
- Game/Học tập/Đứng dậy; bài giấy viết, Tin/Lập trình dùng bàn phím. Hai laptop/ghế, hai giường và reservation chung.
- Ngày 2: chơi cùng Long → tranh cãi → người chơi tự đứng → công tắc → giường; cả hai ngủ mới mở ngày mới. Hủy ngủ nhả giường và không cộng ngày.
- Xe đi/về cùng chiều với mặt xe, reset theo từng lượt, vé 7k đúng một lần; 120 giây ngày 4, pause menu, giữ suất lên xe, nhánh trễ đi bộ miễn phí.
- JSON version 1, atomic temp/replace/backup; Continue/Chơi lại; restore đứng ở checkpoint an toàn. Thưởng/tín chỉ/chuyển năm idempotent.
- Chỉ khi đủ mốc mới có tổng kết, chuẩn bị lẩu, thoại tự mãn và tự chuyển Năm 2 lúc 23:40.

## Phương pháp kiểm tra

Bộ PlayModeInteractionSuite giữ regression đồ đạc/cửa/di chuyển/story cũ và
chạy toàn bộ campaign bằng movement, E, Space và callback của UI Button thật.
Không gán thẳng stage hoặc tín chỉ để vượt campaign. Fixture gần hết giờ ngày 4
được nạp từ tiến độ đã đạt để kiểm tra nhánh trễ, thay vì chờ đủ 120 giây.
Giữ suất trạm và pause clock được kiểm tra riêng trong cùng lượt.

Checkpoint được Continue qua UI ở sau tiết học, bài tập, đầu ngày 4, sau tắt
đèn và trước lẩu/chuyển năm; restore standing/movement và flag được kiểm tra.
Pure data checks chạy riêng với save sandbox: 12 vé trả đủ còn 26k; nhánh runtime
có một lượt đi bộ còn 33k; corrupt/version/missing-primary recovery qua backup.
Xem data-checks.txt và JSON kết quả bên cạnh báo cáo này.

## Review chuyển động

Video được ghi từ Camera thật trong Play Mode với timestamp Time.time,
khoảng 15 fps; encode H.264 960×540 giữ thời gian thật bằng encode.swift.
Đã mở/phát trong QuickTime và kiểm tra theo timeline, không kết luận chỉ từ
số frame hoặc các PNG khác hash. Video không chứa track audio; audio start/
stop và trạng thái typing/writing được kiểm tra runtime riêng.

- [bus-outbound.mp4](bus-outbound.mp4): xe đi tiến từ trái sang phải, dừng và rời trạm theo cùng chiều.
- [bus-homebound.mp4](bus-homebound.mp4): xe về quay mặt trái, vào/rời theo chiều ngược nhất quán.
- [two-gaming.mp4](two-gaming.mp4): sáu pose có gõ/nghỉ/rê chuột, nhịp hai người lệch nhau; đầu/thân giữ điểm neo, tay ở bàn phím và chân ở ghế.
- [night-lights-off.mp4](night-lights-off.mp4): đối thoại, đứng dậy, đi công tắc và tắt đèn bằng thao tác player.
- [two-sleepers.mp4](two-sleepers.mp4): vào hai giường, nằm/thở, thức dậy; Long dùng batch góc nhìn nằm cùng kiểu Lươn.
- [pose-registration-review.mp4](pose-registration-review.mp4): fixture riêng kiểm tra pivot từ gaming sang đứng/đi và giường. Fixture này bổ sung visual QA, không thay thế campaign acceptance.

Các clip campaign bên cạnh là capture của lượt regression cuối. Clip
pose registration đã xác nhận lỗi chân nhảy lên mặt bàn được sửa bằng pivot
NPC giữa thân và scale đứng cũ, không dời layout phòng.

## Asset và references

37 PNG runtime; giữ source/prompt/grid/crop/pivot/script slice của năm batch
trong assets/mvp_sprites/year1_campaign. Luon bed, room, H6 và laptop hiện có
được reuse. Long bed v2 chỉ thay chín pose sai góc nhìn. NPC bạn học C00 được
reuse từ phần tích hợp asset đang có, không gán thành asset do batch này sinh.
Tất cả 37 PNG có alpha và .meta; import Point/100 PPU/Uncompressed.
References đã serialize trong Room404_MVP, không cần assign Inspector thủ công.

## Giới hạn kiểm chứng

Đây là kiểm chứng Unity Editor/Play Mode; chưa phải kết quả build và test trên
thiết bị Android/iOS. Video silent không chứng minh chất lượng mix âm thanh
trên loa điện thoại. Không mở rộng 45 mission ngoài scope Năm 1 của task này.

Timeline, controls, architecture, files và scene integration:
../../../NEXT_STEPS_HANDOVER.md

## Kết quả nghiệm thu cuối

| Nhóm | Kết quả |
|---|---|
| 12 buổi, bài tập/ôn/kiểm tra, 6 đêm, tổng kết, lẩu, Year2 | PASS |
| Không mở 32 tín chỉ/lẩu/chuyển năm sớm | PASS |
| Xe hai chiều, nhiều lượt, vé một lần, nhánh trễ/đi bộ | PASS |
| Pause deadline và giữ suất trạm | PASS |
| Hai ghế/giường; ngồi/nằm không walk; hủy ngủ rồi ngủ lại | PASS |
| Đứng thủ công → công tắc → giường; không replay argument | PASS |
| Continue sau lesson/homework/lights/end-day/before-feast | PASS |
| Save corrupt/version/missing primary → backup; idempotency | PASS |
| Door/movement/furniture và story stages 1–4/ending cũ | PASS |
| Runtime error/exception/assert trong lượt suite cuối | 0 |
| Compile | PASS; còn warning API cũ ở code nền |

Không còn phần gameplay hoặc asset bắt buộc nào thiếu trong scope Năm 1 đã chốt.
Các phần ngoài scope: bến xe quê/đăng ký KTX, đói/kiếm tiền/nợ môn Năm 1,
hoàn thiện 45 mission và device build.

## Follow-up marker công tắc

Sau lượt 506 check, chỉ marker tác động trên sàn được chiếu về cùng trục X của
Light_Switch hiện có, y = -0.7. Không đổi runtime quest/data hoặc vị trí ổ điện.
Kiểm tra Play Mode ngắn: player đi bằng movement tới (0.85,-0.70), detector
chọn đúng công tắc, generic Interact event phát đúng: **PASS**. Xem
light-marker-review.txt và mobile-2d/PlayModeInteractionReports/light-marker-review.png.
Pose-registration-review.mp4 được ghi lại sau thay đổi; night-lights-off.mp4
thuộc lượt full 506 trước thay đổi marker này, chứng minh chuỗi quest/E/tắt đèn.
Compile sau follow-up thành công. Không chạy lại toàn bộ sáu ngày cho thay đổi
marker; kiểm tra bổ sung tập trung vào đường đi/detector và callback.
